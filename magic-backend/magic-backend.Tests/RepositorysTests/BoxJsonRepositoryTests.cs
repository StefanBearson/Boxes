using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using magic_backend.Models;
using magic_backend.Repositorys;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace magic_backend.Tests.RepositorysTests;

public class BoxJsonRepositoryTests
{
    private readonly ILogger<BoxJsonRepository> _logger = Substitute.For<ILogger<BoxJsonRepository>>();
    private readonly BoxJsonRepository _repository;
    private readonly string _jsonFileName = "test.json";
    
    public BoxJsonRepositoryTests()
    {
        _repository = new BoxJsonRepository(_logger);

    }
    
    [Fact]
    public async Task CheckFileStatus_CreatesFile_WhenFileDoesNotExist()
    {
        // Arrange
        if (File.Exists(_jsonFileName))
        {
            File.Delete(_jsonFileName);
        }

        // Act

        await _repository.CheckFileStatus(_jsonFileName);

        // Assert
        
        Assert.True(File.Exists(_jsonFileName));
        Assert.Equal("[]", File.ReadAllText(_jsonFileName));
    }

    [Fact]
    public async Task CheckFileStatus_DoesNotModifyFile_WhenFileExists()
    {
        // Arrange
        File.WriteAllText(_jsonFileName, "[{\"Color\":\"Red\"}]");

        // Act
        await _repository.CheckFileStatus(_jsonFileName);

        // Assert
        Assert.True(File.Exists(_jsonFileName));
        Assert.Equal("[{\"Color\":\"Red\"}]", File.ReadAllText(_jsonFileName));
    }
}