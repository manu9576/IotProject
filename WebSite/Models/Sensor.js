class Sensor{

    id;
    label;
    unit;
    yAxeid;
    isSelected;
    borderColor;
    data;
    fill;

    constructor(id,name,unit){
        this.id = id;
        this.label = name;
        this.unit = unit;
        this.yAxeid = null;
        this.isSelected = false;
        this.borderColor = "blue";
        this.data = [];
        this.fill = false;
    }
}