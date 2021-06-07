// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//Made By: David Manshari! at 4am :(

// Write your JavaScript code.


$(function () {

    $.ajax({
        url: '/Products/StatsByOrders',
        data: {},
        success: function (data) {
            StatsByOrders(data);
        },
        error: function () {
            alert("error");
        }
    });

    $.ajax({
        url: '/Products/StatsByViews',
        data: {},
        success: function (data) {
            if(data !=0)
            StatsByViews(data);
        },
        error: function () {
            alert("error");
        }
    });

    function StatsByOrders(data2) {


        var dataArray = [];
        var headers = ['Name', 'Value', { role: 'style' }, { role: 'annotation' }]
        dataArray.push(headers);
        data2.forEach(function (d, i) {
            d.category === 'A' ? fill = '#20c997' : fill = '#B44682';
            i === 0 || i === data2.length - 1 ? annotation = d.value : annotation = null;
            i === 0 || i === data2.length - 1 ? fill = '#2b9274' : fill = '#20c997';

            dataArray.push([d.name, d.value, fill, annotation]);
        });


        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            var data = google.visualization.arrayToDataTable(dataArray);

            var options = {
                chartArea: {
                    top: 10,
                    bottom: 0,
                    left: 65,
                    right: 50
                },
                hAxis: {
                    gridlines: {
                        color: '#fff'
                    }
                },
                legend: 'none',
                vAxis: {
                    textStyle: {
                        fontName: 'Josefin Sans',
                        fontSize: 16,
                        color: '#4D4D4D'
                    }
                }
            };

            var chart = new google.visualization.BarChart(document.getElementById('chart'));

            chart.draw(data, options);
        }

    }



    function StatsByViews(data2) {
        var dataArray = [];
        var headers = ['Name', 'Value', { role: 'style' }, { role: 'annotation' }]
        dataArray.push(headers);
        data2.forEach(function (d, i) {
            d.category === 'A' ? fill = '#20c997' : fill = '#B44682';
            i === 0 || i === data2.length - 1 ? annotation = d.value : annotation = null;
            i === 0 || i === data2.length - 1 ? fill = '#2b9274' : fill = '#20c997';

            dataArray.push([d.name, d.value, fill, annotation]);
        });

        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            var data = google.visualization.arrayToDataTable(dataArray);

            var options = {
                chartArea: {
                    top: 10,
                    bottom: 0,
                    left: 65,
                    right: 50
                },
                hAxis: {
                    gridlines: {
                        color: '#fff'
                    }
                },
                legend: 'none',
                vAxis: {
                    textStyle: {
                        fontName: 'Josefin Sans',
                        fontSize: 16,
                        color: '#4D4D4D'
                    }
                }
            };

            var chart = new google.visualization.BarChart(document.getElementById('chart2'));

            chart.draw(data, options);
        }
    }
});







