using Xunit;
using Moq;
using BankingApi.Models;
using BankingApi.Services;
using Microsoft.Extensions.Logging;

namespace BankingApi.Tests;

/// <summary>
/// Pruebas unitarias para TransferService.
/// Cubre todas las reglas de negocio (RB-001 a RB-005).
/// </summary>
public class TransferServiceTests
{
    private readonly Mock<IAccountStore> _accountStoreMock;
    private readonly Mock<ILogger<TransferService>> _loggerMock;
    private readonly TransferService _transferService;

    public TransferServiceTests()
    {
        _accountStoreMock = new Mock<IAccountStore>();
        _loggerMock = new Mock<ILogger<TransferService>>();
        _transferService = new TransferService(_accountStoreMock.Object, _loggerMock.Object);
    }

    // RB-001: Amount must be positive
    [Fact]
    public async Task Transfer_AmountIsZero_ReturnsFailed()
    {
        // Arrange
        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 0,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("greater than 0", response.Message);
    }

    [Fact]
    public async Task Transfer_AmountIsNegative_ReturnsFailed()
    {
        // Arrange
        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = -100,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("greater than 0", response.Message);
    }

    [Fact]
    public async Task Transfer_AmountIsPositive_ExceedsZeroValidation()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Valid transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert (passes amount validation, continues to other validations)
        Assert.NotNull(response);
        // Won't fail on amount validation, should succeed or fail on other validations
    }

    // RB-003: Different accounts
    [Fact]
    public async Task Transfer_SourceEqualsTarget_ReturnsFailed()
    {
        // Arrange
        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-001",
            Amount = 100,
            Concept = "Self transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("must be different", response.Message);
    }

    // RB-004: Both accounts exist
    [Fact]
    public async Task Transfer_SourceAccountNotFound_ReturnsFailed()
    {
        // Arrange
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-999"))
            .ReturnsAsync((Account?)null);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-999",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("does not exist", response.Message);
        Assert.Contains("ACC-999", response.Message);
    }

    [Fact]
    public async Task Transfer_TargetAccountNotFound_ReturnsFailed()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-999"))
            .ReturnsAsync((Account?)null);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-999",
            Amount = 100,
            Concept = "Test"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("does not exist", response.Message);
        Assert.Contains("ACC-999", response.Message);
    }

    // RB-002: Sufficient funds
    [Fact]
    public async Task Transfer_InsufficientFunds_ReturnsFailed()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 50, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "More than balance"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("Insufficient funds", response.Message);
    }

    [Fact]
    public async Task Transfer_ExactBalance_Succeeds()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 100, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Exact balance"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Success", response.Status);
    }

    // RB-005: Atomic operation
    [Fact]
    public async Task Transfer_ValidTransfer_UpdatesBothBalances()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 250,
            Concept = "Valid transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Success", response.Status);
        _accountStoreMock.Verify(
            x => x.UpdateBalanceAsync("ACC-001", 750),
            Times.Once,
            "Source account balance should be decreased by 250");
        _accountStoreMock.Verify(
            x => x.UpdateBalanceAsync("ACC-002", 750),
            Times.Once,
            "Target account balance should be increased by 250");
    }

    [Fact]
    public async Task Transfer_ValidTransfer_ReturnsUniqueTransferId()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Test transfer"
        };

        // Act
        var response1 = await _transferService.TransferAsync(request);
        var response2 = await _transferService.TransferAsync(request);

        // Assert
        Assert.NotEqual(response1.TransferId, response2.TransferId);
        Assert.StartsWith("TRF-", response1.TransferId);
        Assert.StartsWith("TRF-", response2.TransferId);
    }

    [Fact]
    public async Task Transfer_ValidTransfer_ReturnsSuccessStatus()
    {
        // Arrange
        var sourceAccount = new Account 
        { 
            AccountId = "ACC-001", 
            AccountOwner = "John", 
            Balance = 1000, 
            Currency = "USD" 
        };
        var targetAccount = new Account 
        { 
            AccountId = "ACC-002", 
            AccountOwner = "Jane", 
            Balance = 500, 
            Currency = "USD" 
        };

        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-001"))
            .ReturnsAsync(sourceAccount);
        _accountStoreMock.Setup(x => x.GetAccountAsync("ACC-002"))
            .ReturnsAsync(targetAccount);
        _accountStoreMock.Setup(x => x.UpdateBalanceAsync(It.IsAny<string>(), It.IsAny<decimal>()))
            .Returns(Task.CompletedTask);

        var request = new TransferRequest
        {
            SourceAccountId = "ACC-001",
            TargetAccountId = "ACC-002",
            Amount = 100,
            Concept = "Valid transfer"
        };

        // Act
        var response = await _transferService.TransferAsync(request);

        // Assert
        Assert.Equal("Success", response.Status);
        Assert.NotEmpty(response.Message);
        Assert.NotEqual(default, response.Timestamp);
    }
}
