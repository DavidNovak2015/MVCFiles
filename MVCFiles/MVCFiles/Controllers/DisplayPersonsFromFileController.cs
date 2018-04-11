using System.Web.Mvc;
using MVCFiles.Models;

namespace MVCFiles.Controllers
{
    public class DisplayPersonsFromFileController : Controller
    {
        FileViewModel fileViewModel = new FileViewModel();
        PersonViewModel personViewModel = new PersonViewModel();

        public ActionResult GetFileName()
        {
            fileViewModel.GetFileNames();
            return View(fileViewModel);
        }

        [HttpPost]
        public ActionResult DisplayPersons(FileViewModel fileViewModel)
        {
            personViewModel.DisplayAllPersons(fileViewModel.FileName);
            return View(personViewModel);
        }
    }
}