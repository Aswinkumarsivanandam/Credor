using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Credor.Data.Entities;
using Credor.Client.Entities;

namespace Credor.Web.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {           
            CreateMap<UserAccount, UserAccountDto>().ReverseMap();
            CreateMap<UserAccount, UpdateUserAccountDto>().ReverseMap();
            CreateMap<Investment, InvestmentDto>().ReverseMap();
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<Investor, InvestorDto>().ReverseMap();
            CreateMap<BankAccount, BankAccountDto>().ReverseMap();
            CreateMap<Document, DocumentDto>().ReverseMap();
            CreateMap<Notifications, NotificationDto>().ReverseMap();
        }
    }
}
