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
                  console.log(tags);
                  sendPush(tags,payload,request);
              }
          });
    }});
    

}


function sendPush(tags,payload,request){
      //max of 20 tags, loop through and ensure request.respond() is called on completion    
      //loop through using recursive message
      if(tags.length<20){
          //send push to final 20 tags    
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
      }else{
          //send push to nonfinal tags
          var excessTags = tags.slice(0,19);
          tags = tags.slice(19,tags.length);
          push.send(excessTags,payload,{
              success:function(response){
                  sendPush(tags,payload,request);
              }
          });
      }
         
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