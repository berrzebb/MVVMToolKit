using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Xaml.Behaviors;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Behaviors
{
    public class FrameBehavior: Behavior<Frame>
    {
        public static readonly DependencyProperty NavigationSourceProperty =
            DependencyProperty.Register(nameof(NavigationSource), typeof(string), typeof(FrameBehavior), new PropertyMetadata(null, NavigationSourceChanged));

        public string NavigationSource
        {
            get => (string)this.GetValue(NavigationSourceProperty);
            set => this.SetValue(NavigationSourceProperty, value);
        }
        private bool _isWorking;

        private static void NavigationSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is FrameBehavior behavior)
            {
                if (behavior._isWorking)
                {
                    return;
                }

                behavior.Navigate();
            }
        }
        private void OnNavigated(object? sender, NavigationEventArgs args)
        {
            this._isWorking = true;
            this.NavigationSource = args.Uri.ToString();
            this._isWorking = false;

            if (this.AssociatedObject.Content is FrameworkElement { DataContext: INavigationAware navigationAware })
            {
                navigationAware?.OnNavigated(sender, args);
            }
        }
        private void OnNavigating(object? sender, NavigatingCancelEventArgs args)
        {
            if (this.AssociatedObject.Content is FrameworkElement { DataContext: INavigationAware navigationAware })
            {
                navigationAware?.OnNavigating(sender, args);
            }
        }
        
        protected override void OnAttached()
        {
            this.AssociatedObject.Navigating += this.OnNavigating;
            this.AssociatedObject.Navigated += this.OnNavigated;

        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Navigating -= this.OnNavigating;
            this.AssociatedObject.Navigated -= this.OnNavigated;
        }

        private void Navigate()
        {
            switch (this.NavigationSource)
            {
                case "GoBack":
                    if (this.AssociatedObject.CanGoBack)
                    {
                        this.AssociatedObject.GoBack();
                    }

                    break;
                case "GoForward":
                    if (this.AssociatedObject.CanGoForward)
                    {
                        this.AssociatedObject.GoForward();
                    }
                    break;
                case null:
                    case "":
                    return;
                default:
                    this.AssociatedObject.Navigate(new Uri(this.NavigationSource, UriKind.RelativeOrAbsolute));
                    break;
            }
        }
    }
}
