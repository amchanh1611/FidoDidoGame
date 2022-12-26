using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Persistents.Repositories;
using FluentValidation;

namespace FidoDidoGame.Modules.FidoDidos.Request
{
    public class CreateFidoDidoRequest
    {
        public int? FidoId { get; set; }
        public int? DidoId { get; set; }
        public int? Percent { get; set; }
        public string? Point { get; set; }
    }
    public class CreateOrUpdateFido
    {
        public int? Percent { get; set; }
    }
    public class CreateFidoRequest : CreateOrUpdateFido
    {
        public string? Name { get; set; }
    }
    public class UpdateFidoPercentRequest : CreateOrUpdateFido { }
    public class CreateFidoDidoValidator : AbstractValidator<CreateFidoDidoRequest>
    {
        public CreateFidoDidoValidator(IRepository repository)
        {
            RuleFor(x => x.FidoId).NotEmpty().WithMessage("{PropertyName} is required")
                .Must((_, fidoId) =>{ return repository.Fido.FindByCondition(x => x.Id == fidoId).Any(); })
                .WithMessage("Fido doest not exists in system");
            RuleFor(x => x.DidoId).NotEmpty().WithMessage("{PropertyName} is required")
                .Must((_, didoId) =>{ return repository.Dido.FindByCondition(x => x.Id == didoId).Any(); })
                .WithMessage("Fido doest not exists in system");
            RuleFor(x => x.Percent).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Point).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x).Must((_, request) =>
            {
                return repository.FidoDido.FindByCondition(x=>x.FidoId == request.FidoId).Sum(x => x.Percent) < 100;
            }).WithMessage("Invalid percent");
            RuleFor(x => x).Must((_, request) =>
            {
                return repository.FidoDido.FindByCondition(x=>x.FidoId == request.FidoId).Sum(x => x.Percent) + request.Percent <= 100;
            }).WithMessage("Percent greater than 100");
        }
    }
    public class CreateOrUpdateFidoValidator : AbstractValidator<CreateOrUpdateFido>
    {
        public CreateOrUpdateFidoValidator(IRepository repository)
        {
            RuleFor(x => x).Must((_, request) =>
            {
                return repository.Fido.FindAll().Sum(x => x.Percent) <= 100;
            }).WithMessage("Invalid percent");
            RuleFor(x => x).Must((_, request) =>
            {
                return repository.Fido.FindAll().Sum(x => x.Percent) + request.Percent <= 100;
            }).WithMessage("Percent greater than 100");
        }
    }
    public class CreateFidoValidator : AbstractValidator<CreateFidoRequest>
    {
        public CreateFidoValidator(IRepository repository)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Invalid {PropertyName}");
            RuleFor(x => x.Percent).NotEmpty().WithMessage("Invalid {PropertyName}");
            RuleFor(x => x).SetValidator(new CreateOrUpdateFidoValidator(repository));
        }
    }
    public class UpdatePercentFidoValidator : AbstractValidator<UpdateFidoPercentRequest>
    {
        public UpdatePercentFidoValidator(IRepository repository)
        {
           RuleFor(x => x).SetValidator(new CreateOrUpdateFidoValidator(repository));
        }
    }
}
