using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Xaml.Behaviors;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Behaviors
{
    /// <summary>
    /// The frame behavior class
    /// </summary>
    /// <seealso cref="Behavior{Frame}"/>
    public class FrameBehavior: Behavior<Frame>
    {
        /// <summary>
        /// The navigation source changed.
        /// </summary>
        public static readonly DependencyProperty NavigationSourceProperty =
            DependencyProperty.Register(nameof(NavigationSource), typeof(string), typeof(FrameBehavior), new PropertyMetadata(null, NavigationSourceChanged));

        /// <summary>
        /// Gets or sets the value of the navigation source.
        /// </summary>
        public string NavigationSource
        {
            get => (string)this.GetValue(NavigationSourceProperty);
            set => this.SetValue(NavigationSourceProperty, value);
        }
        /// <summary>
        /// The is working
        /// </summary>
        private bool _isWorking;

        /// <summary>
        /// Navigations the source changed using the specified d
        /// </summary>
        /// <param name="d">The </param>
        /// <param name="e">The </param>
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
        /// <summary>
        /// Ons the navigated using the specified sender
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The args</param>
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
        /// <summary>
        /// Ons the navigating using the specified sender
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The args</param>
        private void OnNavigating(object? sender, NavigatingCancelEventArgs args)
        {
            if (this.AssociatedObject.Content is FrameworkElement { DataContext: INavigationAware navigationAware })
            {
                navigationAware?.OnNavigating(sender, args);
            }
        }
        
        /// <summary>
        /// Ons the attached
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.Navigating += this.OnNavigating;
            this.AssociatedObject.Navigated += this.OnNavigated;

        }

        /// <summary>
        /// Ons the detaching
        /// </summary>
        protected override void OnDetaching()
        {
            this.AssociatedObject.Navigating -= this.OnNavigating;
            this.AssociatedObject.Navigated -= this.OnNavigated;
        }

        /// <summary>
        /// Navigates this instance
        /// </summary>
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
