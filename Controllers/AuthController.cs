using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoCanchas.Models;


namespace proyectoCanchas.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Buscar al usuario en la base de datos
        var user = await _context.Users
    .Include(u => u.UserRoles)         // Cargar los roles del usuario
    .ThenInclude(ur => ur.Role)        // Cargar los detalles del rol
    .FirstOrDefaultAsync(u => u.Email == model.Email);


        // Verificar que el usuario existe y que la contraseña es correcta
        if (user != null && user.Password == model.Password)
        {
            // Crear los claims para la sesión
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Surname, user.Name),

                // Puedes agregar más claims aquí si es necesario
            };

            foreach (var userRole in user.UserRoles)
            {
                if (userRole.Role.Name != null && !string.IsNullOrEmpty(userRole.Role.Name))
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                }
            }

            // Crear la identidad del usuario
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Crear el principal y firmar al usuario
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true, // Mantiene la sesión después de cerrar el navegador
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });

            if (UserTieneRol(user.UserRoles, "USER"))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("dashboard", "Admin");


        }

        // Si las credenciales son incorrectas, mostrar un error
        ViewBag.credentialsError = "Credenciales Incorrectas";
        return View("index", model);
    }

    public bool UserTieneRol(ICollection<UserRole> userRoles, string roleName)
    {
        return userRoles.Any(ur => ur.Role.Name == roleName);
    }


  
    public async Task<IActionResult> Logout()
    {
        // Cerrar la sesión del usuario
        await HttpContext.SignOutAsync();

        // Redirigir al usuario a la página principal u otra página
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Registrar()
    {
        // Cerrar la sesión del usuario
        // Redirigir al usuario a la página principal u otra página
        return View();
    }

    public async Task<IActionResult> GuardarUsuario(User model)
    {
        string name = Request.Form["nombres"];
        string apellidos = Request.Form["apellidos"];
        string email = Request.Form["email"];
        string nacimiento = Request.Form["fechaNacimiento"];
        string password = Request.Form["password"];
        string format = "yyyy-MM-dd";
        var userWithEmail = _context.Users.Where(u => u.Email == email).FirstOrDefault();
        if (userWithEmail != null)
        {
            ModelState.AddModelError("Email", "El email ya esta en uso");
            return View("Registrar", model);
        }
        DateTime dateTime = DateTime.ParseExact(nacimiento, format, CultureInfo.InvariantCulture);
        var user = new User
        {
            Name = name,
            LastName = apellidos,
            Email = email,
            Birthday = dateTime,
            Password = password,
            Active = true,

        };
        var userRole = new UserRole
        {
            UserId = user.Id,  // Esto se actualizará después de guardar el usuario
            RoleId = 1
        };

        // Agregar el usuario a la base de datos
        _context.Users.Add(user);
        _context.SaveChanges(); // Esto guarda el usuario y genera el UserId

        // Ahora asignar el UserId correcto a los roles

        userRole.UserId = user.Id; // Asegurarse de que UserId esté actualizado
        _context.UserRoles.Add(userRole);
        // Guardar los roles asignados
        _context.SaveChanges();
        var userDoc = await _context.Users
            .Include(u => u.UserRoles)         // Cargar los roles del usuario
            .ThenInclude(ur => ur.Role)        // Cargar los detalles del rol
            .FirstOrDefaultAsync(u => u.Email == email);
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDoc.Email),
                new Claim(ClaimTypes.NameIdentifier, userDoc.Id.ToString()),
                new Claim(ClaimTypes.Surname, userDoc.Name),

                // Puedes agregar más claims aquí si es necesario
            };

        foreach (var userDocRole in userDoc.UserRoles)
        {
            if (userDocRole.Role.Name != null && !string.IsNullOrEmpty(userDocRole.Role.Name))
            {
                claims.Add(new Claim(ClaimTypes.Role, userDocRole.Role.Name));
            }
        }

        // Crear la identidad del usuario
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // Crear el principal y firmar al usuario
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = true, // Mantiene la sesión después de cerrar el navegador
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            });

        return RedirectToAction("index", "Home");

    }



}

