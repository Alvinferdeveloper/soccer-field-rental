using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using proyectoCanchas.Models;
namespace proyectoCanchas.Controllers;

 [AllowAnonymous]
[Authorize(Roles = "USER")]
public class HomeController : Controller
{

    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        DateTime today = DateTime.Today;
        string fecha = today.ToString("yyyy-MM-dd");
        var fecha1 = Request.Query["fechaActualizar1"];
        var fecha2 = Request.Query["fechaActualizar2"];
        
        if (!string.IsNullOrEmpty(fecha1) && !string.IsNullOrEmpty(fecha2))
        {
            DateTime fecha1ABuscar = DateTime.Parse(fecha1);
            DateTime fecha2ABuscar = DateTime.Parse(fecha2);
            var rentsCampo1 = _context.Rents.Where(r => r.DateTime.Date == fecha1ABuscar && r.FieldId == 1 && r.Active == true);
            var rentsCampo2 = _context.Rents.Where(r => r.DateTime.Date == fecha2ABuscar && r.FieldId == 2 && r.Active == true);
            ViewBag.rentsCampo1 = rentsCampo1;
            ViewBag.rentsCampo2 = rentsCampo2;
            ViewBag.fecha1 = fecha1;
            ViewBag.fecha2 = fecha2;
        }
        else
        {
            var rentsCampo1 = _context.Rents.Where(r => r.DateTime.Date == today && r.FieldId == 1 && r.Active == true);
            var rentsCampo2 = _context.Rents.Where(r => r.DateTime.Date == today && r.FieldId == 2 && r.Active == true);
            ViewBag.rentsCampo1 = rentsCampo1;
            ViewBag.rentsCampo2 = rentsCampo2;
            ViewBag.fecha1 = fecha;
            ViewBag.fecha2 = fecha;
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
