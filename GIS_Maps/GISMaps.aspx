<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="GISMaps.aspx.cs" Inherits="mascis.GIS_Maps.GISMaps" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Infragistics45.Web.v17.1, Version=17.1.20171.2073, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.GridControls" tagprefix="ig" %>

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

    <%--<script src="http://maps.google.com/maps/api/js?sensor=true" type="text/javascript"></script>--%>



    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - Mascis MIS</title>

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
            // Google has tweaked their interface somewhat - this tells the api to use that new UI
            google.maps.visualRefresh = true;
            //var Uganda = new google.maps.LatLng(1.3707, 32.3032);

            // Original map options
            var mapOptions = {
                center: new google.maps.LatLng(markers[0].Latititude, markers[0].Longtitude),
                zoom: 8,
                //mapTypeId: google.maps.MapTypeId.ROADMAP
                 mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
                //marker:true  
            };

            var infoWindow = new google.maps.InfoWindow();


            var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var city = markers[i].Longtitude
                //alert(data.description)
                var myLatlng = new google.maps.LatLng(data.Latititude, data.Longtitude);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.Facility,
                    desc: data.District_Name
                });
                (function (marker, data) {

                     // Make the marker-pin blue!
                   // marker.setIcon('http://maps.google.com/mapfiles/ms/icons/blue-dot.png')
                    // Attaching a click event to the current marker
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(
                            '<b>Facility:</b> ' + data.Facility + '<br> ' + '<hr> '
                            + '<b>District Name:</b> ' + data.District_Name + '<br> ' + '<hr> '
                            + '<b>Facility Type:</b> ' + data.FacilityType + '<br> ' + '<hr> '
                            + '<b>Sector:</b> ' + data.ZoneDescription + '<br> ' + '<hr> '
                            + '<b>Implementing Partner:</b> ' + data.ImplementingPartnerDescription + '<br> ' + '<hr> '
                            + '<b>level of Care:</b> ' + data.level_of_care + '<br> ' + '<hr> '
                            + '<b>Client Type:</b> ' + data.client_type_desc + '<br> ' + '<hr> '
                            + '<b>CDC Region:</b> ' + data.CDCRegion + '<br> ' + '<hr> '
                            + '<b>OwnershipDescription:</b> ' + data.OwnershipDescription + '<br> ' + '<hr> '
                            + '<b>GIS Coordinates (Longtitude):</b> ' + data.Latititude + '<br> ' + '<hr> '
                            + '<b>GIS Coordinates (Latititude):</b> ' + data.Longtitude + '<br> ');
                        // alert(data.lat);Latititude
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
               <%-- <div class="navbar navbar-default navbar-fixed-top">
                    <div class="navbar-header">
                        <button class="navbar-toggle" type="button" data-toggle="collapse" data-target=".js-navbar-collapse">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                     
                    </div>

                    <div class="collapse navbar-collapse js-navbar-collapse">
                        <ul class="nav navbar-nav">
                        </ul>
                       
                    </div>
                </div>
               --%>
              <%--  <div class="row">
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
                </div>--%>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h2 style="position:center"> <b>Maul Facility GIS Maps</b> </h2>
                    </div>
                    <div class="panel-body" hidden="hidden">

                        <div class="row">
                            <div class="col-lg-3">
                                <div>
                                    <h5>Facility</h5>
                                </div>
                                <div>
                                  
                                    <asp:DropDownList ID="wdd_Facility" runat="server" CssClass="form-control" AppendDataBoundItems="true" Width="240px" DataTextField="Facility" DataValueField="FacilityCode" SelectMethod="getComponent">
                                        <asp:ListItem Text="--Please Select--" Value=""></asp:ListItem>
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
<%--<script type="text/javascript">
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
</script>--%>
