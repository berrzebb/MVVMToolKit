using System.Collections.Concurrent;
using MVVMToolKit.Ioc;
using MVVMToolKit.Templates;

namespace MVVMToolKit.Services.Dialog
{
    internal static class DialogTypeRegistry
    {
        private static readonly ConcurrentDictionary<string, Type> dialogTypes = new();

        internal static void RegisterDialog(string dialogKey, Type type)
        {
            _ = dialogTypes.AddOrUpdate(dialogKey, type, (_, _) => type);
        }

        private static bool Dialog(string name, out Type? type) => dialogTypes.TryGetValue(name, out type);
        internal static PopupWindow? CreateDialog(string name, bool isDependencyInjection = false)
        {
            if (!Dialog(name, out Type? registeredType))
            {
                return null;
            }

            if (registeredType == null)
                return null;

            PopupWindow? dialogWindow;
            if (isDependencyInjection)
            {
                dialogWindow = (PopupWindow?)ContainerProvider.Resolve(registeredType);
            }
            else
            {
                dialogWindow = (PopupWindow?)Activator.CreateInstance(registeredType);
            }

            return dialogWindow;
        }
    }
}
