using AutoMapper;
using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.Entities;

namespace HighCapitalBot.Core.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamento para o Chat
        CreateMap<ChatMessage, ChatMessageDto>();

        // Mapeamento para Bots
        CreateMap<Bot, BotDto>();
        CreateMap<BotDto, Bot>();
    }
}
