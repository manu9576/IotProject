class ChartHelper {

    constructor(chart) {

        this.yAxes = [];
        this.datasets = [];
        let ctx = chart.getContext('2d');

        this.yAxesBuilder = new YAxesBuilder();
        this.yAxesBuilder.createNewAxe("Axe-1");
        this.yAxes = this.yAxesBuilder.yAxes;

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
        this.datasets.push(sensor);
    }

    updateChart() {
        this.chart.update();
    }

    createYAxe(name){
        this.yAxesBuilder.createNewAxe(name);
    }
}