namespace BeeProject.Config;

public class Whitelist
{
    //I'll forget this later: protects against Server-Side Request Forgery
    public static List<string> AllowedUrls { get; } = new List<string>
    {
        "http://localhost:4200/",
        "http://localhost:5001/",
        "https://project-bee-1d3fb/"
        // TODO: change later to the actual URL
    };
}