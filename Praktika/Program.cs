using Praktika.DAL;
using Praktika_menu;

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
                    Console.WriteLine("1 - добавить команду.\n2 - вывести всех команд.\n3 - удалить команду.\n4 - изменить данные команды.\n5 - найти команду.\n6 - выбрать по параметрам.\n0 - выход.");
                    Console.Write("Введите: ");
                    string inp = Console.ReadLine();

                    if (int.TryParse(inp, out choice))
                    {
                        if (choice >= 0 && choice <= 6)
                            break;
                        else
                            Console.WriteLine("Ошибка! Введите число от 0 до 6.");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка ввода! Введите число.");
                    }
                }

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
                }
            }
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