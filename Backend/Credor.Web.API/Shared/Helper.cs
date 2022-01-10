using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Credor.Client.Entities;
using System.IO;
using System.Text;
using System.Net;
using System.Web;

namespace Credor.Web.API.Shared
{
    public class Helper
    {
        public IConfiguration Configuration { get; }
        public Helper(IConfiguration configuration)
        {           
            Configuration = configuration;
        }
        public Helper()
        {
        }
        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedHash = Combine(toBeHashed, salt);

                return sha256.ComputeHash(combinedHash);
            }
        }
        public static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }
        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public bool MailSend(MailMessage mail, SmtpClient smtpserver)
        {
            try
            {
                string mailAddress = Configuration["MailAddress"];
                string mailPassword = Configuration["MailPassword"];
                mail.IsBodyHtml = true;
                smtpserver.Port = 587;
                smtpserver.UseDefaultCredentials = false;
                smtpserver.Credentials = new System.Net.NetworkCredential(mailAddress, mailPassword);                
                smtpserver.EnableSsl = true;
                smtpserver.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public async Task<string> DocumentSaveAndUpload(IFormFile document, int id, int type)
        {
            CloudBlockBlob blockBlob;
            // cleans file name
            string cFileName = MakeValidFileName(document.FileName.ToString());
            var extension = Path.GetExtension(document.FileName);
            //cFileName += extension;

            // init container
            var container = await InitFileUpload();
            // Creating storage directory if not exist
            if (type == 1)
            {
                var dirPath = container.GetDirectoryReference("Tax/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 2)
            {
                var dirPath = container.GetDirectoryReference("Subscriptions/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 3)
            {
                var dirPath = container.GetDirectoryReference("Accreditation/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 4)
            {
                var dirPath = container.GetDirectoryReference("Offering Documents/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 5)
            {
                var dirPath = container.GetDirectoryReference("Miscellaneous/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 6)
            {
                var dirPath = container.GetDirectoryReference("Welcome Document/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 7)
            {
                var dirPath = container.GetDirectoryReference("UserProfileImage/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 8)
            {
                var dirPath = container.GetDirectoryReference("eSignedDocument/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }
            else if (type == 9)
            {
                var dirPath = container.GetDirectoryReference("GalleryImages/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }            
            else 
            {
                var dirPath = container.GetDirectoryReference("Others/" + id.ToString());
                if (dirPath == null)
                {
                    // Retrieve reference to a blob.
                    blockBlob = container.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
                else
                {
                    // Retrieve reference to a blob.
                    blockBlob = dirPath.GetBlockBlobReference(cFileName);
                    await blockBlob.UploadFromStreamAsync(document.OpenReadStream());
                }
            }

            // Response for storage
            return blockBlob.Uri.AbsoluteUri.ToString();
        }

        public async Task<List<DocumentDto>> MultiDocumentsSaveAndUpload(IFormFileCollection files, int id, int type)
        {
            if (files.Count() <= 0)
            {
                return null;
            }
            try
            {
                CloudBlockBlob blockBlob;
                var container = await InitFileUpload();
                var dir = container.GetDirectoryReference("Messages/" + id.ToString());

                if (type == 1)
                {
                    dir = container.GetDirectoryReference("DocumentRepo/" + id.ToString());
                }
                else if (type == 2)
                {
                    dir = container.GetDirectoryReference("Messages/" + id.ToString());
                }
                else
                {
                    dir = container.GetDirectoryReference("UserProfileImage/" + id.ToString());
                }
                List<DocumentDto> docs = new List<DocumentDto>();
                foreach (var file in files)
                {
                    // cleans file name
                    string cFileName = MakeValidFileName(file.FileName);

                    if (dir == null)
                    {
                        // Retrieve reference to a blob.
                        blockBlob = container.GetBlockBlobReference(cFileName);
                        await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
                    }
                    else
                    {
                        // Retrieve reference to a blob.
                        blockBlob = dir.GetBlockBlobReference(cFileName);
                        await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
                    }

                    var extn = FileExtention(blockBlob.Uri.AbsolutePath);
                    // Response for storage
                    DocumentDto doc = new DocumentDto();
                    doc.Name = file.FileName;
                    doc.FilePath = blockBlob.Uri.AbsoluteUri.ToString();
                    docs.Add(doc);
                }
                return docs;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }

        public static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$ )|([{0}]+)", invalidChars);
            var cname = System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
            return cname.Replace(" ", "-").ToLower();
        }

        public async Task<CloudBlobContainer> InitFileUpload()
        {            
            string storageConnectionString = "";
            storageConnectionString = Configuration["ConnectionStrings:BlobStorage"];           

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();




            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("credor");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();


            // TODO: Need set secured permission currently it public
            await container.SetPermissionsAsync(
            new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });


            return container;
        }

        private string FileExtention(string path)
        {
            var extn = Path.GetExtension(path);
            return extn.Substring(1, extn.Length - 1);
        }

        public string GetRandomString(int length)
        {            
            
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
           
        }

        public string GetRandomOTP()
        {

            StringBuilder str_build = new StringBuilder();
            Random random = new Random();                      
            int otp = random.Next(111111,999999);
            return otp.ToString();                        
        }
        public static string GetResponse(string smsURL)
        {
            try
            {
                WebClient objWebClient = new WebClient();
                System.IO.StreamReader reader = new System.IO.StreamReader(objWebClient.OpenRead(smsURL));
                string ResultHTML = reader.ReadToEnd();
                return ResultHTML;
            }
            catch (Exception e)
            {
                e.ToString();
                return "Fail";
            }
        }
        public static string SendSMS(string MblNo, string Msg)
        {
            string MainUrl = "SMSAPIURL"; //Here need to give SMS API URL
            string UserName = "username"; //Here need to give username
            string Password = "Password"; //Here need to give Password
            string SenderId = "SenderId";
            string strMobileno = MblNo;
            string URL = "";
            URL = MainUrl + "username=" + UserName + "&msg_token=" + Password + "&sender_id=" + SenderId + "&message=" + HttpUtility.UrlEncode(Msg).Trim() + "&mobile=" + strMobileno.Trim() + "";
            string strResponce = GetResponse(URL);
            string msg = "";
            if (strResponce.Equals("Fail"))
            {
                msg = "Fail";
            }
            else
            {
                msg = strResponce;
            }
            return msg;
        }


    }
}
