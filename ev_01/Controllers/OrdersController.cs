using ev_01.Models;
using ev_01.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ev_01.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ResturantDBcontext _db;
        private readonly IWebHostEnvironment _he;

        public OrdersController(ResturantDBcontext context, IWebHostEnvironment he)
        {
            this._db = context;
            this._he = he;  
        }
        public async Task<IActionResult> Index()
        {

            return View(await _db.Orders.Include(o => o.Receipts).ThenInclude(r => r.Food).ToListAsync());
        }

        public IActionResult addNewFood(int? id)
        {
            ViewBag.food = new SelectList(_db.Foods.ToList(), "FoodId", "FoodName", (id != null) ? id.ToString() : "");

            return PartialView("_addNewFood");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderVM orderVM, int[] foodID)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    CustomerName = orderVM.CustomerName,
                    OrderDate = orderVM.OrderDate,
                    TableNumber = orderVM.TableNumber,
                    AmountPaid = orderVM.AmountPaid
                };

                //image
                var file = orderVM.PictureFile;
                string webroot = _he.WebRootPath;
                string folder = "Images";

                string imgFileName = Path.GetFileName(orderVM.PictureFile.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);

                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        orderVM.PictureFile.CopyTo(stream);
                        order.Picture = "/" + folder + "/" + imgFileName;
                    }
                }
                foreach (var item in foodID)
                {
                    Receipt receipt = new Receipt()
                    {
                        Order = order,
                        OrderId = order.OrderId,
                        FoodId = item
                    };
                    _db.Receipts.Add(receipt);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.OrderId == id);

            OrderVM orderVM = new OrderVM() 
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                Picture = order.Picture,
                OrderDate = order.OrderDate,
                TableNumber = order.TableNumber,
                AmountPaid = order.AmountPaid
            };
            var existFood = _db.Receipts.Where(r => r.OrderId == id).ToList();

            foreach (var item in existFood)
            {
                orderVM.FoodList.Add(item.FoodId);
            }
            return View(orderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(OrderVM orderVM, int[] foodID)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    OrderId = orderVM.OrderId,
                    CustomerName = orderVM.CustomerName,
                    OrderDate = orderVM.OrderDate,
                    TableNumber = orderVM.TableNumber,
                    AmountPaid = orderVM.AmountPaid,
                    Picture = orderVM.Picture
                };
                var file = orderVM.PictureFile;
                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(orderVM.PictureFile.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        orderVM.PictureFile.CopyTo(stream);
                        order.Picture = "/" + folder + "/" + imgFileName;
                    }
                }

                var existSkill = _db.Receipts.Where(x => x.OrderId == order.OrderId).ToList();

                foreach (var item in existSkill)
                {
                    _db.Receipts.Remove(item);
                }
                foreach (var item in foodID)
                {
                    Receipt receipt = new Receipt()
                    {

                        OrderId = order.OrderId,
                        FoodId = item
                    };
                    _db.Receipts.Add(receipt);
                }
                _db.Update(order);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {

            var order = _db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);

        }

        private IActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ActionName("Delete")]

        public IActionResult Delete(int id)
        {
            Order order = _db.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }
            _db.Entry(order).State = EntityState.Deleted;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
