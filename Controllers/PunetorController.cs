using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models; 

namespace WebApplication.Controllers
{
    public class PunetorController : Controller
    {
        // GET: Punetor
       public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                List<Punetor> empList = db.Punetors.ToList<Punetor>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Punetor());
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.Punetors.Where(x => x.PunetorID==id).FirstOrDefault<Punetor>());
                }
            }
        }
        //Editimi dhe shtimi behen duke therritur te njejten url, brenda funksioni
        [HttpPost]
        public ActionResult AddOrEdit(Punetor emp)
        {
            using (DBModel db = new DBModel())
            {
                if (emp.PunetorID == 0)
                {
                    db.Punetors.Add(emp);
                    db.SaveChanges();
                
                    return Json(new { success = true, message = "U ruajt me sukses "}, JsonRequestBehavior.AllowGet);
                }
                else {
                    if(EditPunetor(emp))
                        return Json(new { success = true, message = "U ruajt me sukses " }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { success = false, message = "Fatkeqsisht nuk u ruajt " }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //Funksioni boolean qe kthen true dhe ruan te dhenat e databazes kur behet ndonje modifikim
        public bool EditPunetor(Punetor emp)
        {
            try
            {
                using (DBModel db = new DBModel())
                {
                    var result = db.Punetors.Where(p => p.PunetorID == emp.PunetorID).FirstOrDefault();
                    db.Entry(result).State = System.Data.Entity.EntityState.Modified;
                    result.Emri = emp.Emri;
                    result.Pozicion = emp.Pozicion;
                    result.Office = emp.Office;
                    result.Mosha = emp.Mosha;
                    result.Rroga = emp.Rroga;
                    db.SaveChanges();
                    return true;
                }
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return false;
            }
        }
        //Metoda post per fshirjen- Behet kontrolli nese id e derguar me post eshte valide,
        //nese, po realizohet fshirja
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (DBModel db = new DBModel())
            {
                Punetor emp = db.Punetors.Where(x => x.PunetorID == id).FirstOrDefault<Punetor>();
                db.Punetors.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "U fshi me sukses" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}