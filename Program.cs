// =============================================
//   TASK MANAGER API
//   Built step by step for beginners
// =============================================

var builder = WebApplication.CreateBuilder(args);

// Add OpenAPI support (lets us test the API in the browser)
builder.Services.AddOpenApi();

// Allow the frontend (HTML file) to talk to this API
// Without this, the browser blocks requests from a different origin
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Only show the API docs page when running locally
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable CORS (must be before other middleware)
app.UseCors();

// Redirect http:// to https:// automatically
app.UseHttpsRedirection();


// =============================================
//   IN-MEMORY STORAGE
//   A simple list that holds all tasks.
//   (Data is lost when the app restarts — good for learning!)
// =============================================

var tasks = new List<TaskItem>
{
    // Some starter tasks so the API isn't empty
    new TaskItem { Id = 1, Title = "Learn .NET",      IsCompleted = true,  CreatedAt = DateTime.Now },
    new TaskItem { Id = 2, Title = "Build an API",    IsCompleted = false, CreatedAt = DateTime.Now },
    new TaskItem { Id = 3, Title = "Build a website", IsCompleted = false, CreatedAt = DateTime.Now },
};

// We use this to auto-generate the next Id (like 4, 5, 6...)
int nextId = 4;


// =============================================
//   ENDPOINTS
//   An endpoint is a URL your API responds to.
//   Format: METHOD /url => what to do
// =============================================


// --- GET /tasks ---
// Returns ALL tasks in the list
app.MapGet("/tasks", () =>
{
    return Results.Ok(tasks);
})
.WithName("GetAllTasks");


// --- GET /tasks/{id} ---
// Returns ONE task by its Id
// Example: GET /tasks/2  => returns the "Build an API" task
app.MapGet("/tasks/{id}", (int id) =>
{
    // Find the task where the Id matches
    var task = tasks.FirstOrDefault(t => t.Id == id);

    // If not found, return 404 Not Found
    if (task is null)
        return Results.NotFound($"Task with Id {id} was not found.");

    // If found, return 200 OK with the task
    return Results.Ok(task);
})
.WithName("GetTaskById");


// --- POST /tasks ---
// Creates a NEW task
// The user sends a title in the request body, we create the task
app.MapPost("/tasks", (CreateTaskRequest request) =>
{
    // Basic validation — title cannot be empty
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Title cannot be empty.");

    // Build the new task
    var newTask = new TaskItem
    {
        Id          = nextId++,          // auto-increment the Id
        Title       = request.Title,
        IsCompleted = false,             // new tasks always start as not completed
        CreatedAt   = DateTime.Now
    };

    // Add it to our list
    tasks.Add(newTask);

    // Return 201 Created, with the new task and its URL
    return Results.Created($"/tasks/{newTask.Id}", newTask);
})
.WithName("CreateTask");


// --- PUT /tasks/{id} ---
// Updates an EXISTING task (change title or mark complete)
app.MapPut("/tasks/{id}", (int id, UpdateTaskRequest request) =>
{
    // Find the task to update
    var task = tasks.FirstOrDefault(t => t.Id == id);

    if (task is null)
        return Results.NotFound($"Task with Id {id} was not found.");

    // Apply the updates
    task.Title       = request.Title;
    task.IsCompleted = request.IsCompleted;

    // Return 200 OK with the updated task
    return Results.Ok(task);
})
.WithName("UpdateTask");


// --- DELETE /tasks/{id} ---
// Deletes a task permanently
app.MapDelete("/tasks/{id}", (int id) =>
{
    // Find the task to delete
    var task = tasks.FirstOrDefault(t => t.Id == id);

    if (task is null)
        return Results.NotFound($"Task with Id {id} was not found.");

    // Remove it from the list
    tasks.Remove(task);

    // Return 204 No Content (success, but nothing to return)
    return Results.NoContent();
})
.WithName("DeleteTask");


// Start the web server and listen for requests
app.Run();


// =============================================
//   MODELS
//   These define the shape of our data.
// =============================================

// Represents a Task stored in the list
class TaskItem
{
    public int      Id          { get; set; }
    public string   Title       { get; set; } = string.Empty;
    public bool     IsCompleted { get; set; }
    public DateTime CreatedAt   { get; set; }
}

// The data the user sends when CREATING a task (only needs a title)
record CreateTaskRequest(string Title);

// The data the user sends when UPDATING a task
record UpdateTaskRequest(string Title, bool IsCompleted);
