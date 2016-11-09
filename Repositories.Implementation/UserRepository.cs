using SurveyApp.Controllers.Models;
using SurveyApp.Models;
using SurveyApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SurveyApp.Repositories.Implementation
{
    public class UserRepository:IUserRepository
    {
        public void AddUser(UserBaseModel user)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            {                
                    string globalId;
                    var surveyId = Int32.Parse(user.SelectedSurvey);
                    //1. If user not there create one new 
                    var userId = (from u in connection.Users
                                  where u.Name == user.Name && u.Email == user.Email
                                  select u.Id).SingleOrDefault();
                  
                    if (userId == null || userId == 0)
                    {
                        User u = new User()
                        {
                            Name = user.Name,
                            Email = user.Email
                        };
                        connection.Users.Add(u);
                        connection.SaveChanges();

                        userId = (from newUser in connection.Users
                                  where newUser.Name == user.Name && newUser.Email == user.Email
                                  select newUser.Id).SingleOrDefault();
                    }


                    //2. Create GUID for that USer
                    globalId = Guid.NewGuid().ToString();


                    //3. create entry in USerSurvey table for Survey, User,Link,SurveyStatus
                    SurveyLinkInfo link = new SurveyLinkInfo()
                    {
                        GUID = globalId,
                        UserId = userId,
                        SurveyId = surveyId,
                        SurveyStatusId = 1,//(Not Started)
                        ApplicationKey = user.AppKey,
                        ClientId = user.ClientId

                    };
                    connection.SurveyLinkInfoes.Add(link);
                    connection.SaveChanges();                             
            }
        }
  
        
        public bool UserExists(IList<string> names, IList<string> emails, string surveyId)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            {
                var survey = Int32.Parse(surveyId);
                for (int i = 0; i < names.Count; i++)
                {                   
                    var name = names[i];
                    var email = emails[i];
                    var userId = (from u in connection.Users
                                  where u.Name == name && u.Email == email
                                  select u.Id).SingleOrDefault();

                    var surveyLink = (from s in connection.SurveyLinkInfoes
                             where s.SurveyId == survey && s.UserId == userId
                             select s.GUID).SingleOrDefault();

                    if (surveyLink != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool AddUsersList(HttpRequest httpRequest)
        {
           try
            {
                IList<string> names = new List<string>();
                IList<string> emails = new List<string>();

                if (httpRequest.Form["Name"] != "" && httpRequest.Form["Email"] != "")
                {
                    names.Add(httpRequest.Form["Name"]);
                    emails.Add(httpRequest.Form["Email"]);
                    if (UserExists(names, emails, httpRequest.Form["SelectedSurvey"]) == true)
                    {
                        return false;
                    }
                    else
                    {
                        UserBaseModel user = new UserBaseModel()
                        {
                            AppKey = httpRequest.Form["ApplicationKey"],
                            ClientId = Int32.Parse(httpRequest.Form["ClientId"]),
                            SelectedSurvey = httpRequest.Form["SelectedSurvey"],
                            Name = httpRequest.Form["Name"],
                            Email = httpRequest.Form["Email"]
                        };
                        AddUser(user);
                        return true;
                    }
                }
                else if (httpRequest.Files.Count > 0)   //if file exists in the form
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                        postedFile.SaveAs(filePath);                                                

                        //now read the csv file
                        var reader = new StreamReader(File.OpenRead(filePath));

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            string[] values = line.Split(',');
                            names.Add(values[0]);
                            emails.Add(values[1]);
                        
                        }

                        if (UserExists(names, emails, httpRequest.Form["SelectedSurvey"]) == false)
                        { return false; }
                        else
                        {
                            for(int i=0;i<names.Count;i++)
                            {
                                UserBaseModel user = new UserBaseModel()
                                {
                                    AppKey = httpRequest.Form["ApplicationKey"],
                                    ClientId = Int32.Parse(httpRequest.Form["ClientId"]),
                                    SelectedSurvey = httpRequest.Form["SelectedSurvey"],
                                    Name = names[i],
                                    Email = emails[i]
                                };
                            //put code to validate Email address. If even one email address is not in proper format don't save the entire file and send back the error message to the view.
                                AddUser(user);
                            }
                        }
                    }
                }
               return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }}            
}