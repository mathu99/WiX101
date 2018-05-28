using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;

namespace PopulateCountries
{
    public class CustomActions
    {
        static int intex = 1;
        public static void FillComboBox(Session session, string text, string value)
        {
            View view = session.Database.OpenView("SELECT * FROM ComboBox");
            view.Execute();

            Record record = session.Database.CreateRecord(4);
            record.SetString(1, "COUNTRIES");
            record.SetInteger(2, intex);
            record.SetString(3, value);
            record.SetString(4, text);

            view.Modify(ViewModifyMode.InsertTemporary, record);
            view.Close();
            intex++;
        }
        [CustomAction]
        public static ActionResult FillList(Session xiSession)
        {

            FillComboBox(xiSession, "US", "United States");
            FillComboBox(xiSession, "CA", "Canada");
            FillComboBox(xiSession, "MX", "Mexico");

            return ActionResult.Success;
        }
    }
}
