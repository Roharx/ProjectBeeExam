using System.Security.Cryptography;
using CryptSharp;
using infrastructure.DataModels.Enums;
using infrastructure.Interfaces;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class AccountService
{
    private readonly IRepository _repository;
    private readonly ITokenService _tokenService;

    public AccountService(IRepository repository, ITokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public IEnumerable<AccountSafeQuery> GetAllAccounts()
    {
        return _repository.GetAllItems<AccountSafeQuery>("account");
    }

    public IEnumerable<AccountSafeQuery> GetAccountNamesForRank(AccountRank rank)
    {
        return _repository.GetItemsByParameters<AccountSafeQuery>("account", new { rank = (int)rank });
    }

    //TODO: global exception handler
    public int CreateAccount(string accountName, string accountEmail, string accountPassword, AccountRank accountRank)
    {
        var salt = Crypter.Blowfish.GenerateSalt();
        var hashed = Crypter.Blowfish.Crypt(accountPassword, salt);

        var createItemParameters = new
        {
            name = accountName,
            email = accountEmail,
            password = hashed,
            rank = (int)accountRank
        };

        var result = _repository.CreateItem<int>("account", createItemParameters);
        return result != -1 ? result : throw new Exception("Username is already taken.");
    }

    public void UpdateAccount(AccountQuery account)
    {
        var hashed = Crypter.Blowfish.Crypt(account.Password);
        account.Password = hashed;
        var result = _repository.UpdateEntity("account", account, "id");
        if (!result)
            throw new Exception("Could not update account.");
    }

    public void DeleteAccount(int accountId)
    {
        var result = _repository.DeleteItem("account", accountId);
        if (!result)
            throw new Exception("Could not remove account.");
    }

    public string CheckCredentials(string userName, string password)
    {
        //TODO: check later
        var account = _repository.GetSingleItemByParameters<AccountQuery>("account", new { name = userName });
        if (account != null && Crypter.CheckPassword(password, account.Password))
        {
            return _tokenService.GenerateToken(account);
        }

        return null!;
    }

    public IEnumerable<AccountSafeQuery> GetAccountsForField(int fieldId)
    {
        var accountIds = _repository.GetSelectedParametersForItems<int>
            ("account_field", "account_id", new {field_id = fieldId}).ToArray();

        return accountIds.Length != 0
            ? accountIds.Select(id => _repository.GetSingleItemByParameters<AccountSafeQuery>("account", new {id})).ToList()!
            : Enumerable.Empty<AccountSafeQuery>();
    }

    public bool ModifyRank(int accountId, int rank)
    {
        var conditionColumn = new Dictionary<string, object>
        {
            { "id", accountId }
        };
        
        var modifications = new Dictionary<string, object>
        {
            { "rank", rank }
        };
        return _repository.ModifyItem("account", conditionColumn, modifications);
    }
}