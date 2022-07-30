using Blogvio.WebApi.Dtos.Blog;
using FluentValidation;

namespace Blogvio.WebApi.Validators;

public class CreateBlogValidator : AbstractValidator<BlogCreateDto>
{
	public CreateBlogValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
		RuleFor(x => x.Name).MinimumLength(10);
	}
}