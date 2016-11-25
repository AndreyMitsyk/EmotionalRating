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
