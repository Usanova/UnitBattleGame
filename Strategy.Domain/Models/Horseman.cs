namespace Strategy.Domain.Models
{
    /// <summary>
    /// Класс всадника.
    /// </summary>
    public sealed class Horseman : Unit
    {
        public Horseman(Player player, int x, int y) : base(health: 200, player, x, y) { }

        public override int MaximumTravelDistance => 10;

        public override int ShotRange => 1;

        protected override int attackPower => 75;

        protected override string sourcePath => "Resources/Units/Horseman.png";

        public override int GetAttackPower(Unit otherUnit) => attackPower;
    }
}