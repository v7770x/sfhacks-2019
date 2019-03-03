// Import the messaging module
import { Accelerometer } from "accelerometer";
import { Barometer } from "barometer";
import { BodyPresenceSensor } from "body-presence";
import document from "document";
import { Gyroscope } from "gyroscope";
import { HeartRateSensor } from "heart-rate";
import { OrientationSensor } from "orientation";
import * as messaging from "messaging";
import clock from "clock";

// clock.granularity = "seconds";
// // var curr_time = 0;
// clock.ontick = (evt) => {
//    curr_time = evt.date.toTimeString();
//    // console.log(Date.now())
  
// }




//get heartrate data
const hrm = new HeartRateSensor();
hrm.start();

const hrmData = document.getElementById("hrm-data");


function refreshData() {
  const data = {
    hrm: {
      heartRate: hrm.heartRate ? hrm.heartRate : 0
    }
  }

  hrmData.text = JSON.stringify(data.hrm);
  console.log(hrmData.text);
}

refreshData();
// setInterval(refreshData,1000);






// Request data from the companion
function fetchData() {
  if (messaging.peerSocket.readyState === messaging.peerSocket.OPEN) {
    // Send a command to the companion
    const data = {
        hrm: {
          heartRate: hrm.heartRate ? hrm.heartRate : 0
        }
      }
    hrmData.text = JSON.stringify(data.hrm);
    console.log(hrmData.text);
    var today = new Date();
    var curr_time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    console.log(curr_time);
    // console.log(data.hrm);
    messaging.peerSocket.send({
      command: 'data',
      hr:data.hrm,
      time:curr_time
    });
  }
}

// Display the weather data received from the companion
function processWeatherData(data) {
  console.log(data);
}

// Listen for the onopen event
messaging.peerSocket.onopen = function() {
  // Fetch weather when the connection opens
  fetchData();
}

// Listen for messages from the companion
messaging.peerSocket.onmessage = function(evt) {
  if (evt.data) {
    processWeatherData(evt.data);
  }
}

// Listen for the onerror event
messaging.peerSocket.onerror = function(err) {
  // Handle any errors
  console.log("Connection error: " + err.code + " - " + err.message);
}

// Fetch the weather every 30 minutes
setInterval(fetchData, 1000);