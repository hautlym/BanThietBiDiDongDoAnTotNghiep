using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Application.Catalog.Validators
{
    public class CreateRoleValidator: AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithMessage("Vui lòng điền tên quyền");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Vui lòng điền mô tả");
        }
    }
}
