namespace HelloWorld.Services;

public class RealMailService : IMailService {
    private string toAddress = string.Empty;
    private string fromAddress = string.Empty;
    private IConfiguration _config;

    public RealMailService(IConfiguration config) {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        toAddress = _config["mailSettings:toAddress"];
        fromAddress = _config["mailSettings:fromAddress"];
    }

    public void Send(string subject, string message) {
        Console.WriteLine($"Send mail to: {toAddress} from: {fromAddress} with: {nameof(RealMailService)}");
        Console.WriteLine(subject);
        Console.WriteLine(message);
    }
}