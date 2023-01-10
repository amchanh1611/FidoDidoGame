using FluentValidation;

namespace FidoDidoGame.Modules.Rank.Request
{
    public class CreateOrUpdateEvent
    {
        public int? Round { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
    public class CreateEventRequest : CreateOrUpdateEvent { }
    public class UpdateEventRequest : CreateOrUpdateEvent { }
    public class CreateOrUpdateEventValidator : AbstractValidator<CreateOrUpdateEvent>
    {
        public CreateOrUpdateEventValidator()
        {
            RuleFor(x => x.Round).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x=>x.DateStart).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x=>x.DateEnd).NotEmpty().WithMessage("{PropertyName} is required")
                .LessThan(x=>x.DateStart).WithMessage("DateEnd must greater than DateStart");
        }
    }
}
