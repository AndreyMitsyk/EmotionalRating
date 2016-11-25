/*
 Third party
 */


/*
    Custom
 */
 (function() {
'use strict';

// Gauge chart
// Enter a speed between 0 and 180
var gaugeVisualization = function(value) {
  function gaugePath(level) {
    // Trig to calc meter point
    var degreesLevel = 180 * level / 100;
    var degrees = 180 - degreesLevel,
         radius = .5;
    var radians = degrees * Math.PI / 180;
    var x = radius * Math.cos(radians);
    var y = radius * Math.sin(radians);

    // Path: may have to change to create a better triangle
    var mainPath = 'M -.0 -0.025 L .0 0.025 L ',
         pathX = String(x),
         space = ' ',
         pathY = String(y),
         pathEnd = ' Z';

    return mainPath.concat(pathX,space,pathY,pathEnd);
  }

  var gaugeData = [{ type: 'scatter',
      x: [0], y:[0],
      marker: {size: 28, color:'850000'},
      showlegend: false,
      name: 'rating',
      text: value,
      hoverinfo: 'text+name'},
    { values: [50/6, 50/6, 50/6, 50/6, 50/6, 50/6, 50],
    rotation: 90,
    text: ['THE BEST', 'Avesome', 'Good', 'Normal',
              'Useless', 'Disgusting', ''],
    textinfo: 'text',
    textposition:'inside',
    marker: {colors:['rgba(14, 127, 0, .5)', 'rgba(110, 154, 22, .5)',
                           'rgba(170, 202, 42, .5)', 'rgba(202, 209, 95, .5)',
                           'rgba(210, 206, 145, .5)', 'rgba(232, 226, 202, .5)',
                           'rgba(255, 255, 255, 0)']},
    labels: ['84-100', '67-83', '51-66', '34-50', '17-33', '0-16', ''],
    hoverinfo: 'none',
    hole: .5,
    type: 'pie',
    showlegend: false
  }];

  var gaugeLayout = {
    shapes:[{
        type: 'path',
        path: gaugePath(value),
        fillcolor: '850000',
        line: {
          color: '850000'
        }
      }],
    title: 'Favorite emotion',
    xaxis: {zeroline:false, showticklabels:false,
               showgrid: false, range: [-1, 1]},
    yaxis: {zeroline:false, showticklabels:false,
               showgrid: false, range: [-1, 1]}
  };

  Plotly.newPlot('gauge-chart', gaugeData, gaugeLayout);
};
// Bar chart
var barVisualization = function(values) {
  var emotionNames = [];
  var emotionCounts = [];

  for (var key in values) {
    emotionNames.push(key);
    emotionCounts.push(values[key].value);
  }

  var barTrace1 = {
    x: emotionNames,
    y: emotionCounts,
    marker: {
      color: 'rgb(158,202,225)',
      opacity: 0.6,
      line: {
        color: 'rbg(8,48,107)',
        width: 1.5
      }
    },
    type: 'bar'
  };

  var barData = [barTrace1];

  var barLayout = {
    title: 'Emotions'
  };

  Plotly.newPlot('bar-chart', barData, barLayout);
};
// pie-chart
var pieVisualization = function(value) {
  var pieData = [{
    values: [value, 100 - value],
    labels: ['male', 'female'],
    hole: .4,
    type: 'pie',
    hoverinfo: 'label+percent'
  }];

  var pieLayout = {
    title: 'Statistic'
  };

  Plotly.newPlot('pie-chart', pieData, pieLayout, {showLink: false});
};
var getData = function(callback) {
  var DATA_URL = window.location.href + 'api/data';
  var dashboards = document.querySelector('.dashboards');
  var xhr = new XMLHttpRequest();

  dashboards.classList.add('data-loading');

  xhr.onload = function(evt) {
    dashboards.classList.remove('data-loading');

    if (xhr.status !== 200) {
      dashboards.classList.add('data-failure');
    } else {
      var loadedData = JSON.parse(evt.target.response);
      callback(loadedData);
    }
  };

  xhr.onerror = function() {
    dashboards.classList.remove('data-loading');
    dashboards.classList.add('data-failure');
  };

  xhr.timeout = 10000;
  xhr.ontimeout = function() {
    dashboards.classList.remove('data-loading');
    dashboards.classList.add('data-failure');
  };

  xhr.open('GET', DATA_URL);
  xhr.send();
};
(function() {
  var chartsData = {};

  var paintData = function() {
    getData(function(loadedData) {
      chartsData = loadedData;
      gaugeVisualization(chartsData.primaryRating);
      barVisualization(chartsData.emotions);
      pieVisualization(chartsData.sex);
    });
  };

  paintData();

  var refreshBtn = document.getElementById('refresh-btn');
  refreshBtn.addEventListener('click', paintData);

  // создать подключение
  var socket = new WebSocket("ws://localhost:8081");

  // обработчик входящих сообщений
  socket.onmessage = function(event) {
    console.log(event.data);
    // paintData();
  };

  socket.onopen = function() {
    console.log('Open connection');
  };

  socket.onclose = function() {
    console.log('Close connection');
  };
}());


}());