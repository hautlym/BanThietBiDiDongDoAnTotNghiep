using BanThietBiDiDongDATN.Application.Catalog.Users;
using FluentValidation;

namespace BanThietBiDiDongDoAn.Applications.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Vui lòng điền tên người dùng");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Vui lòng điền họ")
                .MaximumLength(200).WithMessage("Độ dài không quá 200 kí tự");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Vui lòng điền tên")
                .MaximumLength(200).WithMessage("Độ dài không quá 200 kí tự");

            RuleFor(x => x.Dob).NotEmpty().WithMessage("Vui lòng chọn ngày sinh");

            RuleFor(x => x.Address).NotEmpty().WithMessage("Vui lòng điền địa chỉ")
                .MaximumLength(250).WithMessage("Độ dài không quá 250 kí tự");

            RuleFor(x => x.Gender).NotEmpty().WithMessage("Vui lòng chọn  giới tính");

            RuleFor(x => x.NumberPhone).NotEmpty().WithMessage("Vui lòng điền số điện thoại")
                .MaximumLength(50).WithMessage("Độ dài không quá 50 kí tự");
        }
    }
}
