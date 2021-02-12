using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICpEP_Night_Results_Application_2020
{
    public class Vote
    {
        public int ID { get; set; }
        public int VoteMale { get; set; }
        public int VoteFemale { get; set; }

        public Vote(int id, int voteMale, int voteFemale)
        {
            ID = id;
            VoteMale = voteMale;
            VoteFemale = voteFemale;
        }
    }
}
