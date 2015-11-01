//imports
var azure = require('azure');
var qs = require('querystring');
var appSettings = require('mobileservice-config').appSettings;

// Get storage account settings from app settings. 
var accountName = appSettings.STORAGE_ACCOUNT_NAME;
var accountKey = appSettings.STORAGE_ACCOUNT_ACCESS_KEY;
var host = accountName + '.blob.core.windows.net';
//create blob service
var blobService = azure.createBlobService(accountName, accountKey, host);

console.log("-----------IMPORTS------------");

function CleanDatabase() {
    console.log("------start------");
    
    //NOTE: this should go in its own job once we have more schedulers
    updateRankings();
    cleanBans();
    
    //Erase old comments ana images if they had one
    var commentTable = tables.getTable('Comment');
    
    commentTable.read({
        success: function(comments){
            for(var i=0;i<comments.length;i++){
                var bDelete = cleanComment(comments[i]);
                console.log(i);
                if(bDelete){
                    //if comments[i] is a picture
                    if(comments[i].Picture){
                        cleanImages(comments[i]);
                    }
                    
                    console.log(i+"-deleting: "+comments[i].id);
                    commentTable.del(comments[i].id);
                }else{
                    console.log(i+"-saving: "+comments[i].id);
                }
                console.log(i);
            }
        }
    });
    
    //Erase old clubs and the tags connected to them
    var clubTable = tables.getTable('Club');
    clubTable.read({
        success:function(clubs){
            for(var i = 0;i<clubs.length;i++){
                if(isNew(clubs[i].Time.getTime())){
                    console.log(i+" new club: "+clubs[i].id);
                    continue;
                }else{
                    console.log(i+" old club: "+clubs[i].id);
                    //CAREFUL: cleanClubs is an order 1, not 0
                    cleanClub(clubs[i]);
                }
            }
        }
    });
    
    console.log("----end----");
}

//boolean function that checks time and returns true if comment is older than 24 hours, false if not
function cleanComment(comment){
    var timeComment = comment.Time.getTime();
    if(!isNew(timeComment)){
        return true;
    }else{
        return false;
    }
}

//returns true if whatever is passed in is new
function isNew(timeObject){
    var timeNow = new Date().getTime();
    var timeExisted = (timeNow-timeObject)/(1000*60*60);
    if(timeExisted<.01){
        return true;
    }else{
        return false;
    }
}

//returns true if all comments are older than 24 hours, false if there is a recent comment
function cleanClub(club){
    var commentTable = tables.getTable('Comment');
    //sorted by time now, so presumably the first is the most recent
    commentTable.where({ClubId: club.id}).orderByDescending('Time').read({
        success: function(comments){
            for(var i=0;i<comments.length;i++){
                console.log("sorted by time: "+comments[i].Time);
            }
            //if there are no comments since they've all been deleted
            if(comments.length==0){
                return deleteClubAndTags(true,club.id);
            }
            //TODO: I NO LONGER NEED OT LOOP THROUGH SINCE IT'S SORTED BY TIME
            for(var i = 0;i<comments.length;i++){
                if(isNew(comments[i].Time.getTime())){
                    //if only an hour before the club is deleted, notify users
                    createWarning(club,comments[i].Time.getTime());
        
                    return deleteClubAndTags(false,club.id);
                }
            }
            return deleteClubAndTags(true,club.id);
        }
    });
}

function deleteClubAndTags(bDelete,clubId){
    if(bDelete){
        console.log("deleting club: "+clubId);
        cleanTags(clubId);
        var clubTable = tables.getTable('Club');
        clubTable.del(clubId);
        
    }else{
        console.log("saving club: "+clubId);
    }
}

function cleanTags(clubId){
    var tagTable = tables.getTable('Tag');
    tagTable.where({ClubId: clubId}).read({
        success:function(tags){
            for(var k = 0;k<tags.length;k++){
                console.log("deleting tags: "+tags[k].ClubId);
                tagTable.del(tags[k].id);
            }
        }
    });
}

//delete the image database entry and blob storage
function cleanImages(comment){
    console.log(comment.Text+" is a picture comment to be deleted");
    var imageTable = tables.getTable('DBImage');
    imageTable.lookup(comment.Text,{
        success:function(image){
            //delete blob
            blobService.deleteBlob(image.ContainerName, image.ResourceName, function(error, response){
                if(!error){
                    console.log("Image successfully deleted");
                }
            });
            //delete image in database (Note: id is lower case since it's predefined, by mine are upper case)
            imageTable.del(image.id);
        }
    });
}

//create a warning and notify all users if club is within an hour of deletion
//note: a precondition is that the comment passed in is the most recent
function createWarning(club,time){
    var timeNow = new Date().getTime();
    var timeExisted = (timeNow-time)/(1000*60*60);
    //if club will be deleted in an hour
    if(timeExisted>0){
        var memberJunctionTable = tables.getTable('MemberJunction');
        var notificationTable = tables.getTable('Notification');
        
        memberJunctionTable.where({ClubId: club.id}).read({
            success:function(memberships){
                for(var i = 0;i<memberships.length;i++){
                    //id is automatically added
                    var notification = {
                        Time: new Date(),
                        AccountId: memberships[i].AccountId,
                        Type: "warning",
                        Text: 'The club "'+club.Title+'" will expire if no one speaks!'
                    };
                    notificationTable.insert(notification);
                }
            }
        });
    }
}

//Update the user accounts with their ranks and add notifications for them
function updateRankings(){
    var accountTable = tables.getTable('Account');
    accountTable.orderByDescending('Points').read({
        success:function(accounts){
            console.log('Rankings updated.');
            for(var i=0;i<accounts.length;i++){
                accounts[i].Ranking=i;
                accountTable.update(accounts[i]);
            }
        }
    });
    
}

//delete bans that are old
function cleanBans(){
    var banTable = tables.getTable('Ban');
    banTable.read({
        success:function(bans){
            for(var i=0;i<bans.length;i++){
                if(!isNew(bans[i].Time.getTime())){
                    banTable.del(bans[i].id);
                }
            }
        }
    });
}