using System;
using System.Reflection;
using System.Windows;

namespace MVVMToolKit.Hosting.Extensions
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// 생성자의 설정이 정상적으로 구성되었는지 확인합니다.
        /// </summary>
        /// <param name="application">WPF <see cref="Application" />.</param>
        /// <exception cref="InvalidOperationException">파라미터가 없는 생성자가 private 인 경우 예외가 발생합니다.</exception>
        public static void CheckForInvalidConstructorConfiguration(this Application application)
        {
            var applicationType = application.GetType();
            var constructors = applicationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            //생성자가 둘 이상인 경우 파라미터가 없는 private 생성자가 있는지 확인합니다.
            if (constructors.Length > 1)
            {
                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters();
                    if (parameters.Length == 0)
                    {
                        if (!constructor.IsPrivate)
                        {
                            throw new InvalidOperationException($"You need to have a paramtless private constructor if you have multiple constructor in {applicationType.FullName}.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// WPF 응용 프로그램이 이미 종료되었는지 확인합니다.
        /// </summary>
        /// <param name="instance">WPF 응용프로그램 <see cref="Application"/></param>
        /// <returns>When WPF application is shutdown return <c>true</c></returns>
        internal static bool IsWPFShutdown<TApplication>(this TApplication instance)
            where TApplication : Application
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo? field = typeof(Application).GetField("_appIsShutdown", bindFlags);

            return (bool)(field?.GetValue(instance) ?? false);
        }

        internal static void InvokeIfRequired<TApplication>(this TApplication instance, Action action)
            where TApplication : Application
        {
            if (!instance.CheckAccess())
            {
                instance.Dispatcher.Invoke(action);
            }
            else
            {
                action();
            }
        }

        internal static void CloseAllWindowsIfAny<TApplication>(this TApplication? application)
            where TApplication : Application
        {
            if (application is null)
            {
                return;
            }

            foreach (var window in application.Windows)
            {
                if (window is Window WPFWindow)
                {
                    WPFWindow.Close();
                }
            }
        }
    }
}
