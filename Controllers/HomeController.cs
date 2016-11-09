using SurveyApp.Controllers.Services;
using SurveyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SurveyApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            SurveyForm sur = new SurveyForm();
            SurveyModel survey = new SurveyModel();

            survey = sur.getSurvey(1);            

            return View(survey);
        }

        [HttpGet]
        public ActionResult getSurvey(int surveyId)
        {
            SurveyForm sur = new SurveyForm();
            SurveyModel survey = new SurveyModel();

            survey = sur.getSurvey(surveyId);

            return Json(survey, JsonRequestBehavior.AllowGet);
            //return survey;
        }

        [HttpGet]
        public JsonResult getSurveyResult(int surveyId, int userId)
        {
           
                SurveyForm sur = new SurveyForm();
                SurveyResultModel survey = new SurveyResultModel();
                ResultModel result = new ResultModel();
                result.SurveyId = surveyId;
                result.UserId = userId;
                result.results  = sur.getSurveyResult(surveyId, userId);

               // return result;
                 return Json(result, JsonRequestBehavior.AllowGet);
          

            //catch(Exception ex)
            //{
            //    return Json(ex.Message, JsonRequestBehavior.AllowGet);
            //}

            
            

        }

        //int id, IList<FormObjectModel> formData
        //put this function in SurveyForm.cs
        [HttpPost]
        public string SaveSurveyResult(FormCollection formData)
        {
            try
            {
                using (var connection = new SurveyAppEntitiesConnection())
                {
                    char[] delim = { '-' };
                    var saveOption = formData["Save-Option-Hidden"];

                    var userId = Int32.Parse(formData["User-Id-Hidden"]);
                    var surveyId = Int32.Parse(formData["Survey-Id-Hidden"]);

                    var surveyInfo = (from s in connection.SurveyLinkInfoes
                                      where s.UserId == userId && s.SurveyId == surveyId
                                      select s).SingleOrDefault();

                    if (saveOption == "1")
                    {
                        surveyInfo.SurveyStatusId = 2;
                    }
                    else if (saveOption == "2")
                    {
                        surveyInfo.SurveyStatusId = 3;
                    }

                        foreach (var key in formData.AllKeys)
                        {
                            if (!key.Contains("Hidden"))
                            {
                            
                            bool saveResult = true;
                            SurveyResult result1 = new SurveyResult()
                            {
                                SurveyId = surveyId,
                                UserId = userId,
                            };

                            if (key.Contains("Select"))     //For Dropdown
                            {
                                result1.EntityId = Int32.Parse(key.Split(delim)[3]);
                                result1.AttrId = Int32.Parse(formData[key].Split(delim)[3]);
                                result1.Value = null;
                            }
                            else
                            {
                                int attr = 0;
                                result1.EntityId = Int32.Parse(key.Split(delim)[1]);

                                if (key.Split(delim).Length > 2)         //contains attribute Id incase checkbox, textbox, textArea
                                {
                                    attr = Int32.Parse(key.Split(delim)[3]);
                                }
                                else if (formData[key] != null)
                                {
                                    attr = Int32.Parse(formData[key].Split(delim)[1]);       //incase of radio
                                }

                                //To Save Values for TextBoxes and TextAreas
                                var type = (from a in connection.Attributes.Include("AttributeTypes")
                                            where a.AttrId == attr
                                            select a.AttributeType.Type).SingleOrDefault();
                                if (type == null)
                                {
                                    saveResult = false;
                                }

                                else if (type == "TextBox" || type == "TextArea")
                                {
                                    if (formData[key] != "")
                                    {
                                        result1.Value = formData[key];
                                        result1.AttrId = Int32.Parse(key.Split(delim)[3]);
                                    }
                                    else
                                    {
                                        saveResult = false;
                                    }
                                }

                               //For radio , checkbox, 
                                else if (type == "Radio")
                                {
                                    result1.Value = null;
                                    result1.AttrId = Int32.Parse(formData[key].Split(delim)[1]);
                                }
                                else if (type == "CheckBox")
                                {
                                    result1.Value = null;
                                    result1.AttrId = Int32.Parse(key.Split(delim)[3]);
                                }

                            }

                            if (saveResult == true)
                            {
                                connection.SurveyResults.Add(result1);
                            }
                        }
                        }

                    connection.SaveChanges();
                    return "Survey Saved Sucessfully";
                }                                               
            }

            catch (Exception ex)
            {
                return ex.Message;            
            }        
        }

        [HttpGet]
        public JsonResult GetRequiredQuestions(int surveyId)
        {
            SurveyForm sur = new SurveyForm();
            return Json(sur.getRequiredQuestions(surveyId), JsonRequestBehavior.AllowGet);
                              
        }
        public ActionResult SurveyResult()
        {          
            return View();
        }

        [HttpPost]
        public string AddUser(string Name, string Email, string SelectedSurvey)
        {
            try
            {
                SurveyForm sur = new SurveyForm();
                if (sur.AddUser(Name, Email, Int32.Parse(SelectedSurvey)) == true)
                {
                    return ("User Added Succesfully");
                }                
                else
                {
                return ("User have been already added for this Survey");
                }
            }
            catch (Exception ex)
            { 
            //return ("There was some error in adding user");
                return ex.Message;
            }            
        }

        public ActionResult SurveyDashboard()
        {
            ViewBag.Message = "All Surveys for this Admin";
            SurveyForm sur = new SurveyForm();
            SurveyTitleModel surveys = new SurveyTitleModel();
            surveys = sur.getSurveyTitles();
            return View(surveys);
        }

        [HttpGet]
        public JsonResult GetSurveyLinksInfo()
        {        
                SurveyForm sur = new SurveyForm();                
                return Json(sur.GetSurveyLinksInfo(), JsonRequestBehavior.AllowGet);                       
        }
    }
}