namespace BlazorServer.Data
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadDataList<T, U>(string sql, U parameters);

        Task<T> LoadData<T, U>(string sql, U parameters);

        Task SaveData<T>(string sql, T parameters);
    }
}