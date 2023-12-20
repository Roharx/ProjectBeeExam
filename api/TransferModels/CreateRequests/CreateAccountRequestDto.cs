using System.ComponentModel.DataAnnotations;

namespace BeeProject.TransferModels.CreateRequests;

public class CreateAccountRequestDto
{
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [MinLength(3)] public string Name { get; set; }
    [MinLength(6)] public string Password { get; set; }

    [Range(0, 4, ErrorMessage = "Must be a number between {1} and {2}.")]
    public int Rank { get; set; }
}