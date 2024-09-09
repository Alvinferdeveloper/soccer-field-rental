using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoCanchas.Models;


namespace proyectoCanchas.Controllers;
[Authorize(Roles = "ADMIN")]
public class AdminController : Controller
{
     private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {

        return View();
    }

    public IActionResult Dashboard(){
        var rentsCampo1 = _context.Rents.Where(r => r.FieldId == 1 && r.Active == true).Include(r => r.Field);
        var rentsCampo2 = _context.Rents.Where(r => r.FieldId ==2 && r.Active == true).Include(r => r.Field);
        var totalGananciaCampo1 = rentsCampo1.Any() 
    ? rentsCampo1.FirstOrDefault()?.Field.Price * rentsCampo1.Count() 
    : 0;
    var totalGananciaCampo2 = rentsCampo2.Any() 
    ? rentsCampo2.FirstOrDefault()?.Field.Price * rentsCampo2.Count() 
    : 0;

    ViewBag.totalGananciaCampo1 = totalGananciaCampo1;
    ViewBag.totalGananciaCampo2 = totalGananciaCampo2;
    
        
        return View();
    }
   

    
}
