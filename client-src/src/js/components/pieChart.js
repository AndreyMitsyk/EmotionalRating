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
