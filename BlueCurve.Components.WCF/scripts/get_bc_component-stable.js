// BlueCurve - gets data for a BC component from the database, returned from a web service - 25/09/14.
// *** CALL THE WCF SERVICE TO GENERATE THE DOCUMENT AND THEN BUILD ***
// Last edited 08/01/2015 10:51
function getComponentData(req, appendTarget, params) {
    // ************ GLOBAL VARS *************** //
    // Get document parameters from URL query of current page. i.e. "?id=123456"
    function getURLParams() {
        var vars = [], hash;
        var q = document.URL.split('?')[1];
        if (q != undefined) {
            q = q.split('&');
            for (var i = 0; i < q.length; i++) {
                hash = q[i].split('=');
                vars.push(hash[1]);
                vars[hash[0]] = hash[1];
            }
        }
        // Split URL parameters into variables.
        var doc_id = vars["id"];
        var entity = vars["e"];
        var component = vars["c"];
        var debug = vars["debug"];
        return { doc_id: doc_id, entity: entity, component: component, debug: debug };
    }
    var urlParams = getURLParams();
    var docid = urlParams.doc_id;
    var ent = urlParams.entity;
    var comp = urlParams.component;
    var debug = urlParams.debug;

    // Domain.
    var domain = "http://autonomous.bluecurve.net/"
    // Create jQuery selector to append the object to.
    var highchartTarget = appendTarget;
    var appendTarget = '#' + appendTarget;
    // Specifies the component id.
    components = { keyValTable: 121, pricePerformChart: 122, featurePortfolioChart: 123, featurePortfolioTable: 124 }
    // Specifies which table row the data starts after.
    chartType = { pricePerform: 3, featurePortfolio: 0 }
    // Convert getMonth() value to Month.
    var months = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
    var ratingType = ["", "Underperform", "Neutral", "Outperform"];
    var plotBandColors = { "": "#EEEEEE", "Underperform": "#F7E2E3", "Neutral": "#F8F8F8", "Outperform": "#D8E8D9" }
    
    // Path to where image files are stored.
    var docIDVal = params[0];
    var pathToImage = domain + "components/" + docIDVal;

    var screenRes = window.screen.availWidth;
    var screenH = window.innerHeight; 

    // HighCharts options.
    var options = {
        credits:  {
            enabled: false
        },
        chart: {
            renderTo: highchartTarget,//'chart-div',//
            type: "line",
            spacingRight: 15,
            events: {
                load: function () {
                    try
                    {
                        svglistener = "svg_loaded";
			console.log("svglistener: " + svglistener);
                    }
                    catch(e)
                    {
                        // Do nothing
                    }
                }
            }
        },
        scrollbar: {
            enabled: false
        },
        rangeSelector: {
            enabled: false,
        },
        title: {
            text: [],
            align: "left",
        },
        legend: {
            borderWidth: 0
        },
        xAxis: {
            categories: [],
            tickColor: '#FFFFFF',
            labels: {
                staggerLines: 1,
                style : {
                    fontSize: '10px'
                }               
            }
        },
        yAxis: {
            title: {
                text: ""
            }            
        },
        plotOptions: {
            series: {
                marker: {
                    radius: 0
                }
            }
        },
        series: []
    };

    // *************************************** //

    // Print loading message to the screen.
    $(appendTarget).prepend('<p id="loadText">Loading...</p>');

    //generic part
    function serviceProxy(serviceUrl) {
        var _I = this;
        this.serviceUrl = serviceUrl;

        // *** Call a wrapped object
        this.invoke = function (method, data, callback, error, bare) {
            // *** Convert input data into JSON - REQUIRES Json2.js

            var json = JSON.stringify(data);

            // *** The service endpoint URL        
            var url = _I.serviceUrl + method;

            jQuery.support.cors = true;
            var ajaxF = $.ajax({
                url: url,
                data: json,
                type: "POST",
                crossDomain: true,
                processData: false,
                contentType: "application/json",
                timeout: 10000,
                dataType: "text",  // not "json" we'll parse
                tryCount: 0,
                retryLimit: 3,
                success: function (res) {
                    
                    if (!callback) return;

                    // *** Use json library so we can fix up MS AJAX dates

                    try{
                        result = JSON.parse(res);
                        result = JSON.parse(result.d);
                    } catch (error) {
                        this.error(ajaxF);
                        return;
                    }
                    if (urlParams.debug == "on") {
                        console.log(JSON.stringify(result))
                    }
                    
                    // *** Bare message IS result
                    if (bare)
                    { callback(result); return; }

                    // *** Wrapped message contains top level object node
                    // *** strip it off

                    for (var property in result) {

                        callback(result[property]);
                        break;
                    }
                    // Log success in the console.
                    var d = new Date();
                    console.log(d + ": BC Service loaded data successfully");
                    console.log("COMPONENT: [" + req + "] | HTMLELEMENT: [" + appendTarget + "] | PARAMETERS: [" + params + "]\n\n");
                },
                error: function (xhr, error, errorThrown) {
                    var d = new Date();
                    console.log(d + ": BC Service failed when loading data.");
                    console.log("ERROR DETAILS: Error calling web service... STATUS CODE: " + xhr.status + " | RESPONSE: " + errorThrown);
                    console.log("COMPONENT: [" + req + "] | HTMLELEMENT: [" + appendTarget + "] | PARAMETERS: [" + params + "]\n\n");
                    if (errorThrown == "timeout" || errorThrown == undefined) {
                        var ajax = this
                        ajax.tryCount++;
                        if (ajax.tryCount <= ajax.retryLimit) {
                            console.log("RETRY ATTEMPT(" + ajax.tryCount + ") Trying again...\n\n");
                            setTimeout(function () { $.ajax(ajax); }, 4000); // Wait 4 secs before retrying.
                            return;
                        } else {
                            console.log("*** COULD NOT LOAD [" + req + "] DATA AFTER " + (ajax.tryCount - 1) + " RETRIES. ***\n\n")
                            // Loading has failed... Remove loading text.
                            if ($(appendTarget + ' p#loadText').length) {
                                $(appendTarget + ' p#loadText').remove();
                            }
                            return false;
                        }
                    }

                    // Add error message to the screen.
                    /* REMOVED BY REQUEST */
                    //$(appendTarget).append("An error occurred when retrieving data. Please check that the parameter values are set correctly.");
                    
                    if (!error) return;
                    if (xhr.responseText) {
                        var err = JSON.parse(xhr.responseText);
                        if (err)
                            console.log(err);
                        else
                            console.log(error({ Message: "Unknown server error." }))
                    }
                    return;
                }
            });
        }
    }

    var Proxy = new serviceProxy(domain + "bc_core_components_svc/Ajax.svc/");

    // Request Key Valuation Data table...
    if (req in components) {
        try {
            // If params is greater than 1 (there are 2 additional params other than entity/portfolio ID)...
            var pObj = new Object;
            if (params.length > 1) {
                var additionalParams = [];
                
                for (var i = 1; i < params.length; i++) {
                    var p = new Object;
                    p.system_defeind = "false";
                    p.value = params[i];
                    additionalParams.push(p);
                }
                pObj = additionalParams;
            }

            var entity_id = params[0];
            var component_type_id = components[req];
            Proxy.invoke("AjaxGetJsonForBCComponent", {
                component: {
                    "type": component_type_id,
                    "entity_id": entity_id,
                    "parameters": pObj
                }
            }, success_func, error_func);
        }
        catch (err) {
            //alert("An error occurred: " + err.message);
        }
    }
    if (req == "whyRead") {
        try {
            Proxy.invoke("AjaxGetJsonBCDocumentComposition", { doc_id: params[0], save_to_file: false }, success_func, error_func);
        }
        catch (err) {
            //alert("An error occurred: " + err.message);
        }
    }
    if (req == "selectedCharts") {
        try {
            Proxy.invoke("AjaxGetJsonBCDocumentComposition", { doc_id: params[0], save_to_file: false }, success_func, error_func);
        }
        catch (err) {
            //alert("An error occurred: " + err.message);
        }
    }


    
    // ********** HANDLE SUCCESS ********** //
    function success_func() {

        var jsonobj = result;

        // *** Remove loading text from the appendTarget ***.
        if ($(appendTarget + ' p#loadText').length) {
            $(appendTarget + ' p#loadText').remove();
        }

        // **** Key Valuation Data Table **** //
        if (req == "keyValTable") {
            tableTemplate(jsonobj, appendTarget, { mergeCells: true });
        }

        // **** Price Performance Chart **** //
        if (req == "pricePerformChart" && params != undefined) {
            // Create data table containing series data.
            tableTemplate(jsonobj, appendTarget, { mergeCells: false });
            var dataRows = $(appendTarget + ' table').find('tr:eq(2) td:eq(1)').html();
            if (dataRows != null && Number(dataRows) > 0) { // If there is data for the selected entity
                options.chart.height = 262;
                options.chart.marginRight = 10;
                options.tooltip = { valueDecimals: 2 };
                // Set Y-Axis text.
                var YaxisText = $(appendTarget + ' table').find('tr:eq(0) td:last-child').html();
                options.yAxis.title.text = YaxisText;
                options.yAxis.title.margin = 3;
                options.yAxis.labels = { x: -5 };
                options.yAxis.floor = 0;
                // Create X-Axis data.
                options.xAxis.labels.step = 190;
                var X = getSeriesData("pricePerform", appendTarget, 1);
                for (var i in X) {
                    options.xAxis.categories.push(formatDate(X[i]));
                }
                // Create series data.
                var S1 = getSeriesData("pricePerform", appendTarget, 2);
                options.series[0] = { data: [] };
                options.series[0].name = "Closing Price";
                options.series[0].color = "#4572A7";
                options.series[0].lineWidth = 2;
                for (var i in S1) {
                    options.series[0].data.push(Number(S1[i]));
                }
                var S2 = getSeriesData("pricePerform", appendTarget, 3);
                options.series[1] = { data: [] };
                options.series[1].name = "Target Price";
                options.series[1].color = "#AA4643";
                options.series[1].lineWidth = 2;
                for (var i in S2) {
                    options.series[1].data.push(Number(S2[i]));
                }
                var S3 = getRatingData("pricePerform", appendTarget, 4);
                var prevRating = 0;
                options.series[2] = { data: [] };
                options.series[2].name = "Rating Change";
                options.series[2].color = "#8AFF59";
                options.series[2].lineWidth = 0;
                options.series[2].enableMouseTracking = false;
                // Select the correct rating symbol based on rating.
                function ratingSymbol(prevRatingVal, currRatingVal) {
                    var symbol;
                    var radius = 7;
                    if (prevRatingVal > currRatingVal) { symbol = "triangle-down"; }
                    if (prevRatingVal < currRatingVal) { symbol = "triangle"; }
                    if (prevRatingVal == currRatingVal || currRatingVal == 0) { radius = 0; }
                    return { s: symbol, r: radius };
                }
                for (var i in S3) {
                    //if (i > 0) { // Dont show the first arrow as there is no change.
                        var sym = ratingSymbol(prevRating, Number($.inArray(S3[i][2], ratingType)));
                        options.series[2].data.push({ x: Number(S3[i][0]), y: 0, marker: { symbol: sym.s, radius: sym.r } });
                        prevRating = Number($.inArray(S3[i][2], ratingType)); // Set the current value as the prev rating after using it.
                    //}
                    
                }
                // Create the plotBands for the rating data.
                var R = getRatingData("pricePerform", appendTarget, 4);
                options.xAxis.plotBands = [];
                for (var i in R) {
                    var plotB = { color: plotBandColors[R[i][2]], from: Number(R[i][0]), to: Number(R[i][1]) };
                    plotB.label = { text: R[i][2], rotation: 270, align: 'left', textAlign: 'right', x: 10, y: 5, verticalAlign: 'top' }
                    options.xAxis.plotBands.push(plotB);
                }
                // Make chart.
                var Hchart = new Highcharts.Chart(options);
            } else {
                $(appendTarget + ' table').remove();
                $(appendTarget).append("An error occurred when retrieving data. Please check that the parameter values are set correctly.");
            }
        }

        // **** Why Read **** //
        if (req == "whyRead") {
            var dataExists = 0;
            var whyReadArgs = { page_number: 1 };
            for (var i in jsonobj.lead_paragraphs) {
                if (jsonobj.lead_paragraphs[i].style == "BC_WhyReadTitle" || jsonobj.lead_paragraphs[i].style == "WhyRead Head") {
                    paraTemplate(jsonobj.lead_paragraphs[i], appendTarget, whyReadArgs);
                    dataExists += 1;
                    break;
                }
            }
            // Handle bullet points.
            if (dataExists > 0) {
                if ($(appendTarget + ' p.BC_Bullet_Level1').length > 0 || $(appendTarget + ' p.BC_Bullet_Level2').length > 0) {
                    // Convert <p> to <li>
                    $(appendTarget + ' p.BC_Bullet_Level1').replaceWith(function () { return "<li class='BC_Bullet_Level1'>" + this.innerHTML + "</li>"; })
                    $(appendTarget + ' p.BC_Bullet_Level2').replaceWith(function () { return "<li class='BC_Bullet_Level1'>" + this.innerHTML + "</li>"; })
                    // Group list items from first... add <ul> to HTML
                    $(appendTarget + ' li.BC_Bullet_Level1').filter(function () {
                        return !$(this).prev().hasClass('BC_Bullet_Level1')
                    }).before('<ul class="BC_Bullet">');
                    // Append all <li> items after each <ul>
                    $.each($('ul.BC_Bullet'), function (index, elem) {
                        while ($(elem).next().is('li')) {
                            $(elem).next('li').appendTo($(elem))
                        }
                    });
                }
            }
            // If there is no Why Read section...
            if (dataExists < 1) { $(appendTarget).append("<p>Please click on the document icon for the content of this document.</p>"); }
        }

        // **** Selected Charts **** //
        if (req == "selectedCharts") {
            $(appendTarget).append('<p id="selectedChartsTitle">Key Charts (click to enlarge)</p>');
            for (var i in jsonobj.lead_paragraphs) {
                getSelectedCharts(jsonobj.lead_paragraphs[i], appendTarget);
            }
            $(appendTarget + ' table').next('br').remove(); // Remove spaces between tables/charts.

            var totalobj = $(appendTarget + ' table').length; // Amount of selected tables/charts.

            // Delete duplicate tables/charts and comments...
            var dataOrdArr = []
            // First get the data order of all tables and push to array.
            for (var t = 0; t < totalobj; t++) {
                dataOrdArr.push($(appendTarget + ' table:eq(' + t + ')').attr('data-order'));
            }
            // Sort the array so any duplicates will be next to each other
            dataOrdArr.sort();
            // Check whether the dataOrder value is the same as the previous dataOrder value. If it is then delete tables (not the first occurence) and comment with this dataOrder value
            for (var t = 0; t < dataOrdArr.length; t++) {
                if (dataOrdArr[t + 1] == dataOrdArr[t]) {
                    try { $(appendTarget + ' p[data-order="' + dataOrdArr[t] + '"]:not(:eq(0))').remove(); }
                    catch (err) { }
                    $(appendTarget + ' table[data-order="' + dataOrdArr[t] + '"]:not(:eq(0))').remove();
                }
            }
            //
            // Append the tables and comments to the target in the correct order.
            for (var i = 0; i < totalobj; i++) {
                var ord = i + 1
                try{ $(appendTarget + ' p[data-order="' + ord + '"]').appendTo(appendTarget); }
                catch (err) { }
                $(appendTarget + ' table[data-order="' + ord + '"]').appendTo(appendTarget).after('<br/><hr><br /><br/>');
            }
            if (totalobj == 0) { $('p#selectedChartsTitle').remove(); console.log("BC Info: No chart/table data to display"); }

            // Hide the tables and replace with a thumbnail image.
            //$(appendTarget + ' table:not(table[class="TF_Title_Chart"])').before('<img src="' + domain + 'html/image/table_thumb.png" class="tableThumb" />').hide();

            // Hide the 'Source' for charts in the thumbnail view.
            // Hide the Chart title if there is a comment
            $.each($(appendTarget + ' table.TF_Title_Chart'),function(index, elem){
                if (elem.rows.length > 2) {
                    $(elem).find('td:last').hide();
                }
                var ord = $(elem).attr('data-order');
                if ($(appendTarget + ' p[data-order="' + ord + '"]').length > 0) { // If textDesc exists for a chart
                    $(appendTarget + ' table[data-order="' + ord + '"]').find('td:first').hide();
                }
            });
            // Hide the Table title if there is a comment
            $.each($(appendTarget + ' table.TF_Title_Table'), function (index, elem) {
                var ord = $(elem).attr('data-order');
                if ($(appendTarget + ' p[data-order="' + ord + '"]').length > 0) { // If textDesc exists for a table
                    $(appendTarget + ' table[data-order="' + ord + '"]').find('td:first').hide();
                }
            });
            // Make tables open in modal window when clicked.
            $(appendTarget + ' table:not(table[class="Aut_Sub_Heading"])').css('cursor', 'pointer').click(handleDataItemClick);
            $(appendTarget + ' img.tableThumb').css('cursor', 'pointer').click(handleDataItemClick);
        }

        // **** Feature Portfolio Chart **** //
        if (req == "featurePortfolioChart" && params != undefined) {
            // Create data table containing series data.
            tableTemplate(jsonobj, appendTarget, { mergeCells: false });
            var dataRows = $(appendTarget + ' table').find('tr:eq(1) td:eq(0)').html();
            if (dataRows != null && dataRows != "") { // If there is data for the selected entity
                options.chart.height = 245;
                options.chart.width = 599;
                options.legend = { enabled: false };
                options.tooltip = { valueDecimals: 2, valueSuffix: '%' };
                // Create X-Axis data.
                options.xAxis.type = 'datetime';
                var Xseries = [];
                var X = getSeriesData("featurePortfolio", appendTarget, 1);
                for (var i in X) {
                    var d = Date.parse(convertDate(X[i]));
                    Xseries.push(d);
                }

                // Create series data.
                var S1 = getSeriesData("featurePortfolio", appendTarget, 2);
                options.series[0] = { data: [] };
                options.series[0].name = "Performance";
                options.series[0].color = "#4572A7";
                options.series[0].lineWidth = 2;
                for (var i in S1) {
                    options.series[0].data.push([Xseries[i], convertPercent(Number(S1[i]))])
                }
                var source = $(appendTarget + ' table').find('tr:eq(0) td:eq(1)').html();
                // Make chart.
                var Hchart = new Highcharts.StockChart(options);
                $(appendTarget).append('<span>Source: ' + source + '</span>');
                
            } else {
                $(appendTarget + ' table').remove();
                $(appendTarget).append("An error occurred when retrieving data. Please check that the parameter values are set correctly.");
                
            }
        }

        // **** Feature Portfolio Table **** //
        if (req == "featurePortfolioTable") {
            tableTemplate(jsonobj, appendTarget, { mergeCells: true, style: "featurePortfolioTable" });
            // If the entered parameter values return no data - hide the table and report an error.
            if ($(appendTarget + ' table.featurePortfolioTable').find('tr:eq(0) td:eq(1)').html() == "0"){
                $('table.featurePortfolioTable').remove();
                $(appendTarget).append("An error occurred when retrieving data. Please check that the parameter values are set correctly.");
            }
        }
    }

    function error_func(data) {
        //alert("AJAX ERROR: " + data.err);
    }

    // *********** PROCESS DATA ************* //
    //Process table object.
    function tableTemplate(obj, target, args) {
        var style;
        
        var tableObj = $('<table />');
        var tableElem = $(tableObj).appendTo(target).after('<br/>'); // Create a table.
        if (args) { $(tableElem).attr("data-order", args.display_order); $(tableElem).addClass(args.style); } // Set the display order to the Table HTMLElement.

        for (var i in obj.rows) {
            var row = obj.rows[i]; // 'row' is each row.
            var rowElem = $('<tr />').appendTo(tableElem) // Create a HTML table row element for each row.
            for (var cell in row) {
                for (var j = 0; j < row.cells.length; j++) {
                    var cellVal = row.cells[j].paragraphs[0];
                    if (cellVal != undefined) { // Paragraph is not empty.
                        // Remove spaces from style name.
                        if (cellVal.style != null) {
                            style = cellVal.style;
                            style = style.replace(/\s+/g, "_");
                        }
                        // Paragraph VALUE is NOT empty.
                        if(cellVal.value != null){ 
                            if (cellVal.image == false) { // Insert cell TEXT in first paragraph...
                                $(rowElem).append('<td>' + cellVal.value + '</td>');
                                if (row.cells[j].paragraphs.length > 1) {
                                    for (var k = 1; k < row.cells[j].paragraphs.length; k++) { // Remaining paragraphs (if any)...
                                        $(rowElem).find('td:last').append('<br />' + row.cells[j].paragraphs[k].value + '<br/>');
                                    }
                                }
                            } else if (cellVal.image == true) { // Insert image...
                                $(rowElem).append('<td><img src="' + pathToImage + "/" + cellVal.value + '"style="height:auto" /></td>');
                            }
                        }
                        // Paragraph VALUE IS empty.
                        else if (cellVal.value == null && j > 0 && args.mergeCells == true) {
                            var getColspan = $(rowElem).find('td:last').attr('colspan');
                            var colspan = (getColspan == undefined) ? 2 : Number(getColspan) + 1;
                            $(rowElem).find('td:last').attr('colspan', colspan);
                        }
                        else if (cellVal.value == null) {
                            $(rowElem).append('<td></td>');
                        }
                    } 
                    // If paragraph is undefined and not first cell merge with previous cell...
                    else if (cellVal == undefined && j > 0 && args.mergeCells == true) {
                        var getColspan = $(rowElem).find('td:last').attr('colspan');
                        var colspan = (getColspan == undefined) ? 2 : Number(getColspan) + 1;
                        $(rowElem).find('td:last').attr('colspan', colspan);
                    } else if (cellVal == undefined) {
                        $(rowElem).append('<td></td>');
                    }
                }
            }
        }
    }
    // Process image object.
    function imageTemplate(obj, target, args) {
        var imgObj = $('<img src="' + pathToImage + obj.text + '" class="' + obj.style + '" />');
        var imgElem = $(imgObj).appendTo(target); // Create an image.
    }
    // Process paragraph object.
    function paraTemplate(obj, target, args) {
        //var style = subPara[i].style
        var paraObj = $('<p class="' + obj.style + '">' + obj.text + '</p>');
        var paraElem = $(paraObj).appendTo(target);
        

        if (obj.paragraphs != undefined && obj.paragraphs.length > 0 && args) {
            // Get key and value from args
            var arg = Object.keys(args)
            var subPara = obj.paragraphs;
            for (var i in subPara) {
                subPara[i].style
                var target = $(target);
                if (subPara[i].is_table && subPara[i][Object.keys(args)] == args[arg]) { tableTemplate(subPara[i].table, target, { style: subPara[i].style }); }
                else if (subPara[i].is_image) { imageTemplate(subPara[i], target); }
                else if (subPara[i][Object.keys(args)] == args[arg]) { paraTemplate(subPara[i], target); }
            }
        }
    }

    function getSeriesData(cType, target, series) {
        $(target + ' table').hide(); // Hide the data table.
        var valArr = []; // Create an array to contain the data for each series.
        var row = $(target + ' table tr');
        $.each(row, function (index, elem) {
            if (elem.rowIndex > chartType[cType]) {
                valArr.push($(elem).find('td:nth-child(' + series + ')').html()); // Get values from each for for specified column (series) in data table.
            }
        });
        return valArr;
    }

    function getRatingData(cType, target, series) {
        var ratingArrGroup = []; // An array to contain the performance rating data (start, end, rating).
        var ratingArr = []; // An array to contain the groups of performance data rating once sliced.
        var counter = 0;
        var row = $(target + ' table tr');
        var rGroup = ["", "Underperform", "Neutral", "Outperform"]
        $.each(row, function (index, elem) {
            if (elem.rowIndex > chartType[cType]) {
                var rowVal = $(elem).find('td:nth-child(' + series + ')').html();
                var prevRowVal = $(elem).prev('tr').find('td:nth-child(' + series + ')').html();
                if (rowVal != prevRowVal && elem.rowIndex != chartType[cType] + 1) {  
                    var from, to, rating;
                    if (index == 0) { from = 0; }
                    to = index - 1; // End plotbands at previous loop index.
                    if (prevRowVal != "") { rating = rGroup[Number(prevRowVal) + 1] } else { rating = rGroup[0] } // Type of rating
                    from = index; // Start new plotbands at loop index.
                    ratingArrGroup.push(to, rating, from);
                }
            }
            if (elem.rowIndex == row.length - 1) {
                var finalRating;
                // End plotbands at last index in loop.
                ratingArrGroup.unshift(0);
                ratingArrGroup.push(index);
                if (rowVal != "") { finalRating = Number(rowVal) + 1 } else { finalRating = 0}
                ratingArrGroup.push(rGroup[finalRating]); 
            }
        }); // End $.each().
        // Slice the groups of arrays to get start, end, and rating values.
        var arrSlice = 3;
        while(ratingArrGroup.length) {
            var slice = ratingArrGroup.splice(0, arrSlice);
            ratingArr[counter] = slice;
            counter += 1;
        }
        return ratingArr;
    }

    function getSelectedCharts(obj, target) {
        var style = obj.style;
        style = style.replace(/\s+/g, "_");
        if (obj.is_table && style == "TF_Title_Chart" || style == "TF_Title_Table" || style == "BC_TableValuation_Heading" || style == "BC_TableKeyData_Heading_Left") {
            if (obj.display_order > 0) {
                tableTemplate(obj.table, target, { display_order: obj.display_order, style: style, mergeCells: true });
                if (obj.desc != null) {
                    $(target).append('<p class="textDesc" data-order="' + obj.display_order + '">' + obj.desc + '</p>');
                }
            }
        }
        if (obj.paragraphs.length > 0) {
            for (var i in obj.paragraphs) {
                var subPara = obj.paragraphs[i];
                if (subPara.display_order > 0) {
                    var style = subPara.style;
                    style = style.replace(/\s+/g, "_");
                    if (subPara.is_table && style == "TF_Title_Chart" || style == "TF_Title_Table" || style == "BC_TableValuation_Heading" || style == "BC_TableKeyData_Heading_Left") {
                        tableTemplate(subPara.table, target, { display_order: subPara.display_order, style: style, mergeCells: true });
                        if (subPara.desc != "") {
                            $(target).append('<p class="textDesc" data-order="' + subPara.display_order + '">' + subPara.desc + '</p>');
                        }
                    }
                }
            }
        }
    }

    function convertDate(date) {
        date = date.replace(/am|pm/gi, "");
        date = date.replace(/-/g, "/");
        var uDate = new Date(date);
        var cDate = uDate.toString();
        return cDate;
    }

    function formatDate(date) {
        date = date.replace(/am|pm/gi, "");
        date = date.replace(/-/g, "/");
        var uDate = new Date(date);
        var d = new String(uDate.getUTCDate());
        var m = months[uDate.getMonth()];
        var y = new String(uDate.getFullYear());
        y = "'" + y.substring(2);
        var fDate = [d, m, y].join(" ");
        fDate = fDate.toString();
        return fDate;
    }

    function convertPercent(value) {
        value = value * 100;
        var str = value.toString();
        if (str.indexOf(".") > -1) {
            x = str.substr(0, (str.indexOf(".") + 2));
            value = Number(x)
        }
        return value;
    }

    // ********************  -[MODAL WINDOW]- ******************** //

    var modalWindow = {
        parent: "body",
        windowId: null,
        content: null,
        width: null,
        height: null,
        closeModal: function () {
            $('#modal-iframe').attr('src', '');
            $('.modal-window').remove();
            $(".modal-overlay").remove();
            $('#selectedObj').removeAttr('id');
        },
        open: function () {
            var modal = "";
            modal += "<div class='modal-overlay'></div>";
            modal += "<div id='" + this.windowId + "' class='modal-window' style='width:" + 0 + "; height:" + 0 + "px; margin-top:-250px;'>";
            modal += this.content;
            modal += "</div>";

            $(this.parent).append(modal);

            // Animate resize of div.
            $('.modal-window').animate({
                'height': this.height, 'width': this.width, 'margin-top': (-(this.height / 2)), 'margin-left': (-(this.width / 2))
            },
            {
                complete: function () {
                    openModalWindow.resolve();
                }
            });

            $(".modal-overlay").on('click', function () { modalWindow.closeModal(); });
        }
    };

    var openMyModal = function (width, height) {
        modalWindow.windowId = "myModal";
        modalWindow.width = width;
        modalWindow.height = height;
        modalWindow.content = "<iframe id='modalFrame' width='100%' height='100%' frameborder='0' scrolling='no' allowtransparency='true' src='modalframe.html'></iframe>";
        modalWindow.open();
    };


    function handleDataItemClick() {
        openModalWindow = $.Deferred();
        var rowCount = 0;
        var jRowElem;
        var selectedObj;
        var verifyFrameTargetCounter = 0
        var verifyTimeOut = 200;

        if ($(this).hasClass("tableThumb")) {
            selectedObj = $(this).next('table');
        } else { selectedObj = $(this); }

        $(selectedObj).attr('id', 'selectedObj');
        rowCount = $('#selectedObj tr').length;     

        openModalWindow.done(function () {
            //console.log("Modal window open.");
            
            // Loop until the modalcontent div exists in the DOM...
            var verifyFrameTarget = setInterval(function () {
                // Check that the frametarget element exists in the DOM.
                if ($('#modalFrame').contents().find('#modalcontent').length && verifyFrameTargetCounter < verifyTimeOut) {
                    var frametarget = $('#modalFrame').contents().find('#modalcontent');
                    
                    function resizeModal() {
                        // Set the width of the modal iFrame.
                        // If window width is less than 1024 set modal frame to 92% of window width.
                        var resetModalW = (window.innerWidth <= 1024) ? (92 / 100) * window.innerWidth : (70 / 100) * window.innerWidth;
                        $('#myModal').css({ 'width': resetModalW, 'margin-left': -(resetModalW / 2) });

                        var maxModalH =  (92 / 100) * screenH;
                        var objImg = $(appendTarget + ' #selectedObj img')
                        var frametargetH = Math.floor($(frametarget).height());
                        var frametargetW = Math.floor($(frametarget).width());
                        var nativeImg = new Image();
                        nativeImg.src = objImg.attr('src');
                        var nativeImgH = Math.floor(nativeImg.height);
                        var nativeImgW = Math.floor(nativeImg.width);
                        var imgOrientation;
                        var modalContentArea;

                        if (nativeImgW >= nativeImgH) {
                            imgOrientation = "L";
                        } else { imgOrientation = "P"; }

                        // Return height as percentage in proportion to width
                        function getAspectRatio(niH, niW) {
                            var aRatio = (niH / niW) * 100
                            return Math.floor(aRatio);
                        }
                        // Set img width to auto when less that available space otherwise shrink to fit
                        if (nativeImgW < frametargetW) {
                            $('#modalFrame').contents().find('#selectedObj img').css('width', 'auto');
                        }
                        var ar = getAspectRatio(nativeImgH, nativeImgW)
                        var modalImgH = (ar / 100) * $('#modalFrame').contents().find('#selectedObj img').width();
                        var modalContentArea = Math.floor($(frametarget).height()) + 80; // Add 80 to account for banner and padding

                        if (modalContentArea > maxModalH) {
                            var diff = modalContentArea - maxModalH;
                            var newImgH = $('#modalFrame').contents().find('#selectedObj img').height() - diff;
                            $('#modalFrame').contents().find('#selectedObj img').css({ 'width': 'auto', 'height': newImgH });
                            modalContentArea = maxModalH;
                        }

                        // Set the modalframe size.
                        $('#myModal').css({
                            'height': modalContentArea, 'margin-top': -(modalContentArea / 2)
                        });
                        // Show the image
                        $(frametarget).find('#selectedObj img').css('visibility', 'visible');
                    }

                    $(window).resize(function () {
                        screenRes = window.screen.availWidth;
                        screenH = window.innerHeight;
                        resizeModal();
                    })

                    // Handle the action when the navigation button is clicked.
                    function handleNavClick() {
                        var obj;
                        //var objH;
                        if ($('#selectedObj').hasClass('TF_Title_Table')) { obj = "TF_Title_Table"; }
                        if ($('#selectedObj').hasClass('TF_Title_Chart')) { obj = "TF_Title_Chart"; }
                        if ($('#selectedObj').hasClass('BC_TableKeyData_Heading_Left')) { obj = "BC_TableKeyData_Heading_Left"; }
                        showNextObj(obj, this.id);
                    }
                    // Cycle through objects of the same type in the modal window.
                    function showNextObj(objClass, getObj) {
                        var currObjIndex = $(appendTarget + ' Table').index($(appendTarget + ' #selectedObj')); // Set the index of the currently selected object.
                        getObj == "prev" ? getObj = currObjIndex - 1 : getObj = currObjIndex + 1; // Set the index of the next object to get
                        if (getObj == $(appendTarget + ' Table').length) { getObj = 0; } // Handle cycle through last element
                        if (getObj < 0) { getObj = $(appendTarget + ' Table').length - 1; }
                        $(frametarget).empty() // Remove the current object.
                        $(appendTarget + ' #selectedObj').removeAttr('id'); // Clear the selectedObj id
                        $(appendTarget + ' Table').filter(':eq(' + getObj + ')').attr('id', 'selectedObj').clone().appendTo(frametarget); // Add the next object to the modal window.
                        var selectedObjOrder = $('#selectedObj').attr('data-order');
                        // Add additional text to modal window.
                        try { $('p[data-order="' + selectedObjOrder + '"]').clone().prependTo($('#modalFrame').contents().find('#selectedObj tr:eq(0) td:eq(0)')); }
                        catch (err) { /* do nothing */ }
                        rowCount = $('#selectedObj tr').length; // Reset rowCount.
                        applyModalStyle(); // Apply the style to the object.

                        $(frametarget).find('#selectedObj').show(); // Show table content
                        $(frametarget).find('#selectedObj td').show() // Show table cells
                        // Resize the modal window once the image has loaded.
                        $(frametarget).find('#selectedObj img').load(function () {
                            resizeModal();
                        }).css('visibility','hidden');
                    }

                    function applyModalStyle() {
                        $(frametarget).find('#selectedObj').css({
                            'border-collapse': 'collapse'
                        });
                        // Apply the styles to the cloned object.
                        for (var i = 0; i < rowCount; i++) {
                            jRowElem = document.getElementById("selectedObj").rows[i]
                            var objStyle = window.getComputedStyle(jRowElem, null);
                            try {
                                $(frametarget).find('#selectedObj tr:eq(' + i + ')').css({
                                    'color': objStyle.getPropertyValue("color"),
                                    'font-size': objStyle.getPropertyValue("font-size"),
                                    'font-family': objStyle.getPropertyValue("font-family"),
                                    'font-weight': objStyle.getPropertyValue("font-weight"),
                                    'background-color': objStyle.getPropertyValue("background-color"),
                                    'border': objStyle.getPropertyValue("border"),
                                    'border-width': objStyle.getPropertyValue("border-width"),
                                    'border-size': objStyle.getPropertyValue("border-size"),
                                    'border-color': objStyle.getPropertyValue("border-color")//,
                                    //'border-top': objStyle.getPropertyValue("border") // For Firefox.
                                });
                            }
                            catch (error) {
                                console.log(error);
                            }
                        }
                    }
                    


                    // Clear elements to prevent doubling them when chart/table is double-clicked.
                    $(frametarget).html(""); // Clear the modalcontent
                    $('#modalFrame').contents().find('td.navLeft').html(""); // Clear the navLeft button cell
                    $('#modalFrame').contents().find('td.navRight').html(""); // Clear the navRight button cell

                    // Close the modal window when the 'X' button is clicked. Must wait until modal window is open before selector can find this element.
                    $('#modalFrame').contents().find("img.closebtn")
                        .css('cursor', 'pointer')
                        .on('click', function () { modalWindow.closeModal(); });

                    var selectedObjOrder = $(selectedObj).attr('data-order');
                    // Add the next object to the modal window.
                    $(selectedObj).clone().appendTo(frametarget);
                    // Add additional text to modal window.
                    try { $('p[data-order="' + selectedObjOrder + '"]').clone().prependTo($('#modalFrame').contents().find('#selectedObj tr:eq(0) td:eq(0)')); } 
                    catch (err) { /* do nothing */ }                 

                    // Add functionality to the navigation buttons.
                    var navLeft = $('#modalFrame').contents().find('.navLeft');
                    var navRight = $('#modalFrame').contents().find('.navRight');
                    $('<img src="' + domain + 'html/image/arrow-left.png" id="prev" />')
                        .css('cursor', 'pointer')
                        .appendTo(navLeft)
                        .click(handleNavClick);
                    $('<img src="' + domain + 'html/image/arrow-right.png" id="next" />')
                        .css('cursor', 'pointer')
                        .appendTo(navRight)
                        .click(handleNavClick);

                    // Show content that was hidden in the thumbnail view.
                    $(frametarget).find('#selectedObj').show(); // Show table content
                    $(frametarget).find('#selectedObj td').show() // Show table cells
                    $(frametarget).find('#selectedObj img').css('visibility', 'hidden'); // hide the image when loading.
                    $(frametarget).find('#selectedObj img').load(function () {
                        resizeModal();
                    })
                    
                    // Apply styling to the content of the modal window (read styles from the thumbnail view).
                    applyModalStyle();

                    // Exit the loop.
                    clearInterval(verifyFrameTarget);
                } else if ($('#modalFrame').contents().find('#modalcontent').length && $('#modalFrame').contents().find('#modalcontent').html().length == 0 && verifyFrameTargetCounter >= verifyTimeOut) {
                    // Show an error if the modalcontent contains no content after timeout.
                    $('#modalFrame').contents().find('#modalcontent').html('<p class="modalError">An error occurred. Please close this window and try again.</p>');
                    clearInterval(verifyFrameTarget);
                } else if ($('#modalFrame').contents().find('#modalcontent').length < 1 && verifyFrameTargetCounter >= verifyTimeOut) {
                    // Close the modal window if no modalcontent does not exist in the DOM.
                    modalWindow.closeModal();
                    clearInterval(verifyFrameTarget);
                }
                verifyFrameTargetCounter += 1;
                //console.log(verifyFrameTargetCounter)
            }, 25)            
        });

        // Open the modal window.
        var modalH = $('#selectedObj').height();
        modalH = (screenRes <= 1024) ? modalH * 1.3 : modalH + 200;
        var modalW = (screenRes <= 1024) ? (92 / 100) * window.innerWidth : (70 / 100) * window.innerWidth;
        openMyModal(modalW, modalH);
    }
}


