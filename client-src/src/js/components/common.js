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

  var chat = document.getElementById('chat');

  var chatBtn = document.getElementById('chat-btn');
  chatBtn.addEventListener('click', function() {
    chat.classList.add('show');
  });

  document.addEventListener('click', function(evt) {
    if(chat.classList.contains('show') && evt.target != chat && evt.target != chatBtn) {
      chat.classList.remove('show');
    }
  });

  polling (paintData);
}());
