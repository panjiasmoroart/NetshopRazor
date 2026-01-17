using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetshopRazor.Pages.Admin.Messages
{
    public class IndexModel : PageModel
    {
		public List<MessageInfo> listMessages = new List<MessageInfo>();
		public void OnGet()
        {
			try 
			{
				string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=netshoprazor_db;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					string sqlCount = "SELECT * FROM messages ORDER BY id DESC";
					using (SqlCommand command = new SqlCommand(sqlCount, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())

							{
								MessageInfo messageInfo = new MessageInfo();
								messageInfo.Id = reader.GetInt32(0);
								messageInfo.FirstName = reader.GetString(1);
								messageInfo.LastName = reader.GetString(2);
								messageInfo.Email = reader.GetString(3);
								messageInfo.Phone = reader.GetString(4);
								messageInfo.Subject = reader.GetString(5);
								messageInfo.Message = reader.GetString(6);
								messageInfo.CreatedAt = reader.GetDateTime(7).ToString("MM/dd/yyyy");

								listMessages.Add(messageInfo);
							}
						}
					}
				}


			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.Message);
			}

        }

		public class MessageInfo
		{
			public int Id { get; set; }
			public string FirstName { get; set; } = "";
			public string LastName { get; set; } = "";
			public string Email { get; set; } = "";
			public string Phone { get; set; } = "";
			public string Subject { get; set; } = "";
			public string Message { get; set; } = "";
			public string CreatedAt { get; set; } = "";
		}
	}
}
