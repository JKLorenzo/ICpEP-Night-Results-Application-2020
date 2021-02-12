using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICpEP_Night_Results_Application_2020
{
    class Data
    {
        public int ID { get; set; }
        public int Vote { get; set; }

        public Data(int ID, int Vote)
        {
            this.ID = ID;
            this.Vote = Vote;
        }
    }
}
