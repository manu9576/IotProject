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
        <legend>Sélection des capteurs à afficher</legend>
        <div id="sensors-table">
            <table>
            
            <tr>
                <th>Nom du capteur</th>
                <th>Unité</th>
                <th>Affichage</th>
                <th>Couleur</th>
                <th>Axe-Y</th>
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
        <td style="text-align: left;">{{sensor.label}}</td>
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
    <fieldset class='y-configuration'>
        <legend>Configuration des axes Y</legend>
        <yAxe-detail 
            v-for="(yAxe) in yAxes" 
            :key="yAxe.id" 
            :yAxe="yAxe"
            v-on:updateRequest="updateRequest">
        </yAxe-detail>
    </fieldset>
    `,
    methods: {
        updateRequest() {
            this.$emit('updateRequest');
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
            visible: true,
            min: 0,
            max: 25
        };
    },
    mounted() {
        this.visible = this.yAxe.display;
        this.min = this.yAxe.ticks.min;
        this.max = this.yAxe.ticks.max;
    },
    methods: {
        switchRangeMode() {

            if (this.hasAutoLimit) {
                this.min = 0;
                this.max = 25;
            } else {
                this.min = undefined;
                this.max = undefined;
            }
        },

        updateRequest() {
            this.$nextTick(() => this.$emit("updateRequest"));
        }
    },
    computed: {
        buttonText() {
            if (this.hasAutoLimit) {
                return 'Plage manuel';
            } else {
                return 'Plage automatique';
            }
        },
        hasAutoLimit() {
            return this.min === undefined && this.max === undefined;
        }
    },
    watch:{
        visible(){
            this.yAxe.display = this.visible;
            this.$emit("updateRequest")
        },
        min(){
            this.yAxe.ticks.min = this.min;
            this.$emit("updateRequest")
        },
        max(){
            this.yAxe.ticks.max = this.max;
            this.$emit("updateRequest")
        }
    },
    template: `
    <div class="y-axe">
        <label class="axe-name">{{yAxe.labelString}}</label>
        <input class="axe-visibility-checkbox" type='checkbox' v-model="visible">
        <label class="axe-visibility-label" :for="yAxe.id">visible</label> 
        <button @click='switchRangeMode' class="button axe-range-selection">{{buttonText}}</button>
        <input class="axe-min" v-if="!hasAutoLimit" v-model.number="min" type="number">
        <input class="axe-max" v-if="!hasAutoLimit" v-model.number="max" type="number">
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
            yAxes: [],
            promises: []
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
    watch: {
        endDate() {
            this.updateChart();
        },
        startDate() {
            this.updateChart();
        }
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
                :yAxes="yAxes"
                v-on:updateRequest="updateChart">
            </sensor-list>

            <yAxe-list 
                id="yAxes-config"
                :yAxes="yAxes" 
                v-on:add-axe="addAxe"
                v-on:updateRequest="updateChart">
            </yAxe-list>

            <fieldset id="xAxes-config">
                <legend>Configuration de l'axe X</legend>
                <label>Du : </label>
                <input type="date" v-model="startDate" :max="endDate">
                <label>  au : </label>
                <input type="date" v-model="endDate" :max="todayDate" :min="startDate">
            </fieldset>
 
            <div id="update-chart">
                <button class="button" @click='updateChart' >Actualiser le graphique</button>
            </div>

        </div>
    </div>
    `,
    methods: {
        updateChart() {
            this.chartHelper.clearDatasets();

            //TODO: view how to cancel old promises before making new ones           

            this.promises = [];

            this.sensors.forEach(sensor => {
                if (sensor.isSelected) {
                    this.promises.push(this.dataRetriever.getValuesForInterval(sensor.id, this.startDate, this.endDate).then((values) => {
                        sensor.data = values;
                        this.chartHelper.addDataSet(sensor);
                    }));
                }
            });

            Promise.all(this.promises).then((values) => {
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

var chart = new Vue({
    el: '#chartPresenter'
});


Vue.component('last-values-presenter', {
    data() {
        return {
            dataRetriever: new DataRetriever(),
            sensors: []
        }
    },
    props: {
        sensors: {
            type: Array,
            require: true
        },
    },
    mounted() {
        this.dataRetriever.getSensorsList().then((sensors) => {

            this.sensors = sensors;

            this.refreshSensorsValue();
        });
    },
    methods: {
        refreshSensorsValue() {
            this.sensors.forEach(sensor => {
                this.dataRetriever.getLastValue(sensor).then((value) => {
                    sensor.lastValue = value;
                });
            });
        }
    },
    template: `
    <div class="item" id="last-value-list">
        <last-value v-for="(sensor) in sensors" :key="sensor.id" :sensor='sensor'></last-value>
        <button class="button" @click="refreshSensorsValue">Rafraîchir</button>
    </div>
    `
});

Vue.component('last-value', {

    props: ['sensor'],

    computed: {
        getValue() {
            if (this.sensor.lastValue == undefined) {
                return "-";
            }
            // to ensure things like 1.05 round correctly, we use 
            return Math.round((this.sensor.lastValue + Number.EPSILON) * 10) / 10;
        }
    },

    template: `
    <div>
        <label>{{sensor.label}}: {{getValue}} {{sensor.unit}}</label>
    </div>
    `
});

var lastValue = new Vue({
    el: '#lastValuePresenter'
});