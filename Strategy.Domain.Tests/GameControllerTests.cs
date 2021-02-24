using System.Collections.Generic;
using NUnit.Framework;
using Strategy.Domain.Models;

namespace Strategy.Domain.Tests
{
    /// <summary>
    /// Тестирование <see cref="GameController" />.
    /// </summary>
    [TestFixture]
    public class GameControllerTests
    {
        #region GetObjectCoordinates

        /// <summary>
        /// Проверить корректность получения координат объекта.
        /// </summary>
        [Test]
        public void GetObjectCoordinates_AllTypes()
        {
            var player = new Player(1, "Игрок №1", null);
            var map = new Map(null, null);
            var gameController = new GameController(map);


            var archer = new Archer(player, 1, 2);
            var archerCoordinates = gameController.GetObjectCoordinates(archer);
            Assert.AreEqual(1, archerCoordinates.X);
            Assert.AreEqual(2, archerCoordinates.Y);

            var catapult = new Catapult(player, 3, 4);
            var catapultCoordinates = gameController.GetObjectCoordinates(catapult);
            Assert.AreEqual(3, catapultCoordinates.X);
            Assert.AreEqual(4, catapultCoordinates.Y);

            var horseman = new Horseman(player, 5, 6);
            var horsemanCoordinates = gameController.GetObjectCoordinates(horseman);
            Assert.AreEqual(5, horsemanCoordinates.X);
            Assert.AreEqual(6, horsemanCoordinates.Y);

            var swordsman = new Swordsman(player, 7, 8);
            var swordsmanCoordinates = gameController.GetObjectCoordinates(swordsman);
            Assert.AreEqual(7, swordsmanCoordinates.X);
            Assert.AreEqual(8, swordsmanCoordinates.Y);


            var grass = new Grass(9, 10);
            var grassCoordinates = gameController.GetObjectCoordinates(grass);
            Assert.AreEqual(9, grassCoordinates.X);
            Assert.AreEqual(10, grassCoordinates.Y);

            var water = new Water(11, 12);
            var waterCoordinates = gameController.GetObjectCoordinates(water);
            Assert.AreEqual(11, waterCoordinates.X);
            Assert.AreEqual(12, waterCoordinates.Y);
        }

        #endregion

        #region CanMoveUnit

        /// <summary>
        /// Проверить перемещение арбалетчика на пустой карте.
        /// </summary>
        [Test]
        [TestCase(6, 7, false)]
        [TestCase(7, 6, false)]
        [TestCase(14, 13, false)]
        [TestCase(13, 14, false)]
        [TestCase(10, 10, false)]
        [TestCase(9, 10, true)]
        [TestCase(11, 10, true)]
        [TestCase(7, 7, true)]
        [TestCase(13, 13, true)]
        public void CanMoveUnit_ArcherOnEmptyMap(int x, int y, bool canMove)
        {
            const int startPositionX = 10;
            const int startPositionY = 10;

            var player = new Player(1, "Игрок №1", null);
            var archer = new Archer(player, startPositionX, startPositionY);
            var map = CreateMap(units: new[] { archer });
            var gameController = new GameController(map);

            Assert.AreEqual(canMove, gameController.CanMoveUnit(archer, x, y));
        }

        /// <summary>
        /// Проверить перемещение катапульты на пустой карте.
        /// </summary>
        [Test]
        [TestCase(8, 9, false)]
        [TestCase(9, 8, false)]
        [TestCase(10, 10, false)]
        [TestCase(11, 12, false)]
        [TestCase(12, 11, false)]
        [TestCase(9, 10, true)]
        [TestCase(11, 10, true)]
        [TestCase(9, 9, true)]
        [TestCase(11, 11, true)]
        public void CanMoveUnit_CatapultOnEmptyMap(int x, int y, bool canMove)
        {
            const int startPositionX = 10;
            const int startPositionY = 10;

            var player = new Player(1, "Игрок №1", null);
            var catapult = new Catapult(player, startPositionX, startPositionY);
            var map = CreateMap(units: new[] { catapult });
            var gameController = new GameController(map);

            Assert.AreEqual(canMove, gameController.CanMoveUnit(catapult, x, y));
        }

