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
            addMedal(medalArray[i],item);
        }
    }

    //execute normal save function
    request.execute();

}

//adds a medal if the user does not have it already
function addMedal(medal,item){
    //check to see if the user has earned a medal
    medalTable.where({AccountId: item.id, MedalName: medal[1]}).read({
        success:function(medals){
            //if the user doesn't have the medal, add it
            if(medals.length==0){
                console.log("make medal");
                createMedal(medal[1],medal[2],item.id);
            }else{
                console.log("medal exists");
            }
        }
    });
}

function createMedal(name,points,id){
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