using Praktika.DAL;
using Praktika.DAL.Entities;
using Praktika_menu;
using System;

namespace Praktika
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ");
            var rep = new PrRepository();
            Menu menu = new Menu();


            while (true)
            {
                int choice;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1 - добавить команду.\n2 - вывести всех команд.\n3 - удалить команду.\n4 - изменить данные команды.\n5 - найти команду.\n6 - выбрать по параметрам.\n7 - функции с матчами и игроками. \n8 - Добавить/Удалить/Изменить матч \n9 - Статистика бомбардиров \n10 - Статистика команд по голам \n0 - выход.");
                    Console.Write("Введите: ");
                    string inp = Console.ReadLine();

                    if (int.TryParse(inp, out choice))
                    {
                        if (choice >= 0 && choice <= 10)
                            break;
                        else
                            Console.WriteLine("Ошибка! Введите число от 0 до 10.");
                            menu.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода! Введите число.");
                        menu.ReadKey();
                    }
                }
                Console.Clear();
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        AddTeam(rep, menu);
                        break;
                    case 2:
                        ShowAllTeams(rep, menu);
                        break;
                    case 3:
                        DeleteTeam(rep, menu);
                        break;
                    case 4:
                        UpdateTeam(rep, menu);
                        break;
                    case 5:
                        SearchTeam(rep, menu);
                        break;
                    case 6:
                        SelectTeam(rep, menu);
                        break;
                    case 7:
                        Console.Clear();
                        MatchFunctions(rep, menu);
                        break;
                    case 8:
                        Console.Clear();
                        Add_Upd_Del_Match(rep, menu);
                        break;
                    case 9:
                        Console.Clear();
                        TopScorers(rep, menu);
                        break;
                    case 10:
                        Console.Clear();
                        TopTeams(rep, menu);
                        break;
                }
            }
        }
        static void TopTeams(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_TopTeams(rep);
        }

        static void TopScorers(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_TopScorers(rep);
        }

        static void Add_Upd_Del_Match(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_Add_Upd_Del_Match(rep);
        }

        static void MatchFunctions(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_MatchFunctions(rep);
        }

        static void AddTeam(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_AddTeam(rep);
        }
        static void ShowAllTeams(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_ShowAllTeams(rep);
        }
        static void DeleteTeam(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_DeleteTeam(rep);
        }
        static void UpdateTeam(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_UpdateTeam(rep);
        }
        static void SearchTeam(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_SearchTeam(rep);
        }
        static void SelectTeam(PrRepository rep, Menu menu)
        {
            Console.Clear();
            menu.Menu_SelectTeam(rep);
        }
    }
}