        /// <summary>
        /// Проверить перемещение всадника на пустой карте.
        /// </summary>
        [Test]
        [TestCase(10, 9, false)]
        [TestCase(9, 10, false)]
        [TestCase(30, 31, false)]
        [TestCase(31, 30, false)]
        [TestCase(20, 20, false)]
        [TestCase(10, 10, true)]
        [TestCase(30, 30, true)]
        [TestCase(11, 15, true)]
        [TestCase(25, 12, true)]
        public void CanMoveUnit_HorsemanOnEmptyMap(int x, int y, bool canMove)
        {
            const int startPositionX = 20;
            const int startPositionY = 20;

            var player = new Player(1, "Игрок №1", null);
            var horseman = new Horseman(player, startPositionX, startPositionY);
            var map = CreateMap(units: new[] { horseman });
            var gameController = new GameController(map);

            Assert.AreEqual(canMove, gameController.CanMoveUnit(horseman, x, y));
        }

        /// <summary>
        /// Проверить перемещение мечника на пустой карте.
        /// </summary>
        [Test]
        [TestCase(4, 5, false)]
        [TestCase(5, 4, false)]
        [TestCase(15, 16, false)]
        [TestCase(16, 15, false)]
        [TestCase(10, 10, false)]
        [TestCase(5, 5, true)]
        [TestCase(15, 15, true)]
        [TestCase(9, 10, true)]
        [TestCase(12, 7, true)]
        public void CanMoveUnit_SwordsmanOnEmptyMap(int x, int y, bool canMove)
        {
            const int startPositionX = 10;
            const int startPositionY = 10;

            var player = new Player(1, "Игрок №1", null);
            var swordsman = new Swordsman(player, startPositionX, startPositionY);
            var map = CreateMap(units: new[] { swordsman });
            var gameController = new GameController(map);

            Assert.AreEqual(canMove, gameController.CanMoveUnit(swordsman, x, y));
        }

        /// <summary>
        /// Проверить, что юнит может переместиться на клетку с травой.
        /// </summary>
        [Test]
        public void CanMoveUnit_SwordsmanOnGrass_True()
        {
            const int grassPositionX = 15;
            const int grassPositionY = 15;

            var player = new Player(1, "Игрок №1", null);
            var swordsman = new Swordsman(player, 10, 10);
            var grass = new Grass(grassPositionX, grassPositionY);
            var map = CreateMap(new[] { grass }, new[] { swordsman });
            var gameController = new GameController(map);

            Assert.True(gameController.CanMoveUnit(swordsman, grassPositionX, grassPositionY));
        }

        /// <summary>
        /// Проверить, что юнит может переместиться на клетку с травой.
        /// </summary>
        [Test]
        public void CanMoveUnit_SwordsmanOnWater_False()
        {
            const int waterPositionX = 15;
            const int waterPositionY = 15;

            var player = new Player(1, "Игрок №1", null);
            var swordsman = new Swordsman(player, 10, 10);
            var water = new Water(waterPositionX, waterPositionY);
            var map = CreateMap(new[] { water }, new[] { swordsman });
            var gameController = new GameController(map);

            Assert.False(gameController.CanMoveUnit(swordsman, waterPositionX, waterPositionY));
        }

        /// <summary>
        /// Проверить, что юнит не может переместиться на клетку, которую занял другой юнит.
        /// </summary>
        [Test]
        public void CanMoveUnit_CatapultOnHorseman_False()
        {
            const int horsemanPositionX = 15;
            const int horsemanPositionY = 15;

            var player = new Player(1, "Игрок №1", null);
            var catapult = new Catapult(player, 10, 10);
            var horseman = new Horseman(player, horsemanPositionX, horsemanPositionY);
            var map = CreateMap(units: new GameObject[] { catapult, horseman });
            var gameController = new GameController(map);

            Assert.False(gameController.CanMoveUnit(horseman, horsemanPositionX, horsemanPositionY));
        }

        #endregion

        #region MoveUnit

        /// <summary>
        /// Проверить корректность получения координат объекта.
        /// </summary>
        [Test]
        public void MoveUnit_AllTypes()
        {
            const int movePositionX = 15;
            const int movePositionY = 15;

            var player = new Player(1, "Игрок №1", null);
            var map = CreateMap();
            var gameController = new GameController(map);


            var archer = new Archer(player, 14, 14);
            gameController.MoveUnit(archer, movePositionX, movePositionY);
            Assert.AreEqual(movePositionX, archer.X);
            Assert.AreEqual(movePositionY, archer.Y);

            var catapult = new Catapult(player, 14, 14);
            gameController.MoveUnit(catapult, movePositionX, movePositionY);
            Assert.AreEqual(movePositionX, catapult.X);
            Assert.AreEqual(movePositionY, catapult.Y);

            var horseman = new Horseman(player, 14, 14);
            gameController.MoveUnit(horseman, movePositionX, movePositionY);
            Assert.AreEqual(movePositionX, horseman.X);
            Assert.AreEqual(movePositionY, horseman.Y);

            var swordsman = new Swordsman(player, 14, 14);
            gameController.MoveUnit(swordsman, movePositionX, movePositionY);
            Assert.AreEqual(movePositionX, swordsman.X);
            Assert.AreEqual(movePositionY, swordsman.Y);
        }

