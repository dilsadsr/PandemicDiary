using System.Collections.Generic;
using System.Threading.Tasks;
using DiaryAPI.Entities;

namespace DiaryAPI.Repositories.Interfaces
{
    public interface IDiaryNoteRepository
    {
        Task<IEnumerable<DiaryNote>> GetDiaryNotes();
        Task Create(DiaryNote diaryNote);
    }
}
