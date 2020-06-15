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
                xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/' + sensorId + '/From/' +
                    convertDate(startDate) + '/To/' + convertDate(endDate), true);
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

                        let sensors = JSON.parse(xhr.responseText)
                        successCallback(sensors);

                    } catch (ex) {
                        failureCallback("Exception while parsing sensors list: " + ex);
                    }

                };

                xhr.open('GET', 'http://localhost:54384/api/Sensor/Device/' + DEVICE_ID, true);
                xhr.setRequestHeader("Content-Type", 'text/plain');
                xhr.send(null);

            } catch (ex) {
                failureCallback("Exception during getSensorsList: " + ex);
            }
        })
    }
}