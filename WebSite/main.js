Vue.component('graph-configuration', {
    inject: ['sensorsList'],
    template: `
    <fieldset   fieldset  id='sensorsList'>
        <ul>
            <li v-for="(sensor) in sensorsList" :key="index">
                <sensor-detail :sensor={sensor}></sensor-detail>
            </li>
        </ul>
    </fieldset>
    `
});


Vue.component('graph-configuration', {
    data() {
        return {

        };
    },
    template: `
    <div >

        <sensor-list></sensor-list>
       
        <input id="startDate" type="date" :value="oneWeekEarlier" :max="todayDate">
        <input id="endDate" type="date" :value="todayDate" :max="todayDate">
        <button id='updatePlageButton' v-on:click="updateGraph">Update plage</button>

    </div>
    `,
    methods: {
        updateGraph: () => {

        }
    },
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

var app = new Vue({
    el: '#chartPresenter',
    data: function () {
        return {
            chartHelper: new ChartHelper(document.getElementById('sensorChart')),
            dataRetriever: new DataRetriever(),
            sensorsList: []
        };
    },
    mounted: () => {
        // this.dataRetriever =

        
  let $vm = this;

        this.dataRetriever.getSensorsList().then((sensorsList) => {
            debugger;
            $vm.sensorsList = sensorsList;
        });
    },
    provide: function () {
        return {
            sensorsList: this.sensorsList
        }
    },
    methods: {
        updateCart(variantId) {
            this.cart.push(variantId);
        },
        removeFromCart(variantId) {
            const index = this.cart.indexOf(variantId);
            if (index > -1) {
                this.cart.splice(index, 1);
            }
        }
    }
});