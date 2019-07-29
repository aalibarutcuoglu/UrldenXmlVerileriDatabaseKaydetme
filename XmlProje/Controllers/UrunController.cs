using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using XmlProje.Models;
using System.Xml;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;


namespace ProjeXml.UI.Web.Controllers
{
    public class UrunController : Controller
    {
        private XmlProjeDBContext db = new XmlProjeDBContext();

        public ActionResult Index(int? page)
        {
            XmlDocument doc1 = new XmlDocument();
            doc1.Load("https://www.korayspor.com/grisport.xml");

            XmlElement root = doc1.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("urun");

            foreach (XmlNode node in nodes)
            {
                int urunid = Convert.ToInt32(node["UrunID"].InnerText);

                if (!db.Urun.Any(q => q.ID == urunid))
                {
                    Urun urun = new Urun();
                    urun.ID = urunid;
                    urun.UrunAdi = node["UrunAdi"].InnerText;
                    urun.Marka = node["Marka"].InnerText;
                    urun.Fiyat = Convert.ToDecimal(node["Fiyat"].InnerText);
                    urun.Renk = node["Renk"].InnerText;
                    urun.Aciklama = node["Aciklama"].InnerText;

                    db.Urun.Add(urun);
                    db.SaveChanges();

                    XmlNodeList nodes2 = node.ChildNodes;
                    foreach (XmlNode st in nodes2)
                    {
                        if (st.Name == "Stoklar")
                        {
                            foreach (XmlNode st2 in st.ChildNodes)
                            {
                                Stok stok = new Stok();
                                stok.Kod = st["Barkod"].InnerText;
                                stok.Label = st["label"].InnerText;
                                stok.Ozellik = st["Ozellik"].InnerText;
                                stok.UrunID = urunid;

                                db.Stok.Add(stok);
                                db.SaveChanges();
                            }
                        }
                    }
                }

            }
            return View(db.Urun.ToList().ToPagedList(page ?? 1, 10));
        }

        public ActionResult Stok(int urunID)
        {
            int id = Convert.ToInt32(urunID);

            List<Stok> stok = db.Stok.Where(x => x.UrunID == id).ToList();

            return View(stok);
        }

        public ActionResult Detay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urun urun = db.Urun.Find(id);
            if (urun == null)
            {
                return HttpNotFound();
            }
            return View(urun);
        }


        public ActionResult Ekle()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ekle([Bind(Include = "ID,UrunAdi,Kod,Fiyat,Marka,Aciklama,Renk")] Urun urun)
        {
            if (ModelState.IsValid)
            {
                db.Urun.Add(urun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(urun);
        }


        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urun urun = db.Urun.Find(id);
            if (urun == null)
            {
                return HttpNotFound();
            }

            return View(urun);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle([Bind(Include = "ID,UrunAdi,Kod,Fiyat,ListFiyat,KdvOran,Marka,Aciklama,ImageName,Renk")] Urun urun)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urun).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(urun);
        }


        public ActionResult Sil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urun urun = db.Urun.Find(id);
            if (urun == null)
            {
                return HttpNotFound();
            }

            return View(urun);
        }


        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public ActionResult Sil(int id)
        {
            Urun urun = db.Urun.Find(id);
            db.Urun.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
