var url = "http://localhost/BlueCurve.Core.WCF/Ajax.svc/";
var process_url = "http://localhost/process/process.html"; 
var app = angular.module('CompanyFlashApp', []);

app.controller('CompanyFlashCtrl', function ($scope, $http, $window, $sce, $interval, $location) {
    var entity_id="";
    var pt_id = "";
    var doc_id=""
    var user_id=0
    var s = $location.$$absUrl
    var refresh_errors = ""
    var refresh_component_count = 0
    var refresh_component_total = 0
    var static_component_count = 0
    var static_component_total = 0
    var refresh_components = [];
        try
        {
            
            //user_id = s.substring(s.indexOf("user_id") + 8, s.length);
            //s = s.substring(0,  s.indexOf("user_id"))
            doc_id = s.substring(s.indexOf("doc_id") + 7, s.length);
            s = s.substring(0, s.indexOf("doc_id"))
            pt_id = s.substring(s.indexOf("pt_id") + 6, s.length);
            s = s.substring(0, s.indexOf("pt_id"))
            entity_id = s.substring(s.indexOf("entity_id") + 10, s.length);
          

            if (pt_id == "" ||  entity_id == "" || doc_id=="")
            {
                $scope.status = "Invalid Url"
            }
            else
            {
                $scope.status = "loading product.."
                load_product();
            }
        }
        catch (err)
        {
            $scope.status = "Invalid Url :" + err;
        }
   
    angular.element(document).ready(function () {

        });
    function load_product()
    {
        //ajax calls for component

        //check if real time refresh or component refresh
        refresh_component_count = 0
        if (doc_id==0)
            custom_load_real_time_components()
        else
            custom_load_static_components()
   
        custom_load_ammend_componenets()
        $scope.refresh_components = refresh_components;
    }
   
    $scope.dorefresh = function () {
        $scope.show_product = false;
        refresh_component_count = 0
        refresh_errors = ""
        custom_load_real_time_components()
    }

    $scope.doreset = function () {
       $scope.show_product = false;
       staic_component_count = 0
        refresh_errors = ""
        custom_load_static_components()
    }

    function check_refresh_status() {
        refresh_component_count = refresh_component_count + 1
        if (refresh_component_count >= refresh_component_total) {
            if (refresh_errors == "")
                $scope.status = "Dynamic Refresh Complete."
            else
                $scope.status = "Dynamic Refresh errored: " + refresh_errors
            $scope.show_product = true;
        }
        else
            $scope.status = "Dynamic Refreshing " + refresh_component_count + " of " + refresh_component_total

        
    }
    function check_static_component_status() {
        static_component_count = static_component_count + 1
        if (static_component_count >= static_component_total) {
            if (refresh_errors == "")
                $scope.status = "Static Refresh Complete."
            else
                $scope.status = "Static Refresh errored: " + refresh_errors
            $scope.show_product = true;
        }
        else
            $scope.status = "Static Refreshing " + static_component_count + " of " + static_component_total
    }

    $scope.dodiscard = function () {
        $window.location.href = process_url
    }
    $scope.dochangeparameter = function()
    {
        alert($scope.component.name);
    }

    var components = [];
    $scope.dosubmit = function () {
        $scope.show_product =false;
        $scope.status = "Submitting Document..."
        var dt = new Date();
        var d = new Date(dt);
        jdate = '\/Date(' + d.getTime() + '-0000)\/';
      
        //metadata = { mode: 1, stage_id: "1", language_id: 1, pub_type_id: pt_id, lead_entity_id: entity_id, title: "test html", sub_title: "", summary: "", extension: ".html", taxonomy: staxonomies, authors: sauthors, doc_date: jdate };
        var sauthors = []
        var staxonomies = []
        sauthors.push(localStorage.userid)
        staxonomies.push(entity_id)
      
        custom_get_write_down_components();
        if (doc_id==0)
            metadata = { taxonomy: staxonomies, authors: sauthors, mode: 8, id: 0, stage_id: "1", language_id: 1, pub_type_id: pt_id, lead_entity_id: entity_id, title: $scope.title, sub_title: "", summary: "", extension: ".html", doc_date: jdate };
        else
            metadata = { taxonomy: staxonomies, authors: sauthors, mode: 4, id: doc_id, stage_id: "1", language_id: 1, pub_type_id: pt_id, lead_entity_id: entity_id, title: $scope.title, sub_title: "", summary: "", extension: ".html", doc_date: jdate };
        docdata = { components: components, credentials: { user_name: localStorage.username, password: localStorage.password }, document_object: "dd", document_metadata: metadata };
   
        $http({
            method: 'POST',
            url: url + "AjaxDocumentUpload",
            data: docdata,
            crossDomain: true,
            //contentType: false,

        }).then(function successCallback(data) {
            try {
                if (data.data.d == "Document Upload Successful" || data.data.d == "Document Metadata Change Successful") {
                    $scope.status = "Submit successful"
                      $window.location.href = process_url
                }
                else {
                    $scope.status = "Upload Failed: " + data.data.d;
                    $scope.show_product = true;
                }

            }
            catch (err) {
                $scope.status = err
            }
        },
       function errorCallback(response) {
           $scope.status = "Upload Failed Cant Connect to Server: " + JSON.stringify(response);
           $scope.show_product = true;
       });
    }

    function ajax_get_real_time_component_value(component_id) {
        $http({
            method: 'POST',
            url: url + "AjaxGetJsonForBCComponent",
            data: { credentials: { user_name: localStorage.username, password: localStorage.password }, component: { "contributor_id": 1, "entity_id" :entity_id, "type": component_id, "user_id": user_id } },
        }).then(function successCallback(data) {
            try {
                jsonobj = JSON.parse(data.data.d);
                custom_set_real_time_component_value(component_id, jsonobj)
                check_refresh_status()
            }
            catch (err) {
                refresh_errors = refresh_errors + "Failed to load real time component " + component_id + ": " + JSON.stringify(data.data.d)
                check_refresh_status()
            }

        },
         function errorCallback(response) {
             refresh_errors = refresh_errors + "Failed to load real time component  " + component_id + ": " + JSON.stringify(response);
             check_refresh_status()
         });
    }

    function ajax_get_static_component_value(component_id) {
        $http({
            method: 'POST',
            url: url + "AjaxGetJsonBCStaticComponent",
            data: { credentials: { user_name: localStorage.username, password: localStorage.password }, doc_id: doc_id, component_id: component_id },
        }).then(function successCallback(data) {
            try {
                custom_set_static_component_value(component_id, data.data.d)
                check_static_component_status()
            }
            catch (err) {
                refresh_errors = refresh_errors + "Failed to load static component " + component_id + ": " + JSON.stringify(data.data.d)
                check_static_component_status()
            }

        },
    function errorCallback(response) {
        refresh_errors = refresh_errors + "Failed to load static component" + component_id + ": " + JSON.stringify(response);
        check_static_component_status()
    });
    }
    // PRODUCT SPECIFIC PARTS
    //document components
    function custom_load_ammend_componenets() {
        // get this definition from ajax
        var lookup_vals=[];
        lookup_vals.push({key:1,name:"hello"});
        lookup_vals.push({key:2,name:"bye"});
        var parameters = [];
        parameters.push({ name: "Start Range", lookup_vals: lookup_vals })
        var lookup_vals = [];
        lookup_vals.push({ key: 1, name: "aa" });
        lookup_vals.push({ key: 2, name: "bbb" });
        parameters.push({ name: "End Range", lookup_vals:lookup_vals })
       
        refresh_components.push({ name: "Earnings Slider", component_id: 12, parameters: parameters, allow_amend: true });

        var parameters = [];
        parameters.push({ name: "hello", lookup_vals: lookup_vals })

        refresh_components.push({ name: "Data Table", component_id: 14, parameters: parameters, allow_amend: true });
     }

    function custom_get_write_down_components()
    {
       
        var component = [];
       
        // ticker
        var table_cell;
        var table_cells = [];
        //analyst name
        table_cell = { row: 1, col: 1, title: $scope.stock_name, style: "auto_rc_table_title_left" }
        table_cells.push(table_cell)    
        component.push({ title: $scope.analyst_name, page_no: 1, btable:true, table_cells: table_cells, style: 'BC_AnalystName', text_box_id: 3 })
        
        //companyname and title
        component.push({ title:$scope.company_name, page_no: 1, style: 'auto_rc_header_company' })
        component.push({ title: $scope.title, page_no: 1, style: 'auto_rc_header_title' })
        //headline
        table_cell = { row: 1, col: 1, paragraph: 1, title: $scope.headline, style: 'Auto_rc_headline' }
        table_cells = [];
        table_cells.push(table_cell)
        component.push({ title: $scope.headline, page_no: 1, style: 'Auto_rc_headline', text_box_id: 0, btable: true, table_cells: table_cells })

        // rating TP
        // target price
        table_cells = [];
        table_cell = { row: 1, col: 4, title: $scope.target_price, style: 'auto_rc_table_title_left' }
        table_cells.push(table_cell)
        table_cell = { row: 1, col:3, title: $scope.target_price_text, style: 'auto_rc_table_title_right' }
        table_cells.push(table_cell)

        // rating
        table_cell = { row: 1, col: 1, paragraph: 1, title: $scope.rating, style: 'auto_rc_rating_' }
        table_cells.push(table_cell)
        component.push({ title: $scope.target_price, page_no: 1, style: '', text_box_id: 0, btable: true, table_cells: table_cells })

        //conclusion
        component.push({ title: $scope.conclusion, page_no: 1, style: 'auto_rc_conclusion', btable: true, })

        //rating

        //target price

        var table_cell;
        var table_cells = [];
        // data table
        var i = 1
        var j = 1
        var co = 1

        $('.aut-rc-price-table td').each(function () {
            if (co != 5) {
                if (i == 1) {
                    if (j == 1)
                        table_cell = { row: i, col: j, title: $(this).html(), style: "auto_rc_DataTable_title" }
                    else
                        table_cell = { row: i, col: j, title: $(this).html(), style: "BC_TableValuation_Heading_Right" }
                }
                if (i > 1) {
                    if (j == 1)
                        table_cell = { row: i, col: j, title: $(this).html(), style: "BC_TableValuation_DataLabel" }
                    else
                        table_cell = { row: i, col: j, title: $(this).html(), style: "BC_TableValuation_DataValue" }
                }
                table_cells.push(table_cell)
           
                j = j + 1
                if (j > 4) {
                    i = i + 1
                    j = 1
                }
            }
            co = co + 1

        });  
    

        component.push({ title: 'Table', page_no: 1, style: 'auto_rc_DataTable_title', btable: true, table_cells: table_cells, text_box_id:2 })
        
        components={component:component};

    }

    function custom_set_static_component_value(component_id , data)
    {
        //ticker
        if (component_id==200)
            $scope.stock_name=data;
       //analyst
       else if (component_id==201)
           $scope.analyst_name=data;
       //title
       else if (component_id==202)
           $scope.title=data;
      //rating
       else if (component_id==203)
           $scope.rating=data;
       //tp
       else if (component_id==204)
           $scope.target_price=data;
       //headline
       else if (component_id==205)
           $scope.headline = data
       //conclusion
       else if (component_id == 208)
           $scope.conclusion =data

    }

    function custom_load_static_components()
    {
        static_component_total =8
        static_component_count = 0
        ajax_get_static_component_value(200)
        ajax_get_static_component_value(201)
        ajax_get_static_component_value(202)
        ajax_get_static_component_value(203)
        ajax_get_static_component_value(204)
        ajax_get_static_component_value(205)
        ajax_get_static_component_value(208)
        ajax_get_static_component_value(209)
        $scope.slider_image = "m6_m4.png";
    }
   

    function custom_set_real_time_component_value(component_id , jsonobj)
    {
       
        if (component_id == 38)
            $scope.rating = jsonobj.rows[0].cells[0].paragraphs[0].value
        if (component_id==40)
            $scope.target_price = jsonobj.rows[0].cells[0].paragraphs[0].value
        if (component_id == 61)
            $scope.target_price_text = jsonobj.rows[0].cells[0].paragraphs[0].value
        else if (component_id==201)
            $scope.stock_name = jsonobj.rows[0].cells[0].paragraphs[0].value
        else if (component_id==306)
            $scope.company_name = jsonobj.rows[0].cells[0].paragraphs[0].value
        else if (component_id == 309) 
            $scope.analyst_name = jsonobj.rows[0].cells[0].paragraphs[0].value
  
    }

    //real time components
    function custom_load_real_time_components() {
        refresh_component_total = 6
        //rating
        ajax_get_real_time_component_value(38)
        //target price
        ajax_get_real_time_component_value(40)
        //target price text
        ajax_get_real_time_component_value(61)
        //ticker
        ajax_get_real_time_component_value(201)
        //company name
        ajax_get_real_time_component_value(306)
        //analyst
        ajax_get_real_time_component_value(309)
      
    }
    // END PRODUCT SPECIFIC PARTS
      
})
    

   

 