        #endregion

        #region CanAttackUnit

        /// <summary>
        /// Проверить, что лучник может атаковать врага.
        /// </summary>
        [Test]
        [TestCase(4, 5, false)]
        [TestCase(5, 4, false)]
        [TestCase(15, 16, false)]
        [TestCase(16, 15, false)]
        [TestCase(5, 5, true)]
        [TestCase(15, 15, true)]
        [TestCase(9, 10, true)]
        [TestCase(12, 7, true)]
        public void CanAttackUnit_Archer(int x, int y, bool canAttack)
        {
            const int startPositionX = 10;
            const int startPositionY = 10;

            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var archer = new Archer(player1, startPositionX, startPositionY);
            var target = new Archer(player2, x, y);
            var map = CreateMap(units: new GameObject[] { archer, target });
            var gameController = new GameController(map);

            Assert.AreEqual(canAttack, gameController.CanAttackUnit(archer, target));
        }

        /// <summary>
        /// Проверить, что катапульта может атаковать врага.
        /// </summary>
        [Test]
        [TestCase(10, 9, false)]
        [TestCase(9, 10, false)]
        [TestCase(30, 31, false)]
        [TestCase(31, 30, false)]
        [TestCase(10, 10, true)]
        [TestCase(30, 30, true)]
        [TestCase(11, 15, true)]
        [TestCase(25, 12, true)]
        public void CanAttackUnit_Catapult(int x, int y, bool canAttack)
        {
            const int startPositionX = 20;
            const int startPositionY = 20;

            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var catapult = new Catapult(player1, startPositionX, startPositionY);
            var target = new Archer(player2, x, y);
            var map = CreateMap(units: new GameObject[] { catapult, target });
            var gameController = new GameController(map);

            Assert.AreEqual(canAttack, gameController.CanAttackUnit(catapult, target));
        }

        /// <summary>
        /// Проверить, что всадник может атаковать врага.
        /// </summary>
        [Test]
        [TestCase(8, 9, false)]
        [TestCase(9, 8, false)]
        [TestCase(11, 12, false)]
        [TestCase(12, 11, false)]
        [TestCase(11, 11, true)]
        [TestCase(10, 11, true)]
        [TestCase(9, 9, true)]
        [TestCase(9, 10, true)]
        public void CanAttackUnit_Horseman(int x, int y, bool canAttack)
        {
            const int startPositionX = 10;
            const int startPositionY = 10;

            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var horseman = new Horseman(player1, startPositionX, startPositionY);
            var target = new Archer(player2, x, y);
            var map = CreateMap(units: new GameObject[] { horseman, target });
            var gameController = new GameController(map);

            Assert.AreEqual(canAttack, gameController.CanAttackUnit(horseman, target));
        }

        /// <summary>
        /// Проверить, что мечник может атаковать врага.
        /// </summary>
        [Test]
        [TestCase(8, 9, false)]
        [TestCase(9, 8, false)]
        [TestCase(11, 12, false)]
        [TestCase(12, 11, false)]
        [TestCase(11, 11, true)]
        [TestCase(10, 11, true)]
        [TestCase(9, 9, true)]
        [TestCase(9, 10, true)]
        public void CanAttackUnit_Swordsman(int x, int y, bool canAttack)
        {
            const int startPositionX = 10;
            const int startPositionY = 10;

            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var swordsman = new Swordsman(player1, startPositionX, startPositionY);
            var target = new Archer(player2, x, y);
            var map = CreateMap(units: new GameObject[] { swordsman, target });
            var gameController = new GameController(map);

            Assert.AreEqual(canAttack, gameController.CanAttackUnit(swordsman, target));
        }

