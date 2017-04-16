using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idle_Game_UWP.Classes
{
    class Upgrade
    {
        public string Name { get; private set; }
        public float Cost { get; set; }
        public int Power { get; set; }
        public int Count { get; set; }

        public Upgrade(string Name, int Cost, int Count, int Power)
        {
            this.Name = Name;
            this.Cost = Cost;
            this.Power = Power;
            this.Count = Count;
        }
    }
}
