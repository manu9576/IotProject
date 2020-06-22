class YAxe {
    type= 'linear';
    display= true;
    position= 'left';
    id= 'y-axis-1';

    constructor(type,position,id){
        this.type = type;
        this.position = position;
        this.id = id;
    }

    hide(){
        this.display = false;
    }

    show(){
        this.display = true;
    }
}