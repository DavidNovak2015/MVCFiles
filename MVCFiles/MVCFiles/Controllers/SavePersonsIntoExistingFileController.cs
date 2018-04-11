using System.Web.Mvc;
using MVCFiles.Models;

namespace MVCFiles.Controllers
{
    public class SavePersonsIntoExistingFileController : Controller
    {
        FileViewModel fileViewModel = new FileViewModel();
        PersonViewModel personViewModel = new PersonViewModel();
         
        public ActionResult GetFileName()
        {
            fileViewModel.GetFileNames();
            return View(fileViewModel);
        }

        [HttpPost]
        public ActionResult GetFileName(FileViewModel fileViewModel)
        {
            if (!ModelState.IsValid)
                return View(fileViewModel);

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

            bool error=personViewModel.CheckDuplicateId(personViewModel.Id);
            if (error)
            {
                ModelState.AddModelError(nameof(personViewModel.Id), $"Entweder die Identifizierungsnummer {personViewModel.Id} in der Datei \"{personViewModel.FileName}\" schon besteht, oder die Kontrolle konnte nicht durchgeführt werden. Überprüfen Sie mithilfe des Angebotes \"Datei {personViewModel.FileName} eröffnen\"");
                return View(personViewModel);
            }
            TempData["AddPerson"] = personViewModel.AddNewPerson(personViewModel);
            return View(personViewModel);
        }

        public ActionResult SaveAddedPersonsIntoExistingFile()
        {
            string resultFromXMLSave = personViewModel.SaveAddedPersonsIntoExistingFile();
            TempData["SavePersons"] = personViewModel.SaveAllAvatars(resultFromXMLSave);
            return RedirectToAction("Option","Option");
        }
    }
}