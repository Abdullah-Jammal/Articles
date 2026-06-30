using Articles.Abstractions.Enums;

namespace Submission.Domain.Entities;

public partial class Journal
{
    public Articles CreateArticle(string title, ArticleType type, string scope)
    {
        var article = new Articles
        {
            Title = title,
            Scope = scope,
            Type = type,
            Stage = ArticleStage.Created,
            Journal = this
        };
        _articles.Add(article);
        return article;
    }
}
