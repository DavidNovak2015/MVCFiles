using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MVCFiles.Models
{
    public class Repository
    {
        private const string path= "C:/Users/David/Desktop/David/Programovani/MVCProjects/MVCFiles/UserFiles/";
        private const string pathForSaveAndDeleteImage = "C:/Users/David/Desktop/David/Programovani/MVCProjects/MVCFiles/MVCFiles/Content/Avatars/";
        private const string pathForDisplayImage = "/Content/Avatars/";

        private static string fileNameStatic="";
        private static List<PersonEntity> personsStatic=new List<PersonEntity>();

        public void SaveFileName(string fileName)
        {
            fileNameStatic = fileName;
        }

        public string GetFilename()
        {
            return fileNameStatic;
        }

        public string GetAvatarPath(int personId)
        {
            return pathForDisplayImage + fileNameStatic + personId + ".png";
        }

        public string AddNewPerson(PersonEntity person)
        {
            var doubleId = personsStatic.Where(p => p.Id == person.Id).LastOrDefault();
            if (doubleId == null)
            {
                personsStatic.Add(person);
                return "Die Person wurde gegründet";
            }
            return $"Die Identifizierungsnummer \"{person.Id}\" existiert schon. Die Person \"{person.Surname}\" wurde nicht gegründet";
        }

        public string SaveAllPersonsIntoNewFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<PersonEntity>));
                using (StreamWriter writer = new StreamWriter(path + fileNameStatic))
                {
                    serializer.Serialize(writer, personsStatic);
                }
                return $"Die Personen wurden in die Datei \"{fileNameStatic}\" gespeichert";
            }
            catch (Exception ex)
            {
                return ($"Die Persons wurden nicht eingespeichert! Unten Fehlerbeschreibung:\n {ex.Message.ToString()} ");
            }
        }

        public string SaveAllAvatars(string resultFromXMLSave)
        {
            try
            {
                foreach (PersonEntity avatar in personsStatic)
                {
                    avatar.Avatar.SaveAs(pathForSaveAndDeleteImage + fileNameStatic + avatar.Id + ".png");
                }
                fileNameStatic = ""; personsStatic.Clear();
                return resultFromXMLSave;
            }
            catch (Exception ex)
            {
                return $"Die Fotografien wurden nicht gespeichert! Unten Fehlerbeschreibung:\n {ex.Message.ToString()}";
            }
        }

        public List<PersonEntity> DisplayAllPersons(string fileName)
        {
            List<PersonEntity> persons = new List<PersonEntity>();
            if (File.Exists(path + fileName))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<PersonEntity>));
                    using (StreamReader reader = new StreamReader(path + fileName))
                    {
                        persons = (List<PersonEntity>)serializer.Deserialize(reader);
                        return persons;
                    }
                }
                catch (Exception ex)
                {
                    persons.Add(new PersonEntity(0, $"Die Persons aus der Datei {fileName}","wurden nicht gelesen!",DateTime.MinValue,0,$" Unten Fehlerbeschreibung:{ex.Message.ToString()}"));
                    return persons;
                }
            }
            else
            {
            persons.Add(new PersonEntity(0, $"Es ist eröffnen","nicht gelungen", DateTime.MinValue,0,string.Empty));
            return persons;
            }
        }

        public List<PersonEntity> GetSavedPersons()
        {
            return DisplayAllPersons(fileNameStatic);
        }

        public string SaveAddedPersonsIntoExistingFile()
        {
            if (File.Exists(path + fileNameStatic))
            {
                try
                {
                    XDocument document = XDocument.Load(path + fileNameStatic);
                    foreach (PersonEntity person in personsStatic)
                    {
                    document.Element("ArrayOfPersonEntity").Add(new XElement("PersonEntity", new XAttribute("Id",person.Id),
                                                                                             new XElement("Name",person.Name),
                                                                                             new XElement("Surname",person.Surname),
                                                                                             new XElement("DateOfBirth",person.DateOfBirth),
                                                                                             new XElement("AvatarPath",person.AvatarPath)
                                                                            )
                                                               );
                    }
                    document.Save(path + fileNameStatic);
                    return $"Die neue Personen wurden in die Datei \"{fileNameStatic}\" erfolgreich eingespeichert";
                }
                catch (Exception ex)
                {
                    return $"Die neue Personen in die Datei {fileNameStatic} wurden nicht eingespeichert. Unten Fehlerbeschreibung: \n{ex.Message.ToString()}";
                }
            }
            else return $"Die Datei {fileNameStatic} wurde nicht gefunden";
        }

        public PersonEntity GetPersonForDelete(int personId)
        {
            List<PersonEntity> persons = DisplayAllPersons(fileNameStatic);
            PersonEntity personToDelete = persons.Where(p => p.Id == personId).FirstOrDefault();
            return personToDelete;
        }

        public string DeletePerson(PersonEntity personToDelete)
        {
            if (File.Exists(path + fileNameStatic))
            {
                try
                {
                    XDocument document = XDocument.Load(path + fileNameStatic);
                    var personToDeletion = document.Element("ArrayOfPersonEntity").Elements("PersonEntity").Where(p => (int)p.Attribute("Id") == personToDelete.Id);
                    personToDeletion.Remove();
                    document.Save(path + fileNameStatic);
                    return $"Die Person Id:{personToDelete.Id}, Familienname: {personToDelete.Surname} wurde erfolgreich aus der Datei \"{fileNameStatic}\" gelöscht";
                }
                catch (Exception ex)
                {
                    return $"Die Person Id:{personToDelete.Id}, Familienname: {personToDelete.Surname} wurde nicht aus der Datei {fileNameStatic}  gelöscht. Unten Fehlerbeschreibung: \n{ex.Message.ToString()}";
                }
            }
            else return $"Die Datei {fileNameStatic} wurde nicht gefunden";
        }

        public string DeletePersonAvatar(string resultXMLdelete, PersonEntity personToDelete)
        {
            try
            {
                File.Delete(pathForSaveAndDeleteImage + fileNameStatic + personToDelete.Id + ".png");
                string result = $"Die Fotografie von der Person Id: {personToDelete.Id} Familienname: {personToDelete.Surname} wurde erfolgreich aus der Datei \"{fileNameStatic}\" gelöscht";
                fileNameStatic = "";
                return result;
            }
            catch (Exception ex)
            {
                return $"Die Fotografie konnte nicht gelöscht werden. Bitte löschen Sie die Datei manuell.Diese Datei befindet sich hier:\n {pathForSaveAndDeleteImage+fileNameStatic+personToDelete.Id+".png"} \n Fehlerbeschreibung:\n{ex.Message.ToString()}";
            }
        }
    }
}