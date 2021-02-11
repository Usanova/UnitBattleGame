using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Strategy.Domain.Models;

namespace Strategy.Domain
{
    /// <summary>
    /// Контроллер хода игры.
    /// </summary>
    public class GameController
    {
        private readonly Map _map;

        /// <inheritdoc />
        public GameController(Map map)
        {
            _map = map;
        }


        /// <summary>
        /// Получить координаты объекта.
        /// </summary>
        /// <param name="o">Координаты объекта, которые необходимо получить.</param>
        /// <returns>Координата x, координата y.</returns>
        public Coordinates GetObjectCoordinates(GameObject gameObject)
        {
            return new Coordinates(gameObject.X, gameObject.Y);
        }

        /// <summary>
        /// Может ли юнит передвинуться в указанную клетку.
        /// </summary>
        /// <param name="u">Юнит.</param>
        /// <param name="x">Координата X клетки.</param>
        /// <param name="y">Координата Y клетки.</param>
        /// <returns>
        /// <see langvalue="true" />, если юнит может переместиться
        /// <see langvalue="false" /> - иначе.
        /// </returns>
        public bool CanMoveUnit(Unit unit, int x, int y)
        {
            if (!unit.CanMove(x, y))
                return false;


            var gameObjectType = _map.GetGameObjectTypeByCoordinates(x, y);
            if (gameObjectType == GameObjectType.Water || gameObjectType == GameObjectType.Unit)
                return false;

            return true;
        }

        /// <summary>
        /// Передвинуть юнита в указанную клетку.
        /// </summary>
        /// <param name="u">Юнит.</param>
        /// <param name="x">Координата X клетки.</param>
        /// <param name="y">Координата Y клетки.</param>
        public void MoveUnit(Unit unit, int x, int y)
        {
            unit.Move(x, y);
        }

        /// <summary>
        /// Проверить, может ли один юнит атаковать другого.
        /// </summary>
        /// <param name="au">Юнит, который собирается совершить атаку.</param>
        /// <param name="tu">Юнит, который является целью.</param>
        /// <returns>
        /// <see langvalue="true" />, если атака возможна
        /// <see langvalue="false" /> - иначе.
        /// </returns>
        public bool CanAttackUnit(Unit attackingUnit, Unit targetUnit)
        {
            if (targetUnit.IsDead || attackingUnit.Player == targetUnit.Player)
                return false;

            return attackingUnit.CanAttackUnit(targetUnit);
        }

        /// <summary>
        /// Атаковать указанного юнита.
        /// </summary>
        /// <param name="au">Юнит, который собирается совершить атаку.</param>
        /// <param name="tu">Юнит, который является целью.</param>
        public void AttackUnit(Unit attackingUnit, Unit targetUnit)
        {
            if (!CanAttackUnit(attackingUnit, targetUnit))
                return;

            var attackPower = attackingUnit.GetAttackPower(targetUnit);
            targetUnit.ReduceHealth(attackPower);
        }

        /// <summary>
        /// Получить изображение объекта.
        /// </summary>
        public ImageSource GetObjectSource(GameObject gameObject) => gameObject.GetSourceFrom();
    }
}