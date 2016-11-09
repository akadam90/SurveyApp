using SurveyApp.Controllers.Models;
using SurveyApp.Repositories.Implementation;
using SurveyApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SurveyApp.Controllers
{
    public class UploadApiController : ApiController
    {
        public IUserRepository userRepository;

        public UploadApiController()
        {
            userRepository = new UserRepository();
        }

        public string AddUsers()
        {                       
            try
            {
                var httpRequest = HttpContext.Current.Request;

                string ip = HttpContext.Current.Request.UserHostAddress;               

                //code to check ip

                if (userRepository.AddUsersList(httpRequest))
                {
                    return "All Users Added Successfully ";
                }
                else
                {
                    return "Error : One or more User already added to this Survey. Check for Duplicates.";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }            
        }
    }
}       
   
