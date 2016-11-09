using SurveyApp.Controllers.Models;
using SurveyApp.Repositories.Implementation;
using SurveyApp.Repositories.Interfaces;
using SurveyApp.Repositories.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SurveyApp.Controllers
{
    public class SurveyApiController : ApiController
    {
        public IUserRepository userRepository;
        public ISurveyTakersRepository surveyTakers;

        public SurveyApiController()
        {
            userRepository = new UserRepository();
            surveyTakers = new SurveyTakersRepository();
        }

        [HttpGet]
        public IList<SurveyTakerInfoModel> GetSurveyTakersInfo(string SelectedSurvey, string AppKey, int ClientId)
        {
                return surveyTakers.GetSurveyTakersInfo(SelectedSurvey,AppKey,ClientId);
        }

        
    }
}
