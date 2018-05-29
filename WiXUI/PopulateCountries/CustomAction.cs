using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Deployment.WindowsInstaller;
using Newtonsoft.Json;

namespace PopulateCountries
{
    public class CustomActions
    {
        static int index = 1;
        private static readonly HttpClient client = new HttpClient();
        public static void FillComboBox(Session session, string text, string value)
        {
            View view = session.Database.OpenView("SELECT * FROM ComboBox");
            view.Execute();

            Record record = session.Database.CreateRecord(4);
            record.SetString(1, "COUNTRIES");
            record.SetInteger(2, index);
            record.SetString(3, value);
            record.SetString(4, text);

            view.Modify(ViewModifyMode.InsertTemporary, record);
            view.Close();
            index++;
        }

        private static async Task<string> getResponseAsync()
        {
            var url = "http://localhost:9998/api/servers";
            var responseMsg = "Error";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseMsg = await response.Content.ReadAsStringAsync();
            }
            return responseMsg;
        }

        [CustomAction]
        public static ActionResult FillList(Session xiSession)
        {
            try
            {
                string countryJson = getResponseAsync().Result;
                if (!countryJson.Equals("Error"))
                {
                    dynamic stuff = JsonConvert.DeserializeObject(countryJson);
                    foreach (dynamic country in stuff.servers)
                    {
                        FillComboBox(xiSession, (string)country.name, (string)country.alias);
                    }
                }
                else /* TODO: Get this error out to screen */
                {
                    FillComboBox(xiSession, "South Africa", "zaf-server");
                }
            } catch (Exception e)   /* TODO: Get this error out to screen: e.ToString() */
            {
                FillComboBox(xiSession, "South Africa", "zaf-server");
            }
            return ActionResult.Success;
        }
    }
}
