using System.Windows.Media;

namespace Strategy.Domain.Models
{
    /// <summary>
    /// Игрок.
    /// </summary>
    public sealed class Player
    {
        /// <inheritdoc />
        public Player(int id, string name, ImageSource portrait)
        {
            Id = id;
            Name = name;
            Portrait = portrait;
        }


        /// <summary>
        /// Идентификатор игрока.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Портрет игрока.
        /// </summary>
        public ImageSource Portrait { get; set; }
    }
}