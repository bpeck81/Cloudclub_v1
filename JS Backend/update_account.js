//an array of functions that return medals as arrays; zeroeth value is if the user qualifies for medal
var medalArray = [
    easyDroplets,
    mediumDroplets,
    hardDroplets,
    mediumTotalClubsRatings,
    hardTotalClubsRatings,
    easyNumFriends,
    mediumNumFriends,
    hardNumFriends,
    easyNumInvites,
    mediumNumInvites,
    hardNumInvites
]

var medalTable = tables.getTable('Medal');


function update(item, user, request) {
    //check if user qualifies for medal
    for(var i = 0;i<medalArray.length;i++){
        var medal = medalArray[i](item);
        //if qualified for medal
        if(medal[0]==true){
            addMedal(medal,item,request);
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
            account.NumMedals++;
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

//QUALIFIER FUNCTIONS:
//FORMAT FOR MEDAL RETURN: [qualify, name, points]

//droplets
function easyDroplets(item){
    if(item.NumDroplets==5 || item.NumDroplets==10){
        return [true,"Earn "+item.NumDroplets+" droplets!",10];
    }else{
        return [false];
    }
}
function mediumDroplets(item){
    if(item.NumDroplets==25 ||item.NumDroplets==50 ||item.NumDroplets==75){
        return [true,"Earn "+item.NumDroplets+" droplets!",25];
    }else{
        return [false];
    }
}
function hardDroplets(item){
    if(item.NumDroplets>75 && item.NumDroplets%50==0){
        return [true,"Earn "+item.NumDroplets+" droplets!",50];
    }else{
        return [false];
    }
}

//total club ratings
function mediumTotalClubsRatings(item){
    if(item.TotalClubsRatings >=5 && item.TotalClubsRatings<10){
        return [true,"Earn 5 stars on clubs you have established!",25];
    }else if(item.TotalClubsRatings >=10 && item.TotalClubsRatings<25){
        return [true,"Earn 10 stars on clubs you have established!",25];
    }else{
        return [false];
    }
}
function hardTotalClubsRatings(item){
    if(item.TotalClubsRatings>24 && item.TotalClubsRatings%25==0){
        return [true,"Earn "+item.TotalClubsRatings+" stars on clubs you have established!",50];
    }else{
        return [false];
    }
}

//friends
function easyNumFriends(item){
    if(item.NumFriends==1){
        return [true,"Make a friend!",10];
    }else{
        return [false];
    }
}
function mediumNumFriends(item){
    if(item.NumFriends==5 ||item.NumFriends==10){
        return [true,"Make "+item.NumFriends+" friends!",25];
    }else{
        return [false];
    }
}
function hardNumFriends(item){
    if(item.NumFriends>24 && item.NumFriends%25==0){
        return [true,"Make "+item.NumFriends+" friends!",50];
    }else{
        return [false];
    }
}

//invites
function easyNumInvites(item){
    if(item.NumInvites==1 || item.NumInvites==3 || item.NumInvites==5){
        return [true,"Invite "+item.NumInvites+" friends into a club!",10];
    }else{
        return [false];
    }
}
function mediumNumInvites(item){
    if(item.NumInvites==10 ||item.NumInvites==15 ||item.NumInvites==20){
        return [true,"Invite "+item.NumInvites+" friends into a club!",25];
    }else{
        return [false];
    }
}
function hardNumInvites(item){
    if(item.NumInvites>20 && item.NumInvites%15==0){
        return [true,"Invite "+item.NumInvites+" friends into a club!",50];
    }else{
        return [false];
    }
}