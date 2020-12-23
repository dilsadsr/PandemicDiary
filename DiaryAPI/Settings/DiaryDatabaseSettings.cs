using System;
namespace DiaryAPI.Settings
{
    public class DiaryDatabaseSettings:IDiaryDatabaseSettings
    {
        public string ConnectionString { get ; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
