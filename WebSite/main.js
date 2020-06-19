Vue.component('sensor-detail', {
    props: {
        sensor: {
            type: Object,
            require: true
        }
    },
    template: `
    <div>
        <input type='checkbox' v-model="sensor.isSelected" v-bind:id="sensor.id">
        <label>{{sensor.name}} ({{sensor.unit}})</label>   
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
        }
    },
    template: `
    <fieldset>
        <legend>Select sensors to display</legend>
        <sensor-detail  v-for="(sensor) in sensors" :key="sensor.id" :sensor="sensor"></sensor-detail>
    </fieldset>
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
            vm: this
        };
    },
    mounted() {
        this.chartHelper = new ChartHelper(this.$refs.chart);
        this.dataRetriever.getSensorsList().then((sensors) => {
            this.sensors = sensors;
        });
    },
    template: `
    <div class="container">

        <div class="item header">Chart</div>

        <canvas ref=chart class="item chart"></canvas>

        <sensor-list :sensors="sensors"  class="item sensor-list"></sensor-list>

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
                <button @click='updateChart(vm)' >Update plage</button>
            </p>
        </div>

    </div>
    `,
    methods: {
        updateChart: (vm) => {
            vm.chartHelper.clearDatasets();

            vm.sensors.forEach(sensor => {
                if (sensor.isSelected) {

                    vm.dataRetriever.getValuesForInterval(sensor.id, vm.startDate, vm.endDate).then((values) => {
                        vm.chartHelper.addDataSet(sensor.name, values);
                        vm.chartHelper.updateChart();
                    })
                }
            });
        },
        convertDate: (date) => {

            let dd = String(date.getDate()).padStart(2, '0');
            let mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = date.getFullYear();

            return yyyy + "-" + mm + "-" + dd;
        }
    },
    computed: {

    }
});


var app = new Vue({
    el: '#chartPresenter'
});