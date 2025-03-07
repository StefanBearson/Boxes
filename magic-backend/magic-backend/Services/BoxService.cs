using magic_backend.Data;
using magic_backend.Mapping;
using magic_backend.Models;
using magic_backend.Repositorys;

namespace magic_backend.Services;

public class BoxService(IBoxRepository repository, ILogger<BoxService> logger) : IBoxService
{
    public Task<string> CreateBox(BoxDTO box)
    {
        var result = repository.CreateBox(box);
        return result;
    }
    
    public Task<List<BoxDTO>> GetBoxes()
    {
        var boxes = repository.GetBoxes();
        return boxes;
    }
    
    public Task RemoveAllBoxes()
    {
        var result = repository.RemoveAllBoxes();
        return result.Result;
    }
}