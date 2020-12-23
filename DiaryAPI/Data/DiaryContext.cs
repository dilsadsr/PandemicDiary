using DiaryAPI.Entities;
using DiaryAPI.Settings;
using MongoDB.Driver;

namespace DiaryAPI.Data
{
    public class DiaryContext :IDiaryContext
    {
        public DiaryContext(IDiaryDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var databse = client.GetDatabase(settings.DatabaseName);

            DiaryNotes = databse.GetCollection<DiaryNote>(settings.CollectionName);

        }

        public IMongoCollection<DiaryNote> DiaryNotes { get; }
    }
}
