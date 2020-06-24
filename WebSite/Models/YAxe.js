class YAxe {

    constructor(type,position,id){
        this.type = type;
        this.position = position;
        this.id = id;
        this.display= true;
    }

    hide(){
        this.display = false;
    }

    show(){
        this.display = true;
    }
}