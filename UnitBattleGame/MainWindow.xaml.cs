using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UnitBattleGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ViewBase
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void InitGameField(int fieldSize)
        {
            var grid = new FrameworkElementFactory(typeof(Grid));
           

            for (int i = 0; i < fieldSize; i++)
            {
                var col = new FrameworkElementFactory(typeof(ColumnDefinition));
                var row = new FrameworkElementFactory(typeof(RowDefinition));
                grid.AppendChild(col);
                grid.AppendChild(row);
            }

            map.ItemsPanel = new ItemsPanelTemplate(grid);
        }
    }
}
