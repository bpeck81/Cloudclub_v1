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
                    }
                });
            }
        }
    });

    
    
    request.execute();

}