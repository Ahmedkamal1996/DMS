namespace DMSTask.BLL.Repositories
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}
