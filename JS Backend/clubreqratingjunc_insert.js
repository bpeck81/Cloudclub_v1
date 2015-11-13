var clubReqRatingJuncTable = tables.getTable('ClubReqRatingJunc');

function insert(item, user, request) {

    //search for declines of the same club request
    clubReqRatingJuncTable.where({ClubRequestId: item.ClubRequestId}).read({
        success:function(ratings){
            //if this is the third decline, set the club request to invisible
            if(ratings.length>=2){
                var clubRequestTable = tables.getTable('ClubRequest');
                clubRequestTable.lookup(item.ClubRequestId,{
                    success:function(clubRequest){
                        clubRequest.Visible=false;
                        clubRequestTable.update(clubRequest);
                    }
                });
            }
        }
    });
    
    request.execute();

}