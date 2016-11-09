if (typeof (SurveyApp == 'undefined')) {
    var SurveyApp = {};
}

SurveyApp.SurveyDashboard = (function () {

    return {
        init: function () {

            //$("#Survey-Dashboard-Form").submit(function (e) {
            //    var passVal = true;
            //    var a = $('select[name=SelectedSurvey]').val()
                               
            //    if (a == 0)
            //    {
            //        passVal = false;
            //        $("#ErrorModal .modal-body").html('<div>Please select a <span style="color:red; font-weight:bold">Survey</span></div>');
            //        $("#ErrorModal").modal('show');                    
            //    }//  passVal = 1;
            //    else if (document.getElementById("User-Name").value == '')
            //     {
            //        passVal = false;
            //        $("#ErrorModal .modal-body").html('<div>Please enter <span style="color:red; font-weight:bold">User Name</span></div>');
            //        $("#ErrorModal").modal('show');
            //      } 
            //    else if (document.getElementById("User-Email").value == '') {
            //        passVal = false;
            //        $("#ErrorModal .modal-body").html('<div>Please enter <span style="color:red; font-weight:bold">Email Address</span></div>');
            //        $("#ErrorModal").modal('show');
            //    }
            //    else {                    
            //        var email = document.getElementById("User-Email").value;
            //        var atpos = email.indexOf("@");                    
            //        var dotpos = email.lastIndexOf(".");                    
            //        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {
            //            alert("Not a valid e-mail address");
            //            passVal = false;
            //        }
            //    }                

            //    return passVal;
            //}),

            //$("#Survey-Dashboard-Form").ajaxsubmitwebapi({
            //    onBeforeSubmit: function (e) {
                    
            //    },
            //    onSuccess: function (response) {
            //        alert(response);
            //        //if (response.Success == true) {
            //        //    alert(response.Message);
            //        //}
            //        //else {
            //        //    alert(response.Message);
            //        //}
            //    },
            //    onError: function (xhr, status, error) {
            //        alert(error);
            //    }
            //});

            $('#Survey-Grid').DataTable({                                
                    ajax: {
                        url: '../api/SurveyApi/GetSurveyTakersInfo',
                        data: {
                            SelectedSurvey: "1",
                            AppKey : "AppLOS101",
                            ClientId:1
                        },
                        //the format expected is { data:[]} but the api returns it as: []
                        //to avoid having to make a c# wrapper - this dataSrc function is used to transform the initial set of data
                        dataSrc: function (json) { return json; }
                    },
                  columns: [                                                              
                        {
                            data: 'Name',
                        //    render: function (data, type, row) {
                        //    return row.FirstName + row.LastName;
                        //}
                        },
                          {
                              data: 'Email',
                          },
                          {
                              data: 'Link',
                              render: function (data, type, row) {
                                  return '<a href='+ data +' target=_blank>'+data+'</a>';
                              }
                          },
                    { data: 'Status' }
                   
            ]                          
                
            });


            //$("#Survey-Grid").jqGrid({
            //    url: '../Home/GetSurveyLinksInfo',
            //    datatype: "json",
            //    mtype: 'GET',
            //    autowidth: true,
            //    height: 200,
            //    colNames: ["Name", "Email", "Survey Link", "Status"],
            //    colModel: [                   
            //         {
            //          
            //             name: 'Name',
            //             index: 'Name',
            //             sorttype: "string",                         
            //             search: true,                         
            //             stype: 'text',                        

            //         },
            //        {
            //            name: "Email", index: "Email", search: true,
            //            stype: 'text'
            //        },
            //        {
            //            name: "Link", index: "Link", search: true,                        
            //            stype: 'text',
            //            formatter: function (cellvalue,options,obj) {
            //                return '<a href="javascript:void(0)">' + cellvalue + ' </a>';
            //            }, width: 200
            //        },
            //        {
            //            name: "Status",index: "Status", search: true,                        
            //            stype: 'text',width:40
            //        },                             
            //    ],
            //    scroll: true,
            //    loadonce: true,
            //    viewrecords: true,
            //    loadComplete: function (data) {
            //        alert("Complete ");
            //    },

            ////rowNum: 10,
            //////rowList: [10, 20, 30],
            //////sortname: "invid",
            //////sortorder: "desc",            
            ////gridview: true,
            ////autoencode: true,                  

            //});

           // $('#Survey-Grid').jqGrid('filterToolbar');
        }
    }
})();