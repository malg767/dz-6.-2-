using Microsoft.IdentityModel.Tokens;
using Praktika;
using Praktika.DAL;
using Praktika.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Praktika_menu
{
    public class Menu
    {
        public Menu() { }

        //Модуль 6 Часть 4
        public void Menu_TopTeams(PrRepository repo)
        {
            while (true)
            {
                Console.WriteLine("1 - Топ-3 команды по забитым голам");
                Console.WriteLine("2 - Команда с наибольшим количеством забитых голов");
                Console.WriteLine("3 - Топ-3 команды по наименьшему количеству пропущенных голов");
                Console.WriteLine("4 - Команда с наименьшим количеством пропущенных голов");
                Console.WriteLine("0 - Выход");
                int choice = EnterInt("Введите пункт: ");
                if (choice == 0) return;
                TopTeams_Swich(repo, choice);
            }
            
        }
        public void TopTeams_Swich(PrRepository repo, int choice)
        {
            switch (choice)
            {
                case 1:
                    Console.Clear();
                    var top3Scored = repo.Top3_TeamsByGoalsScored();
                    PrintTeams(top3Scored, "Топ-3 команды по забитым голам:");
                    ReadKey();
                    break;

                case 2:
                    Console.Clear();
                    var top1Scored = repo.Top1_TeamByGoalsScored();
                    PrintTeams(top1Scored != null ? new List<Team> { top1Scored } : new List<Team>(),
                               "Команда с наибольшим количеством забитых голов:");
                    ReadKey();
                    break;

                case 3:
                    Console.Clear();
                    var top3Conceded = repo.Top3_TeamsByGoalsConceded();
                    PrintTeams(top3Conceded, "Топ-3 команды по наименьшему количеству пропущенных голов:");
                    ReadKey();
                    break;

                case 4:
                    Console.Clear();
                    var top1Conceded = repo.Top1_TeamByGoalsConceded();
                    PrintTeams(top1Conceded != null ? new List<Team> { top1Conceded } : new List<Team>(),
                               "Команда с наименьшим количеством пропущенных голов:");
                    ReadKey();
                    break;

                default:
                    Console.WriteLine("Ошибка выбора!");
                    ReadKey();
                    break;
            }
        }
        public void PrintTeams(List<Team> teams, string message)
        {
            if (teams.Count == 0)
                Console.WriteLine("Команд не найдено.");
            else
            {
                Console.WriteLine(message);
                foreach (var t in teams)
                {
                    Console.WriteLine($"{t.Name} ({t.City}) - Забито: {t.Goals_scored}, Пропущено: {t.Goals_missed}");
                }
            }
        }
        public void Menu_TopScorers(PrRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - Топ-3 бомбардиры команды");
                Console.WriteLine("2 - Лучший бомбардир команды");
                Console.WriteLine("3 - Топ-3 бомбардиры чемпионата");
                Console.WriteLine("4 - Лучший бомбардир чемпионата");
                Console.WriteLine("0 - Выход");
                int choice = EnterInt("Введите пункт: ");
                if (choice == 0) return;
                TopScorers_Swich(repo,choice);
            }
            
        }
        public void TopScorers_Swich(PrRepository repo,int choice)
        {
            Console.Clear();

            string teamName = null;
            if (choice == 1 || choice == 2)
            {
                teamName = EnterString("Введите название команды: ");
                if (!CheckTeamExists(repo, teamName)) return;
            }
            switch (choice)
            {                
                case 1:               
                    PrintTopScorers(repo.Top3_ScorersByTeam(teamName), $"Топ-3 бомбардиров команды {teamName}:");
                    break;

                case 2:
                    PrintTopScorers(repo.Top1_ScorersByTeam(teamName), $"Лучший бомбардир команды {teamName}:");
                    break;

                case 3:
                    PrintTopScorers(repo.Top3_ScorersByAllTeams(), "Топ-3 бомбардиров всего чемпионата:");
                    break;

                case 4:
                    PrintTopScorers(repo.Top1_ScorersByAllTeams(), "Лучший бомбардир чемпионата:");
                    break;

                default:
                    Console.WriteLine("Ошибка выбора!");
                    break;
            }
            ReadKey();
        }
        public void PrintTopScorers(List<(Player player, int goals)> scorers, string message)
        {
            if (scorers.Count == 0)
            {
                Console.WriteLine("Голов не найдено.");
            }
            else
            {
                Console.WriteLine(message);
                foreach (var s in scorers)
                {
                    if (s.player.Team != null)
                        Console.WriteLine($"{s.player.FullName} ({s.player.Team.Name}) - {s.goals} голов");
                    else
                        Console.WriteLine($"{s.player.FullName} - {s.goals} голов");
                }
            }
        }
        public bool CheckTeamExists(PrRepository repo, string teamName)
        {
            var teamExists = repo.GetAll().Any(t => t.Name == teamName);
            if (!teamExists)
            {
                Console.WriteLine($"Команда '{teamName}' не найдена в базе.");
                ReadKey();
                return false;
            }
            return true;
        }
        public void Menu_TopPoints(PrRepository repo)
        {
            Console.Clear();
            Console.WriteLine("1 - Топ-3 команды по набранным очкам");
            Console.WriteLine("2 - Команда с наибольшим количеством очков");
            Console.WriteLine("3 - Топ-3 команды с наименьшим количеством очков");
            Console.WriteLine("4 - Команда с наименьшим количеством очков");

            int choice = EnterInt("Введите пункт: ");

            var teamsWithPoints = repo.TeamsWithPoints();
            Console.Clear();
            switch (choice)
            {
                case 1:                  
                    PrintTeams(teamsWithPoints.OrderByDescending(t => t.Points).Take(3).Select(t => t.Team).ToList(), "Топ-3 команды по набранным очкам:");
                    break;

                case 2:
                    PrintTeams(teamsWithPoints.OrderByDescending(t => t.Points).Take(1).Select(t => t.Team).ToList(), "Команда с наибольшим количеством очков:");
                    break;

                case 3:
                    PrintTeams(teamsWithPoints.OrderBy(t => t.Points).Take(3).Select(t => t.Team).ToList(), "Топ-3 команды с наименьшим количеством очков:");
                    break;

                case 4:
                    PrintTeams(teamsWithPoints.OrderBy(t => t.Points).Take(1).Select(t => t.Team).ToList(), "Команда с наименьшим количеством очков:");
                    break;

                default:
                    Console.WriteLine("Ошибка выбора!");
                    break;
            }
            ReadKey();
        }


        //------------------------------------------------------------------ 
        // Модуль 6 Часть 3
        public void Menu_MatchFunctions(PrRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ДОПОЛНИТЕЛЬНЫЕ ФУНКЦИИ:");
                Console.WriteLine("1 - Разница забитых и пропущенных голов для каждой команды");
                Console.WriteLine("2 - Полная информация о всех матчах");
                Console.WriteLine("3 - Матчи в конкретную дату");
                Console.WriteLine("4 - Матчи конкретной команды");
                Console.WriteLine("5 - Игроки, забившие голы в конкретную дату");
                Console.WriteLine("6 - Добавить матчи случайным образом");
                Console.WriteLine("0 - Назад");

                int choice = EnterInt("Введите номер: ");
                if (choice == 0) { return; }
                Menu_MatchFunctionsSwitch(repo, choice);              
            }
        }
        private void Menu_MatchFunctionsSwitch(PrRepository repo, int choice)
        {
            Console.Clear();
            List<Match> matches = null;
            switch (choice)
            {
                case 1:
                    
                    var teams = repo.GetAll();

                    if (teams.Count == 0)
                    {
                        Console.WriteLine("Команд нет в базе.");
                        ReadKey();
                        return;
                    }

                    Console.WriteLine("Разница забитых и пропущенных голов:");
                    foreach (var t in teams)
                    {
                        int diff = t.Goals_scored - t.Goals_missed;
                        Console.WriteLine($"{t.Name} ({t.City}) -> Разница: {diff}");
                    }
                    break;
                case 2:
                    matches = repo.GetAllMatches();

                    if (matches.Count == 0)
                    {
                        Console.WriteLine("Матчей нет в базе.");
                    }
                    else
                    {
                        foreach (var m in matches)
                        {
                            PrintMatches(m);
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine("Введите дату для поиска матчей (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        matches = repo.GetAllMatches().Where(m => m.MatchDate.Date == date.Date).ToList();

                        if (matches.Count == 0)
                        {
                            Console.WriteLine("Матчей на эту дату нет.");
                        }
                        else
                        {
                            foreach (var m in matches)
                            {
                                Console.WriteLine($"Команда: {m.Team.Name}, Соперник: {m.Opponent.Name}, Забито: {m.GoalsScored}, Пропущено: {m.GoalsConceded}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат даты.");
                    }
                    break;
                case 4:
                    string teamName = EnterString("Введите название команды: ");
                    matches = repo.GetAllMatches().Where(m => m.Team.Name == teamName || m.Opponent.Name == teamName).ToList();

                    if (matches.Count == 0)
                    {
                        Console.WriteLine("Матчей для этой команды нет.");
                    }
                    else
                    {
                        foreach (var m in matches)
                        {
                            PrintMatches(m);
                        }
                    }
                    break;
                case 5:
                    Console.WriteLine("Введите дату для поиска матчей (yyyy-MM-dd): ");

                    string inputDate = Console.ReadLine();
                    if (DateTime.TryParse(inputDate, out DateTime date_1))
                    {
                        matches = repo.GetAllMatches().Where(m => m.MatchDate.Date == date_1.Date).ToList();

                        if (matches.Count == 0)
                        {
                            Console.WriteLine("Матчей на эту дату нет.");
                        }
                        else
                        {
                            List<string> players = new List<string>();

                            foreach (var match in matches)
                            {
                                if (match.Players != null && match.Players.Count > 0)
                                {
                                    foreach (var p in match.Players)
                                    {
                                        if (!players.Contains(p.FullName))
                                            players.Add(p.FullName);
                                    }
                                }
                            }

                            if (players.Count == 0)
                                Console.WriteLine("Голы в этот день не забиты.");
                            else
                            {
                                Console.WriteLine("Игроки, забившие голы в этот день:");
                                foreach (var player in players)
                                    Console.WriteLine(player);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат даты.");
                    }

                    
                    break;
                case 6:
                    Console.Clear();
                    int count = EnterInt("Сколько матчей добавить случайным образом? ");
                    matches = repo.MatchRandom(count);
                    foreach (var match in matches)
                    {
                        UpdateTeamStatsForMatch(match, true); 
                        repo.Update(match.Team);
                        repo.Update(match.Opponent);
                    }
                    break;
                default: Console.WriteLine("Ошибка выбора!"); ReadKey(); break;
            }
            ReadKey();
        }
        private void PrintMatches(Match m)
        {
            Console.WriteLine($"Матч ID: {m.Id}");
            Console.WriteLine($"{m.Team.Name} vs {m.Opponent.Name}");
            Console.WriteLine($"Счёт: {m.GoalsScored} : {m.GoalsConceded}");
            Console.WriteLine($"Дата: {m.MatchDate:dd.MM.yyyy}");

            PrintTeamScorers(m.Team, m.GoalsScored, m.Players);
            PrintTeamScorers(m.Opponent, m.GoalsConceded, m.Players);

            Console.WriteLine("-----------------------------------------");
        }
        private void PrintTeamScorers(Team team, int goals, List<Player> players)
        {
            var scorers = players?.Where(p => p.TeamId == team.Id).ToList() ?? new List<Player>();
            Console.WriteLine($"{team.Name} ({goals}):");

            if (scorers.Count == 0)
                Console.WriteLine("  — никто не забил");
            else
                scorers.ForEach(p => Console.WriteLine($"  - {p.FullName}"));
        }

        public void Menu_Add_Upd_Del_Match(PrRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите что хотите сделать:");
                Console.WriteLine("1 - Добавить матч");
                Console.WriteLine("2 - Изменить матч");
                Console.WriteLine("3 - Удалить матч");
                Console.WriteLine("0 - Выход");                
                int choice = EnterInt("Введите номер: ");
                if (choice == 0) return;
                Add_Upd_Del_Match_Switch(repo, choice);
            }
        }
        public void Add_Upd_Del_Match_Switch(PrRepository repo, int choice)
        {
            switch (choice)
            {
                case 1: AddMatch(repo); break;
                case 2: EditMatch(repo); break;
                case 3: DeleteMatch(repo); break;
                default: Console.WriteLine("Ошибка выбора!"); ReadKey(); break;
            }
        }

        public void AddMatch(PrRepository repo)
        {
            Console.Clear();
            Console.WriteLine("Добавление нового матча:");

            string teamName = EnterString("Введите название команды: ");
            var team = repo.GetAll().FirstOrDefault(t => t.Name == teamName);
            if (team == null)
            {
                Console.WriteLine("Такой команды нет в базе.");
                ReadKey();
                return;
            }

            string opponentName = EnterString("Введите название команды соперника: ");
            var opponent = repo.GetAll().FirstOrDefault(t => t.Name == opponentName);
            if (opponent == null)
            {
                Console.WriteLine("Команда-соперник не найдена.");
                ReadKey();
                return;
            }

            Console.Write("Введите дату матча (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime matchDate))
            {
                Console.WriteLine("Неверный формат даты.");
                ReadKey();
                return;
            }

            var existingMatch = repo.GetAllMatches().FirstOrDefault(m => m.TeamId == team.Id && m.Opponent.Id == opponent.Id && m.MatchDate.Date == matchDate.Date);
            if (existingMatch != null)
            {
                Console.WriteLine("Такой матч уже существует.");
                ReadKey();
                return;
            }

            int goalsScored = EnterInt($"Сколько голов забила команда {team.Name}? ");
            int goalsConceded = EnterInt($"Сколько голов забила команда {opponent.Name}? ");

            var playersTeam = EnterPlayersForGoals(repo, team, goalsScored);
            var playersOpponent = EnterPlayersForGoals(repo, opponent, goalsConceded);

            var allPlayers = new List<Player>();
            allPlayers.AddRange(playersTeam);
            allPlayers.AddRange(playersOpponent);

            var match = new Match
            {
                Team = team,
                TeamId = team.Id,
                Opponent = opponent,
                GoalsScored = goalsScored,
                GoalsConceded = goalsConceded,
                MatchDate = matchDate,
                Players = allPlayers
            };

            UpdateTeamStatsForMatch(match, true); 
            repo.AddMatch(match);

            Console.WriteLine("Матч успешно добавлен!");
            ReadKey();
        }
        public List<Player> EnterPlayersForGoals(PrRepository repo, Team team, int goals)
        {
            var result = new List<Player>();

            if (goals == 0)
            {
                Console.WriteLine($"Команда {team.Name} не забила голов.");
                return result;
            }

            var teamPlayers = repo.GetAllPlayers().Where(p => p.TeamId == team.Id).ToList();

            for (int i = 1; i <= goals; i++)
            {
                while (true)
                {
                    Console.Write($"Кто забил {i}-й гол за {team.Name}? ");
                    string playerName = Console.ReadLine().Trim();

                    var player = teamPlayers.FirstOrDefault(p => p.FullName == playerName);

                    if (player != null)
                    {
                        result.Add(player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Игрок '{playerName}' не найден в команде {team.Name}. Повторите ввод.");
                    }
                }
            }

            return result;
        }

        public Match FindMatch(PrRepository repo)
        {
            string teamName = EnterString("Введите название команды: ");
            var team = repo.GetAll().FirstOrDefault(t => t.Name.Equals(teamName, StringComparison.OrdinalIgnoreCase));
            if (team == null)
            {
                Console.WriteLine("Такой команды нет в базе.");
                ReadKey();
                return null;
            }

            string opponentName = EnterString("Введите название команды соперника: ");
            var opponent = repo.GetAll().FirstOrDefault(t => t.Name.Equals(opponentName, StringComparison.OrdinalIgnoreCase));
            if (opponent == null)
            {
                Console.WriteLine("Команда-соперник не найдена.");
                ReadKey();
                return null;
            }

            Console.Write("Введите дату матча (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime matchDate))
            {
                Console.WriteLine("Неверный формат даты.");
                ReadKey();
                return null;
            }

            var match = repo.GetAllMatches()
                .FirstOrDefault(m => m.Team.Id == team.Id && m.Opponent.Id == opponent.Id && m.MatchDate.Date == matchDate.Date);

            if (match == null)
            {
                Console.WriteLine("Матч не найден.");
                ReadKey();
                return null;
            }

            return match;
        }
        public void EditMatch(PrRepository repo)
        {
            Console.Clear();
            Console.WriteLine("Редактирование матча:");

            Team team, opponent;
            var match_f = FindMatch(repo);
            if (match_f == null) return;

            UpdateTeamStatsForMatch(match_f, false);

            Console.WriteLine("Что хотите изменить?");
            Console.WriteLine("1 - Соперник\n2 - Дата\n3 - Забито первой командой \n4 - Забито второй командой\n5 - Игроки забившие годы\n0 - Выход");
            int choice = EnterInt("Введите: ");

            switch (choice)
            {
                case 0: return;
                case 1:
                    string newOpponentName = EnterString("Новый соперник: ");
                    var newOpponent = repo.GetAll().FirstOrDefault(t => t.Name == newOpponentName);
                    if (newOpponent != null)
                    {
                        var oldOpponent = match_f.Opponent;
                        if (oldOpponent != null)
                        {
                            match_f.Players.RemoveAll(p => p.TeamId == oldOpponent.Id);
                        }

                        match_f.Opponent = newOpponent;

                        if (match_f.GoalsConceded > 0)
                        {
                            Console.WriteLine($"\nНужно заново указать, кто забил {match_f.GoalsConceded} голов за {newOpponent.Name}:");
                            var OpponentScorers = EnterPlayersForGoals(repo, newOpponent, match_f.GoalsConceded);

                            match_f.Players.AddRange(OpponentScorers);
                        }

                        Console.WriteLine($"Соперник изменён на {newOpponent.Name}, голы обновлены.");
                    }
                    else
                    {
                        Console.WriteLine("Такой команды нет, соперник не изменён.");
                    }
                    break;
                case 2:
                    Console.Write("Новая дата (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime newDate))
                        match_f.MatchDate = newDate;
                    break;
                case 3:
                    int newScored = EnterInt("Новые забитые голы первой командой: ");

                    if (newScored != match_f.GoalsScored)
                    {
                        Console.WriteLine("Количество голов изменилось! Нужно заново указать, кто забил.");

                        match_f.Players.Clear();

                        match_f.GoalsScored = newScored;

                        var newTeamScorerss = EnterPlayersForGoals(repo, match_f.Team, match_f.GoalsScored);
                        var newOpponentScorerss = EnterPlayersForGoals(repo, match_f.Opponent, match_f.GoalsConceded);

                        match_f.Players.AddRange(newTeamScorerss);
                        match_f.Players.AddRange(newOpponentScorerss);
                    }
                    break;
                case 4:
                    int newConceded = EnterInt("Новые пропущенные голы второй командой: ");
                    if (newConceded != match_f.GoalsConceded)
                    {
                        Console.WriteLine("Количество пропущенных голов изменилось! Нужно заново указать, кто забил у соперника.");

                        match_f.Players.Clear();

                        match_f.GoalsConceded = newConceded;

                        var newTeamScorerss_2 = EnterPlayersForGoals(repo, match_f.Team, match_f.GoalsScored);
                        var newOpponentScorerss_2 = EnterPlayersForGoals(repo, match_f.Opponent, match_f.GoalsConceded);

                        match_f.Players.AddRange(newTeamScorerss_2);
                        match_f.Players.AddRange(newOpponentScorerss_2);
                    }
                    break;
                case 5:
                    Console.WriteLine($"Изменение списка игроков, забивших голы в матче {match_f.Team.Name} vs {match_f.Opponent.Name}");

                    var newTeamScorers = EnterPlayersForGoals(repo, match_f.Team, match_f.GoalsScored);
                    var newOpponentScorers = EnterPlayersForGoals(repo, match_f.Opponent, match_f.GoalsConceded);

                    match_f.Players.Clear();
                    match_f.Players.AddRange(newTeamScorers);
                    match_f.Players.AddRange(newOpponentScorers);

                    Console.WriteLine("Игроки, забившие голы, успешно обновлены!");
                    break;
                default:
                    Console.WriteLine("Ошибка!");
                    break;
            }

            UpdateTeamStatsForMatch(match_f, true);
            repo.UpdateMatch(match_f);

            Console.WriteLine("Матч обновлён!");
            ReadKey();
        }
        public void DeleteMatch(PrRepository repo)
        {
            Console.Clear();
            Console.WriteLine("Удаление матча:");
                
            var match = FindMatch(repo);
            if (match == null) return;

            UpdateTeamStatsForMatch(match, false);
            repo.RemoveMatch(match.Id);

            Console.WriteLine("Матч удалён!");
            ReadKey();
        }      
        public void UpdateTeamStatsForMatch(Match match, bool isAdding)
        {
            int factor;

            if (isAdding)
            {
                factor = 1; 
            }
            else
            {
                factor = -1; 
            }

            if (match.GoalsScored > match.GoalsConceded)
            {
                match.Team.Win += 1 * factor;
                match.Opponent.Lose += 1 * factor;
            }
            else if (match.GoalsScored < match.GoalsConceded)
            {
                match.Team.Lose += 1 * factor;
                match.Opponent.Win += 1 * factor;
            }
            else
            {
                match.Team.Draw += 1 * factor;
                match.Opponent.Draw += 1 * factor;
            }

            match.Team.Goals_scored += match.GoalsScored * factor;
            match.Team.Goals_missed += match.GoalsConceded * factor;
            match.Opponent.Goals_scored += match.GoalsConceded * factor;
            match.Opponent.Goals_missed += match.GoalsScored * factor;
        }

        //----------------------------------------------------------------------
        // Модуль 6 Часть 2
        public void PrintTeam(Team t)
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

            repo.Add(new Team
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

                Console.WriteLine("Изменение даных команды");
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
        private void UpdateTeamSwitch(Team team, int choice)
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
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Поиск по:\n1 - Названию\n2 - Городу\n3 - Названию + Городу\n0 - Выход");
                int choice = EnterInt("Введите: ");
                if (choice == 0) return;
                SearchTeamSwitch(repo.GetAll(), choice);
                ReadKey();
            }
        }
        private void SearchTeamSwitch(List<Team> teams, int choice)
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
                    default: Console.WriteLine("Ошибка!"); break;
                }        
        }

        public void Menu_SelectTeam(PrRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - max Побед\n2 - max Поражений\n3 - max Ничьих\n4 - max Забитых\n5 - max Пропущенных\n0 - Выход");
                int choice = EnterInt("Введите: ");        
                if(choice == 0) return;
                SelectTeamSwitch(repo.GetAll(), choice);      
                ReadKey();
            }
        }
        private void SelectTeamSwitch(List<Team> teams, int choice)
        {           
                Team result = null;
                switch (choice)
                {
                    case 1: result = teams.OrderByDescending(t => t.Win).FirstOrDefault(); break;
                    case 2: result = teams.OrderByDescending(t => t.Lose).FirstOrDefault(); break;
                    case 3: result = teams.OrderByDescending(t => t.Draw).FirstOrDefault(); break;
                    case 4: result = teams.OrderByDescending(t => t.Goals_scored).FirstOrDefault(); break;
                    case 5: result = teams.OrderByDescending(t => t.Goals_missed).FirstOrDefault(); break;
                    default: Console.WriteLine("Ошибка!"); return;
                }
                Console.WriteLine("Результат:");
                PrintTeam(result);
            
        }
    }
}
