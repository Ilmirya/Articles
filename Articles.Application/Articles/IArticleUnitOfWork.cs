using Articles.Application.Sections;

namespace Articles.Application.Articles;

public interface IArticleUnitOfWork : IDisposable
{
    IArticleRepository Articles { get; }
    ISectionRepository Sections { get; }
    Task StartTransaction(CancellationToken ct = default);
    Task Commit(CancellationToken ct = default);
    Task Rollback(CancellationToken ct = default);
}