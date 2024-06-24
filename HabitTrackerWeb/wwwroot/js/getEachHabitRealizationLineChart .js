
$(document).ready(function () {
    for (var i = 0; i < chartsQuantity; i++){
        loadEachHabitProgressChartData(i);
    }
});

function loadEachHabitProgressChartData(chartNumber) {
    console.log("Parameter: " + chartNumber);

    $(".chart-spinner").show();

    $.ajax({
        url: "/HabitRealization/GetProgressChartDataForEachHabit/"+chartNumber,
        type: 'GET',
        data: { chartNumber: chartNumber },
        dataType: 'json',
        success: function (data) {

            loadEachHabitLineChart("newRealizationLineChartForEachHabit_" + chartNumber, data);

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
            },

        },
        legend: {
            show: true,
            showForSingleSeries: true
        },

    }

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}