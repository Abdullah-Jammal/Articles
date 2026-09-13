using Blocks.Domain.Entities;
using Blocks.EntityFrameworkCore;

namespace Submission.Persistence.Repositories;

public class Repository<TEntity>(SubmissionDbContext context)
    : Repository<SubmissionDbContext, TEntity>(context)
    where TEntity : class, IEntity
{
}
