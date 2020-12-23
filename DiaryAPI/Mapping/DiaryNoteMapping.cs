using System;
using AutoMapper;
using DiaryAPI.Entities;
using EventBusRabbitMq.Events;

namespace DiaryAPI.Mapping
{
    public class DiaryNoteMapping : Profile
    {
        public DiaryNoteMapping()
        {
            CreateMap<DiaryNote, SaveDiaryNoteEvent>().ReverseMap();
        }
    }
}
