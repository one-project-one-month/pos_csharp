window.setPieChart = function (_labels, _series) {
    // ['Team A', 'Team B', 'Team C', 'Team D', 'Team E']
    //[44, 55, 13, 43, 22]
    var options = {
        series: _series,
        chart: {
            width: 380,
            type: 'pie',
        },
        labels: _labels,
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
}

window.setLineColumnChart = function (response) {
    console.log(response);
    const productName = response.productName;
    const quantity = response.quantity;

    // [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
    // [23, 42, 35, 27, 43, 22, 17, 31, 22, 22, 12, 16]
    //  ['01 Jan 2001', '02 Jan 2001', '03 Jan 2001', '04 Jan 2001', '05 Jan 2001', '06 Jan 2001', '07 Jan 2001', '08 Jan 2001', '09 Jan 2001', '10 Jan 2001', '11 Jan 2001', '12 Jan 2001']
    var options = {
        series: [{
            name: 'Quantity',
            type: 'column',
            data: quantity
        }],
        chart: {
            height: 350,
            type: 'line',
        },
        stroke: {
            width: [0, 4]
        },
        title: {
            text: 'Best Seller Product List'
        },
        dataLabels: {
            enabled: true,
            enabledOnSeries: [1]
        },
        labels: productName,
        xaxis: {
            type: 'string'
        },
        yaxis: [{
            title: {
                text: 'Quantity',
            },

        }],
        colors: ['#00C853']
    };

    var chart = new ApexCharts(document.querySelector("#chart"), options);
    chart.render();
}

window.setFunnelChart = function (dailyResponse) {
    console.log(dailyResponse);
    const saleInvoiceDate = dailyResponse.salesInvoiceDate;
    const totalAmount = dailyResponse.totalAmount;

    var options = {
        series: [
            {
                name: "Total Amount",
                data: totalAmount,
            },
        ],
        chart: {
            type: 'bar',
            height: 350,
        },
        plotOptions: {
            bar: {
                borderRadius: 0,
                horizontal: true,
                barHeight: '80%',
                isFunnel: true,
            },
        },
        dataLabels: {
            enabled: true,
            formatter: function (val, opt) {
                return opt.w.globals.labels[opt.dataPointIndex] + ':  ' + val
            },
            dropShadow: {
                enabled: true,
            },
        },
        title: {
            text: 'Weekly Sale',
            align: 'middle',
        },
        xaxis: {
            categories: saleInvoiceDate,
        },
        legend: {
            show: false,
        },
        colors: ['#00C853', '#00C853']
    };

    var chart = new ApexCharts(document.querySelector("#lineChart"), options);
    chart.render();
}