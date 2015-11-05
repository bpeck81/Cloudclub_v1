var accountTable = tables.getTable('Account');
var banTable = tables.getTable('Ban');

function insert(item, user, request) {

    banTable.where({AccountId: item.AccountId}).read({
        success:function(bans){
            console.log("bans found");
            //if the user has 3  bans
            if(bans.length>2){
                accountTable.lookup(item.AccountId,{
                    success:function(account){
                        //ban user for a minute
                        account.Banned=new Date(new Date().getTime()+60*1000);
                        accountTable.update(account);
                        console.log("user has been banned");
                        
                        //create a notification for ban-NOTE: made in script, so push notification 
                        //not automatically made, so it is made along with notification script below
                        createDBNotification(item,request);
                    }
                });
            }
        }
    });

    
    
    request.execute();

}

//create notification - NOTE: i only reference one comment they were banned for here, but
//in actuality, three are needed to be banned
function createDBNotification(item,request){
    var notificationTable = tables.getTable('DBNotification');
    var commentTable = tables.getTable('Comment');
    commentTable.lookup(item.CommentId,{
        success:function(comment){
            //id is automatically added
            var notification = {
            Time: new Date(),
            AccountId: item.AccountId,
            Type: "ban",
            Text: 'You were muted for 24 hours for your comment: "'+comment.Text+'".'
            };
            notificationTable.insert(notification);
            
            //send push notification
            //NOTE: error just passing in notification.text, due to the sequence of quotes
            var payloadText = 'You were muted for 24 hours for your comment: \\"'+comment.Text+'\\".';
            var tags = [item.AccountId];
            var payload = '{ "message" : "dbNotification,'+payloadText+'" }';
            console.log(tags[0]+":"+payload);
            sendPush(tags,payload,request);
        }
    });
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