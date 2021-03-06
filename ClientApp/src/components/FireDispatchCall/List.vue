<template>
  <div>
    <page-title title="Fire Dispatch Calls" />
    <b-row class="mb-3">
      <b-col>
        <b-card bg-variant="light">
          <b-card-text>
            <address-lookup :addressData.sync="addressData" :locationData.sync="locationData" />
          </b-card-text>
        </b-card>        
      </b-col>
    </b-row>
    <p class="small">This list contains fire dispatch calls as reported by the Milwaukee Fire Department. The data is updated constantly, but there's a lag
      of around 15-30 minutes between when the calls are made and when the data is available.
      <a href="https://itmdapps.milwaukee.gov/MFDCallData/index.jsp" target="_blank">More details are available here.</a>
    </p>
    <b-row>
      <b-col>
        <hr />
        <filtered-table :settings="tableSettings" :locationData="locationData" @rowClicked="onRowClicked">
        </filtered-table>
      </b-col>
    </b-row>
  </div>
</template>

<script>
import moment from 'moment'

export default {
  name: "FireDispatchCallList",
  props: {},
  data() {
    return {
      addressData: null,
      locationData: null,

      tableSettings: {
        endpoint: '/api/fireDispatchCall',
        columns: [
          {
            key: 'cfs',
            name: 'CFS',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'reportedDateTime',
            name: 'Date/Time',
            visible: true,
            sortable: true,
            filter: 'date'
          },
          {
            key: 'address',
            name: 'Address',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'city',
            name: 'City',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'natureOfCall',
            name: 'Nature of Call',
            visible: true,
            sortable: true,
            filter: 'text'
          },
          {
            key: 'disposition',
            name: 'Disposition',
            visible: true,
            sortable: true,
            filter: 'text'
          }
        ],
        defaultSortColumn: 'reportedDateTime',
        defaultSortOrder: 'desc',
        getDefaultFilter: function () {
        },
        getItemInfoWindowText: function (item) {
          let raw = item._raw;

          let time = moment(raw.reportedDateTime).format('llll');
          let fromNow = moment(raw.reportedDateTime).fromNow();

          let v = [];

          v.push('<div><span style="float: right;">');
          v.push(fromNow);
          v.push('</span>');
          v.push('<span style="font-size: 125%; font-weight: bold;">')
          v.push(raw.natureOfCall)
          v.push('</span></div>');

          v.push('<hr style="margin-top: 5px; margin-bottom: 5px;" />');

          v.push('<div><b>');
          v.push(raw.address);
          if (raw.apt) {
            v.push(' APT. #')
            v.push(raw.apt);
          }            
          v.push('</b></div>');

          v.push('<div>');
          v.push(time);
          v.push(' (<i>');
          v.push(fromNow);
          v.push('</i>)</div>' );

          v.push('<div>');
          v.push(raw.disposition);
          v.push('</div>');

          v.push('<div><a href="#/fireDispatchCall/');
          v.push(raw.cfs);
          v.push('">Details</a></div>');

          return v.join('');

          return '<p style="font-size: 150%; font-weight: bold;">' + raw.natureOfCall + '</p>' +
            raw.address + (raw.apt ? ' APT. #' + raw.apt : '') + '<hr />' +
            time + ' (' + fromNow + ')<br />' + 
            '<b><i>' + raw.disposition + '</i></b>' +
            '<hr />' +
            '<p style="font-size: 125%;"><a href="#/fireDispatchCall/' + raw.cfs + '">Details</a></p>';
        },
        getItemMarkerPosition: function (item) {
          if (!item || !item._raw || !item._raw.geometry)
            return null;

          return this.$store.getters.getGeometryPosition(item._raw.geometry);
        },
        getItemIcon: function (item) {
          return this.$store.getters.getFireDispatchCallTypeIcon(item._raw.natureOfCall);
        },
        getItemId: function (item) {
          return item._raw.cfs;
        }
      }
    }
  },
  methods: {
    onRowClicked: function (rawItem) {
      this.$router.push('/fireDispatchCall/' + rawItem.cfs);
    }
  },
  mounted () {
    this.$store.dispatch("loadFireDispatchCallTypes");
  }
};
</script>