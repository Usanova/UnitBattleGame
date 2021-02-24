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
        public Water(int x, int y) : base(x, y) { }

        public override BitmapImage SourceFrom
            => new BitmapImage(new Uri("Resources/Ground/Water.png", UriKind.Relative));
    }
}