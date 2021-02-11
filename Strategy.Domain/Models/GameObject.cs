using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Strategy.Domain.Models
{
    public abstract class GameObject
    {
        protected GameObject(GameObjectType type)
        {
            Type = type;
        }

        public GameObjectType Type { get; protected set; }
        /// <summary>
        /// Координата x травы на карте.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координата y травы на карте.
        /// </summary>
        public int Y { get; set; }

        public abstract ImageSource GetSourceFrom();
    }

    public enum GameObjectType
    {
        Unit,
        Grass,
        Water
    }
}
