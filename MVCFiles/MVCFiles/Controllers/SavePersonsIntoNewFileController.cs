using System.Web.Mvc;
using MVCFiles.Models;

namespace MVCFiles.Controllers
{
    public class SavePersonsIntoNewFileController : Controller
    {
        PersonViewModel personViewModel = new PersonViewModel();

        public ActionResult NameOfNewFile()
        {
            FileViewModel fileViewModel = new FileViewModel();
            return View(fileViewModel);
        }

        [HttpPost]
        public ActionResult NameOfNewFile(FileViewModel fileViewModel)
        {
            if (!ModelState.IsValid)
                return View(fileViewModel);

            TempData["FileName"] = $"Die Dateiname \"{fileViewModel.FileName}\" wurde gespeichert";
            return RedirectToAction("CreateNewPerson",fileViewModel);
        }

        public ActionResult CreateNewPerson(FileViewModel fileViewModel)
        {
            personViewModel.SaveFileName(fileViewModel.FileName);
            return View(personViewModel);
        }

        [HttpPost]
        public ActionResult CreateNewPerson(PersonViewModel personViewModel)
        {
            if (!ModelState.IsValid)
                return View(personViewModel);

            TempData["AddPerson"] = personViewModel.AddNewPerson(personViewModel);
            return RedirectToAction("CreateNewPerson",personViewModel);
        }
        public ActionResult SaveAllPersonsIntoNewFile()
        {
            string resultFromXMLSave = personViewModel.SaveAllPersonsIntoNewFile();
            TempData["SavePersons"] = personViewModel.SaveAllAvatars(resultFromXMLSave);
            return RedirectToAction("Option","Option");
        }
    }
}