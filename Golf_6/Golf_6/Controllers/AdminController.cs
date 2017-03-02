using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf_6.Models;
using Golf_6.ViewModels;

namespace Golf_6.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //GET: Admin/RegistreraNyMedlem
        [AllowAnonymous]
        public ActionResult RegistreraNyMedlem()
        {
            var viewModel = new NyMedlemViewModel();

            return View(viewModel);
        }

        // POST: Admin/RegistreraNyMedlem
        [HttpPost]
        [AllowAnonymous]
        //public ActionResult RegistreraNyMedlem(string fnamn, string enamn)
        public ActionResult RegistreraNyMedlem(NyMedlemViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();

            admin.RegistreraNyMedlem(viewModel.RegistreraNyMedlem.Fornamn, viewModel.RegistreraNyMedlem.Efternamn,
                viewModel.RegistreraNyMedlem.Adress, viewModel.RegistreraNyMedlem.Postnummer, viewModel.RegistreraNyMedlem.Ort,
                viewModel.RegistreraNyMedlem.Email, viewModel.RegistreraNyMedlem.Kon, viewModel.RegistreraNyMedlem.Handikapp,
                viewModel.RegistreraNyMedlem.GolfID, viewModel.RegistreraNyMedlem.MedlemsKategori, 
                viewModel.RegistreraNyMedlem.Telefonnummer);
            return View("Index");
        }

        // GET: Admin/RedigeraMedlem
        [AllowAnonymous]
        public ActionResult RedigeraMedlem()
        {
            //DENNA METOD SKA LIGGA I VYN FÖR ATT SÖKA EFTER GOLFID
            //MÅSTE ÄNDRA DE HÅRDKODADE NAMNEN SOM PARAMETRAR SEN
            Admin admin = new Admin();
            List<Admin> medlemsLista = new List<Admin>();
            medlemsLista = admin.GetMedlemmen("Maria", "Rodriguez");
            return View();
            
        }

        //GET: Admin/HanteraSasong
        [AllowAnonymous]
        public ActionResult HanteraSasong()
        {
            var viewModel = new HanteraSasongViewModel();

            return View(viewModel);
        }

        // POST: Admin/HanteraSasong
        [HttpPost]
        [AllowAnonymous]
        public ActionResult HanteraSasong(HanteraSasongViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();

            admin.HanteraSasong(viewModel.HanteraSasong.SasongStart, viewModel.HanteraSasong.SasongSlut);

            return View("Index");
        }

    }
}