using magic_backend.Models;

namespace magic_backend.Services;

public interface IBoxService
{
    Task<string> CreateBox(BoxDTO box);
    Task<List<BoxDTO>> GetBoxes();
    Task RemoveAllBoxes();
}