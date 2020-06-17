Vue.component('sensor-detail', {
    props: ["sensor"],
    template: `
    <div>
        <input type='checkbox' :id="sensor.id">
        <label>{{sensor.name}} ({{sensor.unit}})</label>   
    </div>
    `
});


Vue.component('sensor-list', {
    props: ['sensors'],
    template: `
    <fieldset>
        <sensor-detail  v-for="(sensor) in sensors" :key="sensor.sensorId" :sensor="sensor"></sensor-detail>
    </fieldset>
    `
});


Vue.component('graph-configuration', {
    data() {
        return {

        };
    },
    props: ['sensors'],
    template: `
    <div >

        <sensor-list :sensors="sensors"></sensor-list>
       
        <input id="startDate" type="date" :value="oneWeekEarlier" :max="todayDate">
        <input id="endDate" type="date" :value="todayDate" :max="todayDate">
        
    </div>
    `,
    computed: {
        todayDate() {
            let today = new Date();
            let dd = String(today.getDate()).padStart(2, '0');
            let mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            return yyyy + "-" + mm + "-" + dd;
        },

        oneWeekEarlier() {
            let date = new Date();
            date.setDate(date.getDate() - 7);
            let dd = String(date.getDate()).padStart(2, '0');
            let mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = date.getFullYear();

            return yyyy + "-" + mm + "-" + dd;
        }
    }
});



Vue.component('sensors-chart', {
    data() {
        return {
            chartHelper: undefined,
            dataRetriever: new DataRetriever(),
            sensors: []
        };
    },
    mounted() {
        this.chartHelper = new ChartHelper(this.$refs.chart);
        this.dataRetriever.getSensorsList().then((sensors) => {
            this.sensors = sensors;
        });

    },
    template: `
    <div >

        <canvas ref=chart></canvas>
       
        <graph-configuration :sensors="sensors"></graph-configuration>

        <button id='updatePlageButton'>Update plage</button>

    </div>
    `,
    methods: {

    },
    computed: {

    }
});


var app = new Vue({
    el: '#chartPresenter'
});