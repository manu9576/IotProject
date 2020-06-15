Vue.component('graph-configuration', {
    data() {
        return {
            tabs: ['Reviews', 'Make a Review'],
            selectedTab: 'Reviews'
        };
    },
    template: `
    <div >
          
        <fieldset   fieldset  id='sensorsList'>
            <sensor-list></sensor-list>
        </fieldset>

        <input id="startDate" type="date" :value="oneWeekEarlier" :max="todayDate">
        <input id="endDate" type="date" :value="todayDate" :max="todayDate">
        <button id='updatePlageButton' v-on:click="updateGraph">Update plage</button>

    </div>
    `,
    methods: {
        updateGraph: ()=>{
            
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
    data: {
        chart: document.getElementById('sensorChart'),
        chartHelper: new ChartHelper(document.getElementById('sensorChart')),
        dataRetriever: new DataRetriever()
    },
    provide: {
        dataRetriever: this.dataRetriever
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