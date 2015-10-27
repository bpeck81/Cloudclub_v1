exports.post = function(request, response) {
    // Use "request.service" to access features of your mobile service, e.g.:
    //   var tables = request.service.tables;
    //   var push = request.service.push;
    /*for(var i=0;i<request.body.length;i++){
        console.log(request.body[i]);
    }*/
    console.log("-----no---------");

    var tagTable = request.service.tables.getTable('Tag');
    tagTable.read({
        success:function(tags){
            var tagHits = searchTags(tags,request.body);
            startSort(tagHits,request,response);
        }
    });
    
    //response.send(statusCodes.OK, { message : 'Hello World!' });
};

function searchTags(tags,queryTags){
    //store results as a 2d array: [clubid,numHits]
    var results =[];
    //loop through passed in query tags
    for(var i = 0;i<queryTags.length;i++){
        //loop through tags in table
        for(var j = 0;j<tags.length;j++){
            //if tags equal
            if(tags[j].Key==queryTags[i]){
                var uniqueMatch = true;
                //see if tag's club is in the results 2d array
                for(var k = 0;k<results.length;k++){
                    if(results[k][0]==tags[j].ClubId){
                        results[k][1]++;
                        uniqueMatch = false;
                        break;
                    }
                }
                //if not found in results
                if(uniqueMatch){
                    results.push([tags[j].ClubId,1]);
                }
            }
        }
    }
    console.log(results);
    
    return results;
}

function startSort(tagHits,request,response){
    var sortedTags = sortTags(tagHits,request,response);
    
    var clubTable = request.service.tables.getTable('Club');
    clubTable.read({
        success:function(clubs){
            console.log(sortedTags);
            //response.send(statusCodes.OK,clubs);
        }
    });
}




function sortTags(tagHits,request,response){
    return getPopularity(tagHits,request,response);
}


function getPopularity(tagHits,request,response){
    //make query to table
    var mssql = request.service.mssql;
    var sql = "SELECT * FROM Club WHERE id = ?;";
    //make a new context to save the value of i
    (function addPopularity(i){
        mssql.query(sql, tagHits[i][0], {
            success: function(club) {
                tagHits[i].push(club[0].TotalRating);
                //remember, this is asynchronous so sometimes the last one launched will be the
                //first one finished, so it is not guaranteed that everyting in the array now has a thrid item
                //thus i have to recursively call this function
                if(tagHits.length-1==i){
                    console.log("hit i");
                    return sortPopularity(tagHits,request,response);
                }else{
                    addPopularity(i+1);
                }
            },
            error: function(err) {
               console.log(err);
            }
    });
    })(0);
    
}

function sortPopularity(tagHits,request,response){
    //results is a 2d array: [clubid,numHits,totalRating]
    tagHits.sort(function(a,b){
        if(a[1]==b[1]){
            return b[2]-a[2];
        }else{
            return b[1]-a[1];
        }
    });
    console.log(tagHits);
    //always pass functions to a callback to make sure that sequential execution is maintained
    return findClubs(tagHits,request,response);
}

function findClubs(tagHits,request,response){
    var orderedClubs=[];
    //make query to table
    var mssql = request.service.mssql;
    var sql = "SELECT * FROM Club WHERE id = ?;";
    (function addClub(i){
        mssql.query(sql, tagHits[i][0], {
            success: function(club) {
                orderedClubs.push(club[0]);
                if(i==tagHits.length-1){
                    console.log(orderedClubs);
                    response.send(statusCodes.OK,orderedClubs);
                }else{
                   addClub(i+1); 
                }
            },
            error: function(err) {
               console.log(err);
            }
        });
    })(0);
}



exports.get = function(request, response) {
    response.send(statusCodes.OK, { message : 'Hello World!' });
};