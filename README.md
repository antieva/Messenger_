# _Messenger App_

#### _Messenger, 05/17/18_

#### By _**Eva Antipina, Verna Santos, Dennise Ortega, Jim Palowski **_

## Description

_Messenger is a web site created to let people communicate online. The user can create an account, or login into already existed account, and see all their connection including new messages they has got since last logon. The user can click on any name in their list of connections and will be routed to the dialog page with dialog box conntainig all previuos messages from and to this user from that user on which was clicked. The user can delete any particular message from the dialog box (and database), as well as the whole conversation can be deleted. The user can update their profile or delete it. The user can find another user with whome they don't have a connection yet by typing user name in a search form and hitting "submit" button. The search will return a list of users whose names starts with the letters typed in the search bar._ 

## Specifications

## Link to the deployed project: https://epicodus-chat.azurewebsites.net

## Setup/Installation Requirements

* _Clone or download the repository._
* _Unzip the files into a single directory._
* _Open the Windows PowerShell and move to the directory_
* _Change the DB Connection String in Startup.cs file to "server=localhost;user id=root;password=root;port=8889;database=messenger;"_
* _Run "dotnet restore" command in the PowerShell._
* _Run "dotnet build" command in the PowerShell._
* _Run "dotnet run" command in the PowerShell._
* _Open a web browser of choice._
* _Enter "localhost:5000/home" into the address bar._

# Add Database to the Project

* _> CREATE DATABASE messenger;_
* _> USE messenger;_
* _> CREATE TABLE users (id serial PRIMARY KEY, name VARCHAR(255), password VARCHAR(255));_
* _> CREATE TABLE message (id serial PRIMARY KEY, text VARCHAR(255), fromUserId VARCHAR(255), toUserId VARCHAR(255), seen BIT(1));_

## Known Bugs

_None._

## Support and contact details

_If You run into any issues or have questions, ideas, concerns or would like to make a contribution to the code, please contact me via email: eva.antipina@gmail.com, vernajs@gmail.com, dennise.i.ortega@gmail.com, palowskijim@gmail.com _

## Technologies Used

_C#, HTML, Bootstrap_

### License

*Not licensed.*

Copyright (c) 2018 **_Eva Antipina, Verna Santos, Dennise Ortega, Jim Palowski_**
