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

    public IActionResult Dashboard()
    {
        var rentsCampo1 = _context.Rents.Where(r => r.FieldId == 1 && r.Active == true).Include(r => r.Field);
        var rentsCampo2 = _context.Rents.Where(r => r.FieldId == 2 && r.Active == true).Include(r => r.Field);
        var totalGananciaCampo1 = rentsCampo1.Any()
    ? rentsCampo1.FirstOrDefault()?.Field.Price * rentsCampo1.Count()
    : 0;
        var totalGananciaCampo2 = rentsCampo2.Any()
        ? rentsCampo2.FirstOrDefault()?.Field.Price * rentsCampo2.Count()
        : 0;

        DateTime startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime today = DateTime.Now;
        var gananciaCampo1EnMes = rentsCampo1.Any() ? rentsCampo1.FirstOrDefault()?.Field.Price *  rentsCampo1.Where(r => r.DateTime >= startOfMonth && r.DateTime <= today).Count() : 0;
        var gananciaCampo2EnMes = rentsCampo2.Any() ? rentsCampo2.FirstOrDefault()?.Field.Price *  rentsCampo2.Where(r => r.DateTime >= startOfMonth && r.DateTime <= today).Count() : 0;
    

        ViewBag.totalGananciaCampo1 = totalGananciaCampo1;
        ViewBag.totalGananciaCampo2 = totalGananciaCampo2;
        ViewBag.totalRentasCampo1 = rentsCampo1.Count();
        ViewBag.totalRentasCampo2 = rentsCampo2.Count();
        ViewBag.gananciaCampo1EnMes = gananciaCampo1EnMes;
        ViewBag.gananciaCampo2EnMes = gananciaCampo2EnMes;


        return View();
    }

    public IActionResult Users(){
        var users = _context.Users.Where(u => u.Active == true && (u.UserRoles.Where(r => r.Role.Name == "ADMIN").Count() ==0)).ToList();
        ViewBag.Users = users;
        return View();
    }

    public IActionResult UsuariosInhabilitados(){
        var users = _context.Users.Where(u => u.Active == false && (u.UserRoles.Where(r => r.Role.Name == "ADMIN").Count() ==0)).ToList();
        ViewBag.Users = users;
        return View();
    }

    [HttpGet]
    public IActionResult DeshabilitarUsuario(int id){
        var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
        user.Active = false;
        _context.SaveChanges();
        return RedirectToAction("Users","Admin");
    }

    public IActionResult HabilitarUsuario(int id){
        var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
        user.Active = true;
        _context.SaveChanges();
        return RedirectToAction("UsuariosInhabilitados","Admin");
    }




}
