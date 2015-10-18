using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Common.EntitySql;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;

public partial class server_Default : System.Web.UI.Page
{

    public string[] SearchForAvailableTaxi()
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;

        string[] selectedTaxiDetails=new string[2];


        string queryString = "SELECT name,taxiTelNumber from tblTaxiDetails where status='available';";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(queryString, connection);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    selectedTaxiDetails[0] = (string) reader[0];//name of taxiMan
                    selectedTaxiDetails[1] = (string) reader[1]; //tel Of TaxiMan
                }
            }
        }
        return selectedTaxiDetails;
    }
    public string  SearchForAdressTaxiMustGo(string taxiTelNumber)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;

        string clientLocation="";


        string queryString = "SELECT currentClientLocation from tblTaxiDetails where taxiTelNumber="+taxiTelNumber+";";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(queryString, connection);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
             
                    clientLocation = (string)reader[0]; //tel Of TaxiMan
                }
            }
        }
        return clientLocation;
    }
    public void ChangeTaxiStatus(string taxiTelNumber,string status)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE tblTaxiDetails SET status = @status Where taxiTelNumber = @taxiTelNumber";
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public void ChangeTaxiCurrentClientDetails(string taxiTelNumber,string currentClientTelNumber, string currentClientLocation)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE tblTaxiDetails SET currentClientTelNumber = @currentClientTelNumber, currentClientLocation= @currentClientLocation Where taxiTelNumber = @TaxiTelNumber";
            command.Parameters.AddWithValue("@currentClientTelNumber", currentClientTelNumber);
            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            command.Parameters.AddWithValue("@currentClientLocation", currentClientLocation);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void Loging(string logData)
    {
       

        using (StreamWriter w = File.AppendText("C:/Users/dylan/Desktop/api google.txt"))
        {
            w.WriteLine(logData);
            w.Flush();
        }

    }
    protected void Page_Load(object sender, EventArgs e)  
    {
        //get location + street name from web app post

      
       

        //if web app post address
        if (!string.IsNullOrEmpty(Request.QueryString["address"]))
        {
            String addressOfClient = Request.QueryString["address"];
            string telOfClient = Request.QueryString["currentClientTelNumber"];
            string[] selectedTaxiDetails = new string[2];
            selectedTaxiDetails = SearchForAvailableTaxi();
            // prepare the web page we will be asking for
            if (!selectedTaxiDetails[1].IsNullOrWhiteSpace())
            {

                //send confirmation to Taxi
                String getUri =
                    @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                    selectedTaxiDetails[1] + @"&messageData=" + "Mr." + selectedTaxiDetails[0] + " can you go to :" +
                    addressOfClient + " do confrim with 'confirmation'";
                ChangeTaxiStatus(selectedTaxiDetails[1], "pending"); //put taxi as pending ,until it reply
                Loging("mr." + selectedTaxiDetails[0] + " phone Number : [" + selectedTaxiDetails[1] +
                       "] have a pending taxiClient at " + addressOfClient + "phone NumberClient [" +
                       Request.QueryString["clientPhoneNumber"] + "]");

                //update details of where client taxi need to serve

                ChangeTaxiCurrentClientDetails(selectedTaxiDetails[1], telOfClient, addressOfClient);





                WebRequest request = (WebRequest) WebRequest.Create(getUri);
                // execute the request
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                //read the response
                StreamReader responseReader =
                    new StreamReader(response.GetResponseStream());
                String resultmsg = responseReader.ReadToEnd();
                responseReader.Close();
            }
            else
            {
                String getUri = @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" + Request.QueryString["clientPhoneNumber"] + @"&messageData=" + "no Taxi available";
                WebRequest request = (WebRequest)WebRequest.Create(getUri);
                // execute the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //read the response
                StreamReader responseReader =
                    new StreamReader(response.GetResponseStream());
                String resultmsg = responseReader.ReadToEnd();
                responseReader.Close();

            }

        }



        if (!string.IsNullOrEmpty(Request.QueryString["msg"]))
        {
            if (Request.QueryString["msg"] == "confirmation") //taxi confirm pickUp
            {
                Loging("confirmation ok " + Request.QueryString["sender"]);
                //must deal with case where sender is not in db
                ChangeTaxiStatus(Request.QueryString["sender"], "unavailable"); //put taxi as unavailable,now fectching client
                //sent message to notify taxi of where it is expected to go.
                string taxiMustGoTo = SearchForAdressTaxiMustGo(Request.QueryString["sender"]);

                //sent msg to taxi 
                String getUri = @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" + Request.QueryString["sender"] + @"&messageData="+"you are now official dealing with  A client the adress you must go is :"+taxiMustGoTo;
                WebRequest request = (WebRequest)WebRequest.Create(getUri);
                // execute the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //read the response
                StreamReader responseReader =
                    new StreamReader(response.GetResponseStream());
                String resultmsg = responseReader.ReadToEnd();
                responseReader.Close();
                //log
                Loging(" phone Number : [" + Request.QueryString["sender"] + "] is now with  a client at " + taxiMustGoTo);  
            }
            if (Request.QueryString["msg"] == "finish")
            {
                ChangeTaxiStatus(Request.QueryString["sender"], "available");
                Loging(Request.QueryString["sender"]+"change to available no more job");
                ChangeTaxiCurrentClientDetails(Request.QueryString["sender"], "", "");
                String getUri = @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" + Request.QueryString["sender"] + @"&messageData="+"thank you for serving us : Our system see you as a free Taxi now,and we may forward You  more client";
                Loging("trying to send finish with Taxi");
                WebRequest request = (WebRequest)WebRequest.Create(getUri);
                // execute the request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //read the response
                StreamReader responseReader =
                    new StreamReader(response.GetResponseStream());
                String resultmsg = responseReader.ReadToEnd();
                responseReader.Close();
            }
           

            // Write the string to a file.
            
        }

    }
}