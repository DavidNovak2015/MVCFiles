using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Xml.Serialization;

namespace MVCFiles.Models
{
    public class PersonEntity
    {
        [Display(Name= "Identifikationsnummer")]
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [Display(Name ="Vorname")]
        public string Name { get; set; }

        [Display(Name ="Familienname")]
        public string Surname { get; set; }

        [Display(Name ="Geburtsdatum")]
        public DateTime DateOfBirth { get; set; }

        public string AvatarPath { get; set; }

        [Display(Name ="Alter")]
        [XmlIgnore]
        public int Age { get; private set; }

        [XmlIgnore]
        public HttpPostedFileBase Avatar { get; set; }

        public PersonEntity() // for XMLserializer
        { }
        public PersonEntity(int id,string name, string surname,DateTime dateOfBirth, string avatarPath,HttpPostedFileBase avatar) // saving a person to file
        {
            Id = id;
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            AvatarPath = avatarPath;
            Avatar = avatar;
        }
        public PersonEntity(int id, string name, string surname, DateTime dateOfBirth,int age, string avatarPath) // display a person from file
        {
            Id = id;
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            Age = age;
            AvatarPath = avatarPath;
        }
    }
}