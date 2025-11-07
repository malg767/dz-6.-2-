
namespace Praktika.DAL
{
    public class Teams
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
        public int Draw { get; set; }
        public int Goals_scored { get; set; }
        public int Goals_missed { get; set; }
    }
}
