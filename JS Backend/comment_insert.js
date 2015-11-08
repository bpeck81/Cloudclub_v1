  var memberJunctionTable = tables.getTable("MemberJunction");
  var tempMembJuncTable = tables.getTable("TemporaryMemberJunction");
  
  //TODO: I AM ONLY DOING THE PUSH NOTIFICATIONS CORRECTLY (using request.execute({success:func}))
  //in comment so someday I'll have to rewrite it 
  
  function insert(item, user, request) {
  // Execute the request and send notifications.
    item.Time = new Date();
     
     request.execute({success: function() {                      
      // Create a template-based payload.
      //comma separates type of notification from id of the associated class
      var payload = '{ "message" : "comment,' + item.id + '" }';   
      
      //set the tags to the accountids of whoever the notification is meant for         
      var tags = [];
      
      //send push notification to all temp users of a club
      tempMembJuncTable.where({ClubId: item.ClubId}).read({
          success: function(memberships){
              for(var i =0;i<memberships.length;i++){
                  tags.push(memberships[i].AccountId);
              }
              //sendPush(tags,payload,request);
          }
      });
      
      //NOTE: whichever sendpush is called must be the last one called, and must be called,
      //so since there will always theoretically be permanent members of a club, call it on this one
      //send push notification to all users in club
      memberJunctionTable.where({ClubId: item.ClubId}).read({
          success: function(memberships){
              for(var i =0;i<memberships.length;i++){
                  tags.push(memberships[i].AccountId);
              }
              //NOTE: must call request.execute after all push notification fields have been set
              // otherwise, some fields returned will be null
              console.log(tags);
              sendPush(tags,payload,request);
          }
      });
      
      
      
            
      }
      });
      

      
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
  