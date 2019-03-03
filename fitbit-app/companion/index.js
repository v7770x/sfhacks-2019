// Import the messaging module
import * as messaging from "messaging";


//fetch data from firebase
function callFirebase(hr, time) {
  var data = {"hr":hr, "time":time};
  fetch('https://sf-hacks-19.firebaseio.com/users/Bob/hr_time_data.json', {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", 
        headers: {
            "Content-Type": "application/json",
       
        },
        body: JSON.stringify(data), 
    })
  .then(function (response) {
      response.json()
      .then(function(data) {
 
      });
  })
  .catch(function (err) {
    console.log("Error fetching weather: " + err);
  });
  
  var data2 = {"val":hr}
   fetch('https://sf-hacks-19.firebaseio.com/users/Bob/curr_hr.json', {
        method: "PUT", // *GET, POST, PUT, DELETE, etc.
        mode: "cors",
        headers: {
            "Content-Type": "application/json",

        },
        body: JSON.stringify(data2), 
    })
  .then(function (response) {
      response.json()
      .then(function(data) {

      });
  })
  .catch(function (err) {
    console.log("Error putting: " + err);
  });
  
}

// Send the data to the device
function returnFBData(data) {
  if (messaging.peerSocket.readyState === messaging.peerSocket.OPEN) {
    // Send a command to the device
    messaging.peerSocket.send(data);
  } else {
    console.log("Error: Connection is not open");
  }
}

// Listen for messages from the device
messaging.peerSocket.onmessage = function(evt) {
  if (evt.data && evt.data.command == "data") {
    // The device requested weather data
    console.log(evt.data.hr)
    callFirebase(evt.data.hr, evt.data.time);
  }
}

// Listen for the onerror event
messaging.peerSocket.onerror = function(err) {
  // Handle any errors
  console.log("Connection error: " + err.code + " - " + err.message);
}