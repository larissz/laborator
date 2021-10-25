using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using Newtonsoft.Json.Linq;

namespace L03
{
   class Program
    {
        private static DriveService _service;
        private static string _token;
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello world");
            Initialize();
        }

        static void Initialize() 
        {
            string[] scopes = new string[] 
            {
                DriveService.Scope.Drive,
                DriveService.Scope.DriveFile
            };

            var clientId = "151395117443-qm7rmtsts9n2pqh54umn02h1e76nat4g.apps.googleusercontent.com";
            var clientSecret = "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";

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,

                new FileDataStore("Daimto.GoogleDrive.Auth.Store2")

            ).Result;

            _service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            _token = credential.Token.AccessToken;

            Console.Write("Token: " + credential.Token.AccessToken);

            GetFiles();
            Upload("C:\\Users\\laris\\Documents\\Desktop\\plane.png", _service);
        }

        static void GetFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _token);

            using (var response = request.GetResponse())
            {
                using(Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach(var file in myData["files"])
                    {
                        if (file["mimeType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }
            }
        }

        static void Upload(string path, DriveService _service)
        {
            var driveFile = new Google.Apis.Drive.v3.Data.File();
            driveFile.Name = Path.GetFileName(path);
            driveFile.MimeType = "image/png";

            var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
            var request = _service.Files.Create(driveFile, stream, "image/png");
            request.Fields = "id";
            request.Upload();

            var file = request.ResponseBody;
            Console.WriteLine("File id: " + file.Id);
        }
    }
}
