namespace HelloWorld.Services;
public class LocalMailService : IMailService {
    private string toAddress = string.Empty;
    private string fromAddress = string.Empty;
    private IConfiguration _config;

    public LocalMailService(IConfiguration config) {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        toAddress = config["mailSettings:toAddress"];
        fromAddress = config["mailSettings:fromAddress"];
    }

    public void Send(string subject, string message) {
        Console.WriteLine($"Send mail to: {toAddress} from: {fromAddress} with: {nameof(LocalMailService)}");
        Console.WriteLine(subject);
        Console.WriteLine(message);
    }
}
