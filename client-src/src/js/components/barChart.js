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
