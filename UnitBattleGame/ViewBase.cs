using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace UnitBattleGame
{
    public class ViewBase : Window
    {
        public void ShowMessageError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowMessageWarning(string warningMessage)
        {
            MessageBox.Show(warningMessage, "", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}