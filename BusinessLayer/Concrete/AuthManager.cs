using BusinessLayer.Abstract;
using BusinessLayer.Utilities.Hashing;
using EntityLayer.Concrete;
using EntityLayer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AuthManager:IAuthService
    {
        IAdminService _adminService;
        IWriterService _writerService;

        public AuthManager(IAdminService adminService, IWriterService writerService)
        {
            _adminService = adminService;
            _writerService = writerService;
        }

        public bool Login(LoginDto loginDto)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var userNameHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.AdminUserName));
                var user = _adminService.GetList();
                foreach (var item in user)
                {
                    if (HashingHelper.VerifyAdminHash(loginDto.AdminUserName, loginDto.AdminPassword, item.AdminUserName, item.AdminPasswordHash, item.AdminPasswordSalt))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void Register(string adminMail, string password)
        {
            byte[] userNameHash, passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(adminMail, password, out userNameHash, out passwordHash, out passwordSalt);
            var admin = new Admin
            {
                AdminUserName = userNameHash,
                AdminPasswordHash = passwordHash,
                AdminPasswordSalt = passwordSalt,
                AdminRole = "A"
            };
            _adminService.AdminAdd(admin);
        }

        public void WriterAdd(WriterEditDto writerEditDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.WriterCreatePasswordHash(writerEditDto.WriterPassword, out passwordHash, out passwordSalt);
            var writer = new Writer
            {
                WriterName = writerEditDto.WriterName,
                WriterSurname = writerEditDto.WriterSurname,
                WriterMail = writerEditDto.WriterEmail,
                WriterPasswordHash = passwordHash,
                WriterPasswordSalt = passwordSalt,
                WriterAbout = writerEditDto.WriterAbout,
                WriterImage = writerEditDto.WriterImage,
                WriterStatus = false,
                WriterTitle = writerEditDto.WriterTitle
            };
            _writerService.WriterAdd(writer);
        }

        public void WriterEdit(WriterEditDto writerEditDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.WriterCreatePasswordHash(writerEditDto.WriterPassword, out passwordHash, out passwordSalt);
            var writer = new Writer
            {
                WriterId = writerEditDto.WriterID,
                WriterMail = writerEditDto.WriterEmail,
                WriterAbout = writerEditDto.WriterAbout,
                WriterPasswordHash = passwordHash,
                WriterPasswordSalt = passwordSalt,
                WriterImage = writerEditDto.WriterImage,
                WriterName = writerEditDto.WriterName,
                WriterSurname = writerEditDto.WriterSurname,
                WriterTitle = writerEditDto.WriterTitle,
                WriterStatus = writerEditDto.WriterStatus
            };
            _writerService.WriterUpdate(writer);
        }

        public bool WriterLogin(WriterLoginDto writerLoginDto)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var writer = _writerService.GetList();
                foreach (var item in writer)
                {
                    if (HashingHelper.WriterVerifyPasswordHash(writerLoginDto.WriterPassword, item.WriterPasswordHash, item.WriterPasswordSalt))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void WriterRegister(string mail, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.WriterCreatePasswordHash(password, out passwordHash, out passwordSalt);
            var writer = new Writer
            {
                WriterMail = mail,
                WriterPasswordHash = passwordHash,
                WriterPasswordSalt = passwordSalt,
            };
            _writerService.WriterAdd(writer);
        }
    }
}
