if (typeof (SurveyApp == 'undefined'))
{
    var SurveyApp = {};
}

SurveyApp.SurveyResultDisplay = (function () {
    var _config;
    var _privateVar;


    return {
        init: function (config) {
            _config = config;            

            /*Starts here*/

            $("#get-Survey-1").click(function(){                                           
                $.ajax({
                    url: '../Home/getSurvey',
                    type: 'GET',
                    dataType: 'json',
                    data:
                         {
                             surveyId : 1,                             
                         },
                    success: function (response) {
                        data = response;
                        debug = data;
                        var q = Object.keys(data.questions).length;                        
                        for(i=0;i<q;i++)
                        {
                            SurveyApp.SurveyResultDisplay.createQuestion(data.questions[i]);                            
                        }

                        $('input[type="radio"]').change(function(){
                            $('input[type="radio"]').each(function(){
                                var radioName = $(this).attr('value');
                                if(this.checked) {
                                    $("div[id*="+radioName +"-dep]").css('display', 'block');      // do something when selected
                                } else {
                                    $("div[id*="+radioName +"-dep]").css('display', 'none');       // do something when deselected
                                }
                            });
                        });


                        $('input[type="checkbox"]').change(function(){
                            var itemName = $(this).attr('value');
                            if($(this).prop('checked')==true)
                            {
                                $("div[id*="+itemName +"-dep]").each(function (i, el) {
                                    $(this).css("display", "block");
                                });
                            }
                            else
                            {
                                $("div[id*="+itemName +"-dep]").each(function (i, el) {
                                    $(this).css("display", "none");
                                });
                            }

                        });

                        //if any of the option is selected
                        $('input[type="option"]').on('change', function (e) {
                            var optSelected = $(this).attr('id');              ;
                        });


                        $('select').change( function() {
                            var pos = (this.value).lastIndexOf('-');                

                            $("option[value*=Dep-]").each(function (i, el) {
                                $(this).css("display", "none");
                            });

                            $("[value*=Dep-"+ (this.value).substring(pos+1) +"]").css('display', 'block');
                        });


                    },

                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });                
                                                  
            });
            
            
            $("#get-User-1-Result").click(function () {
                alert("Get Survy 1 results");

                $.ajax({
                    url: '../Home/getSurveyResult',
                    type: 'GET',
                    dataType: 'json',
                    data:
                         {
                             surveyId: 1,
                             userId: 1
                         },
                    success: function (response) {
                        data = response;
                        for (d = 0; d < data.results.length; d++) {
                            if (data.results[d].Type == "Radio") {
                                $("input[id*=Attr-" + data.results[d].AttrId + "]").attr('checked', true);
                                $("div[id*=Attr-" + data.results[d].AttrId + "-dep-]").css('display', 'block');                                
                            }
                            else if (data.results[d].Type == "CheckBox") {
                                $("input[id*=Attr-" + data.results[d].AttrId + "]").attr('checked', true);
                                $("div[id*=Attr-" + data.results[d].AttrId + "-dep-]").css('display', 'block');
                            }
                            else if (data.results[d].Type == "TextBox") {
                                $("input[id*=Attr-" + data.results[d].AttrId + "]").val(data.results[d].Value);
                            }
                            else if (data.results[d].Type == "TextArea") {
                                $("textarea[id*=Attr-" + data.results[d].AttrId + "]").text(data.results[d].Value);
                            }
                                //Work on this
                            else if (data.results[d].Type == "Option") {
                                $("option[value*=Attr-" + data.results[d].AttrId + "]").attr("selected", true);
                            }

                        }

                    },

                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });
            });
            

            /*Ends here*/

        },

        createQuestion: function (quest, dependentEntity, parentName, depno, parentQuestId) {
            if (dependentEntity == 1) //if dependent Question
            {
                $("#Q-" + parentQuestId).append('<div id="' + parentName + '-dep-' + depno + '" style="display: none"></div>');       //create an invisible div inside the main Question div=id="Q-parentQuestionId"

                if (quest.IsRequired == true) {
                    $("#" + parentName + "-dep-" + depno).append('<br><label for="Q-' + quest.EntityId + '" style="margin-left:' + 25 * quest.Indent + 'px" >Q' + quest.QuestionOrder + ' : ' + quest.Value + '<span style="color:red"> * </span></label><br>');     //add question label to div
                }
                else {
                    $("#" + parentName + "-dep-" + depno).append('<br><label for="Q-' + quest.EntityId + '" style="margin-left:' + 25 * quest.Indent + 'px" >Q' + quest.QuestionOrder + ' : ' + quest.Value + '</label><br>');     //add question label to div
                }

                //Case 1 : Has child Attributes
                var children = (Object.keys(quest.ChildAttributes).length);
                if (children > 0) {
                    var j = 0;
                    while (j < Object.keys(quest.ChildAttributes).length)        //add all child attributes to that question in that div
                    {
                        var current = j;
                        $("#" + parentName + "-dep-" + depno).append(SurveyApp.SurveyResultDisplay.createChildAttribute(quest.ChildAttributes[j], quest.EntityId, 1));
                        j = current;
                        j++;
                    }
                }


                //Case 2 : Has Subquestions or Dropdowns i.e Child Entities
                var childEntities = (Object.keys(quest.ChildEntities).length)
                {
                    if (childEntities > 0) {
                        var e = 0;
                        while (e < Object.keys(quest.ChildEntities).length) {
                            var currentEnt = e;
                            if (quest.ChildEntities[e].IsDependency == false) {
                                SurveyApp.SurveyResultDisplay.createEntity(quest.ChildEntities[e], 0, quest.EntityId, 0, parentName + '-dep-' + depno);
                            }
                            else {
                               SurveyApp.SurveyResultDisplay.createEntity(quest.ChildEntities[e], 1, quest.EntityId, 0, parentName + '-dep-' + depno);
                            }

                            e = currentEnt;
                            e++;
                        }

                    }
                }
            }
            else {       //Independent Question
                $("#Survey-Result-form").append('<div id="Q-' + quest.EntityId + '"></div>');
                if (quest.IsRequired == true) {
                    $("#Q-" + quest.EntityId).append('<br><label for="Q-' + quest.EntityId + '" style="margin-left:' + 25 * quest.Indent + 'px" >Q' + quest.QuestionOrder + ' : ' + quest.Value + '<span style="color:red"> * </span></label><br>');
                }
                else {
                    $("#Q-" + quest.EntityId).append('<br><label for="Q-' + quest.EntityId + '" style="margin-left:' + 25 * quest.Indent + 'px" >Q' + quest.QuestionOrder + ' : ' + quest.Value + '</label><br>');
                }

                //Case 1 : Has child Attributes
                var children = (Object.keys(quest.ChildAttributes).length);
                if (children > 0) {
                    var j = 0;
                    while (j < Object.keys(quest.ChildAttributes).length) {
                        var current = j;
                        SurveyApp.SurveyResultDisplay.createChildAttribute(quest.ChildAttributes[j], quest.EntityId, 0);
                        j = current;
                        j++;
                    }
                }

                //Case 2 : Has Subquestions or Dropdowns i.e Child Entities
                var childEntities = (Object.keys(quest.ChildEntities).length)
                {
                    if (childEntities > 0) {
                        var e = 0;
                        while (e < Object.keys(quest.ChildEntities).length) {
                            var currentEnt = e;
                            if (quest.ChildEntities[e].IsDependency == false) {
                               SurveyApp.SurveyResultDisplay.createEntity(quest.ChildEntities[e], 0, quest.EntityId, 0, quest.EntityId);
                            }
                            else {
                               SurveyApp.SurveyResultDisplay.createEntity(quest.ChildEntities[e], 1, quest.EntityId, 0, quest.EntityId);
                            }

                            e = currentEnt;
                            e++;
                        }

                    }
                }
            }

        },

        createChildAttribute : function(child,parentQuest,dep)    //dep specifies whether this attribute belongs to a dependent question or incase of option if it depends on selection of something
    {
        if (child.Type==3) //checkbox
        {
            if(dep==1)
            {
                return '<input type="checkbox" id="Q-'+ parentQuest+'-Attr-'+  child.AttrId+'" name="Q-'+ parentQuest +'-Attr-'+child.AttrId +'" value="Attr-'+child.AttrId +'" style="margin-left:'+ 25*child.Indent +'px" />'+ child.Value +'<br>';
            }
            else
            {
                //createCheckBox(child);
                $('#Q-'+parentQuest).append('<input type="checkbox" id="Q-'+ parentQuest+'-Attr-'+  child.AttrId+'" name="Q-'+parentQuest+'-Attr-'+child.AttrId +'" value="Attr-'+child.AttrId +'" style="margin-left:'+25*child.Indent+'px" />'+ child.Value +'<br>');
                for(depno=0;depno<(Object.keys(child.DependentEntities).length);depno++)
                {
                    SurveyApp.SurveyResultDisplay.createEntity(child.DependentEntities[depno],1,"Attr-"+child.AttrId,child.DependentEntities[depno].QuestionOrder,parentQuest) ;
                }
            }
        }
        else if(child.Type==4)    //radio
        {
            if(dep==1)
            {
                return '<input type="radio" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" name="Q-'+parentQuest +'" value="Attr-'+child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" />'+child.Value +'<br>'
            }
            else
            {
                $('#Q-'+parentQuest).append('<input type="radio" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" name="Q-'+parentQuest +'" value="Attr-'+ child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" />'+ child.Value +'<br>');
                for(k=0;k<(Object.keys(child.DependentEntities).length);k++)
                {
                    SurveyApp.SurveyResultDisplay.createEntity(child.DependentEntities[k],1,"Attr-"+child.AttrId,child.DependentEntities[k].QuestionOrder,parentQuest) ; //question/dropdown displayed based on selection of attribute child and its kth dep entity //second param tells attribute whose selection decides this
                }
            }
        }

        else if(child.Type==5)  //textbox
        {
            if(child.MaxCharacters!=null)
            {
                return '<input type="text" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'-Max-'+ child.MaxCharacters+'" placeholder="Max '+ child.MaxCharacters+'characters" name="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" /><br>';
            }               
            else
            {
                return '<input type="text" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'-Max-0" name="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" /><br>';
            }
        }
        else if (child.Type==6) //textarea
        {
            if(dep==1)
            {
                if(child.MaxCharacters!=null)
                {
                    return ('<textarea rows="4" cols="50" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'-Max-'+ child.MaxCharacters+'" placeholder="Max '+ child.MaxCharacters+' characters" name="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" ></textarea><br>');
                }
                else
                {
                    return ('<textarea rows="4" cols="50" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'-Max-0" name="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" ></textarea><br>');
                }                    
            }
            else
            {if(child.MaxCharacters!=null)
            {                    
                $('#Q-'+parentQuest).append('<textarea rows="4" cols="50" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'-Max-'+ child.MaxCharacters+'" placeholder="Max '+child.MaxCharacters +' characters" name="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" ></textarea><br>');}
            else 
            {
                $('#Q-'+parentQuest).append('<textarea rows="4" cols="50" id="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'-Max-0" name="Q-'+ parentQuest+'-Attr-'+ child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" ></textarea><br>');
            }
            }
        }
        else if(child.Type==7) //option
        {
            //selection of this option has dependent question
            if(child.DependentEntities!=null)
            {
                for(optDep=0;optDep<(Object.keys(child.DependentEntities).length);optDep++)
                {
                   SurveyApp.SurveyResultDisplay.createEntity(child.DependentEntities[optDep],1,child.Value+"-"+child.AttrId,child.DependentEntities[optDep].QuestionOrder,parentQuest) ;
                }

            }
            if(dep==1)
            {
                return('<option value="Dep-'+ parentQuest+'-Attr-'+ child.AttrId+'" style="display:none;margin-left:'+ 25*child.Indent +'px">'+child.Value+'</option>');
            }
            else{
                return('<option value="Dropdown-'+ parentQuest+'-Attr-'+child.AttrId+'" style="margin-left:'+ 25*child.Indent +'px" >'+child.Value +'</option>');
            }

        }

    },

     createEntity : function(entity,dependentEntity,parentName,depno,parentQuestId)  //second param tells if dependent quest/dropdown , parentName basically tells the dependent Attr/Question
    {
        if(entity.Type==1)  //question
        {
            SurveyApp.SurveyResultDisplay.createQuestion(entity,dependentEntity,parentName,depno,parentQuestId);
        }
        else if(entity.Type==2)      //dropdown
        {

            // create dropdown
            if(dependentEntity==1)  //if dependetn on selection of some attribute
            {
                $('#Q-'+parentName).append('<select id="Q-'+ parentName+'-Select-'+entity.EntityId +'-Entity-Dep-'+ entity.DependentEntityId+'" name="Q-'+ parentName+'-Select-'+entity.EntityId +'-Entity-Dep-'+ entity.DependentEntityId+'" style="margin-left:'+ 25*entity.Indent +'px" ></select></br></br>');
                $("#Q-"+ parentName+"-Select-"+entity.EntityId+"-Entity-Dep-"+ entity.DependentEntityId).append('<option value=""></option>');

                var options = (Object.keys(entity.ChildAttributes).length);
                if(options>0)
                {
                    var opt = 0;
                    while(opt < (Object.keys(entity.ChildAttributes).length))
                    {
                        var currentOpt = opt;
                        if(entity.ChildAttributes[opt].AttrDependencyId==null)
                        {
                            $("#Q-"+ parentName+"-Select-"+entity.EntityId +"-Entity-Dep-"+ entity.DependentEntityId).append(SurveyApp.SurveyResultDisplay.createChildAttribute(entity.ChildAttributes[opt],entity.EntityId,0));
                        }
                        else
                        {
                            $("#Q-"+ parentName+"-Select-"+entity.EntityId +"-Entity-Dep-"+ entity.DependentEntityId).append(SurveyApp.SurveyResultDisplay.createChildAttribute(entity.ChildAttributes[opt],entity.ChildAttributes[opt].AttrDependencyId,1));
                        }
                        opt = currentOpt;
                        opt++;
                    }
                }
            }
            else
            {
                $('#Q-'+parentName).append('<select id="Q-'+ parentName+'-Select-'+entity.EntityId +'" name="Q-'+ parentName+'-Select-'+entity.EntityId +'" style="margin-left:'+ 25*entity.Indent +'px"></select></br></br>');
                $("#Q-"+ parentName+"-Select-"+entity.EntityId).append('<option value=""></option>');

                var options = (Object.keys(entity.ChildAttributes).length);
                if(options>0)
                {
                    var opt = 0;
                    while(opt < (Object.keys(entity.ChildAttributes).length))
                    {
                        var currentOpt = opt;
                        if(entity.ChildAttributes[opt].AttrDependencyId==null)
                        {
                            $("#Q-"+ parentName+"-Select-"+entity.EntityId).append(SurveyApp.SurveyResultDisplay.createChildAttribute(entity.ChildAttributes[opt],entity.EntityId,0));
                        }
                        else
                        {
                            $("#Q-"+ parentName+"-Select-"+entity.EntityId).append(SurveyApp.SurveyResultDisplay.createChildAttribute(entity.ChildAttributes[opt],entity.ChildAttributes[opt].AttrDependencyId,1));
                        }
                        opt = currentOpt;
                        opt++;
                    }
                }
            }


        }
    }
    }


})();



