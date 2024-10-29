using System;
using System.Collections.Generic;

namespace MVVMToolKit.Interfaces
{
    public static class ViewSelectorHelper
    {
        private static readonly Dictionary<IViewSelector, Action<string>> updaterDictionary = new();

        internal static void Add(IViewSelector selector, Action<string> updater)
        {
            updaterDictionary.TryAdd(selector, updater);
        }
        internal static void Remove(IViewSelector selector)
        {
            updaterDictionary.Remove(selector);
        }
        public static void NavigateTo(IViewSelector selector, string viewName)
        {
            if (updaterDictionary.TryGetValue(selector, out Action<string>? navigator))
            {
                navigator.Invoke(viewName);
            }
        }
    }
    /// <summary>
    /// 동일한 ViewModel에서 View가 선택될 수 있는 인터페이스.
    /// </summary>
    public interface IViewSelector
    {
        /// <summary>
        /// 입력된 ViewName을 통해 Navigate를 수행합니다. 
        /// </summary>
        public static void NavigateTo(IViewSelector selector, string viewName)
        {
            ViewSelectorHelper.NavigateTo(selector, viewName);
        }
    }
}
