using HelpDesk.Domain.Entities;
using HelpDesk.Infrastructure.Data.Repositories;
using HelpDesk.Infrastructure.Data.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HelpDesk.Tests;

/// <summary>
/// Tests unitarios para el servicio de autenticación
/// </summary>
public class AuthServiceTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockConfiguration = new Mock<IConfiguration>();

        // Configurar mocks para retornar valores válidos
        _mockConfiguration.Setup(c => c["Jwt:SecretKey"])
            .Returns("my-super-secret-key-that-is-at-least-32-characters-long");
        _mockConfiguration.Setup(c => c["Jwt:Issuer"])
            .Returns("helpdesk-api");
        _mockConfiguration.Setup(c => c["Jwt:Audience"])
            .Returns("helpdesk-client");

        _authService = new AuthService(_mockUserRepository.Object, _mockConfiguration.Object);
    }

    /// <summary>
    /// Test: Autenticación exitosa con credenciales válidas
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsTokenAndUser()
    {
        // Arrange
        var username = "admin";
        var password = "123456";
        var user = new User
        {
            Id = 1,
            Username = username,
            Password = password,
            Email = "admin@helpdesk.local",
            FullName = "Administrador",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            AssignedTickets = new List<Ticket>()
        };

        _mockUserRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _authService.AuthenticateAsync(username, password);

        // Assert
        Assert.NotNull(result);
        var (token, returnedUser, expiresAt) = result.Value;
        
        Assert.NotEmpty(token);
        Assert.Equal(username, returnedUser.Username);
        Assert.Equal("admin@helpdesk.local", returnedUser.Email);
        Assert.True(expiresAt > DateTime.UtcNow);
    }

    /// <summary>
    /// Test: Autenticación fallida con contraseña incorrecta
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "admin",
            Password = "123456",
            Email = "admin@helpdesk.local",
            FullName = "Administrador",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            AssignedTickets = new List<Ticket>()
        };

        _mockUserRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _authService.AuthenticateAsync("admin", "wrongpassword");

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Test: Autenticación fallida con usuario no encontrado
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithNonexistentUser_ReturnsNull()
    {
        // Arrange
        _mockUserRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<User>());

        // Act
        var result = await _authService.AuthenticateAsync("nonexistent", "123456");

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Test: Autenticación fallida con usuario inactivo
    /// </summary>
    [Fact]
    public async Task AuthenticateAsync_WithInactiveUser_ReturnsNull()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "admin",
            Password = "123456",
            Email = "admin@helpdesk.local",
            FullName = "Administrador",
            IsActive = false, // Usuario inactivo
            CreatedAt = DateTime.UtcNow,
            AssignedTickets = new List<Ticket>()
        };

        _mockUserRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _authService.AuthenticateAsync("admin", "123456");

        // Assert
        Assert.Null(result);
    }
}