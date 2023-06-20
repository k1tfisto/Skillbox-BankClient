using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Practice13
{
    class Employee: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected string lastName;
        protected string firstName;
        protected string middleName;
        protected string numberPhone;
        protected string passport;



        public Employee(string LastName, string FirstName, string MiddleName, string NumberPhone, string PassportEmpl)
        {
            this.lastName = LastName;
            this.firstName = FirstName;
            this.middleName = MiddleName;
            this.numberPhone = NumberPhone;
            this.passport = PassportEmpl;
        }


        public string LastName
        {
            get { return this.lastName; }
            set
            {
                this.lastName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.LastName)));
            }
        }
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                this.firstName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FirstName)));
            }
        }
        public string MiddleName
        {
            get { return this.middleName; }
            set
            {
                this.middleName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.MiddleName)));
            }
        }
        public string NumberPhone
        {
            get { return this.numberPhone; }
            set
            {
                this.numberPhone = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.NumberPhone)));
            }
        }
        public string PassportEmpl
        {
            get { return this.passport; }
            set
            {
                this.passport = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.PassportEmpl)));
            }
        }
    }
}
