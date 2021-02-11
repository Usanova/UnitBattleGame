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
        protected GameObject(GameObjectType type)
        {
            Type = type;
        }

        public GameObjectType Type { get; protected set; }
        /// <summary>
        /// Координата x травы на карте.
        /// </summary>
        [Reactive] public int X { get; set; }

        /// <summary>
        /// Координата y травы на карте.
        /// </summary>
        [Reactive] public int Y { get; set; }

        [Reactive] public bool Selected { get; private set; }

        public abstract BitmapImage SourceFrom { get; }

        public void ChageSelected()
        {
            Selected = !Selected;
        }
    }

    public enum GameObjectType
    {
        Unit,
        Grass,
        Water
    }
}
