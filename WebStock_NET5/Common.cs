using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebStock_NET5
{
    public class Common
    {
        public class UserInfo
        {
            //public int operId { get; set; }
            //public string operAccount { get; set; }
            //public string operName { get; set; }
            //public bool isAdmin { get; set; }
            //public string JWToken { get; set; }
            public string JWToken { get; set; }
            public int OperId { get; set; }
            public string OperAccount { get; set; }
            public string OperName { get; set; }
            public string OperRole { get; set; }
            public bool OperIsAdmin { get; set; }
        }

        public class FormSearch
        {
            public FormSearch()
            {
                Options o = new Options();
                options = o;
            }

            public string account { get; set; }
            public string password { get; set; }
            public int operId { get; set; }
            public string type { get; set; }
            public string code { get; set; }
            public int shares { get; set; }
            public double closePrice { get; set; }
            public double position1 { get; set; }
            public double position2 { get; set; }
            public DateTime dataDate { get; set; }
            public Options options { get; set; }
        }
        public class Options
        {
            public bool[] sortDesc { get; set; }
            public string[] sortBy { get; set; }
            public bool sortDescBool { get; set; }
            public string sortByStr { get; set; }
            public int page { get; set; }
            public int itemsPerPage { get; set; }

        }

        public class Results<T>
        {
            /// 回傳訊息
            public string Message { get; set; }

            /// 是否成功
            public bool Success { get; set; }

            /// 狀態碼
            public string Code { get; set; }

            /// 資料內容
            private T data;
            public virtual T Data
            {
                get
                {
                    return data;
                }
                set
                {
                    data = value;
                }
            }
            /// 總筆數
            public int? TotalCount;

        }

        public class stockDataAPI
        {
            public string stat { get; set; }
            public string date { get; set; }
            public List<List<string>> data8 { get; set; }
            public List<List<string>> data9 { get; set; }

        }

        public class otcDataAPI
        {
            public string stat { get; set; }
            public string reportDate { get; set; }
            public string iTotalRecords { get; set; }
            public List<List<string>> aaData { get; set; }
        }
        public class RequestObject
        {
            public string senderId { get; set; }
            public string receiverId { get; set; }
            public int operId { get; set; }
            public string user { get; set; }
            public string message { get; set; }
            public string group { get; set; }
        }
        public class ReponseJson
        {
            public string senderId { get; set; }
            public string receiverId { get; set; }
            public int operId { get; set; }
            public string user { get; set; }
            public string message { get; set; }
            public string group { get; set; }
            public string clientType { get; set; }
        }

        public class EditPas
        {
            public int OperId { get; set; }
            public string oldPassword { get; set; }
            public string newPassword { get; set; }
            public string newPasswordCheck { get; set; }
        }

        public class EditMemo
        {
            public string type { get; set; }
            public string codes { get; set; }
            public string memoContent { get; set; }
        }

        public static string TokenSecretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        public static string EncodeJWTToken(object payload)
        {

            var secret = TokenSecretKey;

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();//加密方式
            IJsonSerializer serializer = new JsonNetSerializer();//序列化Json
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();//base64加解密
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);//JWT編碼

            var token = encoder.Encode(payload, secret);
            //Console.WriteLine(token);

            return token;
        }

        public static UserInfo DecodeJWTToken(string jwtToken)
        {

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                UserInfo dd = decoder.DecodeToObject<UserInfo>(jwtToken, TokenSecretKey, true);

                return dd;
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
                return null;
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
                return null;
            }

        }

        public static string SerializeJWTJson(object payload)
        {
            string jsonString = JsonConvert.SerializeObject(payload);
            return jsonString;
        }

        public static UserInfo DeserializeJWTJson(string jwtToken)
        {
            UserInfo UserInfo = JsonConvert.DeserializeObject<UserInfo>(jwtToken);
            return UserInfo;
        }

    }
}
