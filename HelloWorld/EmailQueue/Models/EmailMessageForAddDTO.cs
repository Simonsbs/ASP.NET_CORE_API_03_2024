using System.ComponentModel.DataAnnotations;

namespace EmailQueue.Models;

public class EmailMessageForAddDTO {
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
}