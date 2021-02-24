namespace Strategy.Domain.Models
{
    /// <summary>
    /// Класс мечника.
    /// </summary>
    public sealed class Swordsman : Unit
    {
        public Swordsman(Player player, int x, int y) : base(health: 100, player, x, y) { }

        public override int MaximumTravelDistance => 5;

        public override int ShotRange => 1;

        protected override int attackPower => 50;

        protected override string sourcePath => "Resources/Units/Swordsman.png";

        public override int GetAttackPower(Unit otherUnit) => attackPower;
    }
}