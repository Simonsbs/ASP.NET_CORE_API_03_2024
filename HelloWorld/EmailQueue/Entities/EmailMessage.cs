using System.ComponentModel.DataAnnotations;

namespace EmailQueue.Entities;

public class EmailMessage {
    [Key]
    public int ID { get; set; }

    [Required]
	[EmailAddress]
    public string From { get; set; }

	[Required]
	[EmailAddress]
	public string To { get; set; }

	[Required]
	public string Subject { get; set; }

	[Required]
	public string Body { get; set; }

    public DateTime Created { get; set; }

    public bool Sent { get; set; }
}