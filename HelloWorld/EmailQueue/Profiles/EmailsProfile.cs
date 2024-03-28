using AutoMapper;
using EmailQueue.Entities;
using EmailQueue.Models;

namespace EmailQueue.Profiles;

public class EmailsProfile : Profile {
	public EmailsProfile() {
		CreateMap<EmailMessageForAddDTO, EmailMessage>();
	}
}
