﻿using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Net;
using YuukiPS_Launcher.Json;

namespace YuukiPS_Launcher.Yuuki
{
    public class API
    {
        public static string API_DL_CF = "https://file.yuuki.me/";
        public static string API_DL_OW = "https://drive.yuuki.me/";
        public static string API_DL_WB = "https://ps.yuuki.me/api/";
        public static string API_GITHUB_YuukiPS = "https://api.github.com/repos/akbaryahya/YuukiPS-Launcher/";
        public static string API_GITHUB_Akebi = "https://api.github.com/repos/Akebi-Group/Akebi-GC/";

        public static GS GS_DL(string dl = "os")
        {
            var client = new RestClient(API_DL_WB);
            var request = new RestRequest("genshin/download/latest/" + dl);

            var response = client.Execute(request);
            var getme = response.StatusCode == HttpStatusCode.OK ? response.Content : response.StatusCode.ToString();
            return JsonConvert.DeserializeObject<GS>(getme);
        }

        public static VersionGenshin? GetMD5VersionGS(string md5)
        {
            var client = new RestClient(API_DL_WB);
            var request = new RestRequest("genshin/version?md5=" + md5.ToUpper());

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    if (response.Content != null)
                    {
                        var tes = JsonConvert.DeserializeObject<VersionGenshin>(response.Content);
                        if (tes != null)
                        {
                            return tes;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: ", ex);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
            return null;
        }

        public static KeyGS? GSKEY()
        {
            var client = new RestClient(API_DL_WB);
            var request = new RestRequest("genshin/key/latest");

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    if (response.Content != null)
                    {
                        var tes = JsonConvert.DeserializeObject<KeyGS>(response.Content);
                        if (tes != null)
                        {
                            return tes;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: ", ex);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
            return null;
        }

        public static ServerList? ServerList()
        {
            var client = new RestClient(API_DL_WB);
            var request = new RestRequest("launcher/server");
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null)
                {
                    try
                    {
                        var tes = JsonConvert.DeserializeObject<ServerList>(response.Content);
                        if (tes != null)
                        {
                            return tes;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: ", ex);
                    }

                }
            }
            return null;
        }

        public static VersionServer? GetServerStatus(string url)
        {
            var s = new RestClientOptions(url)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            var client = new RestClient(s);
            var request = new RestRequest();

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null)
                {
                    try
                    {
                        var tes = JsonConvert.DeserializeObject<VersionServer>(response.Content);
                        if (tes != null)
                        {
                            return tes;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: ", ex);
                    }
                }
            }
            else
            {
                Debug.Print("Error Host " + url + ": " + response.StatusCode);
            }
            return null;
        }

        public static Update? GetUpdate()
        {
            var client = new RestClient(API_GITHUB_YuukiPS);
            var request = new RestRequest("releases");
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null)
                {
                    try
                    {
                        var tes = JsonConvert.DeserializeObject<List<Update>>(response.Content);
                        if (tes != null)
                        {
                            return tes[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error GetUpdate1: ", ex);
                    }

                }
            }
            else
            {
                Console.WriteLine("Error GetUpdate2: " + response.StatusCode);
            }
            return null;
        }

        public static string? GetAkebi(int ch = 1)
        {
            var client = new RestClient(API_GITHUB_Akebi);
            var request = new RestRequest("actions/artifacts");
            var response = client.Execute(request);

            var whos = "master";
            if (ch == 2)
            {
                whos = "chinese";
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null)
                {
                    try
                    {
                        var tes = JsonConvert.DeserializeObject<Nightly>(response.Content);
                        if (tes != null)
                        {
                            foreach (var file in tes.artifacts)
                            {
                                if (file.workflow_run != null)
                                {
                                    if (file.workflow_run.head_branch == whos)
                                    {
                                        return file.workflow_run.head_sha + "|https://nightly.link/Akebi-Group/Akebi-GC/actions/runs/" + file.workflow_run.id + "/" + file.name + ".zip";
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error GetAkebi: ", ex);
                    }

                }
            }
            else
            {
                Console.WriteLine("Error GetUpdate2: " + response.StatusCode);
            }
            return "";
        }
    }
}