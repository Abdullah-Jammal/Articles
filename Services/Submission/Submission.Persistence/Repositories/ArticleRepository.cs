using Blocks.EntityFramework;
using Submission.Domain.Entities;

namespace Submission.Persistence.Repositories;

public class ArticleRepository(SubmissionDbContext context) 
    : Repository<SubmissionDbContext, Article>(context)
{
}
