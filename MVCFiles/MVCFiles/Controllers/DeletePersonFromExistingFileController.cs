using System.Web.Mvc;
using MVCFiles.Models;

namespace MVCFiles.Controllers
{
    public class DeletePersonFromExistingFileController : Controller
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

            return RedirectToAction("DisplayAllPersons", fileViewModel);
        }

        public ActionResult DisplayAllPersons(FileViewModel fileViewModel)
        {
            personViewModel.SaveFileName(fileViewModel.FileName);
            personViewModel.DisplayAllPersons(fileViewModel.FileName);

            return View(personViewModel);
        }

        public ActionResult DeletePersonConfirmation(int personId, string fileName)
        {
            personViewModel.SaveFileName(fileName);
            personViewModel.GetPersonForDelete(personId);
            return View(personViewModel);
        }

        [HttpPost]
        public ActionResult DeletePerson(PersonEntity personToDelete)
        {
            string resultXMLdelete = personViewModel.DeletePerson(personToDelete);
            TempData["DeletePerson"] = personViewModel.DeletePersonAvatar(resultXMLdelete, personToDelete);
            return RedirectToAction("Option", "Option");
        }
    }
}