# Task Manager API

A simple Task Manager built with **ASP.NET Core** (.NET 10) — my first .NET project.  
Includes a REST API and a plain HTML/JS frontend with full CRUD support.

---

## What it does

- Add, edit, and delete tasks
- Mark tasks as completed
- Filter tasks by All / Active / Completed
- Live stats (total, completed, remaining)

---

## Tech Stack

| Part     | Tech                        |
|----------|-----------------------------|
| Backend  | ASP.NET Core Minimal API    |
| Storage  | In-memory (no database yet) |
| Frontend | Plain HTML + CSS + JavaScript |

---

## How to Run

**1. Clone the repo**
```bash
git clone https://github.com/Nepul1234/dotnet-task-manager-api.git
cd dotnet-task-manager-api
```

**2. Start the API**
```bash
dotnet run
```

You will see something like:
```
Now listening on: http://localhost:5218
```

**3. Open the frontend**

Open `index.html` in your browser (just double-click it).

> If your port is different from `5218`, update this line in `index.html`:
> ```js
> const API = 'http://localhost:5218';
> ```

---

## API Endpoints

| Method | URL           | Description       |
|--------|---------------|-------------------|
| GET    | `/tasks`      | Get all tasks     |
| GET    | `/tasks/{id}` | Get one task      |
| POST   | `/tasks`      | Create a new task |
| PUT    | `/tasks/{id}` | Update a task     |
| DELETE | `/tasks/{id}` | Delete a task     |

---

## Project Structure

```
MyFirstApi/
├── Program.cs          # API code — all endpoints live here
├── index.html          # Frontend UI
├── appsettings.json    # App configuration
└── MyFirstApi.csproj   # Project file (like package.json)
```
