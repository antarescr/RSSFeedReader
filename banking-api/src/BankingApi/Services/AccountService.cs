using System.Collections.Concurrent;
using BankingApi.Models;

namespace BankingApi.Services;

/// <summary>
/// Servicio de cuentas bancarias con almacenamiento en memoria.
/// </summary>
public class AccountService : IAccountStore
{
    private readonly ConcurrentDictionary<string, Account> _accounts;

    public AccountService()
    {
        _accounts = new ConcurrentDictionary<string, Account>();
        InitializeSeedData();
    }

    public Task<Account?> GetAccountAsync(string accountId)
    {
        _accounts.TryGetValue(accountId, out var account);
        return Task.FromResult(account);
    }

    public Task<bool> AccountExistsAsync(string accountId)
    {
        return Task.FromResult(_accounts.ContainsKey(accountId));
    }

    public Task UpdateBalanceAsync(string accountId, decimal newBalance)
    {
        if (_accounts.TryGetValue(accountId, out var account))
        {
            account.Balance = newBalance;
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return Task.FromResult(_accounts.Values.AsEnumerable());
    }

    private void InitializeSeedData()
    {
        var accounts = new[]
        {
            new Account 
            { 
                AccountId = "ACC-001", 
                AccountOwner = "John Doe", 
                Balance = 1000m, 
                Currency = "USD" 
            },
            new Account 
            { 
                AccountId = "ACC-002", 
                AccountOwner = "Jane Smith", 
                Balance = 500m, 
                Currency = "USD" 
            },
            new Account 
            { 
                AccountId = "ACC-003", 
                AccountOwner = "Bob Johnson", 
                Balance = 0m, 
                Currency = "USD" 
            }
        };

        foreach (var account in accounts)
        {
            _accounts.TryAdd(account.AccountId, account);
        }
    }
}
