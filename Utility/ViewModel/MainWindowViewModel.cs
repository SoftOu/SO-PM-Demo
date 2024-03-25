using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using MySql.Data.MySqlClient;
using FastMember;
using Utility.Model;

namespace Utility.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        private string _connectionString;
        public string ConnectionStrings
        {
            get => _connectionString;
            set => Set(ref _connectionString, value);
        }

        private Visibility _loadingTextVisibility = Visibility.Collapsed;
        public Visibility LoadingTextVisibility
        {
            get => _loadingTextVisibility;
            set => Set(ref _loadingTextVisibility, value);
        }

        private string _totalUpdatedText;
        public string TotalUpdatedText
        {
            get => _totalUpdatedText;
            set => Set(ref _totalUpdatedText, value);
        }

        private string _totalErrorText;
        public string TotalErrorText
        {
            get => _totalErrorText;
            set => Set(ref _totalErrorText, value);
        }

        private Visibility _informationGridVisibility = Visibility.Collapsed;
        public Visibility InformationGridVisibility
        {
            get => _informationGridVisibility;
            set => Set(ref _informationGridVisibility, value);
        }

        private bool _isSaveButtonEnable = true;
        public bool IsSaveButtonEnable
        {
            get => _isSaveButtonEnable;
            set => Set(ref _isSaveButtonEnable, value);
        }

        #endregion

        #region Command

        public ICommand SaveCommand => new RelayCommand(SaveCommandExecuted);

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void SaveCommandExecuted()
        {
            LoadingTextVisibility = Visibility.Visible;
            IsSaveButtonEnable = false;

            List<ProjectData> getProjectsData = new List<ProjectData>();
            try
            {
                using var conn = new MySqlConnection(ConnectionStrings);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM `ProjectData` ";
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        getProjectsData.Add(ConvertToObject<ProjectData>(reader));
                    }

                    reader.Close();

                    if (getProjectsData.Any())
                    {
                        int updatedRecords = 0;
                        int rejectedRecords = 0;

                        foreach (var project in getProjectsData)
                        {
                            if (!string.IsNullOrEmpty(project.Email))
                                project.Email = Encrypt(project.Email);

                            if (!string.IsNullOrEmpty(project.User_Name))
                                project.User_Name = Encrypt(project.User_Name);

                            try
                            {
                                Update(conn, project.User_Name, project.Email, project.ProjectUser_ID);
                                updatedRecords++;
                            }
                            catch (Exception ex)
                            {
                                rejectedRecords++;
                                MessageBox.Show($"fail to encrypt project: {project.Name}, \r\n Please see exception ----: " + ex);
                            }
                        }

                        TotalUpdatedText = "Total Number Of Updated Records : " + updatedRecords;
                        TotalErrorText = "Total Number Of Rejected Records : " + rejectedRecords;

                        InformationGridVisibility = Visibility.Visible;
                    }
                }
                else
                {
                    MessageBox.Show("Can not connect with this connection.", "Error", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while connecting to DB.", "Exception", MessageBoxButton.OK);
            }

            IsSaveButtonEnable = true;
            LoadingTextVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// convert db reader in to class object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rd"></param>
        /// <returns></returns>
        private T ConvertToObject<T>(MySqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            TypeAccessor accessor = TypeAccessor.Create(type);
            MemberSet members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);

                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = rd.GetValue(i);
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// encrypt.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Encrypt(string text)
        {
            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 13, 15, 16, 17, 18, 19, 20, 21, 22, 24, 23 };
            byte[] rgbIv = { 8, 7, 6, 5, 4, 3, 2, 1 };
            byte[] input = new UTF8Encoding().GetBytes(text);
            byte[] output = Transform(input, new TripleDESCryptoServiceProvider().CreateEncryptor(key, rgbIv));
            return Convert.ToBase64String(output);
        }

        private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write);
            //transform the bytes as requested
            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();
            memStream.Position = 0;
            byte[] result = new byte[Convert.ToInt32(memStream.Length - 1) + 1];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));
            memStream.Close();
            cryptoStream.Close();
            return result;
        }

        private void Update(MySqlConnection mySqlConnection, string userName, string email, int projectUserId)
        {
            string query = "UPDATE `ProjectData` SET User_Name=@UserName,Email=@Email WHERE ProjectUser_ID =@ProjectUserId";

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@ProjectUserId", Convert.ToInt16(projectUserId));
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@Email", email);

            cmd.Connection = mySqlConnection;
            cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
