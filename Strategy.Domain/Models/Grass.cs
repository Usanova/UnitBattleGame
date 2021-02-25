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
        public Grass(int x, int y) : base(x, y, true) { }

        public override BitmapImage SourceFrom
            => new BitmapImage(new Uri("Resources/Ground/Grass.png", UriKind.Relative));
    }
}