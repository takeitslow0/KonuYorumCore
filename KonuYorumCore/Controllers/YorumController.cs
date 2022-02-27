using KonuYorumCore.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KonuYorumCore.Controllers
{
    public class YorumController : Controller
    {
        private BA_KonuYorumCoreContext _db = new BA_KonuYorumCoreContext();

        public IActionResult Index()
        {
            // SQL=> select * from Yorum order by Puan desc, Yorumcu
            List<Yorum> yorumlar = _db.Yorum.Include(yorum => yorum.Konu).OrderByDescending(yorum => yorum.Puan).ThenBy(yorum => yorum.Yorumcu).ToList();

            // Bir  Orderby kullanıldıktan sonra diğerlerinin hepsi ThenBy olmalıdır.
            return View(yorumlar);
        }
        public IActionResult Details(int id)
        {
            //2.Yöntem
            Yorum yorumlar = _db.Yorum.Include(yorum => yorum.Konu).SingleOrDefault(yorum => yorum.Id == id);
            //1.Yöntem
            //Yorum yorum = _db.Yorum.Find(id);
            return View(yorumlar);
        }
        public IActionResult Create()
        {
            List<Konu> konular = _db.Konu.OrderBy(konu => konu.Baslik).ToList();

            //ViewData ile ViewBag birbirlerinin yerine kullanılabilir sadece yazımları farklı! ! 
            //ViewData["KonuId"] = new SelectList(konular, "Id", "Baslik");  // SelectList = DropDownList, MultiSelectList = ListBox
            ViewBag.KonuId = new SelectList(konular, "Id", "Baslik");  // SelectList = DropDownList, MultiSelectList = ListBox

            return View();
        }

        [HttpPost]
        public IActionResult Create(Yorum yorum)
        {
            if (string.IsNullOrWhiteSpace(yorum.Icerik))
            {
                ViewBag.Mesaj = "İçerik boş girilemez !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Icerik.Length > 500)
            {
                ViewBag.Mesaj = "İçerik en fazla 500 karakter olmalıdır !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (string.IsNullOrWhiteSpace(yorum.Yorumcu))
            {
                ViewBag.Mesaj = "Yorumcu boş girilemez !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Yorumcu.Length > 50)
            {
                ViewBag.Mesaj = "Yorumcu en fazla 50 karakter olmalıdır !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            //if (yorum.Puan != null)
            if (yorum.Puan.HasValue)
            {
                //if (yorum.Puan.Value > 5 || yorum.Puan.Value < 1)
                if (!(yorum.Puan.Value >= 1 && yorum.Puan.Value <= 5))
                {
                    ViewBag.Mesaj = "Puan 1 ile 5 arasında olmalıdır !";
                    ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                    return View(yorum);
                }
            }
            _db.Yorum.Add(yorum);
            _db.SaveChanges();
            TempData["YorumMesaj"] = "Yorum Başarıyla eklendi !";
            return RedirectToAction("Index");
        }
        
        public IActionResult Edit(int id)
        {
            Yorum yorum = _db.Yorum.SingleOrDefault(yorum => yorum.Id == id);
            ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(konu => konu.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
            return View(yorum);
        }
        [HttpPost]
        public IActionResult Edit(Yorum yorum)
        {

            if (string.IsNullOrWhiteSpace(yorum.Icerik))
            {
                ViewBag.Mesaj = "İçerik boş girilemez !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Icerik.Length > 500)
            {
                ViewBag.Mesaj = "İçerik en fazla 500 karakter olmalıdır !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (string.IsNullOrWhiteSpace(yorum.Yorumcu))
            {
                ViewBag.Mesaj = "Yorumcu boş girilemez !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }
            if (yorum.Yorumcu.Length > 50)
            {
                ViewBag.Mesaj = "Yorumcu en fazla 50 karakter olmalıdır !";
                ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                return View(yorum);
            }

            if (yorum.Puan.HasValue)
            {

                if (!(yorum.Puan.Value >= 1 && yorum.Puan.Value <= 5))
                {
                    ViewBag.Mesaj = "Puan 1 ile 5 arasında olmalıdır !";
                    ViewBag.KonuId = new SelectList(_db.Konu.OrderBy(k => k.Baslik).ToList(), "Id", "Baslik", yorum.KonuId);
                    return View(yorum);
                }
             
            }
            Yorum mevcutYorum = _db.Yorum.SingleOrDefault(mevcutYorum => mevcutYorum.Id == yorum.Id);
            mevcutYorum.Icerik = yorum.Icerik;
            mevcutYorum.Konu = yorum.Konu;
            mevcutYorum.Yorumcu = yorum.Yorumcu;
            mevcutYorum.Puan = yorum.Puan;
            _db.Yorum.Update(mevcutYorum);
            _db.SaveChanges();
            TempData["YorumMesaj"] = "Yorum Başarıyla güncellendi !";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Yorum yorum = _db.Yorum.Include(yorum => yorum.Konu).SingleOrDefault(yorum => yorum.Id == id);
            return View(yorum);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult Melik(int id) //DeleteConfirmed
        {
            Yorum yorum = _db.Yorum.Find(id);
            _db.Yorum.Remove(yorum);
            _db.SaveChanges();
            TempData["YorumMesaj"] = "Yorum başarıyla silindi. ";
            return RedirectToAction("Index");

        }
    }
}
