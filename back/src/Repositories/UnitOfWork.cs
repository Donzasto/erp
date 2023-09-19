class UnitOfWork : IDisposable
{
    private ERPContext _eRPContext = new();
    private GenericRepository<User>? _userRepository;
    private GenericRepository<Nomenclature>? _nomenclatureRepository;

    internal GenericRepository<User> UsersRepository
        => _userRepository ??= new GenericRepository<User>(_eRPContext);


    internal GenericRepository<Nomenclature> NomenclatureRepository
        => _nomenclatureRepository ??= new GenericRepository<Nomenclature>(_eRPContext);


    internal void Save()
    {
        _eRPContext.SaveChanges();
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _eRPContext.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}