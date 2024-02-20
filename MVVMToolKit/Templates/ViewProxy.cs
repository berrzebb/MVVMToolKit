namespace MVVMToolKit.Templates
{
    using System.Windows.Controls;
    using MVVMToolKit.Ioc;
    using MVVMToolKit.Navigation.Mapping;

    internal static class ViewFactory
    {
        private static readonly Dictionary<Type, UserControl> ViewCache = new();


        private static UserControl? GetViewFromNonCached(Type type) => Activator.CreateInstance(type) as UserControl;

        private static UserControl? GetViewFromCached(Type type)
        {
            // 2. DI Container에 등록되어 있지 않은 View라면 Cache에 해당 View가 존재하는지 확인하고 새로운 View를 생성합니다.
            if (ViewCache.TryGetValue(type, out var userControl)) return userControl;
            userControl = GetViewFromNonCached(type);

            if (userControl != null)
            {
                ViewCache.Add(type, userControl);
            }
            return userControl;

        }

        private static UserControl? GetViewFromDependencyInjection(Type type) =>
            (UserControl?)ContainerProvider.Resolve(type);

        private static UserControl GetViewFromException(FrameworkElement owner, string? viewName)
        {
            var reason = $"ViewModel {owner.DataContext.GetType().Name}에 해당하는 화면이 존재하지않습니다.\n입력받은 viewName은 {viewName} 입니다. 확인하여 주십시오.";
            throw new ArgumentNullException(nameof(viewName), reason);
            /*
            return new()
            {
                Content = reason
            };
            */
        }

        /// <summary>
        /// 입력받은 viewName에 대하여 View를 생성하여 반환합니다.
        /// 이때 
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
&& TypeProvider.Resolve(viewName, out var targetViewType)
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
        public static readonly DependencyProperty ViewTypeProperty = DependencyProperty.Register(
            name: nameof(ViewType),
            propertyType: typeof(string),
            ownerType: typeof(ViewProxy),
            new PropertyMetadata(null, OnViewChanged));
        public static readonly DependencyProperty ViewCacheModeProperty = DependencyProperty.Register(
            name: nameof(ViewCacheMode),
            propertyType: typeof(ViewCacheMode),
            ownerType: typeof(ViewProxy),
            new PropertyMetadata(ViewCacheMode.DependencyInjection, OnViewChanged));
        public static readonly DependencyProperty ViewSelectorProperty = DependencyProperty.Register(
            name: nameof(ViewSelector),
            propertyType: typeof(Func<INotifyPropertyChanged, string>),
            ownerType: typeof(ViewProxy),
            new PropertyMetadata(null, OnViewChanged));
        public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register(
            name: nameof(ViewMode),
            propertyType: typeof(ViewMode),
            ownerType: typeof(ViewProxy),
            new PropertyMetadata(ViewMode.Single, OnViewChanged));

        private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ViewProxy viewProxy)
            {
                viewProxy.UpdateView();
            }
        }

        private void UpdateView()
        {
            var targetViewType = this.ViewType;
            if (this.ViewMode == ViewMode.Selector)
            {
                if (this.DataContext is not INotifyPropertyChanged viewModel) return;

                if (this.ViewSelector != null)
                {
                    targetViewType = this.ViewSelector?.Invoke(viewModel);
                }
                else if (viewModel is IViewSelector viewSelector)
                {
                    targetViewType = viewSelector.GetView();
                }
            }

            if (targetViewType == this.CurrentView) return;
            this.Content = ViewFactory.GetView(this, this.ViewCacheMode, targetViewType);
            this.CurrentView = this.Content?.GetType().Name ?? default;

        }
        private string? CurrentView { get; set; }
        public string? ViewType
        {
            get => (string?)this.GetValue(ViewTypeProperty);
            set => this.SetValue(ViewTypeProperty, value);
        }
        public ViewCacheMode ViewCacheMode
        {
            get => (ViewCacheMode)this.GetValue(ViewCacheModeProperty);
            set => this.SetValue(ViewCacheModeProperty, value);
        }
        public ViewMode ViewMode
        {
            get => (ViewMode)this.GetValue(ViewModeProperty);
            set => this.SetValue(ViewModeProperty, value);
        }
        public Func<INotifyPropertyChanged, string>? ViewSelector
        {
            get => (Func<INotifyPropertyChanged, string>?)this.GetValue(ViewSelectorProperty);
            set => this.SetValue(ViewSelectorProperty, value);
        }
        public ViewProxy()
        {
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is INotifyPropertyChanged propertyChanged)
            {
                propertyChanged.PropertyChanged += this.PropertyChangedOnPropertyChanged;
            }
            this.UpdateView();

        }

        private void PropertyChangedOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (this.ViewMode == ViewMode.Selector)
            {
                this.UpdateView();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is INotifyPropertyChanged propertyChanged)
            {
                propertyChanged.PropertyChanged -= this.PropertyChangedOnPropertyChanged;
            }
            this.Unloaded -= this.OnUnloaded;
            this.Content = null;
        }
    }
}
