// Put your zillow.com API key here
//Name: Prawal Sharma
//UTA ID: 1001104720

var zwsid = "X1-ZWz1b3jca2xibv_2doog";
var request = new XMLHttpRequest();
var request1 = new XMLHttpRequest();
var log = new Array();
var map;
var geocoder;
var tempAddress;
var tempAmount;
var tolat;
var tolng;

function initialize () {
	var mapCanvas = document.getElementById('map_canvas');
    geocoder = new google.maps.Geocoder();
	var mapOptions = {
          center: new google.maps.LatLng(32.75, -97.13),
          zoom: 16
    }
	map = new google.maps.Map(mapCanvas, mapOptions);
	google.maps.event.addDomListener(window, 'load', initialize);
	google.maps.event.addListener(map, 'click', function(event) {
		reverseGeoCodeAddress(event.latLng.lat(), event.latLng.lng());
	});
}
//Retrive Geocoding response from zillow
function displayResult () {
	if (request.readyState == 4) {
		var xml = request.responseXML.documentElement;
			try{
				//Finding Address & Amount From Zillow
				var value = xml.getElementsByTagName("zestimate")[0].getElementsByTagName("amount")[0];
				houseAmount = value.childNodes[0].nodeValue;
				var streetA = xml.getElementsByTagName("address")[1].getElementsByTagName("street")[0];
				var street = streetA.childNodes[0].nodeValue;
				var zipcodeA = xml.getElementsByTagName("address")[1].getElementsByTagName("zipcode")[0];
				var zipcode = zipcodeA.childNodes[0].nodeValue;
				var cityA = xml.getElementsByTagName("address")[1].getElementsByTagName("city")[0];
				var city = cityA.childNodes[0].nodeValue;
				var stateA = xml.getElementsByTagName("address")[1].getElementsByTagName("state")[0];
				var state = stateA.childNodes[0].nodeValue;
				var address = street + " " + city + " " + state + " " + zipcode;
				tempAddress = address; //For constructing array (multiple result)
				tempAmount = houseAmount;
				log[tempAmount] = tempAddress;
			}catch(e){
				alert("Address not found. Enter Correct credentials");
			}
		//Call marker on map
		document.getElementById("output").innerHTML = "";
		for(var i in log){
			document.getElementById("output").innerHTML += log[i]+": $" + i + "<br/>";
		}
		geoCodeAddress(address, houseAmount);
	}
}
//Retrieve ReverseGeocoding response from zillow
function displayResult1(){
	if(request1.readyState == 4){
		var xml1 = request1.responseXML.documentElement;
			try{
				var streetA1 = xml1.getElementsByTagName("address")[1].getElementsByTagName("street")[0];
				var street1 = streetA1.childNodes[0].nodeValue;
				var zipcodeA1 = xml1.getElementsByTagName("address")[1].getElementsByTagName("zipcode")[0];
				var zipcode1 = zipcodeA1.childNodes[0].nodeValue;
				var cityA1 = xml1.getElementsByTagName("address")[1].getElementsByTagName("city")[0];
				var city1 = cityA1.childNodes[0].nodeValue;
				var stateA1 = xml1.getElementsByTagName("address")[1].getElementsByTagName("state")[0];
				var state1 = stateA1.childNodes[0].nodeValue;
				var address1 = street1 + " " + city1 + " " + state1 + " " + zipcode1;
				var value1 = xml1.getElementsByTagName("zestimate")[0].getElementsByTagName("amount")[0];
				var latLng = new google.maps.LatLng(tolat, tolng);
				houseAmount1 = value1.childNodes[0].nodeValue;
				tempAddress = address1;
				tempAmount = houseAmount1;
				log[tempAmount] = tempAddress;
				//For Customized Markers: using googleapis
				var url = 'https://chart.googleapis.com/chart?chst=d_bubble_icon_texts_big&chld=home|bb|FFB573|000000|$'+houseAmount1+'|'+address1;
				var markershape ={
					url: url,
					type: 'poly'
				};
				var marker = new google.maps.Marker({
					map: map,
					title: address1,
					icon: markershape,
					position: latLng
				});
				marker.setMap(map);
				map.setCenter(latLng);
			}catch(e){
				alert("No Exact address not found. Please click on a different location");
			}
			document.getElementById("output").innerHTML = "";
			for(var i in log){
				document.getElementById("output").innerHTML +=  log[i] + ": $" + i + "<br/>";
			}
	}
}
//Geocoding Request to Zillow
function sendRequest () {
	request.onreadystatechange = displayResult;
    var inputAddress = document.getElementById("address").value;
	var splitPoint = inputAddress.indexOf(",");
	var address = inputAddress.substr(0, splitPoint);
	var cityStateZip = inputAddress.substr(splitPoint+1);
	request.open("GET","proxy.php?zws-id="+zwsid+"&address="+address+"&citystatezip="+cityStateZip);
    request.withCredentials = "true";
    request.send(null);
}
//Display geocoding result from Zillow to Map
function geoCodeAddress(address, houseAmount){
	//Map marker geocoding
	geocoder.geocode({'address': address }, function(results, status){
		if(status == google.maps.GeocoderStatus.OK){
			var myLat = results[0].geometry.location;
			var url = 'https://chart.googleapis.com/chart?chst=d_bubble_icon_texts_big&chld=home|bb|FFB573|000000|$'+houseAmount+'|'+address;
			var markershape ={
				url: url,
				type: 'poly'
			};
			var marker = new google.maps.Marker({
				map: map,
				title: address,
				icon: markershape,
				position: results[0].geometry.location
			});
			marker.setMap(map);
			map.setCenter(myLat);
			
		}else{
			alert("Location Not Found");
		}
	});
}
//Reverse Geocoding request to Zillow from click
function reverseGeoCodeAddress(Lat, Lng){
	var latLng = new google.maps.LatLng(Lat, Lng);
	tolat = Lat;
	tolng = Lng;
	geocoder.geocode({'latLng': latLng}, function(results, status){
		if(status == google.maps.GeocoderStatus.OK){
			if(results[1]){
				request1.onreadystatechange = displayResult1;
				var inputAddress1 = results[1].formatted_address;
				//, US in the request gives incorrect results in response from Zillow. Omit (, USA) (country) after third ,
				var splitPoint1 = inputAddress1.indexOf(",");
				var address1 = inputAddress1.substr(0, splitPoint1);
				var cityStateZip1 = inputAddress1.substr(splitPoint1+1);
				var minicitysp = cityStateZip1.indexOf(",");
				var minicity = cityStateZip1.substr(0, minicitysp);
				var minicity2 = cityStateZip1.substr(minicitysp + 1);
				var sp3 = minicity2.indexOf(",");
				var sta = minicity2.substr(0, sp3);
				
				request1.open("GET","proxy.php?zws-id="+zwsid+"&address="+address1+"&citystatezip="+minicity + sta);
				request1.withCredentials = "true";
				request1.send(null);
			}
		}
	});
}
//Clear Button
function clearmap(){
	var mapCanvas = document.getElementById('map_canvas');
    geocoder = new google.maps.Geocoder();
	var mapOptions = {
          center: new google.maps.LatLng(32.75, -97.13),
          zoom: 16
    }
	map = new google.maps.Map(mapCanvas, mapOptions);
	google.maps.event.addDomListener(window, 'load', initialize);
	document.getElementById("output").innerHTML = " ";
}

//Reference: Google maps geocoding: https://developers.google.com/maps/documentation/javascript/geocoding?csw=1
//Google Markers: https://developers.google.com/maps/documentation/javascript/markers
//Custom Markers: https://developers.google.com/chart/infographics/docs/dynamic_icons?csw=1