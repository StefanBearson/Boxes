using magic_backend.Data;
using magic_backend.Mapping;
using magic_backend.Models;

namespace magic_backend.Services;

public class BoxService(BoxDbContext context, ILogger<BoxService> logger) : IBoxService
{
    public Task<string> AddBox(BoxDTO box)
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
    
    public Task RemoveAllBoxes()
    {
        context.Box.RemoveRange(context.Box);
        context.SaveChanges();
        
        return Task.CompletedTask;
    }
}