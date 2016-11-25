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

  xhr.timeout = 5000;
  xhr.ontimeout = function() {
    dashboards.classList.remove('data-loading');
    dashboards.classList.add('data-failure');
  };

  xhr.open('GET', DATA_URL);
  xhr.send();
};
