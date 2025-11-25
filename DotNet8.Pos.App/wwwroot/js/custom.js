window.enableLoading = function (isEnable) {
//    if (isEnable) {
//        document.getElementById('loading').style.display = "";
//    } else {
//        document.getElementById('loading').style.display = "none";
//    }
}

window.setBarChart = function (id, labels, series) {
    const _labels = labels;
    const _series = series;

    var options = {
        series: [{
            data: _series
        }],
        chart: {
            type: 'bar',
            height: 350
        },
        plotOptions: {
            bar: {
                borderRadius: 4,
                borderRadiusApplication: 'end',
                horizontal: true,
            }
        },
        dataLabels: {
            enabled: false
        },
        xaxis: {
            categories: _labels,
        },
        colors: ['#00C853']
    };

    var chart = new ApexCharts(document.querySelector(id), options);
    chart.render();
}