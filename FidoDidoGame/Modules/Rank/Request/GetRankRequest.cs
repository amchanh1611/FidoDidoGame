using FidoDidoGame.Persistents.Repositories;
using FluentValidation;

namespace FidoDidoGame.Modules.Ranks.Request;

public class GetRankRequest : RankBase
{
    public int? EventId { get; set; }
}
public class RankBase
{
    public int Current { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class HistoryOfRequest : RankBase 
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
}
public class GetUserRankRequest
{
    public int? EventId { get; set; }
}
public class RankValidator : AbstractValidator<RankBase>
{
    public RankValidator()
    {
        RuleFor(x => x.Current).GreaterThan(0).WithMessage("Invalid {PropertyName}");
        RuleFor(x => x.PageSize).GreaterThan(1).WithMessage("Invalid {PropertyName}");
    }
}
public class GetUserRankValidator : AbstractValidator<GetUserRankRequest>
{
    public GetUserRankValidator(IRepository repository)
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage("{PropertyName} is required")
            .Must((_, eventId) => { return repository.Event.FindByCondition(x => x.Id == eventId).Any(); })
            .WithMessage("Invalid event");
    }
}
public class HistoryOfValidator : AbstractValidator<HistoryOfRequest>
{
    public HistoryOfValidator()
    {
        RuleFor(x => x).SetValidator(new RankValidator());
        RuleFor(x => x.DateStart).NotEmpty().WithMessage("Invalid {PropertyName}");
        RuleFor(x => x.DateEnd).NotEmpty().WithMessage("Invalid {PropertyName}")
            .GreaterThan(x => x.DateStart).WithMessage("DateEnd must greater than DateStart");
    }
}
public class GetRankValidator : AbstractValidator<GetRankRequest>
{
    public GetRankValidator(IRepository repository)
    {
        RuleFor(x => x).SetValidator(new RankValidator());
        RuleFor(x => x.EventId).NotEmpty().WithMessage("{PropertyName} is required")
            .Must((_, eventId) => { return repository.Event.FindByCondition(x => x.Id == eventId).Any(); })
            .WithMessage("Invalid event");
    }
}
