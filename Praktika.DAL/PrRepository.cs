using Microsoft.EntityFrameworkCore;
using Praktika.DAL.Entities;
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

        public void Add(Team team)
        {
            dbContext.Teams.Add(team);
            dbContext.SaveChanges();
        }

        public List<Team> GetAll()
        {
            return dbContext.Teams.ToList();
        }

        public Team GetById(int id)
        {
            return dbContext.Teams.FirstOrDefault(t => t.Id == id);
        }

        public void Update(Team team)
        {
            dbContext.Teams.Update(team);
            dbContext.SaveChanges();
        }

        public void RemoveRange(List<Team> teams)
        {
            dbContext.Teams.RemoveRange(teams);
            dbContext.SaveChanges();
        }

        public void Remove(int id)
        {
            var team = dbContext.Teams.FirstOrDefault(t => t.Id == id);
            if (team != null)
            {
                dbContext.Teams.Remove(team);
                dbContext.SaveChanges();
            }
        }

        public List<Match> GetAllMatches()
        {
            return dbContext.Matches
                .Include(m => m.Team)      
                .Include(m => m.Opponent)  
                .Include(m => m.Players)   
                .ToList();
        }

        public void AddMatch(Match match)
        {
            dbContext.Matches.Add(match);
            dbContext.SaveChanges();
        }

        public void UpdateMatch(Match match)
        {
            dbContext.Matches.Update(match);
            dbContext.SaveChanges();
        }

        public void RemoveMatch(int id)
        {
            var match = dbContext.Matches.FirstOrDefault(m => m.Id == id);
            if (match != null)
            {
                dbContext.Matches.Remove(match);
                dbContext.SaveChanges();
            }
        }

        public void AddPlayer(Player player)
        {
            dbContext.Players.Add(player);
            dbContext.SaveChanges();
        }

        public List<Player> GetAllPlayers()
        {
            return dbContext.Players.Include(p => p.Team).ToList();
        }

        public void UpdatePlayer(Player player)
        {
            dbContext.Players.Update(player);
            dbContext.SaveChanges();
        }

        public bool RemovePlayer(int id)
        {
            var player = dbContext.Players.FirstOrDefault(p => p.Id == id);
            if (player != null)
            {
                dbContext.Players.Remove(player);
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Match> MatchRandom(int numberOfMatches)
        {
            var addedMatches = new List<Match>();

            if (dbContext.Players.Count() == 0)
            {
                Console.WriteLine("Нет игроков в базе! Добавьте игроков перед генерацией матчей.");
                return addedMatches;
            }

            var teams = dbContext.Teams.ToList();
            if (teams.Count < 2)
            {
                Console.WriteLine("Недостаточно команд для генерации матчей.");
                return addedMatches;
            }

            Random rnd = new Random();
            for (int i = 0; i < numberOfMatches; i++)
            {
                var team1 = teams[rnd.Next(teams.Count)];
                Team team2;
                do
                {
                    team2 = teams[rnd.Next(teams.Count)];
                } while (team2.Id == team1.Id);

                var start = new DateTime(DateTime.Now.Year, 1, 1);
                var end = new DateTime(DateTime.Now.Year, 12, 31);
                var randomDate = start.AddDays(rnd.Next((end - start).Days));

                int goalsScored = rnd.Next(0, 6);
                int goalsConceded = rnd.Next(0, 6);

                var team1Players = dbContext.Players.Where(p => p.TeamId == team1.Id).ToList();
                var team1Scorers = new List<Player>();
                if (team1Players.Any() && goalsScored > 0)
                {
                    for (int g = 0; g < goalsScored; g++)
                    {
                        var scorer = team1Players[rnd.Next(team1Players.Count)];
                        team1Scorers.Add(scorer);
                    }
                }

                var team2Players = dbContext.Players.Where(p => p.TeamId == team2.Id).ToList();
                var team2Scorers = new List<Player>();
                if (team2Players.Any() && goalsConceded > 0)
                {
                    for (int g = 0; g < goalsConceded; g++)
                    {
                        var scorer = team2Players[rnd.Next(team2Players.Count)];
                        team2Scorers.Add(scorer);
                    }
                }

                var allScorers = new List<Player>();
                allScorers.AddRange(team1Scorers);
                allScorers.AddRange(team2Scorers);

                var match = new Match
                {
                    TeamId = team1.Id,
                    Team = team1,
                    Opponent = team2,
                    GoalsScored = goalsScored,
                    GoalsConceded = goalsConceded,
                    MatchDate = randomDate,
                    Players = allScorers
                };

                dbContext.Matches.Add(match);
                addedMatches.Add(match);
            }

            dbContext.SaveChanges();
            Console.WriteLine($"{numberOfMatches} матчей успешно добавлено случайным образом!");
            return addedMatches;
        }

        public List<(Player player, int goals)> Top3_ScorersByTeam(string teamName)
        {
            var matches = GetAllMatches().Where(m => m.Team.Name == teamName).ToList();
            return CountGoals(matches)
                   .OrderByDescending(s => s.goals)
                   .Take(3)
                   .ToList();
        }

        public List<(Player player, int goals)> Top1_ScorersByTeam(string teamName)
        {
            var matches = GetAllMatches().Where(m => m.Team.Name == teamName).ToList();
            return CountGoals(matches)
                   .OrderByDescending(s => s.goals)
                   .Take(1)
                   .ToList();
        }

        public List<(Player player, int goals)> Top3_ScorersByAllTeams()
        {
            var matches = GetAllMatches().ToList();
            return CountGoals(matches)
                   .OrderByDescending(s => s.goals)
                   .Take(3)
                   .ToList();

        }

        public List<(Player player, int goals)> Top1_ScorersByAllTeams()
        {
            var matches = GetAllMatches().ToList();
            return CountGoals(matches)
                   .OrderByDescending(s => s.goals)
                   .Take(1)
                   .ToList();

        }

        private List<(Player player, int goals)> CountGoals(List<Match> matches)
        {
            var scorers = new List<(Player player, int goals)>();

            foreach (var match in matches)
            {
                foreach (var player in match.Players)
                {
                    var index = scorers.FindIndex(s => s.player.Id == player.Id);
                    if (index >= 0)
                    {
                        scorers[index] = (player, scorers[index].goals + 1);
                    }
                    else
                    {
                        scorers.Add((player, 1));
                    }
                }
            }

            return scorers;
        }

        public List<Team> Top3_TeamsByGoalsScored()
        {
            return GetAll()
                .OrderByDescending(t => t.Goals_scored)
                .Take(3)
                .ToList();
        }

        public Team Top1_TeamByGoalsScored()
        {
            return GetAll()
                .OrderByDescending(t => t.Goals_scored)
                .FirstOrDefault();
        }

        public List<Team> Top3_TeamsByGoalsConceded()
        {
            return GetAll()
                .OrderBy(t => t.Goals_missed)  
                .Take(3)
                .ToList();
        }

        public Team Top1_TeamByGoalsConceded()
        {
            return GetAll()
                .OrderBy(t => t.Goals_missed)
                .FirstOrDefault();
        }

        public List<(Team Team, int Points)> TeamsWithPoints()
        {
            var teams = GetAll();
            var result = new List<(Team, int)>();

            foreach (var t in teams)
            {
                int points = t.Win * 3 + t.Draw;
                result.Add((t, points));
            }

            return result;
        }
    }
}
