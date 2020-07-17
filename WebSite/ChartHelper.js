class ChartHelper {

    constructor(chart) {


        this.datasets = [];
        this.ctx = chart.getContext('2d');

        this.chart = new Chart(this.ctx, {
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
                        distribution: 'linear',
                        time: {
                            unit: 'day',
                            unitStepSize: 1,
                            displayFormats: {
                                'day': 'DD MMM'
                            },
                        }
                    }],
                    yAxes: [{
                        id: 'y-axis-0',
                        display: true,
                        type: 'linear',
                        labelString: "Y-Axe 1"
                    },
                    {
                        id: 'y-axis-1',
                        display: false,
                        type: 'linear',
                        position: 'right',
                        labelString: "Y-Axe 2",
                        ticks: {
                            min: undefined,
                            max: undefined
                        }

                    }]
                },

            }
        });

        this.yAxes = this.chart.options.scales.yAxes;
        this.xAxes = this.chart.options.scales.xAxes;
    }

    clearDatasets() {
        this.chart.data.datasets.length = 0;
    }

    addDataSet(sensor) {
        this.chart.data.datasets.push(sensor);
    }

    updateChart() {

        this.chart = new Chart(this.ctx, {
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
                    xAxes: this.xAxes,
                    yAxes: this.yAxes
                },

            }
        });

    }

}