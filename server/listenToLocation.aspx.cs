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
    public bool isATaxi(string taxiTelNumber)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;


        string taxiTelNumberResult = "";

        string queryString = "SELECT taxiTelNumber from tblTaxiDetails where taxiTelNumber=@taxiTelNumber;";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(queryString, connection);
          
            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                  taxiTelNumberResult = (string)reader[0];//name of taxiMan
                   

                }
            }
        }
        if (taxiTelNumberResult != "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool isThisTaxiAvaialble(string taxiTelNumber)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;


        string taxiAvaialble = "";

        string queryString = "SELECT taxiTelNumber from tblTaxiDetails where taxiTelNumber=@taxiTelNumber and status='available';";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(queryString, connection);

            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    taxiAvaialble = (string)reader[0];//name of taxiMan


                }
            }
        }
        if (taxiAvaialble != "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public string getTaxiStatus(string taxiTelNumber)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;


        string taxiStatus = "";

        string queryString = "SELECT status from tblTaxiDetails where taxiTelNumber=@taxiTelNumber;";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(queryString, connection);

            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    taxiStatus = (string)reader[0];//name of taxiMan


                }
            }
        }
        return taxiStatus;
    }
    public string  SearchForAdressTaxiMustGo(string taxiTelNumber,string clientStatus)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;

        string clientLocation="";


        string queryString = "SELECT clientLocation from tblClientDetails where  taxiTelNumber=@taxiTelNumber and status=@status;";
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@status", clientStatus);
            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
             
                    clientLocation = (string)reader[0]; //location of client i.e where taxi Must Go
                }
            }
        }
        return clientLocation;
    }

    public string clientTelNumberTaxiMustGo(string taxiTelNumber,string clientStatus)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;

        string clientTelNumber = "";


        string queryString =
            "SELECT clientTelNumber from tblClientDetails where taxiTelNumber=@taxiTelNumber and status=@status;";
            
        
        
   
   
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@status", clientStatus);
            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    clientTelNumber = (string)reader[0]; //location of client i.e where taxi Must Go
                }
            }
        }
        return clientTelNumber;
    }
    public void ChangeTaxiStatus(string taxiTelNumber,string status)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE tblTaxiDetails SET status = @status Where taxiTelNumber =@taxiTelNumber";
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void ChangeClientStatus(string clientTelNumber, string status)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE tblClientDetails SET status = @status Where clientTelNumber = @clientTelNumber";
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@clientTelNumber", clientTelNumber);
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void insertClientDetails(string taxiTelNumber,string clientTelNumber,string clientLocation, string status)
    {
        var connectionString = ConfigurationManager.ConnectionStrings["TaxiConnection"].ConnectionString;
        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = "insert INTO tblClientDetails (status,clientTelNumber,clientLocation,taxiTelNumber) values(@status,@clientTelNumber,@clientLocation,@taxiTelNumber)";
            command.Parameters.AddWithValue("@status", status);
            command.Parameters.AddWithValue("@clientTelNumber", clientTelNumber);
            command.Parameters.AddWithValue("@taxiTelNumber", taxiTelNumber);
            command.Parameters.AddWithValue("@clientLocation", clientLocation);
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


        try
        {

            //if web app post address
            if (!string.IsNullOrEmpty(Request.QueryString["address"]))
            {
                string clientLocation = Request.QueryString["address"];
                string clientTelNumber = Request.QueryString["currentClientTelNumber"];
                string[] selectedTaxiDetails = new string[2];
                selectedTaxiDetails = SearchForAvailableTaxi();
                // prepare the web page we will be asking for
                Loging("receive address");
                if (!selectedTaxiDetails[1].IsNullOrWhiteSpace())
                {
                    string taxiTelNumber = selectedTaxiDetails[1];
                    //send confirmation to Taxi
                    String getUri =
                        @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                        selectedTaxiDetails[1] + @"&messageData=" + "Mr." + selectedTaxiDetails[0] + " can you go to :" +
                        clientLocation + " do confrim with 'confirm'";
                    ChangeTaxiStatus(selectedTaxiDetails[1], "pending"); //put taxi as pending ,until it reply
                    Loging("mr." + selectedTaxiDetails[0] + " phone Number : [" + selectedTaxiDetails[1] +
                           "] have a pending taxiClient at " + clientLocation + "phone NumberClient [" +
                           Request.QueryString["clientPhoneNumber"] + "]");

                    //update details of where client taxi need to serve

                    insertClientDetails(taxiTelNumber, clientTelNumber, clientLocation, "pending");





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
                    String getUri =
                        @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                        Request.QueryString["clientPhoneNumber"] + @"&messageData=" +
                        "no Taxi available,try again in someTime";
                    WebRequest request = (WebRequest) WebRequest.Create(getUri);
                    // execute the request
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    //read the response
                    StreamReader responseReader =
                        new StreamReader(response.GetResponseStream());
                    String resultmsg = responseReader.ReadToEnd();
                    responseReader.Close();

                }

            }



            if (!string.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                if (Request.QueryString["msg"] == "confirm" & isATaxi(Request.QueryString["sender"]) &
                    (getTaxiStatus(Request.QueryString["sender"]) == "pending"))
                    //taxi confirm pickUp
                {
                    if (!isThisTaxiAvaialble(Request.QueryString["sender"]))
                        //taxi Being seen as UnAvaiable Still sent confirm msg (may be a misType)
                    {
                        //sent msg to taxi who is  seen as unAvailable By System
                        String messageToBeSentToTaxiUnavailable =
                            @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                            Request.QueryString["sender"] + @"&messageData=" +
                            "you are now view As unAvaialable please Sent appropriate msg:";
                        WebRequest requestToTaxiUnavailable =
                            (WebRequest) WebRequest.Create(messageToBeSentToTaxiUnavailable);
                        // execute the request
                        HttpWebResponse responseToTaxiUnavailable =
                            (HttpWebResponse) requestToTaxiUnavailable.GetResponse();
                        Loging("message sent By wrong Taxi " + Request.QueryString["sender"]);
                    }
                    else
                    {
                        Loging("confirmation ok " + Request.QueryString["sender"]);
                        //must deal with case where sender is not in db
                        ChangeTaxiStatus(Request.QueryString["sender"], "unavailable");
                        //put taxi as unavailable,now fectching client
                        string clientTelNumber = clientTelNumberTaxiMustGo(Request.QueryString["sender"], "pending");
                        string taxiMustGoTo = SearchForAdressTaxiMustGo(Request.QueryString["sender"], "pending");

                        //sent message to notify taxi of where it is expected to go.


                        //sent msg to taxi 
                        String messageToBeSentToTaxi =
                            @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                            Request.QueryString["sender"] + @"&messageData=" +
                            "you are now official dealing with  A client the adress you must go is :" + taxiMustGoTo;
                        WebRequest request = (WebRequest) WebRequest.Create(messageToBeSentToTaxi);
                        // execute the request
                        HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                        //read the response
                        StreamReader responseReader =
                            new StreamReader(response.GetResponseStream());
                        String resultmsg = responseReader.ReadToEnd();
                        responseReader.Close();
                        //end of msg Sending


                        //send msg to client
                        String messageToBeSentToClient =
                            @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                            clientTelNumber + @"&messageData=" + " A taxi will come at  :" + taxiMustGoTo +
                            "do contact him for more Query:" + Request.QueryString["sender"];
                        WebRequest requestToTaxiClientViaOzeki = (WebRequest) WebRequest.Create(messageToBeSentToClient);
                        // execute the request
                        HttpWebResponse responseToTaxiClientViaOzeki =
                            (HttpWebResponse) requestToTaxiClientViaOzeki.GetResponse();

                        //read the response
                        StreamReader responseReaderToTaxiClientViaOzek =
                            new StreamReader(responseToTaxiClientViaOzeki.GetResponseStream());
                        String resultmsgToTaxiClientViaOzek = responseReaderToTaxiClientViaOzek.ReadToEnd();
                        responseReader.Close();
                        //end of msgSending

                        //log
                        Loging(" phone Number : [" + Request.QueryString["sender"] + "] is now with  a client at " +
                               taxiMustGoTo);
                        ChangeClientStatus(clientTelNumber, "processing");
                        //put client status as processing Taxi Fecthing him
                    }

                    //Taxi Man Sent msg finish
                    if (Request.QueryString["msg"] == "finish" & isATaxi(Request.QueryString["sender"]) &
                        (getTaxiStatus(Request.QueryString["sender"]) == "Unavailable"))
                    {
                        ChangeTaxiStatus(Request.QueryString["sender"], "available");
                        string clientTelNumber = clientTelNumberTaxiMustGo(Request.QueryString["sender"], "finish");

                        ChangeClientStatus(clientTelNumber, "finish");


                        Loging(Request.QueryString["sender"] + "change to available no more job");

                        String getUri =
                            @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                            Request.QueryString["sender"] + @"&messageData=" +
                            "thank you for serving us : Our system see you as a free Taxi now,and we may forward You  more client";
                        Loging("trying to send finish with Taxi");
                        WebRequest request = (WebRequest) WebRequest.Create(getUri);
                        // execute the request
                        HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                        //read the response
                        StreamReader responseReader =
                            new StreamReader(response.GetResponseStream());
                        String resultmsg = responseReader.ReadToEnd();
                        responseReader.Close();
                    }


                    // Write the string to a file.

                }

                    //Just log ppl with bad request Or Driver with bad msg
                else
                {
                    Loging("bad msg from tel number");
                }

                //TaxiMan Sent msg BreakDown
                if (Request.QueryString["msg"] == "breakDown" ||
                    Request.QueryString["msg"] == "pause" & isATaxi(Request.QueryString["sender"]))
                {

                    ChangeTaxiStatus(Request.QueryString["sender"], Request.QueryString["msg"]);
                    String getUri =
                        @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                        Request.QueryString["sender"] + @"&messageData=" +
                        "No More Client Will ne forwarded to You,please notify us when you are ready again";
                    Loging("trying  BreaK Down updated");
                    WebRequest request = (WebRequest) WebRequest.Create(getUri);
                    // execute the request
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    //read the response
                    StreamReader responseReader =
                        new StreamReader(response.GetResponseStream());
                    String resultmsg = responseReader.ReadToEnd();
                    responseReader.Close();
                }

                //Taxi breakDown Repair
                if (Request.QueryString["msg"] == "breakDownFinish" ||
                    Request.QueryString["msg"] == "pauseFinish" & isATaxi(Request.QueryString["sender"]))
                {

                    ChangeTaxiStatus(Request.QueryString["sender"], "available");
                    String getUri =
                        @"http://127.0.0.1:9333/ozeki?action=sendMessage&login=admin&password=abc123&recepient=" +
                        Request.QueryString["sender"] + @"&messageData=" +
                        "Our system have take into account that you are ready to serve Us";
                    Loging("put taxi available due to previous break Down");
                    WebRequest request = (WebRequest) WebRequest.Create(getUri);
                    // execute the request
                    HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                    //read the response
                    StreamReader responseReader =
                        new StreamReader(response.GetResponseStream());
                    String resultmsg = responseReader.ReadToEnd();
                    responseReader.Close();
                }

            }
        }
        catch(Exception ex)
        {
            Loging("!!Error!!!!"+ex.Message);
        }
    }
}