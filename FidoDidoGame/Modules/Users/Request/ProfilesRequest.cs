using FidoDidoGame.Persistents.Repositories;
using FluentValidation;

namespace FidoDidoGame.Modules.Users.Request
{
    public class ProfilesRequest
    {
    }
    public class ProfilesValidator : AbstractValidator<ProfilesRequest>
    {
        public ProfilesValidator(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            RuleFor(x => x).Must((_, request) =>
            {
                int userId = int.Parse(httpContextAccessor.HttpContext!.GetRouteValue("userId")!.ToString()!);
                return repository.User.FindByCondition(x => x.Id == userId).Any();
            }).WithMessage("User doest not exists in system");
        }
    }
}
