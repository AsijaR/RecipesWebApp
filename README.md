# Web application using ASP .NET Core and Angular

## Table of contents
* [General info](#general-info)
* [BPMN](#bpmn)
* [Prototype (Adobe XD)](#adobe-xd)
* [Technologies](#technologies)
* [Backend](#backend)
* [Frontend](#frontend)
* [Setup](#setup)
* [Deployment](#deployment)
* [Credentials](#credentials)

## General info
This project was a part of my university project. The idea is to create a website for the recipes and giving users the ability to search recipes by ingredients. 
Users can create recipes but also can sell their meals to others.
They can track all of the orders in their dashboard. Every order has a "Waiting" status until the chef(recipe owner) changes it to "Approved", "Denied" or "Completed". For every status change
the person who ordered the meal gets an email notification. What's interesting here is when you ordering a meal, users can set a date when they want a meal to be delivered (e.g. for birthdays,
parties, weddings, etc) and they can send a special note to the chef (if they are allergic to something or have a special request).

This project has been developed in 3 stages:
* Creating a Business Process Model and Notation (BPMN)
* Creating a mockup/prototype in ADOBE XD
* Coding :)

## BPMN
Business Process Model and Notation (BPMN) is a standard for business process modeling that provides a graphical notation for specifying business processes in a Business Process Diagram (BPD),[3]
based on a flowcharting technique very similar to activity diagrams from Unified Modeling Language (UML).The objective of BPMN is to support business process management,for both technical users 
and business users, by providing a notation that is intuitive to business users, yet able to represent complex process semantics. [Read more at wikipedia](https://en.wikipedia.org/wiki/Business_Process_Model_and_Notation) 
BPMN model was done in [Cardanit](https://www.cardanit.com/bpmn-editor/).
* #### Users actions
 ![alt text](https://user-images.githubusercontent.com/41241345/133850060-a2a7b8b9-7ec3-4e3e-879e-485bd8d027f5.png?raw=true)
 * Email confirmation
 
 ![alt text](https://user-images.githubusercontent.com/41241345/133850069-e161780a-04e0-4ddf-917e-c2e11d2e9dd2.png?raw=true)
 * Profile editing
 
 ![alt text](https://user-images.githubusercontent.com/41241345/133850084-454ef629-20c4-444a-bbbd-a742ede58c4b.png?raw=true)
* Publishing recipe
 
 ![alt text](https://user-images.githubusercontent.com/41241345/133850096-55c3c9cf-eea2-410f-9989-c13b8ad48c7b.png?raw=true) 
 * Searching recipes
 
  ![alt text](https://user-images.githubusercontent.com/41241345/133850174-cf9b3ade-3943-42f5-a64a-b324777d4f8b.png?raw=true)
  * Order approval
  
  ![alt text](https://user-images.githubusercontent.com/41241345/133850183-a1ba18fd-7011-4cb1-b258-2b07c8032ffa.png?raw=true)
  
###You can download BPMN file by clicking [here](https://github.com/AsijaR/RecipesServer/files/7188312/BPMN.Recipes.Project.zip) 

## Adobe XD
I have created a design in Adobe XD just to have a raw pattern of how my application will look like and how should I design my database before I start coding.
* #### Home page
![alt text](https://user-images.githubusercontent.com/41241345/133853336-b553d68a-ffd5-48bb-8db8-72a87af04c6c.png?raw=true)
* #### Recipe page
![alt text](https://user-images.githubusercontent.com/41241345/133853641-97a8cd4a-a7ba-4f1e-af32-18be0a01c7df.png?raw=true)
![alt text](https://user-images.githubusercontent.com/41241345/133853678-2b3799a1-ef22-42d8-a6a6-6737ad67a7f0.png?raw=true)
![alt text](https://user-images.githubusercontent.com/41241345/133853697-d8cf94cb-aa29-49c4-9b0d-9afe15671da4.png?raw=true)
* #### Add recipe page
![alt text](https://user-images.githubusercontent.com/41241345/133853787-c495affd-00fd-453e-a61f-150dd895f27d.png?raw=true)
* #### My recipes page
![alt text](https://user-images.githubusercontent.com/41241345/133853890-2743a2f3-d3ae-4e79-a244-ad979d35b2a1.png?raw=true)
* #### Order meal
![alt text](https://user-images.githubusercontent.com/41241345/133853933-45b3bca7-c2ae-44e9-bf8b-b994d5894700.png?raw=true)

## Technologies
Project is created with:
* ASP .NET Core (5.0)
* Angular (12.0)
* Sqlite
* Postgres
*Bootstrap
*Material Design

## Backend
For the backend, I have chosen to do my application using Asp .Net Core (5.0) using the "First Code" approach.
###Important developing part
* The database  was created using EntityFramoworkCore 
* For data protection(DTO), I have used AutoMapper
* User is  authenticated using Tokens (JWT) and JWTBearer
* Users and recipes pictures are stored at [Cloudinary](https://cloudinary.com/)
* API's are tested using Postman
* Emails are send using SMTP
* Adding roles and users is done using ASP NET Identity

The most challenging thing for me to do is to make a function for Recipe Editing, to be more precise the part for editing recipes ingredients. I had a many-to-many relationship, a user could delete an existing ingredient, add a new one or change the amount of an ingredient.
*I had to separate ingredients that are deleted and the ones that are the same as in the "old" ingredients list.
*For the ingredients that are the same, I had to check if any changes in the "amount" column were made if so I had to update them.
*If a new ingredient was added, I had to check if I had that ingredient already in the database (so I don't have any duplicates).
#####The important part was making code asynchronous and taking care of threads since I had many database requests.

## Frontend
Angular framework was used for the frontend part of the application with the help of Material Design and Bootstrap.
###Important developing part
*For preventing unauthorized access Guards are used
*Reactive forms with validations are used for adding recipes, making orders, registration, etc
*When user logs in their user name and token are stored in Web browsers local storage, so when a user refreshes the page is still logged in
*Attribute directives are used for user roles
*Interceptor for handling tokens 
*Picture upload and gallery

Everything is made of small components (like puzzle pieces) that are putted together into one page. 
**The part that took me the most of time and the part that im proud of is "Add new recipe". 
"Add new recipe form" is created from 6 little forms that are wroking like one. Every child notifies the parent what data they got and if the validatation errors are caught. 
**For the ingredients and directions form i have used form array, whenever user clicks on ingredient/direction field new form is created(dynamically). Form[i+1] has the information if form[i] has any data, if it doesnt, user can click on form[i+1] but new form wont be created. Validation applies for every form array and im accessing ingredients/directions array from parent control.
## Setup

For running this project in VSC
** starting Asp Net
```aspnet
$ cd RecipesServer
$ dotnet run
```
Or you can just use Visual Studio :)

** starting Angular in VSC
```angular
$ cd RecipesUIAngular
$ npm install
$ ng serve -o
```
## Deployment
Application is deployed using Heroku.
#### Check out the website by clicking [here](https://recipeswebsiteapp.herokuapp.com/home)

## Credentials
Because this project was created as part of my university project, this website is only for personal use. All the content you see is taken 
from [Simple Recipes](https://www.simplyrecipes.com). I dont own any of their pictures or content. 

Icons are taken from [Freepik](https://www.freepik.com) and [Flaticon](https://www.flaticon.com/).

#### The whole code was fully written by Asija Ramovic.
