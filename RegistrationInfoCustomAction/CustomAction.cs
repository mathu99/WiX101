using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Deployment.WindowsInstaller;


namespace RegistrationInfoCustomAction
{

    public class ServerFile
    {
        string clarityUpdateUrl { get; set; }
        string updateUrl { get; set; }
        string pusherUrl { get; set; }

        public ServerFile(string countryConfig)
        {
            this.clarityUpdateUrl = countryConfig + "/komodoui.ui" ;
            this.updateUrl = countryConfig + "/komodoui.ui/UpdateFiles";
            this.pusherUrl = countryConfig + "/Fefmiddletier/api/ConnectedUsers";
        }
        public string jsonOut()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string toJsonString()    /* Returns the server.json structure */
        {
            return ($"{{\n\t\"clarityUpdateUrl\": \"{this.clarityUpdateUrl}\",\n\t\"updateUrl\": \"{this.updateUrl}\",\n\t\"pusherUrl\": \"{this.pusherUrl}\"\n}}");
        }
    }
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult SaveServerFile(Session session)
        {
            try
            {
                session.Log("----------START SaveServerFile------------");
                string path = session.CustomActionData["INSTALLEDPATH"];
                string selectedCountry = session.CustomActionData["SELECTEDCOUNTRY"];
                ServerFile countryConfig = new ServerFile(selectedCountry);
                string countryJson = countryConfig.toJsonString();
                path = path.Replace(@"\", @"\\");
                session.Log("New Installed Path is " + path);
                session.Log(countryConfig.toJsonString());
                System.IO.File.WriteAllText(path + "server.json", countryJson);
                session.Log("----------END SaveServerFile------------");
                return ActionResult.Success;
            }
            catch (Exception e)
            {
                session.Log(e.ToString());
                return ActionResult.Failure;
            }
        }
    }
}
