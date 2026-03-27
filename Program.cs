using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TodoConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "C# Todo Console App";
            var repo = new TodoRepository("todos.json");
            var app = new TodoApp(repo);
            app.Run();
        }
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            var status = IsDone ? "[x]" : "[ ]";
            return $"{status} ({Id}) {Title}  - created {CreatedAt:g}";
        }
    }

    public class TodoRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public TodoRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<TodoItem> Load()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TodoItem>();
            }

            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<TodoItem>();
            }

            try
            {
                var items = JsonSerializer.Deserialize<List<TodoItem>>(json, _options);
                return items ?? new List<TodoItem>();
            }
            catch
            {
                // If the file is corrupted, start fresh.
                return new List<TodoItem>();
            }
        }

        public void Save(List<TodoItem> items)
        {
            var json = JsonSerializer.Serialize(items, _options);
            File.WriteAllText(_filePath, json);
        }
    }

    public class TodoApp
    {
        private readonly TodoRepository _repo;
        private List<TodoItem> _items;
        private bool _running = true;

        public TodoApp(TodoRepository repo)
        {
            _repo = repo;
            _items = _repo.Load();
        }

        public void Run()
        {
            while (_running)
            {
                DrawUi();
                Console.Write("\nSelect an option: ");
                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1":
                        AddTodo();
                        break;
                    case "2":
                        ToggleTodo();
                        break;
                    case "3":
                        DeleteTodo();
                        break;
                    case "4":
                        ClearCompleted();
                        break;
                    case "q":
                    case "Q":
                        Quit();
                        break;
                    default:
                        Console.WriteLine("Unknown option. Press any key to continue...");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        private void DrawUi()
        {
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine("        C# Todo Console Application      ");
            Console.WriteLine("=========================================\n");
            Console.WriteLine("Current items:\n");

            if (_items.Count == 0)
            {
                Console.WriteLine("  (No todos yet. Add one!)");
            }
            else
            {
                foreach (var item in _items.OrderBy(i => i.IsDone).ThenBy(i => i.Id))
                {
                    WriteTodoLine(item);
                }
            }

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("Options:");
            Console.WriteLine("  1 - Add todo");
            Console.WriteLine("  2 - Toggle completed");
            Console.WriteLine("  3 - Delete todo");
            Console.WriteLine("  4 - Clear completed");
            Console.WriteLine("  Q - Quit");
        }

        private void WriteTodoLine(TodoItem item)
        {
            var originalColor = Console.ForegroundColor;
            if (item.IsDone)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }

            Console.WriteLine("  " + item.ToString());
            Console.ForegroundColor = originalColor;
        }

        private void AddTodo()
        {
            Console.Write("\nEnter title for new todo: ");
            var title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            var nextId = _items.Any() ? _items.Max(i => i.Id) + 1 : 1;
            var item = new TodoItem
            {
                Id = nextId,
                Title = title.Trim(),
                IsDone = false,
                CreatedAt = DateTime.Now
            };

            _items.Add(item);
            _repo.Save(_items);
        }

        private void ToggleTodo()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("\nNo todos to toggle. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            Console.Write("\nEnter id to toggle: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid id. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("No todo with that id. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            item.IsDone = !item.IsDone;
            _repo.Save(_items);
        }

        private void DeleteTodo()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("\nNo todos to delete. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            Console.Write("\nEnter id to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid id. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                Console.WriteLine("No todo with that id. Press any key to continue...");
                Console.ReadKey(true);
                return;
            }

            _items.Remove(item);
            _repo.Save(_items);
        }

        private void ClearCompleted()
        {
            var before = _items.Count;
            _items = _items.Where(i => !i.IsDone).ToList();
            var removed = before - _items.Count;
            _repo.Save(_items);
            Console.WriteLine($"\nRemoved {removed} completed todos. Press any key to continue...");
            Console.ReadKey(true);
        }

        private void Quit()
        {
            _running = false;
            Console.WriteLine("\nGoodbye!");
        }
    }
}
