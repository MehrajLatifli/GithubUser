
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

class Program
{


    static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write(" Enter github username: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        string username = Console.ReadLine();

        Console.ForegroundColor = ConsoleColor.White;

        Console.Write(" Enter github token(classic): ");
        Console.ForegroundColor = ConsoleColor.Blue;

        string token = "";

        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                token += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && token.Length > 0)
            {
                token = token.Substring(0, (token.Length - 1));
                Console.Write("\b \b");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.ForegroundColor = ConsoleColor.White;


        Console.WriteLine("\n Your token is: " + token);



        Console.WriteLine(" \n\n ");
        Console.Clear();
        Console.WriteLine(" 1 <- Repositories");
        Console.WriteLine(" 2 <- Followers");
        Console.WriteLine(" 3 <- Followings");
        Console.WriteLine(" 4 <- Languages");
        Console.WriteLine(" 5 <- Topics");

        Console.Write("\n Enter: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        string choose = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.White;



        string url = $"https://api.github.com/users/{username}";

        if (!string.IsNullOrEmpty(token) && !string.IsNullOrWhiteSpace(username))
        {

            using (HttpClient client = new HttpClient())
            {


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

                HttpResponseMessage response = await client.GetAsync(url);

                while (true)
                {



                    if (response.IsSuccessStatusCode)
                    {

                        if (Mainmenu() == "1")
                        {


                            string responseBody = await response.Content.ReadAsStringAsync();

                            JToken tokenJson = JObject.Parse(responseBody);

                            Console.WriteLine($"\n ================= \n {tokenJson["login"]}  {tokenJson["avatar_url"]} \n ");


                            HttpResponseMessage response2 = await client.GetAsync($"{url}/repos?per_page=1000");

                            string responseBody2 = await response2.Content.ReadAsStringAsync();
                            JArray repos = JArray.Parse(responseBody2);



                            if (response2.IsSuccessStatusCode)
                            {



                                string request = string.Empty;
                                int menuSelect = 0;


                                //for (int i = 0; i < repos.Count; i++)
                                //{


                                //    Console.WriteLine($"{i}) {repos.ElementAt(i)["full_name"]}");

                                //    if (int.TryParse(openbrowser, out num))
                                //    {
                                //        // it's a integer
                                //    }


                                //}



                                while (true)
                                {
                                    Console.Clear();
                                    Console.CursorVisible = false;

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\n Repositories: \n ");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\n Open the selected section in the browser using the up and down arrows on the keyboard and pressing the enter key. \n");
                                    Console.ForegroundColor = ConsoleColor.Blue;


                                    for (int i = 0; i < repos.Count; i++)
                                    {
                                        if (i == menuSelect)
                                        {


                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine($"{i}) {repos.ElementAt(i)["full_name"]}");
                                            Console.ForegroundColor = ConsoleColor.White;


                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine($"{i}) {repos.ElementAt(i)["full_name"]}");
                                        }


                                    }

                                    var keyPressed = Console.ReadKey();

                                    if (keyPressed.Key == ConsoleKey.DownArrow && menuSelect != repos.Count - 1)
                                    {
                                        menuSelect++;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.UpArrow && menuSelect >= 1)
                                    {
                                        menuSelect--;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.Enter)
                                    {

                                        request = $"https://www.github.com/{repos.ElementAt(menuSelect)["full_name"]}";
                                        ProcessStartInfo ps = new ProcessStartInfo
                                        {
                                            FileName = request,
                                            UseShellExecute = true
                                        };

                                        Process.Start(ps);

                                    }
                                    else if (keyPressed.Key == ConsoleKey.Spacebar)
                                    {
                                        Mainmenu();

                                        break;
                                    }
                                }
                            }




                        }

                        if (Mainmenu() == "2")
                        {
                            HttpResponseMessage response3 = await client.GetAsync($"{url}/followers?per_page=1000");

                            string responseBody3 = await response3.Content.ReadAsStringAsync();
                            JArray followers = JArray.Parse(responseBody3);


                            if (response3.IsSuccessStatusCode)
                            {



                                string request = string.Empty;
                                int menuSelect = 0;


                                while (true)
                                {
                                    Console.Clear();
                                    Console.CursorVisible = false;

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\n Followers: \n ");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\n Open the selected section in the browser using the up and down arrows on the keyboard and pressing the enter key. \n");
                                    Console.ForegroundColor = ConsoleColor.Blue;


                                    for (int i = 0; i < followers.Count; i++)
                                    {



                                        if (i == menuSelect)
                                        {


                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine($"{followers.ElementAt(i)["login"]}  {followers.ElementAt(i)["avatar_url"]}");

                                            Console.ForegroundColor = ConsoleColor.White;


                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine($"{followers.ElementAt(i)["login"]}  {followers.ElementAt(i)["avatar_url"]}");

                                        }


                                    }

                                    var keyPressed = Console.ReadKey();

                                    if (keyPressed.Key == ConsoleKey.DownArrow && menuSelect != followers.Count - 1)
                                    {
                                        menuSelect++;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.UpArrow && menuSelect >= 1)
                                    {
                                        menuSelect--;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.Enter)
                                    {

                                        request = $"{followers.ElementAt(menuSelect)["avatar_url"]}";
                                        ProcessStartInfo ps = new ProcessStartInfo
                                        {
                                            FileName = request,
                                            UseShellExecute = true
                                        };

                                        Process.Start(ps);

                                    }
                                    else if (keyPressed.Key == ConsoleKey.Spacebar)
                                    {
                                        Mainmenu();
                                        break;
                                    }
                                }





                            }

                        }

                        if (Mainmenu() == "3")
                        {

                            HttpResponseMessage response4 = await client.GetAsync($"{url}/followers?per_page=1000");

                            string responseBody4 = await response4.Content.ReadAsStringAsync();
                            JArray followings = JArray.Parse(responseBody4);


                            if (response4.IsSuccessStatusCode)
                            {


                                string request = string.Empty;
                                int menuSelect = 0;


                                while (true)
                                {
                                    Console.Clear();
                                    Console.CursorVisible = false;

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\n Followings: \n ");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\n Open the selected section in the browser using the up and down arrows on the keyboard and pressing the enter key. \n");
                                    Console.ForegroundColor = ConsoleColor.Blue;


                                    for (int i = 0; i < followings.Count; i++)
                                    {



                                        if (i == menuSelect)
                                        {


                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.WriteLine($"{followings.ElementAt(i)["login"]}  {followings.ElementAt(i)["avatar_url"]}");

                                            Console.ForegroundColor = ConsoleColor.White;


                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.White;
                                            Console.WriteLine($"{followings.ElementAt(i)["login"]}  {followings.ElementAt(i)["avatar_url"]}");

                                        }


                                    }

                                    var keyPressed = Console.ReadKey();

                                    if (keyPressed.Key == ConsoleKey.DownArrow && menuSelect != followings.Count - 1)
                                    {
                                        menuSelect++;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.UpArrow && menuSelect >= 1)
                                    {
                                        menuSelect--;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.Enter)
                                    {

                                        request = $"{followings.ElementAt(menuSelect)["avatar_url"]}";
                                        ProcessStartInfo ps = new ProcessStartInfo
                                        {
                                            FileName = request,
                                            UseShellExecute = true
                                        };

                                        Process.Start(ps);

                                    }
                                    else if (keyPressed.Key == ConsoleKey.Spacebar)
                                    {
                                        Mainmenu();
                                        break;
                                    }
                                }

                            }

                        }

                        if (Mainmenu() == "4")
                        {
                            var languagerepositoriesUrl = $"{url}/repos?per_page=1000";
                            var languagerepositoriesResponse = await client.GetAsync(languagerepositoriesUrl);
                            var languagerepositoriesJson = await languagerepositoriesResponse.Content.ReadAsStringAsync();

                            JArray languagesjson = JArray.Parse(languagerepositoriesJson);

                            var languages = new HashSet<JToken>();



       

                                    foreach (var repository in languagesjson)
                                {


                                    languages.Add(repository["language"]);
                                }



                                string request = string.Empty;
                                int menuSelect = 0;

                            while (true)
                            {
                                Console.Clear();
                                Console.CursorVisible = false;

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n Languages used by user: \n");
                                Console.ForegroundColor = ConsoleColor.White;





                                for (int i = 0; i < languages.Count; i++)
                                {

                                        Console.WriteLine(languages.ElementAt(i));

                                }





                                var keyPressed = Console.ReadKey();

          
                                if (keyPressed.Key == ConsoleKey.Spacebar)
                                {
                                    Mainmenu();
                                    break;
                                }

                            }

                        }

                        if (Mainmenu() == "5")
                        {

                            List<string> allTopics = new List<string>();


                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"\n Please wait... \n");
                            Console.ForegroundColor = ConsoleColor.White;

                            HttpResponseMessage response_ = await client.GetAsync($"https://api.github.com/users/{username}/repos?per_page=1000");
                            response.EnsureSuccessStatusCode();

                            string responseBody_ = await response_.Content.ReadAsStringAsync();
                            dynamic repos_ = JsonConvert.DeserializeObject(responseBody_);

                            foreach (var repo in repos_)
                            {
                                string repoName = repo.name;

                                HttpResponseMessage topicsResponse = await client.GetAsync($"https://api.github.com/repos/{username}/{repoName}/topics");
                                topicsResponse.EnsureSuccessStatusCode();

                                string topicsResponseBody = await topicsResponse.Content.ReadAsStringAsync();
                                dynamic topics = JsonConvert.DeserializeObject(topicsResponseBody);

                                allTopics.AddRange(topics.names.ToObject<List<string>>());
                            }








                            string request = string.Empty;
                            int menuSelect = 0;


                            while (true)
                            {
                                Console.Clear();
                                Console.CursorVisible = false;

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\n Found {allTopics.Count} topics for user: \n");
                                Console.ForegroundColor = ConsoleColor.White;

                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n Open the selected section in the browser using the up and down arrows on the keyboard and pressing the enter key. \n");
                                Console.ForegroundColor = ConsoleColor.Blue;


                                for (int i = 0; i < allTopics.Count; i++)
                                {



                                    if (i == menuSelect)
                                    {


                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.WriteLine(allTopics.ElementAt(i));
                                        Console.ForegroundColor = ConsoleColor.White;


                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.WriteLine(allTopics.ElementAt(i));
                                    }


                                }

                                var keyPressed = Console.ReadKey();

                                if (keyPressed.Key == ConsoleKey.DownArrow && menuSelect != allTopics.Count - 1)
                                {
                                    menuSelect++;
                                }
                                else if (keyPressed.Key == ConsoleKey.UpArrow && menuSelect >= 1)
                                {
                                    menuSelect--;
                                }
                                else if (keyPressed.Key == ConsoleKey.Enter)
                                {

                                    request = $"https://github.com/topics/{allTopics.ElementAt(menuSelect)}";

                                    ProcessStartInfo ps = new ProcessStartInfo
                                    {
                                        FileName = request,
                                        UseShellExecute = true
                                    };

                                    Process.Start(ps);

                                }
                                else if (keyPressed.Key == ConsoleKey.Spacebar)
                                {
                                    Mainmenu();
                                    break;
                                }
                            }


                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n Error retrieving data from Github API.");
                        Console.ForegroundColor = ConsoleColor.White;

                        break;
                    }

                }
            }

            Console.ReadKey();
        }

    }
    public static string Mainmenu()
    {

        Console.WriteLine(" \n\n ");
        Console.Clear();
        Console.WriteLine(" 1 <- Repositories");
        Console.WriteLine(" 2 <- Followers");
        Console.WriteLine(" 3 <- Followings");
        Console.WriteLine(" 4 <- Languages");
        Console.WriteLine(" 5 <- Topics");

        Console.Write("\n Enter: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        string choose = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.White;

        return choose;
    }
}






