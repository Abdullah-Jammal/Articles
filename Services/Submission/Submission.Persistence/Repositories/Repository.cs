using Blocks.Domain.Entities;
using Blocks.EntityFramework;

namespace Submission.Persistence.Repositories;

public class Repository<TEntity>(SubmissionDbContext context)
    : Repository<SubmissionDbContext, TEntity>(context)
    where TEntity : class, IEntity
{
}
