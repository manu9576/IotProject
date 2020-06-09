"use stric";

let _datasets = [];
let chart;
const DEVICE_ID = 6;

let updateValues = function (chart, sensorsId, month) {

  _datasets.length = 0;

  for (let i = 0; i < sensorsId.length; i++) {
    let sensorId = sensorsId[i];


    var xhr = new XMLHttpRequest();
    xhr.onload = function () {

      let vals = JSON.parse(xhr.responseText);
      let formatedVals = [];

      vals.forEach(element => {
        if (element.value > 1) {
          formatedVals.push({
            x: new moment(element.dateTime),
            y: element.value
          });
        }
      });

      _datasets[i] = {
        label: 'Value: ' + sensorId,
        data: formatedVals,
        borderColor: "#3e95cd",
        fill: false,
        yAxisID: 'y-axis-1'
      };

      chart.update();


    };

    // xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/18/Month/5', true);
    xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/' + sensorId + '/Month/' + month, true);
    xhr.setRequestHeader("Content-Type", 'text/plain');
    xhr.send(null);

  }

};

let initializeGraph = function () {

  let ctx = document.getElementById('sensorChart').getContext('2d');
  chart = new Chart(ctx, {
    type: 'line',
    data: {
      datasets: _datasets
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
          distribution: 'linear',
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

  let date = new Date(document.getElementById('date').value);

  let capteursList = document.getElementById('capteursList');
  let sensorsId = [];

  let checkboxs = capteursList.getElementsByTagName('input');

  for (let i = 0; i < checkboxs.length; i++) {
    let chb = checkboxs[i];
    if (chb.checked) {
      sensorsId.push(chb.id);
    }
  }

  updateValues(chart, sensorsId, date.getMonth() + 1);

};

let initializeSensors = function () {

  var xhr = new XMLHttpRequest();
  xhr.onload = function () {

    let sensors = JSON.parse(xhr.responseText);

    let capteursList = document.getElementById('capteursList');

    sensors.forEach(sensor => {
      let checkbox = document.createElement("input");
      checkbox.type = 'checkbox';
      checkbox.id = sensor.sensorId;

      let label = document.createElement("label");
      label.for = sensor.sensorId;
      label.innerText = sensor.name;

      let div = document.createElement("div");
      div.appendChild(checkbox);
      div.appendChild(label);

      capteursList.appendChild(div);
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
  button.addEventListener("click", update);

};