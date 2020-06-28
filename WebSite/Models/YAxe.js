class YAxe {

    constructor(type,position,id,label){
        this.type = type;
        this.position = position;
        this.id = id;
        this.display= true;
        this.labelString = label;
        this.ticks= {
            min: undefined,
            max: undefined
        } 
    }

    hide(){
        this.display = false;
    }

    show(){
        this.display = true;
    }
}