using Praktika.DAL.Entities;
using System.Text.RegularExpressions;

namespace Praktika.DAL.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
        public int Draw { get; set; }
        public int Goals_scored { get; set; }
        public int Goals_missed { get; set; }

        public List<Player> Players { get; set; }
        public List<Match> Matches{ get; set; }
    } 
}
