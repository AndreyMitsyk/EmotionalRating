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
    var degrees = 180 - level,
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
      name: 'speed',
      text: value,
      hoverinfo: 'text+name'},
    { values: [50/6, 50/6, 50/6, 50/6, 50/6, 50/6, 50],
    rotation: 90,
    text: ['TOO FAST!', 'Pretty Fast', 'Fast', 'Average',
              'Slow', 'Super Slow', ''],
    textinfo: 'text',
    textposition:'inside',
    marker: {colors:['rgba(14, 127, 0, .5)', 'rgba(110, 154, 22, .5)',
                           'rgba(170, 202, 42, .5)', 'rgba(202, 209, 95, .5)',
                           'rgba(210, 206, 145, .5)', 'rgba(232, 226, 202, .5)',
                           'rgba(255, 255, 255, 0)']},
    labels: ['151-180', '121-150', '91-120', '61-90', '31-60', '0-30', ''],
    hoverinfo: 'label',
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
(function() {
  // Bar chart
  var barVisualization = function(values) {
    var barTrace1 = {
      x: ['Feature A', 'Feature B', 'Feature C', 'Feature D', 'Feature E'],
      y: values,
      marker:{
        color: ['rgba(204,204,204,1)', 'rgba(222,45,38,0.8)', 'rgba(204,204,204,1)', 'rgba(204,204,204,1)', 'rgba(204,204,204,1)']
      },
      type: 'bar'
    };

    var barData = [barTrace1];

    var barLayout = {
      title: 'Emotions'
    };

    Plotly.newPlot('bar-chart', barData, barLayout);
  };

  barVisualization([20, 14, 23, 25, 22]);

  var refreshBtn = document.getElementById('refresh-bar');
  refreshBtn.addEventListener('click', refreshBar);

  function refreshBar() {
    barVisualization([2.5, 5.6, 10.1, 15, 25]);
  };
}());
// pie-chart
var pieVisualization = function(value) {
  var pieData = [{
    values: [value, 100 - value],
    labels: ['male', 'female'],
    hole: .4,
    type: 'pie'
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

  xhr.timeout = 1000;
  xhr.ontimeout = function() {
    dashboards.classList.remove('data-loading');
    dashboards.classList.add('data-failure');
  };

  xhr.open('GET', DATA_URL);
  xhr.send();
};
(function() {
  var chartsData = {};

  getData(function(loadedData) {
    chartsData = loadedData;
    gaugeVisualization(chartsData.primaryRating);
    pieVisualization(chartsData.sex);
  });

  // var refreshBtn = document.getElementById('refresh-gauge');
  // refreshBtn.addEventListener('click', refreshBar);

  // function refreshBar() {
  //   gaugeVisualization(170);
  // };

  // var refreshBtn = document.getElementById('refresh-pie');
  // refreshBtn.addEventListener('click', refreshPie);

  // function refreshPie() {
  //   pieVisualization([50, 40, 10]);
  // };
}());


}());