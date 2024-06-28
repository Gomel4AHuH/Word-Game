﻿using Microsoft.Identity.Client;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManageCitizens.Models
{
    public class Citizen : INotifyPropertyChanged
    {
        private readonly int _id;
        private string _firstName;
        private string _lastName;
        private string _middleName;
        private DateTime _birthday;
        private string _city;
        private string _country;

        public Citizen(string firstName, string lastName, string middleName, DateTime birthday, string city, string country)
        {
            this._firstName = firstName;
            this._lastName = lastName;
            this._middleName = middleName;
            this._birthday = birthday;
            this._city = city;
            this._country = country;
        }

        public Citizen()
        {
            this._firstName = "";
            this._lastName = "";
            this._middleName = "";
            this._birthday = new DateTime();
            this._city = "";
            this._country = "";
        }

        public int Id { get; set; }
        public string FirstName 
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }
        public DateTime Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged("Birthday");
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }
        public string Country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
