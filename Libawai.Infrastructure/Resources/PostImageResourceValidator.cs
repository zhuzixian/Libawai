using FluentValidation;

namespace Libawai.Infrastructure.Resources
{
    public class PostImageResourceValidator:AbstractValidator<PostImageResource>
    {
        public PostImageResourceValidator()
        {
            RuleFor(x => x.FileName)
                .NotNull()
                .WithName("文件名")
                .WithMessage("required|{PropertyName}是必填的")
                .MaximumLength(100)
                .WithMessage("maxlength|{PropertyName}不可超过{MaxLength}");
        }
    }
}
