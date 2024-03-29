﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Crud_Cs.src.db;
using Crud_Cs.src.view;
using Crud_Cs.ViewModels;

namespace Crud_Cs.src.control
{
    class PersonController
    {
        private SQLite _db;

        public PersonController()
        {
            this._db = new SQLite();
        }
        
        //Create
        public void Create
            (
                string name,
                string lastName,
                string document,
                string address,
                int age,
                string phoneNumber,
                DateTime birthDate
            )
        {
            string phoneRegex = @"\b([0-9]{2})?[9][0-9]{4}[-]?[0-9]{4}\b";
            Match match = Regex.Match(phoneNumber, phoneRegex);

            if (match.Success)
            {
                Person person = new Person(name, lastName, document, address, age, phoneNumber, birthDate);

                Person query = _db.Find(document, where: "document");
                if (query != null) throw new Exception("Pessoa já cadastrada.");

                else _db.Insert(person);
            }
            else throw new Exception("Numero de telefone invalido. " +
                                     "\nTente como os seguintes exemplos:" +
                                     "\n44912345678" +
                                     "\n1291234-5678" +
                                     "\n91234-5678");
            
        }

        //Read
        public Person Find(int id)
        {
            Person person = _db.Find(id, where:"id");

            if (person == null) throw new Exception("Pessoa não cadastrada.");
            else return person;
        }

        public List<Person> Find(string name)
        {
            List<Person> people = _db.FindMany(name);

            if (people == null) throw new Exception("Pessoa não cadastrada.");
            else return people;
        }

        //Update
        public void Update
            (
                int id,
                string name,
                string lastName,
                string document,
                string address,
                int age,
                string phoneNumber,
                DateTime birthDate
            )
        {
            Person person = new Person(id, name, lastName, document, address,age, phoneNumber,birthDate);

            _db.Update(person);
        }
        
        //Delete
        public void Delete( int id )
        {
            Person person = _db.Find(id, where: "id");

            if (person == null) throw new Exception("Pessoa não cadastrada.");
            else _db.Delete(id);
        }

        //List
        public List<Person> List(string query = null)
        {
            List<Person> people = new List<Person>();

            people = _db.FindMany(query);
            if (people == null) throw new Exception("Não existem cadastros.") ;

            return people;
        }
    }
}
