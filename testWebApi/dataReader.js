"use strict";

let _datasets = [];
let chart;
const DEVICE_ID = 6;
const curvesColor = ["red", "blue", "green"];


let updateValues = function (chart, sensorsId, month) {

  _datasets.length = 0;

  for (let i = 0; i < sensorsId.length; i++) {

    let sensorId = sensorsId[i];

    let xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {

      if (xhr.readyState == 4 && xhr.status == 200 && xhr.responseText !== "") {

        let parsedValues = JSON.parse(xhr.responseText);
        let formattedValues = [];

        parsedValues.forEach(element => {
          if (element.value > 1) {
            formattedValues.push({
              x: new moment(element.dateTime),
              y: element.value
            });
          }
        });
        console.log("Add new sensor : " + sensorId + " with " + formattedValues.length + " values");
        _datasets.push({
          label: 'Sensor id: ' + sensorId,
          data: formattedValues,
          borderColor: curvesColor[_datasets.length % curvesColor.length],
          fill: false,
          yAxisID: 'y-axis-1'
        });

        chart.update();
      }
    };
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

};

let update = function () {

  let date = new Date(document.getElementById('date').value);

  let sensorsList = document.getElementById('sensorsList');
  let sensorsId = [];

  let checkboxs = sensorsList.getElementsByTagName('input');

  for (let i = 0; i < checkboxs.length; i++) {
    let chb = checkboxs[i];
    if (chb.checked) {
      sensorsId.push(chb.id);
    }
  }

  updateValues(chart, sensorsId, date.getMonth() + 1);

};

let initializeSensors = function () {

  let xhr = new XMLHttpRequest();
  xhr.onload = function () {

    let sensors = JSON.parse(xhr.responseText);

    let sensorsList = document.getElementById('sensorsList');

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

      sensorsList.appendChild(div);
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