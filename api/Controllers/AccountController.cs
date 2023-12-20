using BeeProject.Config;
using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.DataModels.Enums;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        private bool IsUrlAllowed(string url)
        {
            return Whitelist.AllowedUrls.Any(url.StartsWith);
        }

        private ResponseDto HandleInvalidRequest()
        {
            return new ResponseDto()
            {
                MessageToClient = "Invalid request.",
                ResponseData = null
            };
        }

        private ResponseDto ValidateAndProceed<T>(Func<T> action, string successMessage)
        {
            if (!IsUrlAllowed(Request.Headers["Referer"]))
            {
                return HandleInvalidRequest();
            }

            return new ResponseDto()
            {
                MessageToClient = $"Successfully {successMessage}.",
                ResponseData = action.Invoke()
            };
        }

        [HttpGet]
        [Route("/api/getAccounts")]
        public ResponseDto GetAllAccounts()
        {
            return ValidateAndProceed(() => _accountService.GetAllAccounts(), "fetched all accounts");
        }

        [HttpGet]
        [Route("/api/getAccountsForField/{id:int}")]
        public ResponseDto GetAccountsForField([FromRoute] int id)
        {
            return ValidateAndProceed(() => _accountService.GetAccountsForField(id), "fetched all accounts for field");
        }

        [HttpPost]
        [ValidateModel]
        [Route("/api/createAccount")]
        public ResponseDto CreateAccount([FromBody] CreateAccountRequestDto dto)
        {
            return ValidateAndProceed(() => _accountService.CreateAccount(dto.Name, dto.Email, dto.Password, (AccountRank)Enum.ToObject(typeof(AccountRank), dto.Rank)), "created an account");
        }

        [HttpPut]
        [ValidateModel]
        [Route("/api/updateAccount")]
        public ResponseDto UpdateAccount([FromBody] UpdateAccountRequestDto dto)
        {
            var userRankClaim = User.Claims.FirstOrDefault(c => c.Type == "rank")?.Value;

            var account = new AccountQuery
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Rank = dto.Rank
            };
            _accountService.UpdateAccount(account);
            return new ResponseDto()
            {
                MessageToClient = "Successfully updated account.",
            };
        }

        [HttpDelete] 
        [Route("/api/DeleteAccount/{id:int}")]
        public ResponseDto DeleteAccount([FromRoute] int id)
        {
            return ValidateAndProceed<ResponseDto>(() => { _accountService.DeleteAccount(id); return null; }, "deleted account");
        }

        [HttpPut]
        [Route("/api/checkPassword")]
        public ResponseDto CheckPassword([FromBody] CredentialCheckRequestDto dto)
        {
            return ValidateAndProceed(() => _accountService.CheckCredentials(dto.Username, dto.Password), "checked passwords");
        }

        [HttpGet]
        [Route("/api/getManagers")]
        public ResponseDto GetManagers()
        {
            return ValidateAndProceed(() => _accountService.GetAccountNamesForRank(AccountRank.FieldManager), "retrieved all managers");
        }
        
        [HttpPut]
        [Route("/api/modifyRank")]
        public ResponseDto ModifyAccountRank([FromBody] AccountRankUpdateDto dto)
        {
            return ValidateAndProceed(() => _accountService.ModifyRank(dto.AccountId, dto.Rank), "updated Rank");
        }
    }
}
