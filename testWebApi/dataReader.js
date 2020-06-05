"use stric";

let getValues = function (formatedVals, chart) {
  var xhr = new XMLHttpRequest();
  xhr.onload = function () {

    let vals = JSON.parse(xhr.responseText);

    formatedVals.length = 0;
    vals.forEach(element => {
      if (element.value > 1) {
        formatedVals.push({
          x: new moment(element.dateTime),
          y: element.value
        });
      }
    });


    debugger;

    chart.update();
  };

  // xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/18/Month/5', true);
  xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/28/Month/5', true);
  xhr.timeout = 5000;
  xhr.ontimeout = function (e) {
    alert("Timeout");
  };

  xhr.setRequestHeader("Content-Type", 'text/plain');
  xhr.send(null);
}


window.onload = function () {
  let formatedVals = []
  let ctx = document.getElementById('myChart').getContext('2d');
  let myChart = new Chart(ctx, {
    type: 'line',
    data: {
      datasets: [{
        label: 'Value',
        data: formatedVals,
        borderColor: "#3e95cd",
        fill: false,
        yAxisID: 'y-axis-1'
      }]
    },
    options: {

      responsive: false,
      hoverMode: 'index',
      stacked: false,
      title: {
        display: true,
        text: 'Chart.js Line Chart - Multi Axis'
      },

      scales: {

        xAxes: [{
          type: 'time',
          distribution: 'series',
          time: {
            displayFormats: {
              quarter: 'MMM YYYY H:mm'
            }
          }
        }],

        yAxes: [{
          type: 'linear',
          display: true,
          position: 'left',
          id: 'y-axis-1',
          min: 25,
          max: 50
        }]

      }
    }
  });


  getValues(formatedVals,myChart);
}