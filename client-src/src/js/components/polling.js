function polling(callback, url) {
  url = url || "/api/update";

  var version = 0;
  var xhr = new XMLHttpRequest();
  xhr.onreadystatechange = function() {
    if (xhr.readyState == 4) {
        if(xhr.status == 200) {
            var newVersion = parseInt(xhr.responseText);
            if(Number.isNaN(newVersion)){
              console.info("Version is NaN!");
              version++;
            }else{
                version = newVersion;
            }
            console.info("Version" + version);
            callback();
         }
    }
  }
  
  setInterval(function(){
  				xhr.open("GET", url, true);
  				xhr.setRequestHeader("Version", version);
  				xhr.send();
  }, 1000);
}