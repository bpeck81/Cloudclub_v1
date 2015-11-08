var accountTable = tables.getTable('Account');
var memberJuncTable = tables.getTable('MemberJunction');
var clubReportTable = tables.getTable('ClubReport');
var clubTable = tables.getTable('Club');

function insert(item, user, request) {

    clubReportTable.where({ClubId: item.ClubId}).read({
        success:function(reports){
            //if the club has more than 5 reports
            if(reports.length>5){
                notifyAccounts(item,request);
            }
        }
    });


    request.execute();

}

function notifyAccounts(item,request){
    memberJuncTable.where({ClubId: item.ClubId}).read({
        success:function(members){
            //create notifications for each user
            for(var i=0;i<members.length;i++){
                createDBNotification(item,members[i].AccountId,request);
            }
            //delete the club
            clubTable.del(item.ClubId);
        }
    });
    
}

//create notification
function createDBNotification(item,memberId,request){
    var notificationTable = tables.getTable('DBNotification');
    //id is automatically added
    var notification = {
        Time: new Date(),
        AccountId: memberId,
        Type: "clubReport",
        Text: 'The club "'+item.ClubTitle+'" was deleted because multiple users reported it.'
    };
    notificationTable.insert(notification);
    
    //send push notification
    //NOTE: error just passing in notification.text, due to the sequence of quotes
    var payloadText = 'The club \\"'+item.ClubTitle+'\\" was deleted because multiple users reported it.';
    var tags = [memberId];
    var payload = '{ "message" : "dbNotification,'+payloadText+'" }';
    sendPush(tags,payload,request);
}

//PUSH NOTIFICATION
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