"use stric";

let formatedVals = [];
let myChart;
const DEVICE_ID = 6;

let updateValues = function (formatedVals, chart, sensorId, month) {
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

    chart.update();
  };

  // xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/18/Month/5', true);
  xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/' + sensorId + '/Month/' + month, true);
  xhr.setRequestHeader("Content-Type", 'text/plain');
  xhr.send(null);
};

let initializeGraph = function () {

  let ctx = document.getElementById('myChart').getContext('2d');
  myChart = new Chart(ctx, {
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
        text: 'Sensor'
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

};

let update = function () {

  debugger;

  let capteursList = document.getElementById('capteursList');  
  let sensorId= capteursList.options[capteursList.selectedIndex].value;

  let date =  new Date(document.getElementById('date').value);  


  updateValues(formatedVals, chart, sensorId, date.getMonth());

};

let initializeSensors = function () {

  var xhr = new XMLHttpRequest();
  xhr.onload = function () {

    let sensors = JSON.parse(xhr.responseText);

    let capteursList = document.getElementById('capteursList');


    sensors.forEach(sensor => {
      var option = document.createElement("option");
      option.text = sensor.name;
      option.value = sensor.id;

      capteursList.add(option);
    });
  };

  xhr.open('GET', 'http://localhost:54384/api/Sensor/Device/' + DEVICE_ID, true);
  xhr.setRequestHeader("Content-Type", 'text/plain');
  xhr.send(null);


};

window.onload = function () {
  initializeGraph();
  initializeSensors();

  let button = document.getElementById('updateButton');
  button.addEventListener("update", update);

}