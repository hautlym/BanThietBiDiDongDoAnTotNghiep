using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using FluentValidation;

namespace BanThietBiDiDongDoAn.Applications.Validators
{
    public class RoleValidator : AbstractValidator<CreateRoleRequest>
    {
        public RoleValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Vui lòng điền tên quyền");

        }
    }
}
