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

        public override ImageSource GetSourceFrom() 
            => new BitmapImage(new Uri("Resources/Ground/Grass.png", UriKind.Relative));
    }
}