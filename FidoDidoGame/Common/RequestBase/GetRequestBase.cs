using FluentValidation;

namespace FidoDidoGame.Common.RequestBase
{
    public class GetRequestBase
    {
        public string? InfoSearch { get; set; }
        public int Current { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Orderby { get; set; }
    }
    public class RankValidator : AbstractValidator<GetRequestBase>
    {
        public RankValidator()
        {
            RuleFor(x => x.Current).GreaterThan(0).WithMessage("Invalid {PropertyName}");
            RuleFor(x => x.PageSize).GreaterThan(1).WithMessage("Invalid {PropertyName}");
        }
    }
}
