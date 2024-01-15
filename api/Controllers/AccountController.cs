using BeeProject.Filters;
using BeeProject.TransferModels;
using BeeProject.TransferModels.CreateRequests;
using BeeProject.TransferModels.UpdateRequests;
using infrastructure.DataModels.Enums;
using infrastructure.QueryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;

namespace BeeProject.Controllers
{
    public class AccountController : ControllerBase<AccountService>
    {
        public AccountController(AccountService accountService): base(accountService) { }
        

        [HttpGet]
        [Authorize]
        [Route("/api/getAccounts")]
        public ResponseDto GetAllAccounts()
        {
            return ValidateAndProceed(() => Service.GetAllAccounts(), "fetched all accounts");
        }

        [HttpGet]
        [Authorize]
        [Route("/api/getAccountsForField/{id:int}")]
        public ResponseDto GetAccountsForField([FromRoute] int id)
        {
            return ValidateAndProceed(() => Service.GetAccountsForField(id), "fetched all accounts for field");
        }

        [HttpPost]
        [Authorize]
        [ValidateModel]
        [Route("/api/createAccount")]
        public ResponseDto CreateAccount([FromBody] CreateAccountRequestDto dto)
        {
            return ValidateAndProceed(() => Service.CreateAccount(
                dto.Name, 
                dto.Email, 
                dto.Password, 
                (AccountRank)Enum.ToObject(typeof(AccountRank), dto.Rank)
                ), "created an account");
        }

        [HttpPut]
        [Authorize]
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
            Service.UpdateAccount(account);
            return new ResponseDto()
            {
                MessageToClient = "Successfully updated account.",
            };
        }

        [HttpDelete] 
        [Authorize]
        [Route("/api/DeleteAccount/{id:int}")]
        public ResponseDto DeleteAccount([FromRoute] int id)
        {
            return ValidateAndProceed<ResponseDto>(() => { Service.DeleteAccount(id); return null; }, "deleted account");
        }

        [HttpPut]
        [Authorize]
        [AllowAnonymous]
        [Route("/api/checkPassword")]
        public ResponseDto CheckPassword([FromBody] CredentialCheckRequestDto dto)
        {
            return ValidateAndProceed(() => Service.CheckCredentials(dto.Username, dto.Password), "checked passwords");
        }

        [HttpGet]
        [Authorize]
        [Route("/api/getManagers")]
        public ResponseDto GetManagers()
        {
            return ValidateAndProceed(() => Service.GetAccountNamesForRank(AccountRank.FieldManager), "retrieved all managers");
        }
        [HttpGet]
        [Authorize]
        [Route("/api/getKeepers")]
        public ResponseDto GetKeepers()
        {
            return ValidateAndProceed(() => Service.GetAccountNamesForRank(AccountRank.Keeper), "retrieved all managers");
        }
        
        [HttpPut]
        [Authorize]
        [Route("/api/modifyRank")]
        public ResponseDto ModifyAccountRank([FromBody] AccountRankUpdateDto dto)
        {
            return ValidateAndProceed(() => Service.ModifyRank(dto.AccountId, dto.Rank), "updated Rank");
        }
    }
}
