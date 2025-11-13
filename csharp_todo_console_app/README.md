# ✅ C# Todo Console App

A clean **C# .NET 6 console application** that implements a small **todo list manager** with:

- Persistent storage using a local `todos.json` file
- A simple, menu-driven console UI
- Basic colored output for a nicer developer experience

This is perfect as a **desktop/console app project** to showcase on GitHub.

---

## ✨ Features

- Add a new todo item with a title
- List todos with:
  - ID
  - Title
  - Created timestamp
  - Completion status
- Toggle a todo as completed / not completed
- Delete a todo by ID
- Clear all completed todos
- Data is saved to `todos.json` in the working directory using `System.Text.Json`

---

## 🧠 Tech Stack

- **Language**: C#
- **Runtime**: .NET 6
- Uses:
  - `System.Text.Json` for JSON serialization
  - LINQ for simple list operations
  - Console-based UI / input loop

---

## ▶️ How to Run

Prerequisite: **.NET 6 SDK** installed.  
You can check with:

```bash
dotnet --version
```

If needed, download from the official .NET site.

---

### 1. Restore and build

From the project folder:

```bash
dotnet build
```

### 2. Run the app

```bash
dotnet run
```

You’ll see a menu like:

```text
=========================================
        C# Todo Console Application
=========================================

Current items:

  (No todos yet. Add one!)

-----------------------------------------
Options:
  1 - Add todo
  2 - Toggle completed
  3 - Delete todo
  4 - Clear completed
  Q - Quit
Select an option:
```

---

## 💾 Data Persistence

Todos are saved in a local file:

```text
todos.json
```

This makes it easy to:

- Inspect / commit sample data
- Demonstrate basic **file I/O** and serialization

---

## 📂 Project Structure

```text
csharp_todo_console_app/
├─ Program.cs       # All logic: model, repository, app loop
└─ TodoConsoleApp.csproj  # .NET project file
```

You can freely rename the folder and project for your GitHub repo.

---

## 🌟 Talking Points for Your GitHub

This project shows that you can:

- Build a **C# console application** with .NET
- Implement a simple **domain model** (`TodoItem`)
- Build a **repository layer** that persists to JSON
- Use **LINQ** and collections to manipulate in-memory data
- Design a user-friendly, loop-based CLI

You can extend it later with:

- Editing todo titles
- Filtering only completed / only active items
- Due dates and priority levels
- Exporting todos to CSV
