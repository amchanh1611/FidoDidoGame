using FidoDidoGame.Persistents.Repositories;
using FluentValidation;

namespace FidoDidoGame.Modules.Users.Request
{
    public class CreateUserRequest
    {
        public string? Name { get; set; }
        public string? NickName { get; set; }
    }
    public class UpdateUserRequest
    {
        public string? NickName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator(IRepository repository,IHttpContextAccessor httpContextAccessor)
        {
            RuleFor(x => x).Must((_, request) =>
            {
                int userId = int.Parse(httpContextAccessor.HttpContext!.GetRouteValue("userId")!.ToString()!);
                return repository.User.FindByCondition(x => x.Id == userId).Any();
            }).WithMessage("User doest not exists in system");
            RuleFor(x => x.NickName).NotEmpty().When(x => x.NickName != null).WithMessage("{PropertyName} is not valid");
            RuleFor(x => x.Address).NotEmpty().When(x => x.NickName != null).WithMessage("{PropertyName} is not valid");
            RuleFor(x => x.Phone).Matches("((84|0)+[3|5|7|8|9])+([0-9]{8})\b").When(x => x.Phone != null).WithMessage("{PropertyName} is not valid");

        }
    }
}
