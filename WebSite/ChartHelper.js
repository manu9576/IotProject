const curvesColor = ["red", "blue", "green"];

class ChartHelper {

    chart;
    datasets = [];
    
    constructor(chart) {
        
        let ctx = chart.getContext('2d');

        this.chart = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: this.datasets
            },
            options: {
                responsive: false,
                hoverMode: 'index',
                stacked: false,
                title: {
                    display: false
                },

                scales: {

                    xAxes: [{
                        type: 'time',
                        distribution: 'linear'
                    }],

                    yAxes: [{
                        type: 'linear',
                        display: true,
                        position: 'left',
                        id: 'y-axis-1',
                        // min: 25,
                        // max: 50
                    }]

                }
            }
        });

    }

    clearDatasets(){
        this.datasets.length = 0;
    }

    addDataSet(sensorName,values){
        this.datasets.push({
            label: sensorName,
            data: values,
            borderColor: curvesColor[this.datasets.length % curvesColor.length],
            fill: false
          });
    }

    updateChart(){
        this.chart.update();
    }
}