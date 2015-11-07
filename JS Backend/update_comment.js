var memberJunctionTable = tables.getTable("MemberJunction");
var tempMembJuncTable = tables.getTable("TemporaryMemberJunction");

function update(item, user, request) {

    //if the number of droplets is divisible by 5 and greator than 0
    if(item.NumDroplets%5==0 && item.NumDroplets>0){
        var notificationTable = tables.getTable('DBNotification');
        //id is automatically added
        var notification = {
            Time: new Date(),
            AccountId: item.AuthorId,
            Type: "droplet",
            Text: 'Your post "'+item.Text+'" has received '+item.NumDroplets+' droplets!'
        };
        notificationTable.insert(notification);
        
        //send push notification
        var payload = '{ "message" : "droplet,' + item.id + '" }';         
        var tags = [item.AuthorId];
        sendPushNoReturn(tags,payload,request);
    }
    
    request.execute({success:function(){
        // Create a template-based payload.
        //comma separates type of notification from id of the associated class
        var payload = '{ "message" : "like,' + item.id + '" }';   
        //set the tags to the accountids of whoever the notification is meant for         
        var tags = [];
        
        //send push notification to all temporary users in club
          tempMembJuncTable.where({ClubId: item.ClubId}).read({
              success: function(memberships){
                  for(var i =0;i<memberships.length;i++){
                      tags.push(memberships[i].AccountId);
                  }
                  //sendPush(tags,payload,request);
              }
          });
        
        //send push notification to all users in club
          memberJunctionTable.where({ClubId: item.ClubId}).read({
              success: function(memberships){
                  for(var i =0;i<memberships.length;i++){
                      tags.push(memberships[i].AccountId);
                  }
                  sendPush(tags,payload,request);
              }
          });
    }});
    

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
  
    function sendPushNoReturn(tags,payload,request){
      // Write the default response and send a notification
      // to all platforms.            
      push.send(tags, payload, {               
          success: function(pushResponse){
          console.log("Sent push:", pushResponse);
          // Send the default response.
          //request.respond();
          },              
          error: function (pushResponse) {
              console.log("Error Sending push:", pushResponse);
               // Send the an error response.
              //request.respond(500, { error: pushResponse });
              }           
       });    
  }