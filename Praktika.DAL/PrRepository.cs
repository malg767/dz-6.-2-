using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Praktika.DAL
{
    public class PrRepository
    {
        private readonly AppDbContext dbContext;

        public PrRepository()
        {
            dbContext = new AppDbContext();
        }

        public void Add(Teams team)
        {
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();
        }

        public List<Teams> GetAll()
        {
            return dbContext.Teams.ToList();
        }

        public Teams GetById(int id)
        {
            return dbContext.Teams.FirstOrDefault(t => t.Id == id);
        }

        public void Update(Teams team)
        {
            dbContext.Teams.Update(team);
            dbContext.SaveChanges();
        }

        public bool Remove(int id)
        {
            var team = GetById(id);
            if (team != null)
            {
                dbContext.Teams.Remove(team);
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public void RemoveRange(List<Teams> teams)
        {
            dbContext.Teams.RemoveRange(teams);
            dbContext.SaveChanges();
        }
    }
}
