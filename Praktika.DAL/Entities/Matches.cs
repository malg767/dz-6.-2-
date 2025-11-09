using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktika.DAL.Entities
{
    public class Match
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int OpponentId { get; set; }
        public Team Opponent { get; set; }

        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public DateTime MatchDate { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
    }
}
