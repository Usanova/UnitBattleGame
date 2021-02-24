using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using ReactiveUI.Fody.Helpers;
using Strategy.Domain.Models;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Strategy.Domain;

namespace UnitBattleGame.ViewModels
{
    public sealed class GameDeskViewModel : ViewModelBase
    {
        public GameDeskViewModel()
        {
            var ground = new List<GameObject>();
            var units = new List<GameObject>();

            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    if (i == 9 || i == 10 || (j != 9 && j != 10))
                        AddToGamesAndList(new Grass(x: j, y: i) , ground);

            for(int i = 0; i < 20; i++)
                for(int j = 9; j <= 10; j++)
                    if(i != 9 && i != 10)
                        AddToGamesAndList(new Water(x: j, y: i), ground);

            var firstPlayer = new Player(1, "Nastya", null);
            var secondPlayer = new Player(1, "Nadeha", null);

            AddToGamesAndList(new Archer(firstPlayer, x: 7, y: 2), units);
            AddToGamesAndList(new Catapult(firstPlayer, x: 7, y: 6), units);
            AddToGamesAndList(new Horseman(firstPlayer, x: 7, y: 13), units);
            AddToGamesAndList(new Swordsman(firstPlayer, x: 7, y: 17), units);


            AddToGamesAndList(new Archer(secondPlayer, x: 12, y: 2), units);
            AddToGamesAndList(new Catapult(secondPlayer, x: 12, y: 6), units);
            AddToGamesAndList(new Horseman(secondPlayer, x: 12, y: 13), units);
            AddToGamesAndList(new Swordsman(secondPlayer, x: 12, y: 17), units);

            gameController = new GameController(new Map(ground, units), firstPlayer, secondPlayer);
            gameController.PropertyChanged += (s, e) => 
            { 
                this.RaisePropertyChanged(e.PropertyName);
                if (e.PropertyName == "FirstSelectedGameObject" || e.PropertyName == "SecondSelectedGameObject"
                    || e.PropertyName == "State")
                {
                    this.RaisePropertyChanged("CanStartMove");
                    this.RaisePropertyChanged("CanMove");
                    this.RaisePropertyChanged("CanAttack");
                    this.RaisePropertyChanged("CanCancel");
                } 
                else if (e.PropertyName == "Damage")
                {
                    this.RaisePropertyChanged("ExistDamage");
                }
            };
        }

        private void AddToGamesAndList(GameObject gameObject, List<GameObject> list)
        {
            TheGame.Add(gameObject);
            list.Add(gameObject);
        }

        GameController gameController;

        [Reactive] public ObservableCollection<GameObject> TheGame { get; set; } = new ObservableCollection<GameObject>();

        public Player MovingPlayer => gameController.MovingPlayer;

        public Unit PointedUnit => gameController.PointedUnit;

        public DelegateCommand<GameObject> StartShowInformationAboutUnit => new DelegateCommand<GameObject>(gameObject =>
        {
            if (!(gameObject is Unit))
                return;

            gameController.StartPointingUnit((Unit)gameObject);
        });

        public DelegateCommand<GameObject> EndShowInformationAboutUnit => new DelegateCommand<GameObject>(gameObject =>
        {
            if (!(gameObject is Unit))
                return;

            gameController.EndPointingUnit((Unit)gameObject);
        });


        public GameState State => gameController.State;
        public Unit FirstSelectedGameObject => gameController.FirstSelectedGameObject;
        public GameObject SecondSelectedGameObject => gameController.SecondSelectedGameObject;
        public bool CanStartMove => FirstSelectedGameObject != null && State == GameState.FindUnitForMove;
        public bool CanMove => SecondSelectedGameObject != null && State == GameState.FindCellForMove;
        public bool CanAttack => SecondSelectedGameObject != null && State == GameState.FindUnitForAttack;
        public bool CanCancel => FirstSelectedGameObject != null;

        public DelegateCommand<GameObject> ChageSelected => new DelegateCommand<GameObject>(gameObject =>
        {
            gameController.ChangeSelected(gameObject);
        });
        public DelegateCommand StartMoving => new DelegateCommand(() =>
        {
            gameController.StartMoving();
        });

        public DelegateCommand StartAttacking => new DelegateCommand(() =>
        {
            gameController.StartAttacking();
        });

        public DelegateCommand Move => new DelegateCommand(() =>
        {
            if (!gameController.CanMoveUnit(FirstSelectedGameObject, SecondSelectedGameObject.X, SecondSelectedGameObject.Y))
            {
                Error("Юнит не может перейти туда");
                return;
            }

            gameController.MoveUnit(FirstSelectedGameObject, SecondSelectedGameObject.X, SecondSelectedGameObject.Y);
        });

        public DelegateCommand Attack => new DelegateCommand(() =>
        {
            if (!gameController.CanAttackUnit(FirstSelectedGameObject, (Unit)SecondSelectedGameObject))
            {
                Error("Юнит не может аттаковать этого юнита");
                return;
            }

            gameController.AttackUnit(FirstSelectedGameObject, (Unit)SecondSelectedGameObject);
        });

        public DelegateCommand Cancel => new DelegateCommand(() =>
        {
            gameController.CancelMove();
        });

        public int? Damage => gameController.Damage;

        public bool ExistDamage => Damage != null;
    }
}