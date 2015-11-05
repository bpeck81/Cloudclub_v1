  var memberJunctionTable = tables.getTable("MemberJunction");

function insert(item, user, request) {

    item.Time = new Date();
    request.execute({
     success: function() {                      
      var payload = '{ "message" : "clubRequest,' + item.id + '" }';     
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