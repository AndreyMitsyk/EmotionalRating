function polling(callback, url, version) {
  url = url || "api/update";

  if(version == undefined){
      version = 0;
  }
  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function() {
    if (xhr.readyState == 4) {
        if(xhr.status == 200) {
            var newVersion = parseInt(xhr.responseText);
            if(Number.isNaN(newVersion)){
                polling(callback, url, version+1);
            }else{
                polling(callback, url, newVersion);
            }
            callback();
         }
         if(xhr.status == 304){
             polling(callback, url, version);
         }
    }
  }
  xhr.open("GET", url, true);
  xhr.setRequestHeader("Version", version);
  xhr.send();
}
