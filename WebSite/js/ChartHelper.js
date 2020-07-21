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


let translate_date_label = function (label) {

    if(!isFrenchLanguage()){
        return label;
    }

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

let isFrenchLanguage= function(){
    return (navigator.language || navigator.userLanguage) == 'fr-FR';
}

class ChartHelper {

    constructor(chart) {

        if(isFrenchLanguage()){
            moment.locale('fr');
        }

        this.datasets = [];
        this.ctx = chart.getContext('2d');
        this.tooltips = {
            mode: 'index',
            callbacks: {
                label: function (tooltipItem, data) {
                    var label = data.datasets[tooltipItem.datasetIndex].label || '';

                    if (label) {
                        label += ': ';
                    }
                    label += Math.round(tooltipItem.yLabel * 10) / 10;
                    label += data.datasets[tooltipItem.datasetIndex].unit;
                    return label;
                },

                title: function (tooltipItem, data) {
                    let sensor = data.datasets[tooltipItem[0].datasetIndex];

                    let sensorData = sensor.data;

                    let index = tooltipItem[0].index;

                    let dateTime = sensorData[index].x;

                    return dateTime.format('lll');
            
                }
            }
        };
        this.xAxes = [{
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
                callback: function (label, index, labels) {
                    return translate_date_label(label);
                }
            }
        }];

        this.yAxes = [{
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

        }];

        this.updateChart();
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

                tooltips: this.tooltips
            }
        });
    }
}