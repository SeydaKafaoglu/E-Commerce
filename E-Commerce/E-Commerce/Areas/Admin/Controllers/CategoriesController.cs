﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Commerce;
using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ECommerceContext _context;
        AuthorizationClass authorization = new AuthorizationClass();

        public CategoriesController(ECommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            if (authorization.IsAuthorized("viewCategories", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            return _context.Categories != null ? 
                          View(await _context.Categories.Where(m=>m.IsDeleted==false).ToListAsync()) :
                          Problem("Entity set 'ECommerceContext.Categories'  is null.");
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (authorization.IsAuthorized("viewCategories", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            if (authorization.IsAuthorized("createCategory", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,IsDeleted,Image")] Category category)
        {
            FileStream imageStream;
            string fileName;
            
            if (authorization.IsAuthorized("createCategory", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                //fileName = Directory.GetCurrentDirectory() + "\\wwwroot\\Images\\" + category.CategoryId.ToString() + Path.GetExtension(category.Image.FileName);
                //imageStream = new FileStream(fileName, FileMode.Create);
                //await category.Image.CopyToAsync(imageStream);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(short? id)     
        {
            if (authorization.IsAuthorized("editCategory", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("CategoryId,CategoryName,IsDeleted")] Category category)
        {
            if (authorization.IsAuthorized("editCategory", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (authorization.IsAuthorized("deleteCategory", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (authorization.IsAuthorized("deleteCategory", this.HttpContext.Session) == false)
            {
                return Problem("You do not have authorization to view this page");
            }
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ECommerceContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;
                //_context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(short id)
        {
          return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
