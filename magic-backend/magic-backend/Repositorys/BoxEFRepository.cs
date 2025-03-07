using magic_backend.Data;
using magic_backend.Mapping;
using magic_backend.Models;
using magic_backend.Services;

namespace magic_backend.Repositorys;

public class BoxEFRepository(BoxDbContext context, ILogger<BoxService> logger) : IBoxRepository
{
    public Task<string> CreateBox(BoxDTO box)
    {
        var entity = context.Add(box.ToEntity());
        context.SaveChanges();
        
        return Task.FromResult("Box added successfully");
    }
    
    public Task<List<BoxDTO>> GetBoxes()
    {
        var boxes = context.Box
            .Select(b => b.ToDTO())
            .ToList();
        
        logger.LogInformation($"{boxes.Count} boxes found"); 
        
        return Task.FromResult(boxes);
    }
    
    public async Task<Task> RemoveAllBoxes()
    {
        context.Box.RemoveRange(context.Box);
        context.SaveChanges();
        
        return Task.CompletedTask;
    }
}