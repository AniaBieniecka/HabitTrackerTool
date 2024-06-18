
$(document).ready(function () {
    loadEachHabitProgressChartData();
});

function loadEachHabitProgressChartData() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/HabitRealization/GetProgressChartDataForEachHabit",
        type: 'GET',
        dataType: 'json',
        success: function (data) {


            loadEachHabitLineChart("newRealizationLineChartForEachHabit", data);

            $(".chart-spinner").hide();
        }
    })
}

function loadEachHabitLineChart(id, data) {
    var options = {
        series: data.series,
        chart: {
            type: 'line',
            width: "100%",
            height: "140%",
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

        yaxis: {
            min: 0,          // Minimalna wartość osi Y
            max: 10,        // Maksymalna wartość osi Y
            tickAmount: 2,  // Liczba znaczników osi Y

            title: {
                text: 'Points'
            }
        }

    }

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}