        /// <summary>
        /// Проверить, что невозможна атака дружественного юнита.
        /// </summary>
        [Test]
        public void CanAttackUnit_ArcherAttackFriend_False()
        {
            var player = new Player(1, "Игрок №1", null);
            var archer = new Archer(player, 10, 10);
            var target = new Archer(player, 11, 11);
            var map = CreateMap(units: new[] { archer, target });
            var gameController = new GameController(map);

            Assert.False(gameController.CanAttackUnit(archer, target));
        }

        #endregion

        #region AttackUnit

        /// <summary>
        /// Проверить дальнюю атаку лучника.
        /// </summary>
        [Test]
        public void AttackUnit_ArcherAttackAllTypes()
        {
            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var archer = new Archer(player1, 8, 8);
            var map = CreateMap();
            var gameController = new GameController(map);


            // Лучник имеет 50 жизней. Погибнет за один удар.
            var archerTarget = new Archer(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, archer, archerTarget));

            // Катапульта имеет 75 жизней. Погибнет за два удара.
            var catapultTarget = new Catapult(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, archer, catapultTarget));

            // Всадник имеет 200 жизней. Необходимо 4 удара.
            var horsemanTarget = new Horseman(player2, 10, 10);
            Assert.AreEqual(4, GetAttacksCount(gameController, archer, horsemanTarget));

