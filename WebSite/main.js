let convertDate = function (date) {

    let dd = String(date.getDate()).padStart(2, '0');
    let mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = date.getFullYear();

    return yyyy + "-" + mm + "-" + dd;
};

let todayDate = function () {
    return new Date();
}

let oneWeekEarlier = function () {

    let date = new Date();
    date.setDate(date.getDate() - 7);

    return date;
};

Vue.component('sensor-list', {
    props: {
        sensors: {
            type: Array,
            require: true
        },
        yAxes: {
            type: Array,
            require: true
        }
    },
    template: `
    <fieldset>
        <legend>Select sensors to display</legend>
        <div id="sensors-table">
            <table>
            
            <tr>
                <th>Senor name</th>
                <th>Unit</th>
                <th>Display</th>
                <th>Color</th>
                <th>Y-axe</th>
            </tr>

            <sensor-detail
                id="sensors-table-content"
                v-for="(sensor) in sensors" 
                :key="sensor.id" :sensor="sensor" 
                :yAxes="yAxes">
            </sensor-detail>

            </table>
        </div>    
        
    </fieldset>
    `
});

Vue.component('sensor-detail', {
    props: {
        sensor: {
            type: Object,
            require: true
        },
        yAxes: {
            type: Array,
            require: true
        }
    },
    template: `
    <tr>
        <td>{{sensor.label}}</td>
        <td>{{sensor.unit}}</td>
        <td><input type='checkbox' v-model="sensor.isSelected" v-bind:id="sensor.id"></td>
        <td><input type="color" class="yAxeColor" v-model="sensor.borderColor"></td>
        <td>
            <select v-model="sensor.yAxisID">
                <option v-for="yAxe in yAxes" v-bind:value="yAxe.id">
                    {{ yAxe.labelString }}
                </option>
            </select>
        </td>
    </tr>
    `
});

Vue.component('yAxe-list', {
    data() {
        return {
            axeName: ""
        };
    },
    props: {
        yAxes: {
            type: Array,
            require: true
        }
    },
    template: `
    <fieldset>
        <legend>Y-Axe configuration</legend>
        <yAxe-detail v-for="(yAxe) in yAxes" :key="yAxe.id" :yAxe="yAxe"></yAxe-detail>
    </fieldset>
    `,
    methods: {
        addNewAxe() {

            this.$emit('add-axe', this.axeName);
            this.axeName = '';
        }
    }
});

Vue.component('yAxe-detail', {
    props: {
        yAxe: {
            type: Object,
            require: true
        }
    },
    data: function () {
        return {
            hasAutoLimit: true
        };
    },
    mounted() {
        this.updateHasAutoLimit();
    },
    methods: {
        switchRangeMode() {

            if(this.hasAutoLimit){
                this.yAxe.ticks.min = 0;
                this.yAxe.ticks.max = 25;
            }else{
                this.yAxe.ticks.min = undefined;
                this.yAxe.ticks.max = undefined;
            }
            this.updateHasAutoLimit();
        },
        updateHasAutoLimit() {
            this.hasAutoLimit = this.yAxe.ticks === undefined || (this.yAxe.ticks.min === undefined && this.yAxe.ticks.max === undefined)
        }
    },
    computed:{
        buttonText(){
            if(this.hasAutoLimit){
                return 'Manual range';
            } else{
                return 'Automatic range';
            }
        }
    },

    template: `
    <div>
        <div>    
            <label>{{yAxe.labelString}} : </label>
            <label :for="yAxe.id">visible</label> 
            <input type='checkbox' v-model="yAxe.display" v-bind:id="yAxe.id">
        </div>
        <button @click='switchRangeMode'>{{buttonText}}</button>
        <div v-if="!hasAutoLimit">
            <input v-model.number="yAxe.ticks.min" type="number">
            <input v-model.number="yAxe.ticks.max" type="number">
        </div>
    </div>
    `
});

Vue.component('sensors-chart', {
    data() {
        return {
            chartHelper: undefined,
            dataRetriever: new DataRetriever(),
            sensors: [],
            startDate: convertDate(oneWeekEarlier()),
            endDate: convertDate(todayDate()),
            todayDate: convertDate(todayDate()),
            yAxes: []
        };
    },
    mounted() {
        this.chartHelper = new ChartHelper(this.$refs.chart);
        this.yAxes = this.chartHelper.yAxes;
        this.dataRetriever.getSensorsList().then((sensors) => {
            this.sensors = sensors;

            this.sensors.forEach(sensor => {
                sensor.yAxisID = this.yAxes[0].id;
            });
        });
    },
    template: `
    <div class="container item">

        <div>
            <canvas ref=chart style="height: 100%; width: 100%; min-height:500px" ></canvas>
        </div>

        <div id="legend">

            <sensor-list
                id="sensor-list"
                :sensors="sensors"
                :yAxes="yAxes">
            </sensor-list>

            <yAxe-list 
                id="yAxes-config"
                :yAxes="yAxes" 
                v-on:add-axe="addAxe">
            </yAxe-list>

            <fieldset id="xAxes-config">
                <legend>X-Axe configuration</legend>
                <div>
                    <label>Start date: </label>
                    <input type="date" v-model="startDate" :max="endDate">
                </div>
                <div>
                    <label>End date:   </label>
                    <input type="date" v-model="endDate" :max="todayDate" :min="startDate">
                </div>
            </fieldset>
 
            <div id="update-chart">
                <button @click='updateChart' >Update chart</button>
            </div>

        </div>
    </div>
    `,
    methods: {
        updateChart() {
            this.chartHelper.clearDatasets();

            let promises = [];

            this.sensors.forEach(sensor => {
                if (sensor.isSelected) {

                    promises.push(this.dataRetriever.getValuesForInterval(sensor.id, this.startDate, this.endDate).then((values) => {
                        sensor.data = values;
                        this.chartHelper.addDataSet(sensor);
                    }));

                }

            });

            Promise.all(promises).then((values) => {
                this.chartHelper.updateChart();
            });
        },
        convertDate(date) {

            let dd = String(date.getDate()).padStart(2, '0');
            let mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = date.getFullYear();

            return yyyy + "-" + mm + "-" + dd;
        },
        addAxe(yAxeName) {
            this.chartHelper.createYAxe(yAxeName);
        }
    }
});

var app = new Vue({
    el: '#chartPresenter'
});