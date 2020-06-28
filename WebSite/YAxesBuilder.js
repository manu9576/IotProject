class YAxesBuilder{
    
    constructor(){
        this.yAxes = [];
        this.idCounter = 0;
    }

    createNewAxe(label){
        
        if(this.yAxes.some((yAxe)=>{
            yAxe.label === label
        })){
            throw("Axe with the same label already exist");
        }
        
        this.yAxes.push(new YAxe("linear","left","yAxis_" + this.idCounter++,label));
    }

}