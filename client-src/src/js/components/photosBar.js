// photos-bar
var photoVisualization = function(values) {
  var photoContainer = document.getElementById('photos-bar');
  var container = document.createDocumentFragment();

  for (var key in values) {
    var img = new Image(100);
    img.src = values[key].url;
    container.appendChild(img);
  }

  photoContainer.innerHTML = '';
  photoContainer.appendChild(container);
};
