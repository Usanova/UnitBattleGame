using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UnitBattleGame
{
    public class DisplayRootRegistry
    {
        Dictionary<Type, Type> vmToViewBaseMapping = new Dictionary<Type, Type>();

        public void RegisterWindowType<VM, Win>() where Win : ViewBase, new() where VM : class
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (vmToViewBaseMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is already registered");
            vmToViewBaseMapping[vmType] = typeof(Win);
        }

        public void UnregisterWindowType<VM>()
        {
            var vmType = typeof(VM);
            if (vmType.IsInterface)
                throw new ArgumentException("Cannot register interfaces");
            if (!vmToViewBaseMapping.ContainsKey(vmType))
                throw new InvalidOperationException(
                    $"Type {vmType.FullName} is not registered");
            vmToViewBaseMapping.Remove(vmType);
        }

        public ViewBase CreateWindowInstanceWithVM(ViewModelBase vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            Type ViewBaseType = null;

            var vmType = vm.GetType();
            while (vmType != null && !vmToViewBaseMapping.TryGetValue(vmType, out ViewBaseType))
                vmType = vmType.BaseType;

            if (ViewBaseType == null)
                throw new ArgumentException(
                    $"No registered ViewBase type for argument type {vm.GetType().FullName}");

            var ViewBase = (ViewBase)Activator.CreateInstance(ViewBaseType);
            vm.Error = new Action<string>(ViewBase.ShowMessageError);

            ViewBase.DataContext = vm;
            return ViewBase;
        }


        Dictionary<object, ViewBase> openViewBases = new Dictionary<object, ViewBase>();
        public void ShowPresentation(ViewModelBase vm)
        {
            if (vm == null)
                throw new ArgumentNullException("vm");
            if (openViewBases.ContainsKey(vm))
                throw new InvalidOperationException("UI for this VM is already displayed");
            var ViewBase = CreateWindowInstanceWithVM(vm);
            ViewBase.Show();
            openViewBases[vm] = ViewBase;
        }

        public void HidePresentation(ViewModelBase vm)
        {
            ViewBase ViewBase;
            if (!openViewBases.TryGetValue(vm, out ViewBase))
                throw new InvalidOperationException("UI for this VM is not displayed");
            ViewBase.Close();
            openViewBases.Remove(vm);
        }

        public async Task ShowModalPresentation(ViewModelBase vm)
        {
            var view = CreateWindowInstanceWithVM(vm);
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            await view.Dispatcher.InvokeAsync(() => view.ShowDialog());
        }

    }
}
