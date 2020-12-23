using System;
using DiaryAPI.Entities;
using MongoDB.Driver;

namespace DiaryAPI.Data
{
    public interface IDiaryContext
    {
        IMongoCollection<DiaryNote> DiaryNotes { get; }
    }
}
