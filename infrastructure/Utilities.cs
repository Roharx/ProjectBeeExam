namespace infrastructure;

public class Utilities
{
    private static readonly Uri Uri = new Uri(Environment.GetEnvironmentVariable("pgconn")!);

    public static readonly string
        ProperlyFormattedConnectionString =
            $"Server={Uri.Host};" +
            $"Database={Uri.AbsolutePath.Trim('/')};" +
            $"User Id={Uri.UserInfo.Split(':')[0]};" +
            $"Password={Uri.UserInfo.Split(':')[1]};" +
            $"Port={(Uri.Port > 0 ? Uri.Port : 5432)};" +
            $"Pooling=true;" +
            $"MaxPoolSize=3;";
}