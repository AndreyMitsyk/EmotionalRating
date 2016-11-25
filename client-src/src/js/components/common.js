(function() {
'use strict';


  // Gauge chart
  // Enter a speed between 0 and 180
  var gaugeLevel = 115;

  // Trig to calc meter point
  var gaugeDegrees = 180 - gaugeLevel,
       gaugeRadius = .5;
  var gaugeRadians = gaugeDegrees * Math.PI / 180;
  var gaugeX = gaugeRadius * Math.cos(gaugeRadians);
  var gaugeY = gaugeRadius * Math.sin(gaugeRadians);

  // Path: may have to change to create a better triangle
  var gaugeMainPath = 'M -.0 -0.025 L .0 0.025 L ',
       gaugePathX = String(gaugeX),
       gaugeSpace = ' ',
       gaugePathY = String(gaugeY),
       gaugePathEnd = ' Z';
  var gaugePath = gaugeMainPath.concat(gaugePathX,gaugeSpace,gaugePathY,gaugePathEnd);

  var gaugeData = [{ type: 'scatter',
     x: [0], y:[0],
      marker: {size: 28, color:'850000'},
      showlegend: false,
      name: 'speed',
      text: gaugeLevel,
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
        path: gaugePath,
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

  // Bar chart
  var barTrace1 = {
    x: ['Feature A', 'Feature B', 'Feature C', 'Feature D', 'Feature E'],
    y: [20, 14, 23, 25, 22],
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

  // pie-chart
  var pieData = [{
    values: [16, 15, 12, 6, 5, 4, 42],
    labels: ['US', 'China', 'European', 'Russian', 'Brazil', 'India', 'Rest of World' ],
    hole: .4,
    type: 'pie'
  }];

  var pieLayout = {
    title: 'Statistic'
  };

  Plotly.newPlot('pie-chart', pieData, pieLayout);
}());
