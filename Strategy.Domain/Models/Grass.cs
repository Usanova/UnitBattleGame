using DevExpress.Mvvm;
using ReactiveUI.Fody.Helpers;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Strategy.Domain.Models
{
    /// <summary>
    /// Проходимая поверхность на земле.
    /// </summary>
    public sealed class Grass : GameObject
    {
        public Grass() : base(type: GameObjectType.Grass) { }

        public override BitmapImage SourceFrom
            => new BitmapImage(new Uri("Resources/Ground/Grass.png", UriKind.Relative));

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        private DelegateCommand<object> _move;
        public DelegateCommand<object> Movee => new DelegateCommand<object>(o =>
        {
            var GameObject = (GameObject)o;
            //((Grass)TheGame[0]).Move(3, 3);
        });
    }
}