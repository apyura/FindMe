namespace FindMe.Repositories
{
    public interface IUnitOfWork
    {
        void Save();
        public IUserRepository UserRepository { get; }
    }
}
