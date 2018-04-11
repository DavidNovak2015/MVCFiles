using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MVCFiles.Models
{
    public class FileViewModel
    {
        [Display(Name ="Name der Datei")]
        [Required(ErrorMessage ="Geben Sie bitte eine Name neuer Datei ein")]
        public string FileName { get; set; }

        public List<SelectListItem> Files { get; private set; }

        public void GetFileNames()
        {
            List<string> foundPathsFiles = Directory.EnumerateFiles("C:/Users/David/Desktop/David/Programovani/MVCProjects/MVCFiles/UserFiles/").ToList();
            List<string> foundNamesFiles = new List<string>();
            foreach (string namePathFile in foundPathsFiles)
            {
                foundNamesFiles.Add(Path.GetFileName(namePathFile));
            }

            if (Files == null)
                Files = new List<SelectListItem>();
            Files.Clear();

            foreach (string fileName in foundNamesFiles)
            {
                Files.Add(new SelectListItem { Text=fileName, Value=fileName});
            }

            if (foundNamesFiles.Count == 0)
                Files.Add(new SelectListItem { Text = "Keine Datei wurde gefunden", Value = null });
        }
    }
}