(function() {
  var chartsData = {};

  var paintData = function() {
    getData(function(loadedData) {
      chartsData = loadedData;
      gaugeVisualization(chartsData.primaryRating);
      barVisualization(chartsData.emotions);
      pieVisualization(chartsData.sex);
      photoVisualization(chartsData.emotions);
    });
  };

  paintData();

  var refreshBtn = document.getElementById('refresh-btn');
  refreshBtn.addEventListener('click', paintData);

  polling (paintData);
}());
