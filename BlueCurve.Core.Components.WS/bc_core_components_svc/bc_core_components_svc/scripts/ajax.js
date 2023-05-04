//<reference path="jquery-1.11.1.js" />

$(document).ready(function () {

function serviceProxy(serviceUrl) {
    var _I = this;
    this.serviceUrl = serviceUrl;
    

    // *** Call a wrapped object
    this.invoke = function (method, data, callback, error, bare) {
        // *** Convert input data into JSON - REQUIRES Json2.js
       
        //var json = JSON2.stringify(data);
        var json = JSON.stringify(data);
        
        // *** The service endpoint URL        
        var url = _I.serviceUrl + method;
      
        $.ajax({
            url: url,
            data: json,
            type: "POST",
            processData: false,
            contentType: "application/json",
            timeout: 10000,
            dataType: "text",  // not "json" we'll parse
            success:
            function (res) {
                
                if (!callback) return;
                
                // *** Use json library so we can fix up MS AJAX dates
                var result = JSON.parse(res);

                // *** Bare message IS result
                if (bare)
                { callback(result); return; }

                // *** Wrapped message contains top level object node
                // *** strip it off
                for (var property in result) {
                    callback(result[property]);
                    break;
                }
            },
            error: function (xhr,errorr) {
                alert("Error calling web service: " + xhr.status)
                
                if (!error) return;
                if (xhr.responseText) {
                    var err = JSON2.parse(xhr.responseText);
                    if (err)
                        error(err);
                    else
                        error({ Message: "Unknown server error." })
                }
                return;
            }
        });
    }
} 
    // *** Create a static instance
var Proxy = new serviceProxy("http://localhost/bc_core_components_svc/Service2.svc/");
    //var symbol = $("#txtSymbol").val();
    try{
       // Proxy.invoke("DoWork", {},
        Proxy.invoke("AjaxGetJson", { component: { "type": "68", "entity_id": "36932" } },
            function (data) {
                //var result = serviceResponse.GetStockQuoteResult;
                alert(result);
                var $rightPane_section = $('#rightPane section');
                           $rightPane_section.append("<table class='dataTable1'></table>");
                           $.each(data, function (i, d) {

                               for (i = 0; i < d.rows.length; i++) {
                                   $('.dataTable1').append("<tr class='tableRow'></tr>");
                                   for (j = 0; j < d.rows[i].cells.length; j++) {
                                       $('.dataTable1 tr:nth-child(' + i + ')').append('<td>' + d.rows[i].cells[j].value + '</td>');
                                   }
                               }
                           });

                           $('.dataTable1 td:odd').css({
                               'text-align': 'right'
                           });
            },
            function (onPageError) { });
    }
    catch (err)
    {
        alert(err.message);

    }
   

    // ********** FUNCTIONS **********

 //   function myJSON(jURL) {
 //       $.ajaxSetup({ cache: false });

 //       $.getJSON(jURL, function(data) {
 //           //var html = [];
            
 //           var $rightPane_section = $('#rightPane section');
 //           $rightPane_section.append("<table class='dataTable1'></table>");
 //           $.each(data, function (i, d) {
                
 //               for (i = 0; i < d.rows.length; i++) {
 //                   $('.dataTable1').append("<tr class='tableRow'></tr>");
 //                   for (j = 0; j < d.rows[i].cells.length; j++) {
 //                       $('.dataTable1 tr:nth-child(' + i + ')').append('<td>' + d.rows[i].cells[j].value + '</td>');
 //                   }
 //               }
 //           });

 //           $('.dataTable1 td:odd').css({
 //               'text-align': 'right'
 //           });
 //       }).error(function(jqXHR, textStatus, errorThrown){ // assign handler
 //          // alert("error occurred!"); 
 //alert(jqXHR.responseText)
 //       });
 //   }

 //   myJSON("http://localhost/jerome/scripts/json.txt");
    
    

    
    
    // ********** TEST **********
    //$('#leftPane section').load('test.php').css('text-align', 'justify');
   




}); // END jQuery()
