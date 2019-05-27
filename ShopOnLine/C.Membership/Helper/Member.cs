using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace C.Membership.Helper
{
    public class Member
    {
        private string _MemberGuid;
        private int _MemberId;
        private string _MemberName;
        private string _MemberLogin;
        private string _Password;
        private string _ImageFile;
        private string _Address;
        private int _Sex;
        private string _Email;
        private string _Tel;
        private DateTime _BirthDay;
        private string _Phone;
        private string _Notes;

        public string MemberGuid { get => _MemberGuid; set => _MemberGuid = value; }
        public int MemberId { get => _MemberId; set => _MemberId = value; }
        public string MemberName { get => _MemberName; set => _MemberName = value; }
        public string MemberLogin { get => _MemberLogin; set => _MemberLogin = value; }
        public string Password { get => _Password; set => _Password = value; }
        public string ImageFile { get => _ImageFile; set => _ImageFile = value; }
        public string Address { get => _Address; set => _Address = value; }
        public int Sex { get => _Sex; set => _Sex = value; }
        public string Email { get => _Email; set => _Email = value; }
        public string Tel { get => _Tel; set => _Tel = value; }
        public DateTime BirthDay { get => _BirthDay; set => _BirthDay = value; }
        public string Phone { get => _Phone; set => _Phone = value; }
        public string Notes { get => _Notes; set => _Notes = value; }

        public Member()
        {

        }

        public Member(string MemberGuid, int MemberId, string MemberName, string MemberLogin, string Pasword, string ImageFile, string Address, int Sex, string Email, string Tel, DateTime BirthDay, string Phone, string Notes)
        {
            _MemberGuid = MemberGuid;
            _MemberId = MemberId;
            _MemberName = MemberName;
            _MemberLogin = MemberLogin;
            _Password = Pasword;
            _ImageFile = ImageFile;
            _Address = Address;
            _Sex = Sex;
            _Email = Email;
            _Tel = Tel;
            _BirthDay = BirthDay;
            _Phone = Phone;
            _Notes = Notes;

        }
    }
}
