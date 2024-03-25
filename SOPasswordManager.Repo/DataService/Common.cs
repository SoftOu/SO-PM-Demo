using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SOPasswordManager.Repo.DataService
{
    public class Common
    {

        public static T GetItem<T>(SqlDataReader dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == dr.GetName(i).ToLower())
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[i])))
                        {
                            if (pro.PropertyType.Name == "String")
                                pro.SetValue(obj, Convert.ToString(dr[i]));
                            else if (pro.PropertyType.Name == "Byte[]" && string.IsNullOrEmpty(Convert.ToString(dr[i])))
                                pro.SetValue(obj, new byte[0]);
                            else
                                pro.SetValue(obj, dr[i]);
                        }
                        break;
                    }
                }
            }
            return obj;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            var data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            var temp = typeof(T);
            var obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        // ReSharper disable once RedundantJumpStatement
                        continue;
                }
            }
            return obj;
        }

        public static string encrypt(string ToEncrypt)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(ToEncrypt));
        }

        public static string decrypt(string cypherString)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(cypherString));
        }


        public static Task SendEmailAsync(string ToEmail, string Body, string Subject)
        {
            try
            {
                var mm = new MailMessage();
                mm.To.Add(new MailAddress(ToEmail.Trim()));
                mm.From = new MailAddress(DBConnectionString.Email, "SO-Password Manager Support");
                mm.Subject = Subject;
                mm.Body = Body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = DBConnectionString.Host;
                smtp.EnableSsl = DBConnectionString.EnableSsl;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = DBConnectionString.Email;
                NetworkCred.Password = DBConnectionString.Password;
                smtp.UseDefaultCredentials = DBConnectionString.UseDefaultCredentials;
                smtp.Credentials = NetworkCred;
                smtp.Port = DBConnectionString.Port;
                smtp.Send(mm);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(0);
        }
    }

    public static class EncryptDecrypt
    {
        //Define the tripple Des Provider
        private static TripleDESCryptoServiceProvider m_des = new TripleDESCryptoServiceProvider();
        //Define the string Handler
        private static UTF8Encoding m_utf8 = new UTF8Encoding();
        //Define the local Propertirs Array
        private static byte[] m_key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 13, 15, 16, 17, 18, 19, 20, 21, 22, 24, 23 };
        private static byte[] m_iv = { 8, 7, 6, 5, 4, 3, 2, 1 };

        private static byte[] Encypt(byte[] input)
        {
            return Transform(input, m_des.CreateEncryptor(m_key, m_iv));
        }

        private static byte[] Decrypt(byte[] input)
        {
            return Transform(input, m_des.CreateDecryptor(m_key, m_iv));
        }

        public static string Encrypt(string text)
        {
            byte[] input = m_utf8.GetBytes(text);
            byte[] output = Transform(input, m_des.CreateEncryptor(m_key, m_iv));
            return Convert.ToBase64String(output);
        }

        public static string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input, m_des.CreateDecryptor(m_key, m_iv));
            return m_utf8.GetString(output);
        }

        private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            //Create the Neccessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write);
            //transform the bytes as requested
            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();
            //Reasd the memory stream and convert it to byte array
            memStream.Position = 0;
            byte[] result = new byte[Convert.ToInt32(memStream.Length - 1) + 1];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));
            memStream.Close();
            cryptoStream.Close();
            return result;
        }
    }

    public static class CreatePara
    {
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, string paraVal, int size = 10000000, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.Size = size;
            para.SqlDbType = System.Data.SqlDbType.NVarChar;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, int paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Int;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, decimal paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Decimal;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, float paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Float;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, DateTime paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.IsNullable = true;
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.DateTime;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, System.Data.DataTable paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Structured;
            para.Direction = dir;
            return para;
        }
    }
}
