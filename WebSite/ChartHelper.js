class ChartHelper {

    constructor(chart) {

        this.yAxes = [];
        this.datasets = [];
         let ctx = chart.getContext('2d');
        this.defaultYAxe = new YAxe("linear", "left", "AXe1");
        this.yAxes.push(
            this.defaultYAxe
        );

        this.chart = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: this.datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,

                legend: {
                    display: false
                },

                scales: {
                    xAxes: [{
                        type: 'time',
                        distribution: 'linear'
                    }],
                    yAxes: this.yAxes
                },

            }
        });
    }

    clearDatasets() {
        this.datasets.length = 0;
    }

    addDataSet(sensor) {
        sensor.yAxeid = this.defaultYAxe.id;
        this.datasets.push(sensor);
    }

    updateChart() {
        this.chart.update();
    }
}