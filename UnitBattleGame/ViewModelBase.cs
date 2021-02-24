using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitBattleGame
{
    public class ViewModelBase : ReactiveObject
    {
        public Action<string> Error { get; set; }

        public Action<string> Warning { get; set; }
    }
}
