namespace Strategy.Domain.Models
{
    /// <summary>
    /// Лучник.
    /// </summary>

    public sealed class Archer : Unit
    {
        public Archer(Player player) : base(player, health: 50) { }

        public override int MaximumTravelDistance => 3;

        public override int ShotRange => 5;

        protected override int attackPower => 50;

        protected override string sourcePath => "Resources/Units/Archer.png";

        public override int GetAttackPower(Unit otherUnit)
        {
            if (IsUnitNearby(otherUnit))
                return attackPower / 2;

            return attackPower;
        }
    }
}