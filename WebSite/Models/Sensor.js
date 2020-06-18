class Sensor{

    id;
    name;
    unit;
    yAxe;
    isSelected;

    constructor(id,name,unit){
        this.id = id;
        this.name = name;
        this.unit = unit;
        this.yAxe = null;
        this.isSelected = false;
    }
}