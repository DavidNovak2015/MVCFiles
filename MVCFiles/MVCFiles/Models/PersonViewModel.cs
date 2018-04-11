using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCFiles.Models
{
    public class PersonViewModel
    {
        Repository repository = new Repository();

        [Display(Name="Identifikationsnummer")]
        [Required(ErrorMessage ="Geben Sie bitte die Idendifikationsnummer ein")]
        public int? Id { get; set; }

        [Display(Name="Vorname")]
        [Required(ErrorMessage ="Geben Sie bitte die Vorname ein")]
        public string Name { get; set; }

        [Display(Name="Familienname")]
        [Required(ErrorMessage ="Geben Sie bitte die Familienname ein")]
        public string Surname { get; set; }

        [Display(Name ="Alter")] // for Attributte only
        public int Age { get; }

        [Display(Name ="Geburtsdatum")]
        [Required(ErrorMessage ="Geben Sie bitte das Geburtsdatum ein")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name ="Wählen bitte Sie eine Fotografie im .png Format aus")]
        [Required(ErrorMessage = "Wählen bitte Sie eine Fotografie im .png Format aus")]
        public HttpPostedFileBase Avatar { get; set; }

        public List<PersonEntity> Persons { get; private set; } // for saving to files
        public List<PersonEntity> PersonsWithAge { get; private set; } // for display from file

        public PersonEntity PersonToDelete { get; private set; }

        public string FileName { get; private set; }

        private int GetAge(DateTime dateOfBirth)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (DateTime.Today < dateOfBirth.AddYears(age)) age--;
            return age;
        }

        public void SaveFileName(string fileName)
        {
            repository.SaveFileName(fileName);
            FileName = fileName;
        }

        private int GetId()
        {
            if (!Id.HasValue)
                throw new InvalidOperationException($"{nameof(Id)} is empty");

            return Id.Value;
        }

        private DateTime GetDateOfBirth()
        {
            if (!DateOfBirth.HasValue)
                throw new InvalidOperationException($"{nameof(DateOfBirth)} is empty");

            return DateOfBirth.Value;
        }
        private string GetAvatarPath(int personId)
        {
            return repository.GetAvatarPath(personId);
        }

        public string AddNewPerson(PersonViewModel personViewModel)
        {
            PersonEntity person = new PersonEntity(GetId(), Name, Surname, GetDateOfBirth(),GetAvatarPath(GetId()),personViewModel.Avatar);
            FileName = repository.GetFilename();
            return repository.AddNewPerson(person);
        }

        public string SaveAllPersonsIntoNewFile()
        {
            return repository.SaveAllPersonsIntoNewFile();
        }
        public string SaveAllAvatars(string resultFromXMLSave)
        {
            return repository.SaveAllAvatars(resultFromXMLSave);
        }

        public void DisplayAllPersons(string fileName)
        {
            if (Persons == null)
                Persons = new List<PersonEntity>();
            Persons.Clear();

            if (PersonsWithAge == null)
                PersonsWithAge = new List<PersonEntity>();
            PersonsWithAge.Clear();

            FileName = fileName;

            Persons = repository.DisplayAllPersons(fileName);

            PersonsWithAge = Persons.Select(x => new PersonEntity(x.Id, x.Name, x.Surname, x.DateOfBirth, GetAge(x.DateOfBirth),x.AvatarPath)).ToList();

            if (PersonsWithAge.Count == 0)
                PersonsWithAge.Add(new PersonEntity(0, $"Keine Personen"," wurden gefunden",DateTime.MinValue,0,string.Empty));
        }

        public bool CheckDuplicateId(int? requestedId)
        {
            if (Persons == null)
                Persons = new List<PersonEntity>();
            Persons.Clear();

            FileName = repository.GetFilename();
            Persons = repository.GetSavedPersons();
            bool error = false;
            foreach (PersonEntity person in Persons)
            {
                if ((person.Id == requestedId) || (person.Id == 0))
                {
                    error = true;return error;
                }
            }
            return error;
        }
        public string SaveAddedPersonsIntoExistingFile()
        {
            return repository.SaveAddedPersonsIntoExistingFile();
        }
        public void GetPersonForDelete (int personId)
        {
            PersonToDelete = new PersonEntity();
            PersonEntity personFromFile = repository.GetPersonForDelete(personId);
            PersonToDelete = new PersonEntity(personFromFile.Id, personFromFile.Name, personFromFile.Surname, personFromFile.DateOfBirth, GetAge(personFromFile.DateOfBirth),personFromFile.AvatarPath);
        }
        public string DeletePerson(PersonEntity personToDelete)
        {
            return repository.DeletePerson(personToDelete);
        }

        public string DeletePersonAvatar(string resultXMLdelete, PersonEntity personToDelete)
        {
            return repository.DeletePersonAvatar(resultXMLdelete, personToDelete);
        }
    }
}