google.load("maps", "2");
// make a json request to get the map data from the Map action
$(function () {
    if (google.maps.BrowserIsCompatible()) {
        $.getJSON("/Home/Map", initialise);
    }
});
function initialise(mapData) {
    $("#mapName").text(mapData.Name);
    // create the map
    var map = new google.maps.Map2($("#map")[0]);
    map.addControl(new google.maps.SmallMapControl());
    map.addControl(new google.maps.MapTypeControl());
    map.setMapType(G_SATELLITE_MAP);
    var latlng = new google.maps.LatLng(mapData.LatLng.Latitude, mapData.LatLng.Longitude);
    var zoom = mapData.Zoom;
    map.setCenter(latlng, zoom);
    // set the marker for each location
    $.each(mapData.Locations, function (i, location) {
        setupLocationMarker(map, location);
    });
}
function setupLocationMarker(map, location) {
    // create a marker
    var latlng = new google.maps.LatLng(location.LatLng.Latitude, location.LatLng.Longitude);
    var marker = new google.maps.Marker(latlng);
    map.addOverlay(marker);
    // add a marker click event
    google.maps.Event.addListener(marker, "click", function (latlng) {

        // show the name and image on the page
        $("#info").text(location.Name);
        $("#image")[0].src = "../../Content/" + location.Image;

        // open the info window with the location name
        map.openInfoWindow(latlng, $("<p></p>").text(location.Name)[0]);
    });

}

