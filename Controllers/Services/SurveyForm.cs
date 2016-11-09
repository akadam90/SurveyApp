using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SurveyApp.Models;

namespace SurveyApp.Controllers.Services
{
    public class SurveyForm
    {
        public IList<EntityModel> GetAllQuestions(int SurveyId)
        {
            IList<EntityModel> questions = new List<EntityModel>();

            //build each question 

            using (var connection = new SurveyAppEntitiesConnection())            
            {
                //get all entities ids representing questions
                var entities = (from e in connection.Entities
                               where e.SurveyId == SurveyId  && e.Type==1 && e.IsDependency==false 
                               orderby e.QuestionOrder
                               select e.EntityId).ToList();

                //Build question n its respective attributes around each entity
                for (int i = 0; i < entities.Count; i++)
                {
                    
                    //QuestionModel quest = new QuestionModel();
                    //quest.entity = entities.ElementAt(i);
                    //quest.LocalAttributes = GetLocalAttributes(entities.ElementAt(i).EntityId);
                    //quest.LocalEntities = GetLocalEntities(entities.ElementAt(i).EntityId);

                    questions.Add(getEntity(entities.ElementAt(i)));
                
                }
            }

            return questions;
        }
       

        /*To Build an Entity or Question*/
        public EntityModel getEntity(int id)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            {
                //get all entities representing questions
                var entity = (from e in connection.Entities
                              where e.EntityId == id
                              select new EntityModel()
                              {
                                  EntityId = e.EntityId,
                                  SurveyId = e.SurveyId,
                                  Name = e.Name,
                                  Caption = e.Caption,
                                  Id = e.Id,
                                  Type = e.Type,
                                  Value = e.Value,
                                  ParentQuestionId = e.ParentQuestionId,
                                  IsDependency = e.IsDependency,
                                  DependentAttrId = e.DependentAttrId,
                                  DependentEntityId = e.DependentEntityId,
                                  Indent = e.Indent,
                                  IsRequired = e.IsRequired,
                                  QuestionOrder = e.QuestionOrder
                              }).FirstOrDefault();

                entity.ChildAttributes = GetChildAttributes(entity.EntityId);

                if (entity.Type == 1)
                {
                    entity.ChildEntities = GetChildEntities(entity.EntityId);               
                }

                else if (entity.Type == 2)
                { 
              
                    //check for DependentAttributes
                    var dependentAttrs = (from e in connection.Attributes
                                             where e.EntityDepId == entity.EntityId
                                             select e.AttrId).ToList();

                    IList<AttributeModel> depAttrsList = new List<AttributeModel>();                                        
                        for (int k = 0; k < dependentAttrs.Count; k++)
                        {                                                           
                            depAttrsList.Add(getAttribute(dependentAttrs.ElementAt(k)));
                        }
                        entity.DependentAttributes = depAttrsList;
                    
 
                          //check for DependentEntities
                            var dependentEntities = (from e in connection.Entities
                                                     where e.DependentEntityId == entity.EntityId
                                                     select e.EntityId).ToList();
                            IList<EntityModel> depEntitiesList = new List<EntityModel>();
                            if (dependentEntities != null)
                            {
                                for (int k = 0; k < dependentEntities.Count; k++)
                                {                                                                    
                                    depEntitiesList.Add(getEntity(dependentEntities.ElementAt(k)));                                    
                                }
                              entity.DependentEntities = depEntitiesList;
                            }                 
                }
                               
                    return entity;                          
            }
        }


