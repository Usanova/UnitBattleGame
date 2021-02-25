using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Strategy.Domain.Models;

namespace Strategy.Domain
{
    /// <summary>
    /// Контроллер хода игры.
    /// </summary>
    public class GameController : ReactiveObject
    {
        private readonly Map _map;

        private Player firstPlayer;

        private Player secondPlayer;

        /// <inheritdoc />
        public GameController(Map map, Player firstPlayer, Player secondPlayer) : this(map)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            MovingPlayer = firstPlayer;
        }

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

            var gameObject = _map.GetGameObjectByCoordinates(x, y);
            if (gameObject != null && !gameObject.Earth)
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

            EndMove();
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
            Damage = Math.Max(-attackPower, -targetUnit.Health);

            targetUnit.ReduceHealth(attackPower);

            EndMove();
        }

        [Reactive] public Unit PointedUnit { get; private set; }

        public void StartPointingUnit(Unit unit)
        {
            if (unit.IsDead)
                return;

            PointedUnit = unit;

            if (State != GameState.FindUnitForMove || unit.Player != MovingPlayer)
                return;

            _map.SelectMovingArea(PointedUnit);
        }

        public void EndPointingUnit(Unit unit)
        {
            if (unit != PointedUnit)
                return;

            _map.UnselectMovingArea(PointedUnit);

            PointedUnit = null;
        }

        [Reactive] public GameState State { get; set; } = GameState.FindUnitForMove;

        [Reactive] public Unit FirstSelectedGameObject { get; set; }
        private void ChangeFirstSelectedGameObject(Unit unit)
            => FirstSelectedGameObject = ChangeSelectedGameObject<Unit>(unit, FirstSelectedGameObject);

        [Reactive] public GameObject SecondSelectedGameObject { get; set; }
        private void ChangeSecondSelectedGameObject(GameObject gameObject)
            => SecondSelectedGameObject = ChangeSelectedGameObject<GameObject>(gameObject, SecondSelectedGameObject);

        [Reactive] public int? Damage { get; set; }

        private T ChangeSelectedGameObject<T>(T gameObject, GameObject selectedGameObject) where T : GameObject
        {
            if (gameObject == selectedGameObject)
            {
                selectedGameObject?.IsSelected(false);
                return null;
            }

            selectedGameObject?.IsSelected(false);

            gameObject.IsSelected(true);
            return gameObject;
        }

        private void StartMove(Unit unit)
        {
            ChangeFirstSelectedGameObject(unit);

            Damage = null;
        }

        public void ChangeSelected(GameObject gameObject)
        {
            switch(State)
            {
                case GameState.FindUnitForMove:
                    if (!Unit.TryParse(gameObject, out var unit) || unit.IsDead || unit.Player != MovingPlayer)
                        return;

                    StartMove(unit);

                    break;

                case GameState.FindCellForMove:
                    if (gameObject == FirstSelectedGameObject)
                        return;

                    ChangeSecondSelectedGameObject(gameObject);

                    break;

                case GameState.FindUnitForAttack:
                    if (!Unit.TryParse(gameObject, out unit) || unit.IsDead || unit.Player == MovingPlayer)
                        return;

                    ChangeSecondSelectedGameObject(gameObject);

                    break;
            }
        }

        public void StartMoving() => State = GameState.FindCellForMove;

        public void StartAttacking() => State = GameState.FindUnitForAttack;

        [Reactive] public Player MovingPlayer { get; set; }

        public void ChangeMovingPlayer()
        {
            if (MovingPlayer == firstPlayer)
            {
                MovingPlayer = secondPlayer;
                return;
            }

            MovingPlayer = firstPlayer;
        }

        public void EndMove()
        {
            CancelMove();

            ChangeMovingPlayer();
        }

        public void CancelMove()
        {
            State = GameState.FindUnitForMove;

            FirstSelectedGameObject?.IsSelected(false);
            SecondSelectedGameObject?.IsSelected(false);

            FirstSelectedGameObject = null;
            SecondSelectedGameObject = null;
        }

        public ImageSource GetObjectSource(GameObject gameObject) => gameObject.SourceFrom;
    }

    public enum GameState
    {
        FindUnitForMove,
        FindCellForMove,
        FindUnitForAttack
    }
}