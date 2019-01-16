using System.Data.SqlClient;

namespace UI.Pages
{
    public class CBUSASqlActions
    {
        private string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        public void CreateBuilderData()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("exec [dbo].[sp_AutomationTestDataCreation]", sqlConnection))
                {
                    sqlCommand.CommandTimeout = 100;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        public void DeleteBuilderData()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("exec [dbo].[sp_AutomationTestDataDeletion]", sqlConnection))
                {
                    sqlCommand.CommandTimeout = 100;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
