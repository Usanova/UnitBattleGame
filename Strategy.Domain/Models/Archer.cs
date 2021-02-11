namespace Strategy.Domain.Models
{
    /// <summary>
    /// Лучник.
    /// </summary>

    public sealed class Archer : Unit
    {
        public Archer(Player player) : base(player, health: 50) { }

        protected override int maximumTravelDistance => 3;

        protected override int shotRange => 5;

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