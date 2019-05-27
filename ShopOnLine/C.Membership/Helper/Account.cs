using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C.Membership.Helper
{
    public class Account
    {
        private int _userid;
        private string _username;
        private string _userlogin;
        private string _password;
        private string _address;
        private string _sex;
        private string _email;
        private string _tel;
        private string _phone;
        private string _notes;

        private string _unitid;
        private string _unitName;
        private string _departmentid;
        private string _departmentname;
        private string _positionid;
        private string _positionname;

        public Account(int userid, string username, string userlogin, string password, string address, string sex, string email, string tel, string phone, string notes, string unitid, string unitname, string departmentid, string departmentname, string positionid, string positionname)
        {
            this._userid = userid;
            this._username = username;
            this._userlogin = userlogin;
            this._password = password;
            this._address = address;
            this._sex = sex;
            this._email = email;
            this._tel = tel;
            this._phone = phone;
            this._notes = notes;

            this._unitid = unitid;
            this._unitName = unitname;
            this._departmentid = departmentid;
            this._departmentname = departmentname;
            this._positionid = positionid;
            this._positionname = positionname;
        }

        public Account()
        {

        }


        public int Userid
        {
            get => _userid;
            set => _userid = value;
        }

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Userlogin
        {
            get => _userlogin;
            set => _userlogin = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string Sex
        {
            get => _sex;
            set => _sex = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public string Tel
        {
            get => _tel;
            set => _tel = value;
        }

        public string Phone
        {
            get => _phone;
            set => _phone = value;
        }

        public string Notes
        {
            get => _notes;
            set => _notes = value;
        }

        public string Unitid
        {
            get => _unitid;
            set => _unitid = value;
        }

        public string UnitName
        {
            get => _unitName;
            set => _unitName = value;
        }

        public string Departmentid
        {
            get => _departmentid;
            set => _departmentid = value;
        }

        public string Departmentname
        {
            get => _departmentname;
            set => _departmentname = value;
        }

        public string Positionid
        {
            get => _positionid;
            set => _positionid = value;
        }

        public string Positionname
        {
            get => _positionname;
            set => _positionname = value;
        }
    }
}
