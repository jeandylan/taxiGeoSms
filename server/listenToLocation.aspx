<%@ Page Language="C#" AutoEventWireup="true" CodeFile="listenToLocation.aspx.cs" Inherits="server_Default" %>

<!DOCTYPE html>


<html>
<head>
	<meta name="viewport" content="width-device-width, initial-scale=1">
	<link rel="stylesheet" href="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.css">
  	<script src="http://code.jquery.com/jquery-1.11.3.min.js"></script>
  	<script src="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.js"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js">
</script>
<script>
    $(document).ready(function () {
        alert("Location was sent sucessfully");
    });
</script>
<style>
  #msg{
    width:400px;
    height:400px;
    text-align:center;
     margin: auto;

  }
</style>
</head>
    <body>
<div data-role="page" id="homepage" data-theme="b">
  <div data-role="header" data-position="fixed">
    <h1>Taxi Service</h1>
  </div>
   
  <div data-role="main" class="ui-content">



  <div id="msg"  data-role="main" class="ui-content" >
    <p><b>Thank you for using choosing our taxi service.</b></p>
    <p>A taxi will be dispatched and you will shortly receive a confirmation sms for you trip.</p>
  </div>
  
  </div>
      

    

  <div data-role="footer" data-position="fixed" >
    <div data-role="controlgroup" data-type="horizontal" style="text-align:center;">
     <p> &copy; Copyright 2015, AFA Taxi Service</p>
    </div>
  </div>
</div>
</body>

</html>
