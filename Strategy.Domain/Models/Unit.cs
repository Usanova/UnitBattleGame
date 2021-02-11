using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Strategy.Domain.Models
{
    public abstract class Unit : GameObject
    {
        protected Unit(Player player, int health) : base(GameObjectType.Unit)
        {
            Player = player;
            health = health;
        }

        public Player Player { get; private set; }

        protected abstract int maximumTravelDistance { get; }

        protected abstract int shotRange { get; }

        protected abstract int attackPower { get; }

        protected abstract string sourcePath { get; }

        private string deathSourcePath => "Resources/Units/Dead.png";

        private int health;

        public bool IsDead => health == 0;

        public bool CanMove(int x, int y) => Distance(x, X) <= maximumTravelDistance && Distance(y, Y) <= maximumTravelDistance;

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ReduceHealth(int healthReductionAmount)
        {
            health = Math.Max(health - healthReductionAmount, 0);
        }

        public bool CanAttackUnit(Unit otherUnit) => InRegion(otherUnit, shotRange);
        public abstract int GetAttackPower(Unit otherUnit);

        protected bool IsUnitNearby(Unit otherUnit) => InRegion(otherUnit, 1);

        public override ImageSource GetSourceFrom() 
        {
            if(IsDead)
                return new BitmapImage(new Uri(deathSourcePath, UriKind.Relative));

            return new BitmapImage(new Uri(sourcePath, UriKind.Relative)); 
        }

        protected int Distance(int firstPoint, int secondPoint) => Math.Abs(firstPoint - secondPoint);

        protected bool InRegion(Unit otherUnit, int regionSize) 
            => Distance(otherUnit.X, X) <= regionSize && Distance(otherUnit.Y, Y) <= regionSize;
    }
}
