using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Strategy.Domain.Models
{
    public abstract class GameObject : ReactiveObject
    {
        protected GameObject(int x, int y)
        {
            X = x;
            Y = y;
        }

        [Reactive] public int X { get; protected set; }

        [Reactive] public int Y { get; protected set; }

        [Reactive] public bool Selected { get; private set; }

        [Reactive] public bool IsMovingArea { get; set; }

        public abstract BitmapImage SourceFrom { get; }

        public void IsSelected(bool isSelected) => Selected = isSelected;
    }
}
