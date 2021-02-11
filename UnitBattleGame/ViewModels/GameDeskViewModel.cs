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

namespace UnitBattleGame.ViewModels
{
    public sealed class GameDeskViewModel : ReactiveObject
    {
        public GameDeskViewModel()
        {
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    if(i == 9 || i == 10 || (j != 9 && j != 10))
                        TheGame.Add(new Grass { X = j, Y = i });

            for(int i = 0; i < 20; i++)
                for(int j = 9; j <= 10; j++)
                    if(i != 9 && i != 10)
                        TheGame.Add(new Water { X = j, Y = i });

            var firstPlayer = new Player(1, "Nastya", null);
            var secondPlayer = new Player(1, "Nadeha", null);

            TheGame.Add(new Archer(firstPlayer) { X = 7, Y = 2 });
            TheGame.Add(new Catapult(firstPlayer) { X = 7, Y = 6 });
            TheGame.Add(new Horseman(firstPlayer) { X = 7, Y = 13 });
            TheGame.Add(new Swordsman(firstPlayer) { X = 7, Y = 17 });

            TheGame.Add(new Archer(secondPlayer) { X = 12, Y = 2 });
            TheGame.Add(new Catapult(secondPlayer) { X = 12, Y = 6 });
            TheGame.Add(new Horseman(secondPlayer) { X = 12, Y = 13 });
            TheGame.Add(new Swordsman(secondPlayer) { X = 12, Y = 17 });
        }

        [Reactive]
        public ObservableCollection<GameObject> TheGame { get; set; } = new ObservableCollection<GameObject>();

        [Reactive] public Unit PointetUnit { get; set; }


        public DelegateCommand<GameObject> Move => new DelegateCommand<GameObject>(gameObject =>
        {
            if (gameObject.Type != GameObjectType.Unit)
                return;

            gameObject.ChageSelected();
        });

        public DelegateCommand<GameObject> StartShowInformationAboutUnit => new DelegateCommand<GameObject>(gameObject =>
        {
            if (gameObject.Type != GameObjectType.Unit)
                return;

            PointetUnit = (Unit)gameObject;
        });

        public DelegateCommand<GameObject> EndShowInformationAboutUnit => new DelegateCommand<GameObject>(gameObject =>
        {
            PointetUnit = null;
        });
    }
}