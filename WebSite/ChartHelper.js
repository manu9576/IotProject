class ChartHelper {

    chartContext;
    dataset = [];
    
    constructor(chart) {
        
        let ctx = chart.getContext('2d');

        this.chartContext = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: this.datasets
            },
            options: {

                responsive: false,
                hoverMode: 'index',
                stacked: false,
                title: {
                    display: true,
                    text: 'Sensors'
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
}