        //this gets all local attributes to be displayed with the question
        public IList<AttributeModel> GetChildAttributes(int entityId)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            {
                IList<AttributeModel> childAttributes = new List<AttributeModel>();

                /*LocalAttributes are the ones which are immediately displayed after the question*/
                var attributes = (from att in connection.Attributes
                                  where att.ParentEntityId == entityId
                                  select att.AttrId).ToList();
                            

                for (int i = 0; i < attributes.Count; i++)
                {
                    childAttributes.Add(getAttribute(attributes.ElementAt(i)));
                }
                    
                return childAttributes;

            }
        }      

        /*To Build any Attribute*/
        public AttributeModel getAttribute(int id)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            {
                AttributeModel attribute = (from att in connection.Attributes
                                 where att.AttrId == id
                                 select new AttributeModel()                               
                                {
                                AttrId = att.AttrId,                              
                                ParentEntityId = att.ParentEntityId,
                                AttrDependencyId = att.AttrDependencyId,
                                Type = att.Type,
                                Value = att.Value,
                                Indent = att.Indent,
                                MaxCharacters = att.MaxCharacters
                               }).FirstOrDefault();                            

                if (attribute!=null)
                {
                   
                        /*Now all below scenarios are possible for dependent attributes of radio , checkbox, dropdown, ListBox*/
                        if (attribute.Type != 5 && attribute.Type != 6 && attribute.Type != 7)
                        {

                            //check for dependents attributes
                            var dependents = (from attr in connection.Attributes
                                              where attr.AttrDependencyId == attribute.AttrId
                                              select attr.AttrId).ToList();
                            IList<AttributeModel> depList = new List<AttributeModel>();

                                for (int j = 0; j < dependents.Count; j++)
                                {
                                    depList.Add(getAttribute(dependents.ElementAt(j)));       
                                }
                                attribute.DependentAttributes = depList;   //add to dependentattributes.Add()                                                                                                                                                            

                            //check for DependentEntities
                            var dependentEntities = (from e in connection.Entities
                                                     where e.DependentAttrId == attribute.AttrId
                                                     select e.EntityId).ToList();
                            IList<EntityModel> depEntitiesList = new List<EntityModel>();
                            if (dependentEntities != null)
                            {
                                for (int k = 0; k < dependentEntities.Count; k++)
                                {
                                    //add to dependentattributes                                    
                                    depEntitiesList.Add(getEntity(dependentEntities.ElementAt(k)));                                    

                                }
                                attribute.DependentEntities = depEntitiesList;
                            }

                            //if (attribute.Type == 2) //i.e dropdown
                            //{
                            //    //get all options for it 
                            //    var children = (from attr in connection.Attributes
                            //                    where attr.ParentAttrId == attribute.AttrId
                            //                    select attr.AttrId).ToList();
                            //    IList<AttributeModel> childList = new List<AttributeModel>();
                                
                            //        for (int m = 0; m < children.Count; m++)
                            //        {
                                       
                            //                childList.Add(getAttribute(children.ElementAt(m)));                                        
                            //        }
                            //        attribute.ChildAttributes = childList;                                
                            //}
                        }                    
                }

                return attribute;
            
            }
        }

        //Build a local Entity (Dropdown or sub-question)
        public IList<EntityModel> GetChildEntities(int entityId)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            { 
                IList<EntityModel> localEntities = new List<EntityModel>();

                var entities = (from e in connection.Entities
                                where e.ParentQuestionId == entityId
                                select e.EntityId).ToList();
                
                for (int i = 0; i < entities.Count; i++)
                {
                    localEntities.Add(getEntity(entities.ElementAt(i)));
                }

                //var subQuestions = (from e in connection.Entities                                
                //                where e.ParentQuestionId == entityId && e.Type==1
                //                select e.EntityId).ToList();
                
                //for (int i = 0; i < subQuestions.Count; i++)
                //{                    
                //    localEntities.Add(getQuestion(subQuestions.ElementAt(i)));
                //}

                ////incase of dropdowns
                //var subEntities = (from e in connection.Entities
                //                   where e.ParentQuestionId == entityId && e.Type == 2
                //                   select e.EntityId).ToList();
                
                //for (int i = 0; i < subEntities.Count; i++)
                //{
                //    localEntities.Add(getEntity(subEntities.ElementAt(i)));
                //}

                    return localEntities;

            }
        }


        public SurveyModel getSurvey(int SurveyId)
        {
            using (var connection = new SurveyAppEntitiesConnection())
            {
                var survey = (from s in connection.Surveys
                              where s.Id == SurveyId
                              select new SurveyModel()
                              {
                                  SurveyId = s.Id,
                                  Title = s.Title,
                                  CreatedOn = s.CreatedOn                                  
                                 
                              }).FirstOrDefault();

                if (survey != null)
                {
                    survey.questions = GetAllQuestions(SurveyId);
                    survey.RequiredQuestions = getRequiredQuestions(SurveyId);
                }

                

                return survey;
            }
        }
    
       public IList<SurveyResultModel> getSurveyResult(int surveyId,int userId)
       {
           using (var connection = new SurveyAppEntitiesConnection())
           {
           IList<SurveyResultModel> surveyResults = new List<SurveyResultModel>();
           var results = (from s in connection.SurveyResults.
                            Include("Attribute").Include("AttributeType")
                          where s.SurveyId == surveyId && s.UserId == userId 
                          select new SurveyResultModel()
                          {
                              Surveyid = s.SurveyId,
                              UserId = s.UserId,
                              EntityId = s.EntityId,
                              AttrId =s.AttrId,
                              Value = s.Value,
                              Type = s.Attribute.AttributeType.Type                              
                          }).ToList();
           for (int i = 0; i < 5; i++)
           {
               var x = i;
           }
               return results;
           }
           
       
       }

       public IList<EntityModel> getRequiredQuestions(int surveyId)
      {
          using (var connection = new SurveyAppEntitiesConnection())
            {
                var questIds = (from e in connection.Entities
                                where e.IsRequired == true && e.SurveyId==surveyId && e.Type==1 //(Type for Questions)
                                select new EntityModel(){
                                EntityId = e.EntityId,
                                QuestionOrder=e.QuestionOrder,
                                Value = e.Value,
                                IsDependency=e.IsDependency,
                                DependentAttrId=e.DependentAttrId,
                                DependentEntityId=e.DependentEntityId
                                }).ToList();
                return questIds;                                    
            }
            
       }
        
       public SurveyTitleModel getSurveyTitles()
       {
           using (var connection = new SurveyAppEntitiesConnection())
           {
               SurveyTitleModel sur = new SurveyTitleModel();

               var surveys = (from s in connection.Surveys
                              select new TitleModel()
                              {
                                  SurveyId = s.Id,
                                  SurveyTitle = s.Title
                              }).ToList();
               sur.titles = surveys;
               return sur;
           }
       }

       public bool AddUser(string Name, string Email, int SelectedSurvey)
       {
           using (var connection = new SurveyAppEntitiesConnection())
           { 
               string globalId;

           //1. If user not there create one new 
               var userId = (from u in connection.Users
                           where u.Name == Name && u.Email==Email
                            select u.Id).SingleOrDefault();

               var i = (from s in connection.SurveyLinkInfoes
                        where s.SurveyId == SelectedSurvey && s.UserId == userId
                        select s.GUID).SingleOrDefault();

               if (i != null)
               {
                   return false;
               }
               if(userId==null)
               {
                   User u = new User() { 
                   Name = Name,
                   Email = Email
                   };
                   connection.Users.Add(u);
                   connection.SaveChanges();

                   userId = (from newUser in connection.Users
                                 where newUser.Name == Name && newUser.Email == Email
                                 select newUser.Id).SingleOrDefault();
               }
               

           //2. Create GUID for that USer
               
              globalId = Guid.NewGuid().ToString();
               

           //3. create entry in USerSurvey table for Survey, User,Link,SurveyStatus
              SurveyLinkInfo link = new SurveyLinkInfo() { 
              GUID = globalId,
              UserId = userId,
              SurveyId = SelectedSurvey,
              SurveyStatusId = 1 //(Not Started)
              };
              connection.SurveyLinkInfoes.Add(link);
              connection.SaveChanges();
              
               return true;
           }       
       }

       public IList<SurveyLinkInfoModel> GetSurveyLinksInfo()
       {
           using (var connection = new SurveyAppEntitiesConnection())
           {
               var links = (from link in connection.SurveyLinkInfoes.Include("User").Include("Survey").Include("SurveyStatu")
                            select new SurveyLinkInfoModel()
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