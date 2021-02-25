using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows.Media.Imaging;

namespace Strategy.Domain.Models
{
    public abstract class GameObject : ReactiveObject
    {
        protected GameObject(int x, int y, bool earth)
        {
            X = x;
            Y = y;
            Earth = earth;
        }

        [Reactive] public int X { get; protected set; }

        [Reactive] public int Y { get; protected set; }

        [Reactive] public bool Selected { get; private set; }

        [Reactive] public bool IsMovingArea { get; set; }

        public abstract BitmapImage SourceFrom { get; }

        public bool Earth { get; private set; }

        public void IsSelected(bool isSelected) => Selected = isSelected;
    }
}
