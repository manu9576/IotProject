
    let  device= function() {

        var xhr = new XMLHttpRequest();

        xhr.onload  = function () {
            alert(xhr.responseText);
        };
        xhr.open('GET', 'http://localhost:54384/api/Device',true);
        xhr.setRequestHeader("Content-Type", 'text/plain');
        xhr.send(null);

        return [
            {
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


window.onload = function() {
    let textArea = window.document.getElementById("text");

    textArea.innerText = JSON.stringify(device());
}