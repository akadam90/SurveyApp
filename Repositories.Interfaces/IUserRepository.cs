using SurveyApp.Controllers.Models;
using SurveyApp.Repositories.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SurveyApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        //UserModel GetUser();
        void AddUser(UserBaseModel user);
        bool UserExists(IList<string> names, IList<string> emails, string survey);
        bool AddUsersList(HttpRequest httpRequest);
        //IList<UserModel> getAllUsers(string SelectedSurvey, int ClientId);
        
    }
}
