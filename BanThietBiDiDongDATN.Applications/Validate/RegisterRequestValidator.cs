using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATNApplication.Validate
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Vui lòng điền tài khoản");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Vui lòng điền mật khẩu");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Vui lòng điền họ");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Vui lòng điền tên");
            RuleFor(x => x).Custom((request, context) =>
              {
                  if(request.Password != request.ConfirmPassword)
                  {
                      context.AddFailure("mật khẩu không chính xách");
                  }
              });
            RuleFor(x => x.Email).NotEmpty().WithMessage("Vui lòng điền Email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }
    }
}
