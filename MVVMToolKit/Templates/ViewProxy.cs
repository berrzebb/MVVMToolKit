﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Navigation.Mapping;

namespace MVVMToolKit.Templates
{
    internal static class ViewFactory
    {
        private static readonly Dictionary<Type, UserControl> viewCache = new();

        private static UserControl? GetViewFromNonCached(Type type) => Activator.CreateInstance(type) as UserControl;

        private static UserControl? GetViewFromCached(Type type)
        {
            // 2. DI Container에 등록되어 있지 않은 View라면 Cache에 해당 View가 존재하는지 확인하고 새로운 View를 생성합니다.
            if (viewCache.TryGetValue(type, out UserControl? userControl))
                return userControl;
            userControl = GetViewFromNonCached(type);

            if (userControl != null)
            {
                viewCache.Add(type, userControl);
            }

            return userControl;

        }

        private static UserControl? GetViewFromDependencyInjection(Type type) => (UserControl?)ContainerProvider.Resolve(type);

        private static UserControl GetViewFromException(FrameworkElement owner, string? viewName)
        {
            string reason = $"ViewModel {owner.DataContext?.GetType().Name}에 해당하는 화면이 존재하지않습니다.\n입력받은 viewName은 {viewName} 입니다. 확인하여 주십시오.";

            return new()
            {
                Content = reason
            };

        }

        /// <summary>
        /// 입력받은 viewName에 대하여 View를 생성하여 반환합니다.
        /// </summary>
        /// <param name="owner">view를 사용하는 주체.</param>
        /// <param name="cacheMode">View를 생성하는 방법.</param>
        /// <param name="viewName">사용할 view의 이름.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static UserControl? GetView(FrameworkElement owner, ViewCacheMode cacheMode, string? viewName)

        {
            UserControl? userControl = default;
            if (!string.IsNullOrEmpty(viewName)
&& TypeProvider.Resolve(viewName, out Type? targetViewType)
                && targetViewType != null)
            {
                userControl = cacheMode switch
                {
                    ViewCacheMode.Cached => GetViewFromCached(targetViewType),
                    ViewCacheMode.NonCached => GetViewFromNonCached(targetViewType),
                    ViewCacheMode.DependencyInjection => GetViewFromDependencyInjection(targetViewType),
                    _ => GetViewFromDependencyInjection(targetViewType)
                };
            }

            userControl ??= GetViewFromException(owner, viewName);
            return userControl;
        }
    }

    internal sealed class ViewProxy : UserControl
    {
        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.Register(
            name: nameof(ViewName),
            propertyType: typeof(string),
            ownerType: typeof(ViewProxy),
            new PropertyMetadata(null, OnViewChanged));
        public static readonly DependencyProperty ViewCacheModeProperty = DependencyProperty.Register(
            name: nameof(ViewCacheMode),
            propertyType: typeof(ViewCacheMode),
            ownerType: typeof(ViewProxy),
            new PropertyMetadata(ViewCacheMode.DependencyInjection, OnViewChanged));

        private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ViewProxy viewProxy)
            {
                viewProxy.UpdateView();
            }
        }

        private void UpdateView()
        {
            UpdateView(ViewName);
        }
        private void UpdateView(string? targetViewType)
        {
            if (targetViewType == CurrentView)
                return;
            Content = ViewFactory.GetView(this, ViewCacheMode, targetViewType);
            CurrentView = Content?.GetType().Name ?? default;
        }
        private string? CurrentView { get; set; }
        public string? ViewName
        {
            get => (string?)GetValue(ViewNameProperty);
            set => SetValue(ViewNameProperty, value);
        }
        public ViewCacheMode ViewCacheMode
        {
            get => (ViewCacheMode)GetValue(ViewCacheModeProperty);
            set => SetValue(ViewCacheModeProperty, value);
        }

        public ViewProxy()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IViewSelector viewSelector)
            {
                ViewSelectorHelper.Add(viewSelector, UpdateView);
            }

            UpdateView();

        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IViewSelector viewSelector)
            {
                ViewSelectorHelper.Remove(viewSelector);
            }

            Content = null;
            CurrentView = null;
        }
    }
}
