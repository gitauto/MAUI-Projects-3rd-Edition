namespace DoToo.Repositories;

using DoToo.Models;
using SQLite;

public class TodoItemRepository : ITodoItemRepository
{
	private SQLiteAsyncConnection _connection;
    public event EventHandler<TodoItem> OnItemAdded; 
    public event EventHandler<TodoItem> OnItemUpdated;

    public async Task<List<TodoItem>> GetItemsAsync()
    {
        await CreateConnectionAsync();
        return await _connection.Table<TodoItem>().ToListAsync();
    }

    public async Task AddItemAsync(TodoItem item)
    {
        await CreateConnectionAsync();
        await _connection.InsertAsync(item); 
        OnItemAdded?.Invoke(this, item);
    }

    public async Task UpdateItemAsync(TodoItem item)
    {
        await CreateConnectionAsync();
        await _connection.UpdateAsync(item); 
        OnItemUpdated?.Invoke(this, item);
    }

    public async Task AddOrUpdateAsync(TodoItem item)
    {
        if (item.Id == 0)
        {
            await AddItemAsync(item);
        }
        else
        {
            await UpdateItemAsync(item);
        }
    }
    
    private async Task CreateConnectionAsync()
    {
        if (_connection != null) { return; }

        var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var databasePath = Path.Combine(documentPath, "TodoItems.db");

        _connection = new SQLiteAsyncConnection(databasePath); 
        await _connection.CreateTableAsync<TodoItem>();

        if (await _connection.Table<TodoItem>().CountAsync() == 0)
        {
            await _connection.InsertAsync(new TodoItem()
            {
                Title = "Welcome to DoToo",
                Due = DateTime.Now
            });
        }
    }
}
