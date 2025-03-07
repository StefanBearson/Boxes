using System.Text.Json;
using magic_backend.Mapping;
using magic_backend.Models;

namespace magic_backend.Repositorys;

public class BoxJsonRepository(ILogger<BoxJsonRepository> logger) : IBoxRepository
{
    private static readonly string JsonFileName = "boxes.json";
    public async Task<string> CreateBox(BoxDTO box)
    {
        //check if jsonfile exists otherwise create a empty json file
        await CheckFileStatus();

        try
        {
            Box entity = box.ToEntity();
            string json = File.ReadAllText(JsonFileName);
            List<Box>? boxes = JsonSerializer.Deserialize<List<Box>>(json);
            if (boxes == null)
            {
                boxes = new List<Box>();
            }
            boxes.Add(entity);
            json = JsonSerializer.Serialize(boxes);
            File.WriteAllText(JsonFileName, json);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to add box");
            return await Task.FromResult(e.Message);
        }
        return await Task.FromResult("Box added successfully");
    }

    public async Task<List<BoxDTO>> GetBoxes()
    {
        await CheckFileStatus();
        string json = File.ReadAllText(JsonFileName);
        List<Box> boxes = JsonSerializer.Deserialize<List<Box>>(json);
        List<BoxDTO> boxDTOs = boxes.Select(b => b.ToDTO()).ToList();
        return await Task.FromResult(boxDTOs);
    }

    public async Task<Task> RemoveAllBoxes()
    {
        await CheckFileStatus();
        File.WriteAllText(JsonFileName, "[]");
        return Task.FromResult("All boxes removed");
    }
    
    private Task CheckFileStatus()
    {
        if (!File.Exists(JsonFileName))
        {
            File.WriteAllText(JsonFileName, "[]");
        }
        return Task.CompletedTask;
    }
}