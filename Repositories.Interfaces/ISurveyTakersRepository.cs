using SurveyApp.Repositories.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Repositories.Interfaces
{
    public interface ISurveyTakersRepository
    {
         IList<SurveyTakerInfoModel> GetSurveyTakersInfo(string SelectedSurvey, string AppKey, int ClientId);       

    }
}
