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
    <div>
        <input type='checkbox' v-model="sensor.isSelected" v-bind:id="sensor.id">
        <label :for="sensor.id">{{sensor.label}} ({{sensor.unit}})</label>  
        
        <label> - Color </label>
        <input type="color" v-model="sensor.borderColor"> 

        <label> - Y axe: </label>
        <select v-model="sensor.yAxisID">
            <option v-for="yAxe in yAxes" v-bind:value="yAxe.id">
                {{ yAxe.labelString }}
            </option>
        </select>
    </div>
    `
});

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
        <sensor-detail v-for="(sensor) in sensors" :key="sensor.id" :sensor="sensor" :yAxes="yAxes"></sensor-detail>
    </fieldset>
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

        <label for="axeName">Name of new axe:</label>
        <input type="text" id="axeName" size="10" v-model="axeName">

        <button @click="addNewAxe">Add</button>

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
    template: `
    <div>
        <label>{{yAxe.labelString}}</label>  
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
        this.chartHelper.createYAxe("test");
        this.yAxes[1].position = 'right';
        this.dataRetriever.getSensorsList().then((sensors) => {
            this.sensors = sensors;

            this.sensors.forEach(sensor => {
                sensor.yAxisID = this.yAxes[0].id;
            });
        });
    },
    template: `
    <div class="container">

        <div class="item header">Chart</div>
        
        <div class="item chart">
            <canvas ref=chart style="height: 100%; width: 100%" ></canvas>
        </div>

        <sensor-list class="item sensor-list"
            :sensors="sensors"
            :yAxes="yAxes"
            ></sensor-list>

        <yAxe-list :yAxes="yAxes" v-on:add-axe="addAxe" class="item yAxe-list"></yAxe-list>

        <div class="item date-selection">
            <p>
                <label>Start date: </label>
                <input type="date" v-model="startDate" :max="endDate">
            </p>
            <p>
                <label>End date:   </label>
                <input type="date" v-model="endDate" :max="todayDate" :min="startDate">
            </p>
            <p>
                <button @click='updateChart' >Update plage</button>
            </p>
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
    },
    computed: {

    }
});


var app = new Vue({
    el: '#chartPresenter'
});