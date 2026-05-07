/*
 * ============================================================
 * File        : AccountController.cs
 * Người viết  : Linh
 * Ngày tạo    : 07/05/2026
 * ------------------------------------------------------------
 * Mô tả:
 *  Controller xử lý chức năng đăng nhập, đăng xuất
 *  và phân quyền người dùng trong hệ thống OptiShop.
 *
 *  Chức năng:
 *   - Đăng nhập tài khoản
 *   - Xác thực bằng Cookie Authentication
 *   - Đăng xuất hệ thống
 *   - Xử lý truy cập bị từ chối
 * ============================================================
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OptiShop.DataAccess.Repositories;

namespace OptiShop.Web.Controllers;

// Controller xử lý xác thực tài khoản người dùng
public class AccountController : Controller
{
    private readonly IUnitOfWork _uow;

    // Constructor inject UnitOfWork
    public AccountController(IUnitOfWork uow) => _uow = uow;

    // Hiển thị giao diện đăng nhập
    [HttpGet]
    public IActionResult Login() => View();

    // Xử lý đăng nhập
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        // Tìm tài khoản theo username
        var users = await _uow.Users.FindAsync(u => u.Username == username);
        var user = users.FirstOrDefault();

        // Kiểm tra tài khoản và mật khẩu
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }

        // Kiểm tra trạng thái tài khoản
        if (!user.IsActive)
        {
            ViewBag.Error = "Tài khoản đã bị khóa";
            return View();
        }

        // Tạo thông tin xác thực người dùng
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role),
            new("Username", user.Username)
        };

        // Tạo cookie đăng nhập
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

        // Điều hướng theo vai trò người dùng
        return user.Role == "KhachHang"
            ? RedirectToAction("Index", "Shop")
            : RedirectToAction("Index", "Home");
    }

    // Đăng xuất hệ thống
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }

    // Trang báo lỗi không đủ quyền truy cập
    public IActionResult AccessDenied() => View();
}