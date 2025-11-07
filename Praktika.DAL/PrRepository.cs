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

        //Добавить команду
        public void Add(Teams team)
        {
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();
        }

        //Получить все команды
        public List<Teams> GetAll()
        {
            return dbContext.Teams.ToList();
        }

        //Найти по Id
        public Teams GetById(int id)
        {
            return dbContext.Teams.FirstOrDefault(t => t.Id == id);
        }

        // Обновить команду
        public void Update(Teams team)
        {
            dbContext.Teams.Update(team);
            dbContext.SaveChanges();
        }

        //Удалить по Id
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

        //Удалить несколько
        public void RemoveRange(List<Teams> teams)
        {
            dbContext.Teams.RemoveRange(teams);
            dbContext.SaveChanges();
        }
    }
}
