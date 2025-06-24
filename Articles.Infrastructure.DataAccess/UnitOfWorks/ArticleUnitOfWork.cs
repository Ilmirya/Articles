using Articles.Application.Articles;
using Articles.Application.Sections;
using Articles.Infrastructure.DataAccess.DbContext;
using Articles.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Articles.Infrastructure.DataAccess.UnitOfWorks;

internal sealed class ArticleUnitOfWork : IArticleUnitOfWork, IAsyncDisposable
{
    public IArticleRepository Articles { get; }
    public ISectionRepository Sections { get; }
    
    private IDbContextTransaction? _transaction;
    
    private readonly ArticleDbContext _context;

    public ArticleUnitOfWork(ArticleDbContext context)
    {
        _context = context;
        Articles = new ArticleRepository(_context);
        Sections = new SectionRepository(_context);
    }

    public async Task StartTransaction(CancellationToken ct)
    {
        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task Commit(CancellationToken ct = default)
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction not started");
            
        await _transaction.CommitAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task Rollback(CancellationToken ct)
    {
        if (_transaction == null)
            return;
            
        await _transaction.RollbackAsync(ct);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null) await _transaction.DisposeAsync();
        await _context.DisposeAsync();
    }
}