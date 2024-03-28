using AutoMapper;
using EmailQueue.Entities;
using EmailQueue.Models;
using EmailQueue.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmailQueue.Controllers;
[ApiController]
[Route("/api/email")]
public class EmailController : ControllerBase {
	private readonly IEmailsRepository _repo;
	private readonly IMapper _map;

	public EmailController(IEmailsRepository repo, IMapper map)    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));        
		_map = map ?? throw new ArgumentNullException(nameof(map));
    }

	[HttpGet]
	public async Task<ActionResult<List<EmailMessage>>> GetPendingEmails() {
		return Ok(await _repo.GetPendingEmails());
	}

	[HttpPost]
	public async Task<ActionResult> AddEmail(EmailMessageForAddDTO message) {
		var messageToAdd = _map.Map<EmailMessage>(message);

		messageToAdd.Sent = false;
		messageToAdd.Created = DateTime.UtcNow;

		await _repo.AddEmail(messageToAdd);

		return NoContent();
	}

	[HttpPut("{id}/send")]
	public async Task<ActionResult> MarkAsSent(int id) {
		await _repo.MarkAsSent(id);

		return NoContent();
	}
}
