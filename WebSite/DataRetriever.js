"use strict";
const DEVICE_ID = 6;
class DataRetriever {

    getValuesForInterval(sensorId, startDate, endDate) {
        return new Promise((successCallback, failureCallback) => {
            try {

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

                        successCallback(formattedValues);

                    }
                };
                xhr.open('GET', 'https://manu9576.net/api/Measure/Sensor/' + sensorId + '/From/' +
                    startDate + '/To/' + endDate, true);
                xhr.setRequestHeader("Content-Type", 'text/plain');
                xhr.send(null);
            } catch (ex) {
                failureCallback("Exception during getValuesForInterval: " + ex);
            }
        });
    }

    getSensorsList() {
        return new Promise((successCallback, failureCallback) => {
            try {
                let xhr = new XMLHttpRequest();
                xhr.onload = function () {
                    try {

                        let jsonSensors = JSON.parse(xhr.responseText);

                        let sensors =[];

                        jsonSensors.forEach((jsonSensor) => {
                            sensors.push(
                                new Sensor(
                                    jsonSensor.sensorId,
                                    jsonSensor.name,
                                    jsonSensor.unit)
                                    );
                        });

                        successCallback(sensors);

                    } catch (ex) {
                        failureCallback("Exception while parsing sensors list: " + ex);
                    }

                };

                xhr.open('GET', 'https://manu9576.net/api/Sensor/Device/' + DEVICE_ID, true);
                xhr.setRequestHeader("Content-Type", 'text/plain');
                xhr.send(null);

            } catch (ex) {
                failureCallback("Exception during getSensorsList: " + ex);
            }
        })
    }
}