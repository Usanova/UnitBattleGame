namespace Strategy.Domain.Models
{
    /// <summary>
    /// Класс мечника.
    /// </summary>
    public sealed class Swordsman : Unit
    {
        public Swordsman(Player player) : base(player, health: 100) { }

        public override int MaximumTravelDistance => 5;

        public override int ShotRange => 1;

        protected override int attackPower => 50;

        protected override string sourcePath => "Resources/Units/Swordsman.png";

        public override int GetAttackPower(Unit otherUnit) => attackPower;
    }
}