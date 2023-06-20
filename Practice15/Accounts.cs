using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice13
{
    class Accounts
    {
        string passport;
        double accountNumber;
         bool deposit;
        double accountAmount;
        bool accountStatus;



        public Accounts(string Passport, double AccountNumber, bool Deposit, double AccountAmount, bool AccountStatus)
        {
            this.passport = Passport;
            this.accountNumber = AccountNumber;
            this.deposit = Deposit;
            this.accountAmount = AccountAmount;
            this.accountStatus = AccountStatus;
        }

        public string Passport { get { return this.passport; } set { this.passport = value; } }
        public string AccountNumber { get { return Convert.ToString(this.accountNumber); } set { this.accountNumber = Convert.ToDouble(value); } }
        public string Deposit { get { return Convert.ToString(this.deposit); } set { this.deposit = Convert.ToBoolean(value); } }
        public string AccountAmount { get { return Convert.ToString(this.accountAmount); } set { this.accountAmount = Convert.ToDouble(value); } }
        public string AccountStatus { get { return Convert.ToString(this.accountStatus); } set { this.accountStatus = Convert.ToBoolean(value); } }
    }
}
