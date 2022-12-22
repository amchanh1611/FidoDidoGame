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
        public UpdateUserValidator()
        {
            RuleFor(x => x.NickName).NotEmpty().When(x => x.NickName != null).WithMessage("{PropertyName} is not valid");
            RuleFor(x => x.Address).NotEmpty().When(x => x.NickName != null).WithMessage("{PropertyName} is not valid");
            RuleFor(x => x.Phone).Matches("((84|0)+[3|5|7|8|9])+([0-9]{8})\b").When(x => x.Phone != null).WithMessage("{PropertyName} is not valid");

        }
    }
}
