using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KIWReservationApp.Data;
using KIWReservationApp.Models;
using System.Net.Mail;
using OfficeOpenXml;
using QRCoder;
using System.Drawing;
using MailKit.Search;

namespace KIWReservationApp.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly KIWReservationAppContext _context;

        public MaterialsController(KIWReservationAppContext context)
        {
            _context = context;
        }

        // GET: Materials
        public async Task<IActionResult> Index()
        {
                    

                        return _context.Material != null ?
                        View(await _context.Material.Where(i => i.Type != "Dummy Item").ToListAsync()) :
                        Problem("Entity set 'KIWReservationAppContext.Material'  is null.");
        }


        // POST: Materials/Filter/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Filter(string selectedItem)
        {
            if (selectedItem == null || _context.Material == null)
            {
                return NotFound();
            }

            if (selectedItem == "All")
            {
                return View("Index", await _context.Material.Where(c => c.Type != "Dummy Item").ToListAsync());
            }

            var materials = await _context.Material.Where(m => m.Type == selectedItem).ToListAsync();
            if (materials == null)
            {
                return NotFound();
            }

            return View("Index", materials);
        }

        //POST: Materials/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchItem)
        {
            
            if (searchItem == null || _context.Material == null)
            {
                return View("Index", await _context.Material.Where(c => c.Type != "Dummy Item").ToListAsync());
            }

            var materials = await _context.Material.Where(m => m.Name.Contains(searchItem)).ToListAsync();
            if (materials == null)
            {
                return NotFound();
            }

            return View("Index", materials);
        }

        // GET: Materials/Details/5
        public async Task<IActionResult> generateQrCode(int? id)
        {
            if (id == null || _context.Material == null)
            {
                return NotFound();
            }
            //remember to change the url to the correct one once the application is deployed
            string url = "https://localhost:7085/Materials/PickUp/" + id;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            //save the image within the wwwroot/images folder
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/qrCode" + id + ".png");
            qrCodeImage.Save(path);


            var materials = await _context.Material.Where(c => c.Type != "Dummy Item").ToListAsync();

            return View("Index", materials);
        }

        // GET: Materials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,PickupTime,IsPickedUp,ReturnTime")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Material == null)
            {
                return NotFound();
            }

            var material = await _context.Material.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,PickupTime,IsPickedUp,ReturnTime")] Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
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
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Material == null)
            {
                return NotFound();
            }

            var material = await _context.Material
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Material == null)
            {
                return Problem("Entity set 'KIWReservationAppContext.Material'  is null.");
            }
            var material = await _context.Material.FindAsync(id);
            if (material != null)
            {
                _context.Material.Remove(material);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET: Materials/PickUp/5
        public async Task<IActionResult> PickUp(int? id)
        {
            if (id == null || _context.Material == null)
            {
                return NotFound();
            }

            var material = await _context.Material.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        //POST: Materials/PickUp/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PickUp(int id, [Bind("Id,Type,Name,PickupTime,IsPickedUp,ReturnTime")] Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    material.IsReserved = true;

                    if(User.Identity.IsAuthenticated)
                    {
                        material.UserReserved = User.Identity.Name;
                    } else
                    {
                        material.UserReserved = "Leerling";
                    }

                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
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
            return View(material);
        }

        //GET: Materials/Return/5
        public async Task<IActionResult> Return(int? id)
        {
			if (id == null || _context.Material == null)
            {
				return NotFound();
			}

			var material = await _context.Material.FindAsync(id);
			if (material == null)
            {
				return NotFound();
			}
			return View(material);
		}

        //POST: Materials/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, [Bind("Id,Type,Name,PickupTime,IsPickedUp,ReturnTime")] Material material)
        {
			if (id != material.Id)
            {
				return NotFound();
			}

			if (ModelState.IsValid)
            {
				try
                {
					material.IsPickedUp = false;
                    material.PickupTime = null;
                    material.UserReserved = null;
					material.ReturnTime = null;
                    material.IsReturned = true;
					_context.Update(material);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
                {
					if (!MaterialExists(material.Id))
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
			return View(material);
		}

        //write a GET and POST method for uploading an excel file, and then read the file and save the data to the database
        //GET: Materials/Upload
        public IActionResult Upload()
        {
            return View();
        }

        //POST: Materials/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                //read the file and save the data to the database
                using (var stream = new MemoryStream())
                {
					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
					await formFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 1; row <= rowCount; row++)
                        {
                            Material material = new Material
                            {
                                Type = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                Name = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                SerialNumber = worksheet.Cells[row, 3].Value.ToString().Trim(),
                                IsReturned = true
                            };
                            _context.Material.Add(material);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(formFile);
        }

        private bool MaterialExists(int id)
        {
            return (_context.Material?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
