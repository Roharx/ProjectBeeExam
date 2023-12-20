using infrastructure.DataModels.Enums;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class AccountService
{
    private readonly AccountRepository _accountRepository;
    private readonly ITokenService _tokenService;

    public AccountService(AccountRepository accountRepository, ITokenService tokenService)
    {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
    }

    public IEnumerable<AccountSafeQuery> GetAllAccounts()
    {
        return _accountRepository.GetAllAccounts();
    }

    public IEnumerable<AccountSafeQuery> GetAccountNamesForRank(AccountRank rank)
    {
        return _accountRepository.GetAccountNamesForRank((int)rank);
    }

    //TODO: global exception handler
    public int CreateAccount(string accountName, string accountEmail, string accountPassword, AccountRank accountRank)
    {
        var hashed = CryptSharp.Crypter.Blowfish.Crypt(accountPassword);
        var result = _accountRepository.CreateAccount(accountName, accountEmail, hashed, (int)accountRank);
        return result != -1 ? result : throw new Exception("Username is already taken.");
    }

    public void UpdateAccount(AccountQuery account)
    {
        var hashed = CryptSharp.Crypter.Blowfish.Crypt(account.Password);
        account.Password = hashed;
        var result = _accountRepository.UpdateAccount(account);
        if (!result)
            throw new Exception("Could not update account.");
    }

    public void DeleteAccount(int accountId)
    {
        var result = _accountRepository.DeleteAccount(accountId);
        if (!result)
            throw new Exception("Could not remove account.");
    }

    public string CheckCredentials(string userName, string password)
    {
        //TODO: check later
        var account = _accountRepository.GetAccountByName(userName);
        if (account != null && CryptSharp.Crypter.CheckPassword(password, account.Password))
        {
            return _tokenService.GenerateToken(account);
        }
        return null!;
    }
    public IEnumerable<AccountSafeQuery> GetAccountsForField(int fieldId)
    {
        var accountIds = _accountRepository.GetAccountsForField(fieldId).ToArray();

        return accountIds.Length != 0
            ? accountIds.Select(id => _accountRepository.GetAccountById(id)).ToList()
            : Enumerable.Empty<AccountSafeQuery>();
    }

    public bool ModifyRank(int accountId, int rank)
    {
        return _accountRepository.ModifyAccountRank(accountId, rank);
    }
}