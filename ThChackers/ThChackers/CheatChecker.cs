using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThChackers
{
    public class CheatChecker : Checker
    {
        public bool IsCheat { get; set; } = true;

        public CheatChecker(int playerId, bool IsCheat) : base(playerId)
        {
            this.IsCheat = IsCheat;
        }
    }
}