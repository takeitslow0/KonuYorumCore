﻿using KonuYorumCore.DataAccess;
using KonuYorumCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace KonuYorumCore.Controllers
{
    public class KonularYorumlarJoinController : Controller
    {

        BA_KonuYorumCoreContext _db = new BA_KonuYorumCoreContext();
        /*
            select k.Baslik, k.Aciklama, y.Icerik, y.Yorumcu, y.Puan,
            case when y.Puan < 3 then 'Kötü' when y.Puan = 3 then 'Orta' else 'İyi' end as PuanDurum
            from Konu k inner join Yorum y 
            on k.Id = y.KonuId
            order by y.Puan desc, y.Yorumcu
         */
        public IActionResult InnerJoin()
        {
            IQueryable<Konu> konuQuery = _db.Konu.AsQueryable(); // select * from Konu
            IQueryable<Yorum> yorumQuery = _db.Yorum.AsQueryable(); // select * from Yorum
            var joinQuery = from konu in konuQuery
                            join yorum in yorumQuery
                            on konu.Id equals yorum.KonuId
                            //where kq.Id= 5
                            orderby yorum.Puan descending, yorum.Yorumcu
                            select new KonuYorumInnerJoinModel()
                            {
                                Aciklama = konu.Aciklama,
                                Baslik = konu.Baslik,
                                Icerik = yorum.Icerik,
                                Yorumcu = yorum.Yorumcu,
                                Puan = yorum.Puan,


                                PuanDurumu = yorum.Puan < 3 ? "Kötü" : yorum.Puan == 3 ? "Orta" : "İyi"
                            };

            var model = joinQuery.ToList();
            return View(model);
        }
        //     ---------------------------------------------------------------------------------------------------------------------------------------------
        /*
         select k.Baslik, k.Aciklama, y.Icerik, y.Yorumcu, y.Puan,
        case when y.Puan < 3 then 'Kötü' when y.Puan = 3 then 'Orta' else 'İyi' end as PuanDurum
        from Konu k left outer join Yorum y 
        on k.Id = y.KonuId
        order by y.Puan desc, y.Yorumcu
         */
        public IActionResult LeftOuterJoin()
        {
            IQueryable<Konu> konuQuery = _db.Konu.AsQueryable();
            IQueryable<Yorum> yorumQuery = _db.Yorum.AsQueryable();
            var joinQuery = from konu in konuQuery
                            join yorum in yorumQuery
                            on konu.Id equals yorum.KonuId into konuYorumJoin
                            from subKonuYorumJoin in konuYorumJoin.DefaultIfEmpty()
                                //where konu.Id == 5
                            orderby subKonuYorumJoin.Puan descending, subKonuYorumJoin.Yorumcu
                            select new KonuYorumLeftOuterJoinModel()
                            {
                                Baslik = konu.Baslik,
                                Aciklama = konu.Aciklama,
                                Icerik = subKonuYorumJoin.Icerik,
                                Puan= subKonuYorumJoin.Puan,
                                Yorumcu = subKonuYorumJoin.Yorumcu
                            };

            var model = joinQuery.ToList();
            return View(model);

        }
    }
}
