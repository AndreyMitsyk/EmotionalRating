// Bar chart
var barVisualization = function(values) {
  var emotionNames = values.map(function(emotion){
    return emotion.name;
  });

  var emotionCounts = values.map(function(emotion){
    return emotion.count;
  });

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
