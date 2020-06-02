let device = function () {

    var xhr = new XMLHttpRequest();

    xhr.onload = function () {

        let vals = JSON.parse(xhr.responseText);
        let formatedVals = []

        vals.forEach(element => {
            formatedVals.push({
                x: element.dateTime,
                y: element.value
            });
        });

        var ctx = document.getElementById('myChart').getContext('2d');
        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                // labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
                datasets: [{
                    label: '# of Votes',
                    data: formatedVals,
                    borderColor: [
                        'rgba(255, 99, 132, 1)',
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 206, 86, 1)',
                        'rgba(75, 192, 192, 1)',
                        'rgba(153, 102, 255, 1)',
                        'rgba(255, 159, 64, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    scales: {
                        xAxes: [{
                            type: 'time'
                        }]
                    }
                }
            }
        });


    };
    xhr.open('GET', 'http://localhost:54384/api/Measure/Sensor/18/Date/2020-06-02', true);
    xhr.timeout = 5000;
    xhr.ontimeout = function (e) {
        alert("Timeout");
    };


    xhr.setRequestHeader("Content-Type", 'text/plain');
    xhr.send(null);

    return [{
            "deviceId": 4,
            "name": "PC_BUREAU",
            "sensors": []
        },
        {
            "deviceId": 5,
            "name": "PC_TABLETTE",
            "sensors": []
        },
        {
            "deviceId": 6,
            "name": "raspberrypi",
            "sensors": []
        }
    ]
}


window.onload = function () {
    let textArea = window.document.getElementById("text");

    textArea.innerText = JSON.stringify(device());
}