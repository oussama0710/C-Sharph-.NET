﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDelicious.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<Dish> AllDishes = _context.Dishes.ToList();
        ViewBag.AllDishes = AllDishes;
        return View();
    }

    [HttpPost(template: "/Dishes/create")]
    public IActionResult CreateDish(Dish newDish)
    {
        if (ModelState.IsValid)
        {
            // Add
            _context.Add(newDish);
            // Save
            _context.SaveChanges();
            // -Redirect
            return RedirectToAction("Index");
        }
        return View("Index");
    }
    [HttpGet("/dishes/{DishId}")]
    public IActionResult DishShow(int DishId)
    {
        List<Dish> AllDishes = _context.Dishes.ToList();
        IEnumerable<Dish> OneDish = AllDishes.Where(dish => dish.DishId == DishId);
        foreach (var One in OneDish)
        {
            ViewBag.OneDish = One;
        }

        return View();
    }
    [HttpGet("dishes/delete/{DishId}")]
    public IActionResult DeleteDish(int DishId)
    {
        
        Dish RetrievedDish = _context.Dishes.SingleOrDefault(dish => dish.DishId == DishId);

        // Then pass the object we queried for to .Remove() on Dishs
        _context.Dishes.Remove(RetrievedDish);

        // Finally, .SaveChanges() will remove the corresponding row representing this Dish from DB 
        _context.SaveChanges();
        return RedirectToAction("Index");
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