            // Мечник имеет 100 жизней. Погибнет за два удара.
            var swordsmanTarget = new Swordsman(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, archer, swordsmanTarget));
        }

        /// <summary>
        /// Проверить дальнюю атаку катапульты.
        /// </summary>
        [Test]
        public void AttackUnit_CatapultAttackAllTypes()
        {
            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var catapult = new Catapult(player1, 8, 8);
            var map = CreateMap();
            var gameController = new GameController(map);


            // Лучник имеет 50 жизней. Погибнет за один удар.
            var archerTarget = new Archer(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, catapult, archerTarget));

            // Катапульта имеет 75 жизней. Погибнет за один удар.
            var catapultTarget = new Catapult(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, catapult, catapultTarget));

            // Всадник имеет 200 жизней. Необходимо 2 удара.
            var horsemanTarget = new Horseman(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, catapult, horsemanTarget));

            // Мечник имеет 100 жизней. Погибнет за один удар.
            var swordsmanTarget = new Swordsman(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, catapult, swordsmanTarget));
        }

        /// <summary>
        /// Проверить атаку всадника.
        /// </summary>
        [Test]
        public void AttackUnit_HorsemanAttackAllTypes()
        {
            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var horseman = new Horseman(player1, 9, 9);
            var map = CreateMap();
            var gameController = new GameController(map);


            // Лучник имеет 50 жизней. Погибнет за один удар.
            var archerTarget = new Archer(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, horseman, archerTarget));

            // Катапульта имеет 75 жизней. Погибнет за один удар.
            var catapultTarget = new Catapult(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, horseman, catapultTarget));

            // Всадник имеет 200 жизней. Необходимо 3 удара.
            var horsemanTarget = new Horseman(player2, 10, 10);
            Assert.AreEqual(3, GetAttacksCount(gameController, horseman, horsemanTarget));

            // Мечник имеет 100 жизней. Погибнет за два удара.
            var swordsmanTarget = new Swordsman(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, horseman, swordsmanTarget));
        }

        /// <summary>
        /// Проверить атаку мечника.
        /// </summary>
        [Test]
        public void AttackUnit_SwordsmanAttackAllTypes()
        {
            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var swordsman = new Swordsman(player1, 9, 9);
            var map = CreateMap();
            var gameController = new GameController(map);


            // Лучник имеет 50 жизней. Погибнет за один удар.
            var archerTarget = new Archer(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, swordsman, archerTarget));

            // Катапульта имеет 75 жизней. Погибнет за два удара.
            var catapultTarget = new Catapult(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, swordsman, catapultTarget));

            // Всадник имеет 200 жизней. Необходимо 4 удара.
            var horsemanTarget = new Horseman(player2, 10, 10);
            Assert.AreEqual(4, GetAttacksCount(gameController, swordsman, horsemanTarget));

            // Мечник имеет 100 жизней. Погибнет за два удара.
            var swordsmanTarget = new Swordsman(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, swordsman, swordsmanTarget));
        }

        /// <summary>
        /// Проверить ближнюю атаку лучника.
        /// </summary>
        [Test]
        public void AttackUnit_ArcherAttackCloseCombatAllTypes()
        {
            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var archer = new Archer(player1, 9, 9);
            var map = CreateMap();
            var gameController = new GameController(map);


            // Лучник имеет 50 жизней. Погибнет за два удара.
            var archerTarget = new Archer(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, archer, archerTarget));

            // Катапульта имеет 75 жизней. Погибнет за три удара.
            var catapultTarget = new Catapult(player2, 10, 10);
            Assert.AreEqual(3, GetAttacksCount(gameController, archer, catapultTarget));

            // Всадник имеет 200 жизней. Необходимо 8 ударов.
            var horsemanTarget = new Horseman(player2, 10, 10);
            Assert.AreEqual(8, GetAttacksCount(gameController, archer, horsemanTarget));

            // Мечник имеет 100 жизней. Погибнет за 4 удара.
            var swordsmanTarget = new Swordsman(player2, 10, 10);
            Assert.AreEqual(4, GetAttacksCount(gameController, archer, swordsmanTarget));
        }

        /// <summary>
        /// Проверить ближнюю атаку катапульты.
        /// </summary>
        [Test]
        public void AttackUnit_CatapultAttackCloseCombatAllTypes()
        {
            var player1 = new Player(1, "Игрок №1", null);
            var player2 = new Player(2, "Игрок №2", null);
            var catapult = new Catapult(player1, 9, 9);
            var map = CreateMap();
            var gameController = new GameController(map);


            // Лучник имеет 50 жизней. Погибнет за один удар.
            var archerTarget = new Archer(player2, 10, 10);
            Assert.AreEqual(1, GetAttacksCount(gameController, catapult, archerTarget));

            // Катапульта имеет 75 жизней. Погибнет за два удара.
            var catapultTarget = new Catapult(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, catapult, catapultTarget));

            // Всадник имеет 200 жизней. Необходимо 4 удара.
            var horsemanTarget = new Horseman(player2, 10, 10);
            Assert.AreEqual(4, GetAttacksCount(gameController, catapult, horsemanTarget));

            // Мечник имеет 100 жизней. Погибнет за два удара.
            var swordsmanTarget = new Swordsman(player2, 10, 10);
            Assert.AreEqual(2, GetAttacksCount(gameController, catapult, swordsmanTarget));
        }

        /// <summary>
        /// Рассчитать количество ударов, которое необходимо, чтобы убить юнита.
        /// </summary>
        /// <param name="gameController">Контроллер игры.</param>
        /// <param name="attackerUnit">Юнит, который наносит удар.</param>
        /// <param name="targetUnit">Юнит, который является целью.</param>
        /// <returns>Количество ударов, которое было нанесено юниту.</returns>
        /// <remarks>
        /// Проверка не точная. Считается какое количество ударов нужно, чтобы убить противника.
        /// Смерть считается по тому, что больше нельзя атаковать. В общем случае, такая проверка работоспособна.
        /// </remarks>
        private static int GetAttacksCount(GameController gameController, Unit attackerUnit, Unit targetUnit)
        {
            var count = 0;
            while (gameController.CanAttackUnit(attackerUnit, targetUnit))
            {
                gameController.AttackUnit(attackerUnit, targetUnit);
                ++count;
            }

            return count;
        }

        #endregion

        #region GetObjectSource

        /// <summary>
        /// Проверить корректность получения изображения юнита.
        /// </summary>
        [Test]
        public void GetObjectSource_AllTypes()
        {
            var player = new Player(1, "Игрок №1", null);
            var map = CreateMap();
            var gameController = new GameController(map);

            Assert.NotNull(gameController.GetObjectSource(new Archer(player, 0, 0)));
            Assert.NotNull(gameController.GetObjectSource(new Catapult(player, 0, 0)));
            Assert.NotNull(gameController.GetObjectSource(new Horseman(player, 0, 0)));
            Assert.NotNull(gameController.GetObjectSource(new Swordsman(player, 0, 0)));

            Assert.NotNull(gameController.GetObjectSource(new Grass(0, 0)));
            Assert.NotNull(gameController.GetObjectSource(new Water(0, 0)));
        }

        #endregion

        /// <summary>
        /// Создать карту.
        /// </summary>
        /// <param name="ground">Информация о местности.</param>
        /// <param name="units">Список юнитов.</param>
        private static Map CreateMap(IReadOnlyList<GameObject> ground = null, IReadOnlyList<GameObject> units = null)
        {
            return new Map(ground ?? new GameObject[0], units ?? new GameObject[0]);
        }
    }
}