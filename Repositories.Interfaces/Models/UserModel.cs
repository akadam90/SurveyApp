using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyApp.Repositories.Interfaces.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}