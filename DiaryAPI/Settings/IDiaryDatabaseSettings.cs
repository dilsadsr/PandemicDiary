namespace DiaryAPI.Settings
{
    public interface IDiaryDatabaseSettings
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string CollectionName { get; set; }
    }
}
