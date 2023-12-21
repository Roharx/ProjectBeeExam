namespace BeeProject.Config;

public class Whitelist
{
    public static List<string> AllowedUrls { get; } = new List<string>
    {
        "http://localhost:4200/",
        "http://localhost:5001/",
        "https://project-bee-1d3fb.web.app/",
        "https://project-bee-1d3fb.firebaseapp.com/",
        "https://project-bee-1d3fb"
    };
}