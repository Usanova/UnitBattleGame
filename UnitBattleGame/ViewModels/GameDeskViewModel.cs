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
using System.IO;

namespace UnitBattleGame.ViewModels
{
    public sealed class GameDeskViewModel : ViewModelBase
    {
        public int FieldSize { get; set; }

        public GameDeskViewModel()
        {
            var ground = new List<GameObject>();
            var units = new List<GameObject>();
            var firstPlayer = new Player(1, "Nastya", null);
            var secondPlayer = new Player(1, "Nadeha", null);

            var sr = new StreamReader("InitField1.txt", Encoding.Default);

            int rowNumber = 0;
            while(!sr.EndOfStream)
            {
                var gameObjectsString = sr.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for(int columnNumber = 0; columnNumber < gameObjectsString.Length; columnNumber++)
                {
                    var currentPlayer = firstPlayer;
                    if(columnNumber > gameObjectsString.Length/2)
                        currentPlayer = secondPlayer;

                    if(gameObjectsString[columnNumber] != "1")
                        ground.Add(new Grass(x: columnNumber, y: rowNumber));

                    switch (gameObjectsString[columnNumber])
                    {
                        case "1":
                            ground.Add(new Water(x: columnNumber, y: rowNumber));
                            break;
                        case "2":
                            units.Add(new Archer(currentPlayer, x: columnNumber, y: rowNumber));
                            break;
                        case "3":
                            units.Add(new Catapult(currentPlayer, x: columnNumber, y: rowNumber));
                            break;
                        case "4":
                            units.Add(new Horseman(currentPlayer, x: columnNumber, y: rowNumber));
                            break;
                        case "5":
                            units.Add(new Swordsman(currentPlayer, x: columnNumber, y: rowNumber));
                            break;
                    }
                }
                rowNumber++;
            }

            sr.Close();

            FieldSize = rowNumber;

            foreach (var groundItem in ground)
                TheGame.Add(groundItem);

            foreach (var unit in units)
                TheGame.Add(unit);


            gameController = new GameController(new Map(ground, units, FieldSize), firstPlayer, secondPlayer);
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