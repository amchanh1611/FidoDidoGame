using FluentValidation;

namespace FidoDidoGame.Modules.Ranks.Request;

public class GetRankRequest : RankBase
{
}
public class RankBase
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public int Current { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class HistoryOfRequest : RankBase { }
public class GetUserRankRequest
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
}
public class RankValidator : AbstractValidator<RankBase>
{
    public RankValidator()
    {
        RuleFor(x => x.DateStart).NotEmpty().WithMessage("Invalid {PropertyName}");
        RuleFor(x => x.DateEnd).NotEmpty().WithMessage("Invalid {PropertyName}")
            .GreaterThan(x=>x.DateStart).WithMessage("DateEnd must greater than DateStart");
        RuleFor(x => x.Current).GreaterThan(0).WithMessage("Invalid {PropertyName}");
        RuleFor(x => x.PageSize).GreaterThan(1).WithMessage("Invalid {PropertyName}");
    }
}
public class GetUserRankValidator : AbstractValidator<GetUserRankRequest>
{
    public GetUserRankValidator()
    {
        RuleFor(x => x.DateStart).NotEmpty().WithMessage("Invalid {PropertyName}");
        RuleFor(x => x.DateEnd).NotEmpty().WithMessage("Invalid {PropertyName}")
            .GreaterThan(x => x.DateStart).WithMessage("DateEnd must greater than DateStart");
    }
}
