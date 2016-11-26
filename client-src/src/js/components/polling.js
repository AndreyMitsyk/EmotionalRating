function polling(url, version, callback) {
  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function() {
    if (xhr.readyState == 4) {
        if(xhr.status == 200) {
            var newVersion = parseInt(xhr.responseText);
            if(Number.isNaN(newVersion)){
                polling(url, version+1, callback);
            }else{
                polling(url, newVersion, callback);
            }
            callback();
         }
         if(xhr.status == 304){
             polling(url, version, callback);
         }
    }
  }
  xhr.open("GET", url, true);
  xhr.setRequestHeader("Version", version);
  xhr.send();
}