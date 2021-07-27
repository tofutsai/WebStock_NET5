using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebStock_NET5.DB;

namespace WebStock_NET5.DAL
{
    public interface IStockMemberDAL
    {
        public member Login(member formData);
        public string Register(member formData);
    }
    public class StockMemberDAL:IStockMemberDAL
    {
        private readonly WebStockContextDTO _db;
        public StockMemberDAL(WebStockContextDTO webStockContextDTO)
        {
            _db = webStockContextDTO;
        }
        public member Login(member formData)
        {
            var sha1 = SHA1.Create();
            byte[] passwordData = Encoding.UTF8.GetBytes(formData.password);
            byte[] hashPasswordData = sha1.ComputeHash(passwordData);
            string hashPasswordResult = BitConverter.ToString(hashPasswordData).Replace("-", String.Empty).ToUpper();
           
            formData.password = hashPasswordResult;
            var member = _db.member.Where(m => m.account == formData.account && m.password == formData.password).FirstOrDefault();

            return member;

        }

        public string Register(member formData)
        {
            var check = _db.member.Where(m => m.account == formData.account).FirstOrDefault();
            if(check == null)
            {
                var sha1 = SHA1.Create();
                byte[] passwordData = Encoding.UTF8.GetBytes(formData.password);
                byte[] hashPasswordData = sha1.ComputeHash(passwordData);
                string hashPasswordResult = BitConverter.ToString(hashPasswordData).Replace("-", String.Empty).ToUpper();
                
                formData.password = hashPasswordResult;
                formData.role = "user";
                _db.member.Add(formData);
                int r = _db.SaveChanges();
                return r > 0 ? "01" : "02";
            }
            else
            {
                return "03";
            }
        }
    }
}
