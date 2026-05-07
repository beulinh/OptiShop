using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptiShop.Business.Interfaces;

namespace OptiShop.Web.Controllers;

[Authorize(Roles = "Admin,NVBanHang,NVKho")]
public class HomeController : Controller
{
    private readonly IReportService _reportService;
    public HomeController(IReportService reportService) => _reportService = reportService;

    public async Task<IActionResult> Index()
    {
        var dashboard = await _reportService.GetDashboardAsync();
        return View(dashboard);
    }
}
