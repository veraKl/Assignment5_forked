
// Step 1: Set up the Leaflet map
const mymap = L.map('map').setView([47.7511, -120.7401], 7); // Example coordinates

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(mymap);

// Step 2: Load the GeoJSON data and add it to the map
fetch('../json/WA_County_Boundaries.geojson')
    .then(response => response.json())
    .then(data => {
        // Step 3: Handle the GeoJSON data
        addGeoJSONToMap(data);
    })
    .catch(error => console.error('Error loading GeoJSON:', error));

// Step 4: Define a function to add GeoJSON data to the map
function addGeoJSONToMap(data) {
    L.geoJSON(data, {
        style: function (feature) {
            return {
                fillColor: 'blue',
                weight: 2,
                color: 'white',
                fillOpacity: 0.5
            };
        }
    }).addTo(mymap);
}


