(function() {
  var chartsData = {};

  getData(function(loadedData) {
    chartsData = loadedData;
    gaugeVisualization(chartsData.primaryRating);
    barVisualization(chartsData.emotions);
    pieVisualization(chartsData.sex);
  });

  var refreshBtn = document.getElementById('refresh-gauge');
  refreshBtn.addEventListener('click', refreshBar);

  function refreshBar() {
    gaugeVisualization(170);
  };

  // var refreshBtn = document.getElementById('refresh-bar');
  // refreshBtn.addEventListener('click', refreshBar);

  // function refreshBar() {
  //   barVisualization([2.5, 5.6, 10.1, 15, 25]);
  // };

  // var refreshBtn = document.getElementById('refresh-pie');
  // refreshBtn.addEventListener('click', refreshPie);

  // function refreshPie() {
  //   pieVisualization([50, 40, 10]);
  // };
}());
