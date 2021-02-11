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
            this.Health = health;
        }

        public Player Player { get; private set; }

        public abstract int MaximumTravelDistance { get; }

        public abstract int ShotRange { get; }

        protected abstract int attackPower { get; }

        protected abstract string sourcePath { get; }

        private string deathSourcePath => "Resources/Units/Dead.png";

        public int Health { get; private set; }

        public bool IsDead => Health == 0;

        public bool CanMove(int x, int y) => Distance(x, X) <= MaximumTravelDistance && Distance(y, Y) <= MaximumTravelDistance;

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ReduceHealth(int healthReductionAmount)
        {
            Health = Math.Max(Health - healthReductionAmount, 0);
        }

        public bool CanAttackUnit(Unit otherUnit) => InRegion(otherUnit, ShotRange);
        public abstract int GetAttackPower(Unit otherUnit);

        protected bool IsUnitNearby(Unit otherUnit) => InRegion(otherUnit, 1);

        public override BitmapImage SourceFrom => IsDead ? new BitmapImage(new Uri(deathSourcePath, UriKind.Relative))
            : new BitmapImage(new Uri(sourcePath, UriKind.Relative));

        protected int Distance(int firstPoint, int secondPoint) => Math.Abs(firstPoint - secondPoint);

        protected bool InRegion(Unit otherUnit, int regionSize) 
            => Distance(otherUnit.X, X) <= regionSize && Distance(otherUnit.Y, Y) <= regionSize;
    }
}
