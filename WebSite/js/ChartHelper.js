/**
 * Class that manage a chartjs
 */
class ChartHelper {

    /**
     * Constructor : initialize axes and options
     * @param {canvas} chart 
     */
    constructor(chart) {

        let self = this;

        this.indexRequest = 0;
        this.translator = new Translator();
        this.datasets = [];
        this.ctx = chart.getContext('2d');

        this.tooltips = {
            mode: 'index',
            callbacks: {
                label: function (tooltipItem, data) {
                    var label = data.datasets[tooltipItem.datasetIndex].label || '';

                    if (label) {
                        label =
                            label + ': ' +
                            (Math.round(tooltipItem.yLabel * 10) / 10) +
                            data.datasets[tooltipItem.datasetIndex].unit;
                    }

                    return label;
                },

                title: function (tooltipItem, data) {
                    if (tooltipItem.length < 1) {
                        return '';
                    }

                    let sensorData = data.datasets[tooltipItem[0].datasetIndex].data;
                    let dateTime = sensorData[tooltipItem[0].index].x;

                    return dateTime.format('lll');
                }
            }
        };

        this.xAxes = [{
            type: 'time',
            distribution: 'linear',
            time: {
                unit: 'day',
                unitStepSize: 1,
                displayFormats: {
                    day: 'dddd DD MMM'
                }
            },
            scaleLabel: {
                display: true,
            },
            ticks: {
                callback: function (label) {
                    return self.translator.translate_date_label(label);
                }
            }
        }];

        this.yAxes = [{
                id: 'y-axis-0',
                display: true,
                type: 'linear',
                labelString: "Axe Y1",
                ticks: {
                    min: undefined,
                    max: undefined
                }
            },
            {
                id: 'y-axis-1',
                display: false,
                type: 'linear',
                position: 'right',
                labelString: "Axe Y2",
                ticks: {
                    min: undefined,
                    max: undefined
                }
            }
        ];

        if (this.translator.isFrenchLanguage()) {
            moment.locale('fr');
        }

        this.updateChart();
    }

    /**
     * Remove all data from the chart
     */
    initializeChart() {
        this.chart.data.datasets.length = 0;
        this.indexRequest++;
        return this.indexRequest;
    }

    /**
     * Add a sensor to the datasets of the chart
     * @param {Sensor} sensor 
     */
    addDataSet(sensor) {
        this.chart.data.datasets.push(sensor);
    }

    /**
     * Update the chart
     */
    updateChart() {

        if (this.chart) {
            this.chart.destroy();
        }

        this.chart = new Chart(this.ctx, {
            type: 'line',
            data: {
                datasets: this.datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                responsiveAnimationDuration: 0,

                animation: {
                    duration: 0
                },

                hover: {
                    animationDuration: 0
                },

                legend: {
                    display: false
                },

                scales: {
                    xAxes: this.xAxes,
                    yAxes: this.yAxes
                },

                tooltips: this.tooltips
            }
        });
    }
}