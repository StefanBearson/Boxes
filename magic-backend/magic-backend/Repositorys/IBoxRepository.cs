using magic_backend.Models;

namespace magic_backend.Repositorys;

public interface IBoxRepository
{
    Task<string> AddBox(BoxDTO box);
    Task<List<BoxDTO>> GetBoxes();
    Task<Task> RemoveAllBoxes();
}