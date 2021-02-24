using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
        protected Unit(int health, Player player, int x, int y): base(x, y)
        {
            this.Health = health;
            Player = player;
        }

        public Player Player { get; private set; }

        public abstract int MaximumTravelDistance { get; }

        public abstract int ShotRange { get; }

        protected abstract int attackPower { get; }

        protected abstract string sourcePath { get; }

        private string deathSourcePath => "Resources/Units/Death.png";

        private int health;
        public int Health { get => health;
            private set
            {
                health = value;
                this.RaisePropertyChanged("IsDead");
                this.RaisePropertyChanged("SourceFrom");
            } 
        }

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

        public override BitmapImage SourceFrom => IsDead ? new BitmapImage(new Uri(deathSourcePath, UriKind.Relative))
              : new BitmapImage(new Uri(sourcePath, UriKind.Relative));

        protected bool IsUnitNearby(Unit otherUnit) => InRegion(otherUnit, 1);

        protected bool InRegion(Unit otherUnit, int regionSize)
            => Distance(otherUnit.X, X) <= regionSize && Distance(otherUnit.Y, Y) <= regionSize;

        protected int Distance(int firstPoint, int secondPoint) => Math.Abs(firstPoint - secondPoint);

        public static bool TryParse(GameObject gameObject, out Unit unit)
        {
            unit = null;

            if (!(gameObject is Unit))
                return false;

            unit = (Unit)gameObject;

            return true;
        }
    }
}
