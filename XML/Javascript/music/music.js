// Put your Last.fm API key here
// Student Name: Prawal Sharma
var api_key = "82542b99f6d0822c9052ec7ceb8937e3";

var request1 = new XMLHttpRequest();
var request2 = new XMLHttpRequest();
var request3 = new XMLHttpRequest();

function displayResult () {
    var albumlist = new Array();
	var albumpic = new Array();
	var eventlist = new Array();
	
	if (request1.readyState == 4 && request2.readyState == 4 && request3.readyState == 4) {
	
		var json1 = JSON.parse(request1.responseText);
		var json2 = JSON.parse(request2.responseText);
		var json3 = JSON.parse(request3.responseText);
		
		//About Artist
		document.getElementById("output1").innerHTML = "<h3><a href='"+json1.artist.url +"'>"+ json1.artist.name+"</a></h3><p>"+json1.artist.bio.summary+"</p>";
		document.getElementById("opImages").innerHTML = "<img src='" + json1.artist.image[3]["#text"] + "' alt='Pic'/>";	
		
		//For Upcoming Events
		var str1 = JSON.stringify(json2,undefined,2);
		
			//for(var i=0;i<json2.length;i++){
			for(k in json2.events.event){	
				tempurl = JSON.stringify(json2.events.event.url,undefined,2);
				tempevent = JSON.stringify(json2.events.event.title,undefined,2);
				eventlist[tempurl] = tempevent;
			}
			for(var url in eventlist){
				document.getElementById("output2").innerHTML +="<a href='"+url+"'>"+eventlist[url]+"</a><br/>";
			}
		
		//For Top Albums
		var index = 0;
		var str2 = JSON.stringify(json3,undefined,2);
		
			for(var j=0;j<json3.topalbums.album.length;j++){
				tempAlbum = JSON.stringify(json3.topalbums.album[j].name,undefined,2);
				tempImg = JSON.stringify(json3.topalbums.album[j].image[1]["#text"],undefined,2);
				tempAlbumUrl = JSON.stringify(json3.topalbums.album[j].url);
				albumlist[tempAlbumUrl] = tempAlbum;
				albumpic[index] = tempImg;
				index++;
			}
			index = 0;
			for(var aUrl in albumlist){
				document.getElementById("output3").innerHTML += "<img src= "+ albumpic[index]+" alt=pic /><a href='"+aUrl+"'>"+albumlist[aUrl]+"</a><br/>";
				index++;
			}
		
	}
}

function sendRequest () {
	//To clear page div 
	document.getElementById("output3").innerHTML = "";
	document.getElementById("output1").innerHTML = "";
	document.getElementById("output2").innerHTML = "";
	document.getElementById("opImages").innerHTML = "";

    var method1 = "artist.getinfo";
	var method2 = "artist.getEvents";
	var method3 = "artist.getTopAlbums";
    
	request1.onreadystatechange = displayResult;
    request2.onreadystatechange = displayResult;
	request3.onreadystatechange = displayResult;
	
	var artist = document.getElementById("form-input").value;
	
    request1.open("GET","proxy.php?method="+method1+"&artist="+artist+"&api_key="+api_key+"&format=json",true);
	request2.open("GET","proxy.php?method="+method2+"&artist="+artist+"&api_key="+api_key+"&format=json",true);
	request3.open("GET","proxy.php?method="+method3+"&artist="+artist+"&api_key="+api_key+"&format=json",true);
    
	request1.withCredentials = "true";
    request2.withCredentials = "true";
	request3.withCredentials = "true";
	
	request1.send(null);
	request2.send(null);
	request3.send(null);
}
