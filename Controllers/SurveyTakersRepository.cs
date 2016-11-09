using SurveyApp.Models;
using SurveyApp.Repositories.Interfaces;
using SurveyApp.Repositories.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyApp.Controllers
{
    class SurveyTakersRepository : ISurveyTakersRepository
    {
        public IList<SurveyTakerInfoModel> GetSurveyTakersInfo(string SelectedSurvey, string AppKey, int ClientId)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            {
                var links = (from link in connection.SurveyLinkInfoes.Include("User").Include("Survey").Include("SurveyStatu")
                             select new SurveyTakerInfoModel()
                             {
                                 Name = link.User.Name,
                                 Email = link.User.Email,
                                 Status = link.SurveyStatu.Status,

                                 Link = "https://www.smganalytics.com:444/SurveyApp_qa/Home/Index/" + link.GUID

                             }).ToList();
                return links;
            }
        }
    }
}
