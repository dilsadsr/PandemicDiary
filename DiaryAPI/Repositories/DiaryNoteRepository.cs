using System.Collections.Generic;
using System.Threading.Tasks;
using DiaryAPI.Data;
using DiaryAPI.Entities;
using DiaryAPI.Repositories.Interfaces;
using MongoDB.Driver;

namespace DiaryAPI.Repositories
{
    public class DiaryNoteRepository : IDiaryNoteRepository
    {
        private readonly IDiaryContext _context;
        public DiaryNoteRepository(IDiaryContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<DiaryNote>> GetDiaryNotes()
        {
            return await _context.DiaryNotes.Find(p => true).ToListAsync();
        }


        public async Task Create(DiaryNote diaryNote)
        {
            await _context.DiaryNotes.InsertOneAsync(diaryNote);
        }
    }
}
