﻿
using BanThietBiDiDongDATN.Application.Catalog.Commom;
using Microsoft.AspNetCore.Mvc;

namespace BanThietBiDiDongDATN.MvcApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
