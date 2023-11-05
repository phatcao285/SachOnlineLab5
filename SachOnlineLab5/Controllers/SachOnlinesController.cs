using SachOnlineLab5.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;


namespace SachOnline_Lab_3.Controllers
{
    public class SachOnlinesController : Controller
    {
        private SachOnlineEntities db = new SachOnlineEntities();

        // GET: SachOnlines
        public ActionResult Index()
        {
            var sACHes = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN).Take(8);
            return View(sACHes.ToList());

        }

        public ActionResult ChuDePartials()
        {
            var chuDeList = db.CHUDEs.ToList();
            return PartialView(chuDeList);
        }

        public ActionResult NXBPartials()
        {
            var NBXList = db.NHAXUATBANs.ToList();
            return PartialView(NBXList);
        }

        public ActionResult SachBNPartials()
        {
          var topSach = db.SACHes
         .OrderByDescending(s => s.CHITIETDATHANGs.Sum(ctdh => ctdh.SoLuong))
         .Take(6)
         .ToList();

            return PartialView(topSach);
        }


        // GET: SachOnlines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        public ActionResult SachTheoChuDe(int? id, int? page)
        {
            ViewBag.MaCD = id;
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm chủ đề theo ID
            CHUDE chuDe = db.CHUDEs.Find(id);

            if (chuDe == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách sách thuộc chủ đề và sắp xếp chúng
            var sachTheoChuDe = from s in db.SACHes where s.MaCD == id select s;
            sachTheoChuDe = sachTheoChuDe.OrderBy(s => s.TenSach); // Sắp xếp theo tên sách, bạn có thể thay đổi trường sắp xếp theo yêu cầu của mình.

            return View(sachTheoChuDe.ToPagedList(pageNumber, pageSize));
        }




        public ActionResult SachTheoNXB(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm chủ đề theo ID
            NHAXUATBAN nHAXUATBAN = db.NHAXUATBANs.Find(id);

            if (nHAXUATBAN == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách sách thuộc chủ đề
            var sachTheoNhaXuatBan = db.SACHes.Where(s => s.MaNXB == id).ToList();

            // Truyền dữ liệu chủ đề và danh sách sách vào view
            ViewBag.nhaXuatBan = nHAXUATBAN;
            return View(sachTheoNhaXuatBan);
        }

        // GET: SachOnlines/Create
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB");
            return View();
        }

        // POST: SachOnlines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSach,TenSach,MoTa,AnhBia,NgayCapNhat,SoLuongBan,GiaBan,MaCD,MaNXB")] SACH sACH)
        {
            if (ModelState.IsValid)
            {
                db.SACHes.Add(sACH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: SachOnlines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // POST: SachOnlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSach,TenSach,MoTa,AnhBia,NgayCapNhat,SoLuongBan,GiaBan,MaCD,MaNXB")] SACH sACH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sACH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: SachOnlines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        // POST: SachOnlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SACH sACH = db.SACHes.Find(id);
            db.SACHes.Remove(sACH);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
