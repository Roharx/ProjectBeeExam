using CryptSharp;
using infrastructure.DataModels.Enums;
using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class AccountService : ServiceBase
{
    private readonly ITokenService _tokenService;

    public AccountService(IRepository repository, ITokenService tokenService) : base(repository)
    {
        _tokenService = tokenService;
    }

    public IEnumerable<AccountSafeQuery> GetAllAccounts()
    {
        return GetAllItems<AccountSafeQuery>("account");
    }

    public IEnumerable<AccountSafeQuery> GetAccountNamesForRank(AccountRank rank)
    {
        return Repository.GetItemsByParameters<AccountSafeQuery>("account", new { rank = (int)rank });
    }

    //TODO: global exception handler
    public int CreateAccount(string accountName, string accountEmail, string accountPassword, AccountRank accountRank)
    {
        var salt = Crypter.Blowfish.GenerateSalt(); //implement salting later
        var hashed = Crypter.Blowfish.Crypt(accountPassword, salt);

        var createItemParameters = new
        {
            name = accountName,
            email = accountEmail,
            password = hashed,
            rank = (int)accountRank
        };
        
        return CreateItem<int>("account", createItemParameters);
    }

    public void UpdateAccount(AccountQuery account)
    {
        var hashed = Crypter.Blowfish.Crypt(account.Password);
        account.Password = hashed;
        
        UpdateItem("account", account);
    }

    public void DeleteAccount(int accountId)
    {
        DeleteItem("account", accountId);
    }

    public string CheckCredentials(string userName, string password)
    {
        //TODO: check later
        var account = Repository.GetSingleItemByParameters<AccountQuery>("account", new { name = userName });
        if (account != null && Crypter.CheckPassword(password, account.Password))
        {
            return _tokenService.GenerateToken(account);
        }

        return null!;
    }

    public IEnumerable<AccountSafeQuery> GetAccountsForField(int fieldId)
    {
        var accountIds = Repository.GetSelectedParametersForItems<int>
            ("account_field", "account_id", new {field_id = fieldId}).ToArray();

        return accountIds.Length != 0
            ? accountIds.Select(id => Repository.GetSingleItemByParameters<AccountSafeQuery>("account", new {id})).ToList()!
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
        return Repository.ModifyItem("account", conditionColumn, modifications);
    }
}