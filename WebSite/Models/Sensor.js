let sensorCount = 0;
sensorColor = [
    "#FF0000",
    "#00FF00",
    "#0000FF",
    "#DEB887",
    "#8B008B",
    "#483D8B",
    "#DAA520",
    "#7FFF00"
];

class Sensor {

    constructor(id, name, unit) {
        this.id = id;
        this.label = name;
        this.unit = unit;
        this.yAxisID = null;
        this.isSelected = false;
        this.borderColor = sensorColor[sensorCount % sensorColor.length];
        this.data = [];
        this.fill = false;
        this.pointRadius = 0;
        this.lineTension = 0;

        sensorCount++;
    }
}