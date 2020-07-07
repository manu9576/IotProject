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
        <div class="sensors-table">
            <table>
            
            <tr>
                <th>Senor name</th>
                <th>Unit</th>
                <th>Display</th>
                <th>Color</th>
                <th>Y-axe</th>
            </tr>

            <sensor-detail v-for="(sensor) in sensors" :key="sensor.id" :sensor="sensor" :yAxes="yAxes"></sensor-detail>

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
        <td><input type="color" v-model="sensor.borderColor"></td>
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
        switchToManualRange() {
            this.yAxe.ticks.min = 0;
            this.yAxe.ticks.max = 25;
            this.updateHasAutoLimit();
        },
        switchToAutoRange() {
            this.yAxe.ticks.min = undefined;
            this.yAxe.ticks.max = undefined;
            this.updateHasAutoLimit()
        },
        updateHasAutoLimit() {
            this.hasAutoLimit = this.yAxe.ticks === undefined || (this.yAxe.ticks.min === undefined && this.yAxe.ticks.max === undefined)
        }

    },

    template: `
    <div>
        <label>{{yAxe.labelString}} : </label>

        <input type='checkbox' v-model="yAxe.display" v-bind:id="yAxe.id">
        <label :for="yAxe.id">visible</label> 

        <a> - </a>

        <a v-if="hasAutoLimit">
            <button @click='switchToManualRange'>Manual range</button>
        </a>
        <a v-else>
            <button @click='switchToAutoRange'>Auto range</button>
            <input v-model.number="yAxe.ticks.min" type="number">
            <input v-model.number="yAxe.ticks.max" type="number">
        </a>
        
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
    <div class="container">

        <div class="item header">Chart</div>
        
        <div class="item chart">
            <canvas ref=chart style="height: 100%; width: 100%" ></canvas>
        </div>

        <sensor-list class="item sensors-list"
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
                <button @click='updateChart' >Update chart</button>
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
    }
});

var app = new Vue({
    el: '#chartPresenter'
});