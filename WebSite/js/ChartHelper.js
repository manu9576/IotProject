let translate_month = function (month) {

    var result = month;

    switch (month) {
        case 'Jan':
            result = 'Janv';
            break;
        case 'Feb':
            result = 'Fév';
            break;
        case 'Mar':
            result = 'Mars';
            break;
        case 'Apr':
            result = 'Avr';
            break;
        case 'May':
            result = 'Mai';
            break;
        case 'Jun':
            result = "Juin"
            break;
        case 'Jul':
            result = "Juil"
            break;
        case 'Aug':
            result = 'Août';
            break;
        case 'Sep':
            result = 'Sept';
            break;
        case 'Oct':
            result = 'Oct';
            break;
        case 'Nov':
            result = 'Nov';
            break;
        case 'Dec':
            result = 'Déc';
            break;
    }

    return result;
};

let translate_day = function (day) {

    var result = day;

    switch (day) {
        case 'Monday':
            result = 'Lun';
            break;
        case 'Tuesday':
            result = 'Mar';
            break;
        case 'Wednesday':
            result = 'Mer';
            break;
        case 'Thursday':
            result = 'Jeu';
            break;
        case 'Friday':
            result = 'Ven';
            break;
        case 'Saturday':
            result = "Sam"
            break;
        case 'Sunday':
            result = "Dim"
            break;
    }

    return result;
};


let translate_this_label = function (label) {

    let month = label.match(/Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec/g);

    let day = label.match(/Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday/g);

    let translatedDate = label;

    if (month) {
        let translation = translate_month(month[0]);
        translatedDate = translatedDate.replace(month, translation);
    }

    if (day) {
        let translation = translate_day(day[0]);
        translatedDate = translatedDate.replace(day, translation);
    }

    return translatedDate;
};

class ChartHelper {

    constructor(chart) {

        moment.locale('fr');
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
                            tooltipFormat: 'dddd DD MMM, YYYY',
                            unitStepSize: 1,
                            displayFormats: {
                                day: 'dddd DD MMM'
                            }
                        },
                        scaleLabel: {
                            display: true,
                        },
                        ticks: {

                            // Here's where the magic happens:
                            callback: function (label, index, labels) {

                                return translate_this_label(label);
                            }
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