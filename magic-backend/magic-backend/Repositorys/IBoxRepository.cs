using magic_backend.Models;

namespace magic_backend.Repositorys;

public interface IBoxRepository
{
    Task<string> CreateBox(BoxDTO box);
    Task<List<BoxDTO>> GetBoxes();
    Task<string> DeleteAllBoxes();
}