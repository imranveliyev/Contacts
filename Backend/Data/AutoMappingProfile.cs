using AutoMapper;
using ContactsAppApi.Models;
using ContactsAppApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAppApi.Data
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Contact, ContactDTO>()
                .ForMember(d => d.AdditionalPhones, s => s.MapFrom(x => x.AdditionalPhones.Select(a => a.Phone)));

            CreateMap<ContactDTO, Contact>()
                .ForMember(d => d.AdditionalPhones, s => s.MapFrom(x => x.AdditionalPhones.Select(a => new AdditionalPhone { Phone = a, ContactId = x.Id })));

            CreateMap<AdditionalPhone, AdditionalPhoneDTO>();
            CreateMap<AdditionalPhoneDTO, AdditionalPhone>();
        }
    }
}
