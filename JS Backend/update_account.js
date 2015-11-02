//an array of all possible medals
var medalArray = [
    [qualifyOne,"First Comment",10],
    [qualifyTwo,"Millionth Comment",100],
    [qualifyThree,"Veteran Commentor6",20]
]

var medalTable = tables.getTable('Medal');


function update(item, user, request) {
    //check if user qualifies for medal
    for(var i = 0;i<medalArray.length;i++){
        //if qualified for medal
        if(medalArray[i][0](item)==true){
            addMedal(medalArray[i],item,request);
        }
    }

    //execute normal save function
    request.execute();

}

//adds a medal if the user does not have it already
function addMedal(medal,item,request){
    //check to see if the user has earned a medal
    medalTable.where({AccountId: item.id, MedalName: medal[1]}).read({
        success:function(medals){
            //if the user doesn't have the medal, add it
            if(medals.length==0){
                console.log("make medal");
                createMedal(medal[1],medal[2],item.id,request);
            }else{
                console.log("medal exists");
            }
        }
    });
}

function createMedal(name,points,id,request){
    //id is automatically added
    var medal = {
        Time: new Date(),
        AccountId: id,
        MedalName: name,
        Points: points
    };
    
    medalTable.insert(medal);
    
    //give the user points
    var accountTable = tables.getTable('Account');
    accountTable.lookup(medal.AccountId,{
        success:function(account){
            console.log("points being added");
            account.Points+=medal.Points;
            accountTable.update(account);
            //send push notification
            var tags = [medal.AccountId];
            var payload = '{ "message" : "medal,You have earned a medal!" }';
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

//QUALIFIER FUNCTIONS:
function qualifyOne(item){
    if(item.NumComments>0){
        return true;
    }else{
        return false;
    }
}
function qualifyTwo(item){
    if(item.NumComments>1000000){
        return true;
    }else{
        return false;
    }
}
function qualifyThree(item){
    if(item.NumComments>10){
        return true;
    }else{
        return false;
    }
}