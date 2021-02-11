﻿namespace Strategy.Domain.Models
{
    /// <summary>
    /// Катапульта.
    /// </summary>
    public sealed class Catapult : Unit
    {
        public Catapult(Player player) : base(player, health: 75) { }

        public override int MaximumTravelDistance => 1;

        public override int ShotRange => 10;

        protected override int attackPower => 100;

        protected override string sourcePath => "Resources/Units/Catapult.png";

        public override int GetAttackPower(Unit otherUnit)
        {
            if (IsUnitNearby(otherUnit))
                return attackPower / 2;

            return attackPower;
        }
    }
}