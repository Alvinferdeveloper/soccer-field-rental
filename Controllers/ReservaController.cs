using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoCanchas.Models;


namespace proyectoCanchas.Controllers;

public class ReservaController : Controller
{
    private readonly AppDbContext _context;

    public ReservaController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(ReservaModel model)
    {
        // Procesar la reserva con las horas seleccionadas
        // Puedes almacenar estos datos en la base de datos o hacer otra lógica necesaria
        string horasCampo1 = Request.Form["HorasCampo1"];
        string horasCampo2 = Request.Form["HorasCampo2"];
        string fechaCampo1 = Request.Form["fechacampo1"];
        string fechaCampo2 = Request.Form["fechacampo2"];

        // Ahora puedes procesar las horas como sea necesario
        var horas1 = horasCampo1.Split(','); // Convierte en array o lista
        var horas2 = horasCampo2.Split(',');
        ViewBag.fechaCampo1 = fechaCampo1;
        ViewBag.fechaCampo2 = fechaCampo2;
        ViewBag.horasPrimerCampo = horasCampo1.Replace(",", " - ");
        ViewBag.horasSegundoCampo = horasCampo2.Replace(",", " - ");
        return View();
    }

    public IActionResult rentaUsuario()
    {
        return View();
    }

    public IActionResult Confirmacion()
    {

        string nombre = Request.Form["nombre"];
        string apellido = Request.Form["apellido"];
        string cedula = Request.Form["cedula"];
        string horasprimercampo = Request.Form["horasprimercampo"];
        string horassegundocampo = Request.Form["horassegundocampo"];
        var horasPrimerCampoAInsertar = horasprimercampo.Split(" - ");
        var horasSegundoCampoAInsertar = horassegundocampo.Split(" - ");
        string fechaCampo1 = Request.Form["fechacampo1"];
        string fechaCampo2 = Request.Form["fechacampo2"];  //
        string formato = "yyyy-MM-dd HH:mm";        // Formato correspondiente

        if (horasPrimerCampoAInsertar.Length > 0 && horasPrimerCampoAInsertar[0] != "")
            foreach (var horaPrimerCampo in horasPrimerCampoAInsertar)
            {
                string fechaHoraTextoCampo1 = fechaCampo1 + " " + horaPrimerCampo;
                DateTime fechaHoraCampo1 = DateTime.ParseExact(fechaHoraTextoCampo1, formato, null);
                var rent = new Rent
                {
                    ClientName = nombre,
                    ClientLastName = apellido,
                    ClientCedula = cedula,
                    FieldId = 1,
                    DateTime = fechaHoraCampo1,
                    ClientType = "INVITED"
                };
                _context.Rents.Add(rent);
            }
        else if (horasSegundoCampoAInsertar.Length > 0 && horasSegundoCampoAInsertar[0] != "")
            foreach (var horaSegundoCampo in horasSegundoCampoAInsertar)
            {
                string fechaHoraTextoCampo2 = fechaCampo2 + " " + horaSegundoCampo;
                DateTime fechaHoraCampo2 = DateTime.ParseExact(fechaHoraTextoCampo2, formato, null);
                var rent = new Rent
                {
                    ClientName = nombre,
                    ClientLastName = apellido,
                    ClientCedula = cedula,
                    FieldId = 2,
                    DateTime = fechaHoraCampo2,
                    ClientType = "INVITED"
                };
                _context.Rents.Add(rent);
            }

        _context.SaveChanges();



        return RedirectToAction("index", "Home");
    }
    [HttpPost]
    public ActionResult CreateRent()
    {
        string horasprimercampo = Request.Form["HorasCampo1"];
        string horassegundocampo = Request.Form["HorasCampo2"];
        var horasPrimerCampoAInsertar = horasprimercampo.Split(",");
        var horasSegundoCampoAInsertar = horassegundocampo.Split(",");
        string fechaCampo1 = Request.Form["fechacampo1"];
        string fechaCampo2 = Request.Form["fechacampo2"];
        string formato = "yyyy-MM-dd HH:mm"; 
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (horasPrimerCampoAInsertar.Length > 0 && horasPrimerCampoAInsertar[0] != "")
            foreach (var horaPrimerCampo in horasPrimerCampoAInsertar)
            {
                string fechaHoraTextoCampo1 = fechaCampo1 + " " + horaPrimerCampo;
                DateTime fechaHoraCampo1 = DateTime.ParseExact(fechaHoraTextoCampo1, formato, null);
                var rent = new Rent
                {
                    FieldId = 1,
                    UserId = int.Parse(userId),
                    DateTime = fechaHoraCampo1,
                    ClientType = "REGISTERED"
                };
                _context.Rents.Add(rent);
            }
        else if (horasSegundoCampoAInsertar.Length > 0 && horasSegundoCampoAInsertar[0] != "")
            foreach (var horaSegundoCampo in horasSegundoCampoAInsertar)
            {
                string fechaHoraTextoCampo2 = fechaCampo2 + " " + horaSegundoCampo;
                DateTime fechaHoraCampo2 = DateTime.ParseExact(fechaHoraTextoCampo2, formato, null);
                var rent = new Rent
                {
                    FieldId = 2,
                    UserId = int.Parse(userId),
                    DateTime = fechaHoraCampo2,
                    ClientType = "REGISTERED"
                };
                _context.Rents.Add(rent);
            }
              _context.SaveChanges();

        return RedirectToAction("index", "Home");
    }

    public IActionResult Buscar()
    {
        return View();
    }
    [HttpPost]
    public IActionResult BuscarReserva(string searchTerm)
    {
        // Validar el término de búsqueda
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            // Si no se ingresa nada, redirigir o mostrar mensaje
            ModelState.AddModelError("searchTerm", "Debes ingresar un término de búsqueda.");
            return View(new List<User>());
        }

        // Buscar usuarios que coincidan con el término de búsqueda (en este caso, por nombre)
        var rents = _context.Rents
                            .Where(r => r.ClientCedula.Contains(searchTerm))
                            .ToList();

        // Devolver los resultados a la misma vista
        return View("Buscar", rents);
    }
    [Authorize]
    public IActionResult MisReservas()
    {
        // Validar el término de bdúsqueda
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        // Buscar usuarios que coincidan con el término de búsqueda (en este caso, por nombre)
        var rents = _context.Rents
                            .Where(r => r.UserId == userId && r.Active == true)
                            .Include(r => r.User)
                            .Include(r=> r.Field)
                            .ToList();

        // Devolver los resultados a la misma vista
        return View(rents);
    }

    public IActionResult Eliminar(int id){
        var rent = _context.Rents.FirstOrDefault(u => u.Id == id);
        rent.Active = false;
        _context.SaveChanges();
        return RedirectToAction("MisReservas", "Reserva");

    }


}

