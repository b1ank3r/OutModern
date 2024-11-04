<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="OutModern.src.Admin.Dashboard.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/tailwindcss">
        @layer components {
            .dashboard-item-container {
                @apply min-w-52;
            }

                .dashboard-item-container .dashboard-item {
                    @apply bg-gray-100 drop-shadow rounded-2xl p-4 mt-4;
                }

                    .dashboard-item-container .dashboard-item .text > * {
                        @apply text-2xl font-bold font-black;
                    }
        }
    </style>

    <script src='<%# Page.ResolveClientUrl("~/lib/highcharts/highcharts.js") %>'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Genaral Data -->
    <div class="flex gap-8 w-full justify-around">
        <!--Customer-->
        <div class="dashboard-item-container">
            <div class="dashboard-item">
                <div class="title">
                    Total Customer
                </div>
                <div class="text">
                    <asp:Label ID="lblTotalCustomer" runat="server"></asp:Label>
                </div>
            </div>
            <div class="dashboard-item">
                <div class="title">
                    Total Staff
                </div>
                <div class="text">
                    <asp:Label ID="lblTotalStaff" runat="server" Text="hvnt implement"></asp:Label>
                </div>
            </div>
        </div>

        <!--Orders-->
        <div class="dashboard-item-container">
            <div class="dashboard-item">
                <div class="title">
                    This Month Orders
                </div>
                <div class="text">
                    <asp:Label ID="lblMonthOrders" runat="server"></asp:Label>
                </div>
            </div>
            <div class="dashboard-item">
                <div class="title">
                    Today New Orders
                </div>
                <div class="text">
                    <asp:Label ID="lblNewOrders" runat="server"></asp:Label>
                </div>
            </div>
        </div>

<%--        <!-- Cancelled -->
        <div class="dashboard-item-container">
            <div class="dashboard-item">
                <div class="title">
                    This Month Cancelled
                </div>
                <div class="text">
                    <asp:Label ID="lblMonthCancelled" runat="server"></asp:Label>
                </div>
            </div>
            <div class="dashboard-item">
                <div class="title">
                    Today Cancelled
                </div>
                <div class="text">
                    <asp:Label ID="lblTodayCancelled" runat="server"></asp:Label>
                </div>
            </div>
        </div>--%>

        <!-- Review Pending Reply -->
        <div class="dashboard-item-container">
            <div class="dashboard-item">
                <div class="title">
                    Today New Review
                </div>
                <div class="text">
                    <asp:Label ID="lblTodayReviews" runat="server"></asp:Label>
                </div>
            </div>
            <div class="dashboard-item">
                <div class="title">
                    Overall Rating
                </div>
                <div class="text">
                    <asp:Label ID="lblOverallRating" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <!--Charts-->
    <div class="mt-10 overflow-hidden">
        <div id="sales-chart" class="w-[60%] float-left">
        </div>
        <div id="sales-chart-category" class="w-[40%] float-right">
        </div>
    </div>


    <script>
        //Overall Sales chart

        (() => {

            // Data for line chart
            let lineData = <%= lineData %>; // Replace with your generated data array

            console.log(lineData)

            Highcharts.chart('sales-chart', {
                chart: {
                    type: 'line' // Set chart type as line
                },
                title: {
                    text: 'Sales In One Year' // Chart title
                },
                tooltip: {
                    valueSuffix: '',
                    valuePrefix: 'RM '
                },
                xAxis: {
                    title: { // Label for X-axis
                        text: 'Months'
                    },
                    type: 'datetime',
                    labels: {
                        formatter: function () {
                            let date = new Date(this.value);
                            return Highcharts.dateFormat("%b %Y", date);
                        }
                    }
                },
                yAxis: {
                    title: { // Label for Y-axis
                        text: ''
                    }
                },
                series: [{ // Define data series
                    name: 'Sales', // Series name
                    data: lineData// Replace with your generated data array
                }]
            });

        })();

        // Pie chart, sales for each category
        (() => {

            const salesByCategory = <%= salesByCategoryData %>; // Replace with your generated data array

            Highcharts.chart('sales-chart-category', {
                chart: {
                    type: 'pie', // Set chart type as line
                },
                title: {
                    text: 'Total Sales By Category' // Chart title
                },
                tooltip: {
                    valueSuffix: '',
                    valuePrefix: 'RM '
                },
                plotOptions: {
                    series: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: [{
                            enabled: true,
                            distance: 20
                        },
                        {
                            enabled: true,
                            distance: -40,
                            format: '{point.percentage:.1f}%',
                            style: {
                                fontSize: '1.2em',
                                textOutline: 'none',
                                opacity: 0.7
                            },
                            filter: {
                                operator: '>',
                                property: 'percentage',
                                value: 10
                            }
                        }]
                    }
                },
                series: [{ // Define data series
                    name: 'Sales',
                    colorByPoint: true,
                    data: salesByCategory// Replace with your generated data array
                }]
            });
        })();

    </script>
</asp:Content>
