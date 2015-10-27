function update(item, user, request) {

    //if the number of droplets is divisible by 5 and greator than 0
    if(item.NumDroplets%5==1 && item.NumDroplets>0){
        var notificationTable = tables.getTable('DBNotification');
        //id is automatically added
        var notification = {
            Time: new Date(),
            AccountId: item.AuthorId,
            Type: "droplet",
            Text: 'Your post "'+item.Text+'" has received '+item.NumDroplets+' droplets!'
        };
        notificationTable.insert(notification);
    }

    request.execute();

}