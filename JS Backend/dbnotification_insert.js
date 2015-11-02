function insert(item, user, request) {

    //send push notification
    var tags = [item.AccountId];
    var payload = '{ "message" : "dbNotification,'+item.Text+'" }';
    sendPush(tags,payload,request);

    request.execute();

}

//PUSH NOTIFICATION
 function sendPush(tags,payload,request){
      // Write the default response and send a notification
      // to all platforms.            
      push.send(tags, payload, {               
          success: function(pushResponse){
          console.log("Sent push:", pushResponse);
          // Send the default response.
          request.respond();
          },              
          error: function (pushResponse) {
              console.log("Error Sending push:", pushResponse);
               // Send the an error response.
              request.respond(500, { error: pushResponse });
              }           
       });    
  }