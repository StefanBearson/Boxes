using magic_backend.Data;
using magic_backend.Mapping;
using magic_backend.Models;
using magic_backend.Repositorys;

namespace magic_backend.Services;

public class BoxService(IBoxRepository repository) : IBoxService
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
    
    public Task<string> DeleteAllBoxes()
    {
        var result = repository.DeleteAllBoxes();
        return result;
    }
}