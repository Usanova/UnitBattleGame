using System.Windows.Media;

namespace Strategy.Domain.Models
{
    public sealed class Player
    {
        public Player(int id, string name, ImageSource portrait)
        {
            Id = id;
            Name = name;
            Portrait = portrait;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ImageSource Portrait { get; set; }
    }
}