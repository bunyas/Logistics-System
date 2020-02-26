<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="mascis.GIS_Maps.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        html {
            height: 100%;
        }

        body {
            height: 100%;
            margin: 0;
            padding: 0;
        }

        #map_canvas {
            height: 100%;
        }
    </style>
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC6v5-2uaq_wusHDktM9ILcqIrlPtnZgEk&sensor=false">
    </script>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - MoGLSD UWEP MIS</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <%--<webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="../Content/MegaMenu.css" rel="stylesheet" />
    <link href="font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="css/animate.css" rel="stylesheet" />--%>

   <%-- <style type="text/css">
        .dropdown-submenu {
            position: relative;
        }
            .dropdown-submenu > .dropdown-menu {
                top: 0;
                left: 100%;
                margin-top: -6px;
                margin-left: -1px;
                -webkit-border-radius: 0 6px 
                -moz-b rer-adis: 0 border-radius: 0 6px 6p px;
                .dropdow -subme o dis l y: bloc;
                .d a:after lay: b ock;
                ntent: " float r width: 0 bord ent; brder bord r-width: b #cccccc; o margin-right: 1 px;
                .dropdown-s bmenu:ho;
        {
            border-left-color: #fffff;
            m float: none;
        }
        n-sub enu.pu nu {
            ma ginlft: 10px -webkit-border-rad us: 6x 0 6px -moz-borde -rais: px 0 border-radius: 6px 06px 6px;
        }
    </style>--%>
</head>
<body class="CustomBackground" onload="initialize()">
    <script type="text/javascript">
        function initialize() {
            var markers = JSON.parse('<%= ConvertDataTabletoString() %>');

            // Original map options
            var mapOptions = {
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                zoom: 8,
                mapTypeId: google.maps.MapTypeId.ROADMAP
                //marker:true  
            };

            var infoWindow = new google.maps.InfoWindow();


            var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var city = markers[i].lat
                //alert(data.description)
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title,
                    desc: data.description
                });
                (function (marker, data) {

                    // Attaching a click event to the current marker
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(
                            '<b>Name of Enterprise:</b> ' + data.title + '<br> ' + '<hr> '
                            + '<b>Component:</b> ' + data.Component + '<br> ' + '<hr> '
                            + '<b>Sector:</b> ' + data.Sector + '<br> ' + '<hr> '
                            + '<b>Enterprise Type:</b> ' + data.Enterprise_Type + '<br> ' + '<hr> '
                            + '<b>No. Of Women In Group:</b> ' + data.Enterprise_No_of_women + '<br> ' + '<hr> '
                            + '<b>Amount Requested For:</b> ' + data.Total_Enterprise_Budget + '<br> ' + '<hr> '
                            + '<b>GIS Coordinates (Longtitude):</b> ' + data.lng + '<br> ' + '<hr> '
                            + '<b>GIS Coordinates (Latitude):</b> ' + data.lat + '<br> ');
                        // alert(data.lat);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }
        }
    </script>
    <form id="form1" runat="server">
      
        <div class="wrapper wrapper-content animated fadeInRight">
            <asp:ScriptManager runat="server">
                <Scripts>
                   
<%--                    <asp:ScriptReference Name="MsAjaxBundle" />
                    <asp:ScriptReference Name="jquery" />
                    <asp:ScriptReference Name="bootstrap" />
                    <asp:ScriptReference Name="respond" />
                    <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                    <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                    <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                    <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                    <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                    <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                    <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                    <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                    <asp:ScriptReference Name="WebFormsBundle" />--%>
                   
                </Scripts>
            </asp:ScriptManager>
            <div class="container body-content">
                <div class="navbar navbar-default navbar-fixed-top">
                    <div class="navbar-header">
                        <button class="navbar-toggle" type="button" data-toggle="collapse" data-target=".js-navbar-collapse">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                        <%-- <a href="~/Ministry_Enterprise/Ministry_Home.aspx" runat="server">Home</a>--%>
                        <%--<a class="navbar-brand" runat="server" href="~/Ministry_Enterprise/Ministry_Home.aspx">UWEP</a>--%>
                    </div>

                    <div class="collapse navbar-collapse js-navbar-collapse">
                        <ul class="nav navbar-nav">
                        </ul>
                       
                    </div>
                </div>
               
                <div class="row">
                    <div class="col-lg-12">
                        <ol class="breadcrumb">
                            <li>
                                <a href="/District_Enterprise/District_Home.aspx">Home</a>
                            </li>
                            <li class="active">
                                <strong>Facility GIS Maps</strong>
                            </li>
                        </ol>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4>Facility GIS Maps</h4>
                    </div>
                    <div class="panel-body">

                        <div class="row">
                            <div class="col-lg-3">
                                <div>
                                    <h5>Facility</h5>
                                </div>
                                <div>
                                    <asp:DropDownList ID="wdd_Facility" CssClass="form-control" runat="server" Width="240px">
                                    </asp:DropDownList>
                                </div>
                             </div>
                          
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                         <div id="map_canvas" style="width: 100%; height: 800px"></div>
                  
                    </div>
                </div>
                <br />
                
            </div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    $(document).ready(function () {
        $(".dropdown").hover(
            function () {
                $('.dropdown-menu', this).not('.in .dropdown-menu').stop(true, true).slideDown("400");
                $(this).toggleClass('open');
            },
            function () {
                $('.dropdown-menu', this).not('.in .dropdown-menu').stop(true, true).slideUp("400");
                $(this).toggleClass('open');
            }
        );
    });
</script>