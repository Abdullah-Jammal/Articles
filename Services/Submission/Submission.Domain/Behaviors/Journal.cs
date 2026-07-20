using Articles.Abstractions.Enums;

namespace Submission.Domain.Entities;

public partial class Journal
{
    public Article CreateArticle(string title, ArticleType type, string scope)
    {
        var article = new Article
        {
            Title = title,
            Scope = scope,
            Type = type,
            Stage = ArticleStage.Created,
            Journal = this,
            JournalId = Id
        };
        _articles.Add(article);
        return article;
    }
}
