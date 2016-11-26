function polling(url, callback, version) {
  if(!version){
      version=0;
  }
  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function() {
    if (xhr.readyState == 4) {
        if(xhr.status == 200) {
            var newVersion = parseInt(xhr.responseText);
            if(Number.isNaN(newVersion)){
                polling(url, callback, version+1);
            }else{
                polling(url, callback, newVersion);
            }
            callback();
         }
         if(xhr.status == 304){
             polling(url, callback, version);
         }
    }
  }
  xhr.open("GET", url, true);
  xhr.setRequestHeader("Version", version);
  xhr.send();
}