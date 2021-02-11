using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Strategy.Domain.Models
{
    /// <summary>
    /// Непроходимая наземная поверхность.
    /// </summary>
    public sealed class Water : GameObject
    {
        public Water() : base(type: GameObjectType.Water) { }

        public override BitmapImage SourceFrom
            => new BitmapImage(new Uri("Resources/Ground/Water.png", UriKind.Relative));
    }
}