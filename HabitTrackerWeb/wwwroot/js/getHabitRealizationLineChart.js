
$(document).ready(function () {
    loadProgressChartData();
});

function loadProgressChartData() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/HabitRealization/GetProgressChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {


            loadLineChart("newHabitRealizationLineChart", data);

            $(".chart-spinner").hide();
        }
    })
}

function loadLineChart(id, data) {
    var chartColors = getChartColorsArray(id);

    var options = {
        series: data.series,

        colors: chartColors,
        chart: {
            type: 'line',
            width: "100%",
            height: "280%",
        },
        stroke: {
            width: 4,
            curve: 'smooth'

        },
        markers: {
            size: 6,
            hover: {
                sizeOffset: 4
            }
        },
        xaxis: {
            categories: data.categories,
        },

    }

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}

function getChartColorsArray(id) {
    if (document.getElementById(id) != null) {
        var colors = document.getElementById(id).getAttribute("data-colors");
        if (colors) {
            colors = JSON.parse(colors);
            return colors.map(function (value) {
                var newValue = value.replace(" ", "");
                if (newValue.indexOf(",") === -1) {
                    var color = getComputedStyle(document.documentElement).getPropertyValue(newValue);
                    if (color) return color;
                    else return newValue;;
                }
            });
        }
    }
}