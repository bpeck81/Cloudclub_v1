  var memberJunctionTable = tables.getTable("MemberJunction");
  
  function insert(item, user, request) {
  // Execute the request and send notifications.
    item.Time = new Date();
     request.execute({
     success: function() {                      
      // Create a template-based payload.
      //comma separates type of notification from id of the associated class
      var payload = '{ "message" : "comment,' + item.id + '" }';   
      
      //set the tags to the accountids of whoever the notification is meant for         
      var tags = [];
      
      //send push notification to all users in club
      memberJunctionTable.where({ClubId: item.ClubId}).read({
          success: function(memberships){
              for(var i =0;i<memberships.length;i++){
                  tags.push(memberships[i].AccountId);
              }
              sendPush(tags,payload,request);
          }
      });
      
            
      }
   });   
  }
  
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