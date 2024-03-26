using MailKit.Net.Smtp;
using MimeKit;

public class Message
{
    public List<MailboxAddress> To { get; set; }
    public List<MailboxAddress> CC { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    public Message(IEnumerable<string> to, IEnumerable<string> cc, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress("test", x)));

        CC = new List<MailboxAddress>(); // Initialize the CC list
        // Cc.AddRange(to.Select(x => new MailboxAddress("test", x)())
        CC.AddRange(cc.Select(x => new MailboxAddress("test", x)));
        Subject = subject;
        Content = content;
    }
}
