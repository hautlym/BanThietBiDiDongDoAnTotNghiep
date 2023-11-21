using BanThietBiDiDongDATN.Application.Catalog.System.Dtos;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;

namespace BanThietBiDiDongDATN.Admin.Models
{
    public class UserModel
    {
        public UserViewModels user{ set; get; }
        public List<Order>? order { set; get; }
    }
}
