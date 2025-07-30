using AutoMapper;
using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.Entities;

namespace HighCapitalBot.Core.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Bot mappings
        CreateMap<CreateBotDto, Bot>();
        CreateMap<Bot, BotDto>();
        
        // ChatMessage mappings
        CreateMap<ChatMessage, ChatMessageDto>();
    }
}
