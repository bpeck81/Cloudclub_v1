function insert(item, user, request) {
    //NOTE: tried to give user points here, but apparently the insert function called from the api is 
    //different than this one

    //add the medal normally
    item.Time = new Date();
    request.execute();

}