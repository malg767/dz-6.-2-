using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Praktika.DAL;
using Praktika;

namespace Praktika_menu
{
    public class Menu
    {
        public Menu() { }

        public void PrintTeam(Teams t)
        {
            if (t == null)
            {
                Console.WriteLine("Команда не найдена.");
                return;
            }

            Console.WriteLine($"Id: {t.Id}, Название: {t.Name}, Город: {t.City}, Побед: {t.Win}, Поражений: {t.Lose}, Ничьих: {t.Draw},Забито: {t.Goals_scored}, Пропущено:{t.Goals_missed}");
        }

        public void ReadKey()
        {
            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }

        public string EnterString(string text)
        {
            while (true)
            {
                Console.Write(text);
                string input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.WriteLine("Ошибка! Поле не может быть пустым.");
            }
        }

        public int EnterInt(string text)
        {
            while (true)
            {
                Console.Write(text);
                if (int.TryParse(Console.ReadLine(), out int num))
                    return num;

                Console.WriteLine("Ошибка! Введите число.");
            }
        }

        public void Menu_ShowAllTeams(PrRepository repo)
        {
            Console.Clear();
            var teams = repo.GetAll();

            if (teams.Count == 0)
                Console.WriteLine("Команд нет в базе.");
            else
            {
                Console.WriteLine("Список всех команд:");
                foreach (var t in teams)
                    PrintTeam(t);
            }
            ReadKey();
        }

        public void Menu_AddTeam(PrRepository repo)
        {
            Console.Clear();
            string name = EnterString("Введите название команды: ");
            string city = EnterString("Введите город: ");

            if (repo.GetAll().Any(t => t.Name == name && t.City == city))
            {
                Console.WriteLine("Такая команда уже существует.");
                ReadKey();
                return;
            }

            int win = EnterInt("Введите количество побед: ");
            int lose = EnterInt("Введите количество поражений: ");
            int draw = EnterInt("Введите количество ничьих: ");
            int goalsScored = EnterInt("Введите количество забитых голов: ");
            int goalsMissed = EnterInt("Введите количество пропущенных голов: ");

            repo.Add(new Teams
            {
                Name = name,
                City = city,
                Win = win,
                Lose = lose,
                Draw = draw,
                Goals_scored = goalsScored,
                Goals_missed = goalsMissed
            });

            Console.WriteLine("Команда успешно добавлена!");
            ReadKey();
        }
        public void Menu_UpdateTeam(PrRepository repo)
        {
            Console.Clear();
            string name = EnterString("Введите название команды: ");
            string city = EnterString("Введите город: ");

            var team = repo.GetAll().FirstOrDefault(t => t.Name == name && t.City == city);

            if (team == null)
            {
                Console.WriteLine("Команда не найдена.");
                ReadKey();
                return;
            }

            Console.WriteLine("Текущие данные:");
            PrintTeam(team);

            Console.WriteLine("\nЧто хотите изменить?");
            Console.WriteLine("1 - Название\n2 - Город\n3 - Победы\n4 - Поражения\n5 - Ничьи\n6 - Забитые голы\n7 - Пропущенные голы\n0 - Выход");
            int choice = EnterInt("Введите: ");

            UpdateTeamSwitch(team, choice);
            repo.Update(team);

            Console.WriteLine("Команда обновлена!");
            PrintTeam(team);
            ReadKey();
        }

        private void UpdateTeamSwitch(Teams team, int choice)
        {
            switch (choice)
            {
                case 1: team.Name = EnterString("Новое название: "); break;
                case 2: team.City = EnterString("Новый город: "); break;
                case 3: team.Win = EnterInt("Новые победы: "); break;
                case 4: team.Lose = EnterInt("Новые поражения: "); break;
                case 5: team.Draw = EnterInt("Новые ничьи: "); break;
                case 6: team.Goals_scored = EnterInt("Новые забитые голы: "); break;
                case 7: team.Goals_missed = EnterInt("Новые пропущенные голы: "); break;
                case 0: return;
                default: Console.WriteLine("Ошибка!"); break;
            }
        }

        public void Menu_DeleteTeam(PrRepository repo)
        {
            Console.Clear();
            string name = EnterString("Введите название команды: ");
            string city = EnterString("Введите город: ");

            var team = repo.GetAll().FirstOrDefault(t => t.Name == name && t.City == city);

            if (team == null)
            {
                Console.WriteLine("Команда не найдена.");
                ReadKey();
                return;
            }

            PrintTeam(team);
            Console.Write("Удалить? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                repo.Remove(team.Id);
                Console.WriteLine("Команда удалена!");
            }
            else
                Console.WriteLine("Удаление отменено.");

            ReadKey();
        }

        public void Menu_SearchTeam(PrRepository repo)
        {
            Console.Clear();
            Console.WriteLine("Поиск по:\n1 - Названию\n2 - Городу\n3 - Названию + Городу\n0 - Выход");
            int choice = EnterInt("Введите: ");
            SearchTeamSwitch(repo.GetAll(), choice);
            ReadKey();
        }

        private void SearchTeamSwitch(List<Teams> teams, int choice)
        {
            switch (choice)
            {
                case 1:
                    var name = EnterString("Название: ");
                    teams.Where(t => t.Name == name).ToList().ForEach(PrintTeam);
                    break;
                case 2:
                    var city = EnterString("Город: ");
                    teams.Where(t => t.City == city).ToList().ForEach(PrintTeam);
                    break;
                case 3:
                    name = EnterString("Название: ");
                    city = EnterString("Город: ");
                    teams.Where(t => t.Name == name && t.City == city).ToList().ForEach(PrintTeam);
                    break;
                case 0: return;
                default: Console.WriteLine("Ошибка!"); break;
            }
        }
        public void Menu_SelectTeam(PrRepository repo)
        {
            Console.Clear();
            Console.WriteLine("1 - max Побед\n2 - max Поражений\n3 - max Ничьих\n4 - max Забитых\n5 - max Пропущенных\n0 - Выход");
            int choice = EnterInt("Введите: ");
            SelectTeamSwitch(repo.GetAll(), choice);
            ReadKey();
        }

        private void SelectTeamSwitch(List<Teams> teams, int choice)
        {
            Teams result = null;
            switch (choice)
            {
                case 1: result = teams.OrderByDescending(t => t.Win).FirstOrDefault(); break;
                case 2: result = teams.OrderByDescending(t => t.Lose).FirstOrDefault(); break;
                case 3: result = teams.OrderByDescending(t => t.Draw).FirstOrDefault(); break;
                case 4: result = teams.OrderByDescending(t => t.Goals_scored).FirstOrDefault(); break;
                case 5: result = teams.OrderByDescending(t => t.Goals_missed).FirstOrDefault(); break;
                case 0: return;
                default: Console.WriteLine("Ошибка!"); return;
            }
            Console.WriteLine("Результат:");
            PrintTeam(result);
        }
    }
}
