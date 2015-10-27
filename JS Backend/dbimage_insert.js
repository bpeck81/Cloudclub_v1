function insert(item, user, request) {
    item.Time = new Date();
    
    var azure = require('azure');
    var qs = require('querystring');
    var appSettings = require('mobileservice-config').appSettings;
    
    // Get storage account settings from app settings. 
    var accountName = appSettings.STORAGE_ACCOUNT_NAME;
    var accountKey = appSettings.STORAGE_ACCOUNT_ACCESS_KEY;
    var host = accountName + '.blob.core.windows.net';
    
    console.log(accountName+accountKey+host);

    if ((typeof item.ContainerName !== "undefined") && (
    item.ContainerName !== null)) {
        // Set the BLOB store container name on the item, which must be lowercase.
        item.ContainerName = item.ContainerName.toLowerCase();

        // If it does not already exist, create the container 
        // with public read access for blobs.        
        var blobService = azure.createBlobService(accountName, accountKey, host);
        
        console.log("blob service: "+blobService);
        console.log("checkpoint a");
        
        blobService.createContainerIfNotExists(item.ContainerName, {
            publicAccessLevel: 'blob'
        }, function(error) {
            if (!error) {
                
                console.log("checkpoint b");

                // Provide write access to the container for the next 5 mins.        
                var sharedAccessPolicy = {
                    AccessPolicy: {
                        Permissions: azure.Constants.BlobConstants.SharedAccessPermissions.WRITE,
                        Expiry: new Date(new Date().getTime() + 5 * 60 * 1000)
                    }
                };

                // Generate the upload URL with SAS for the new image.
                var sasQueryUrl = 
                blobService.generateSharedAccessSignature(item.ContainerName, 
                item.ResourceName, sharedAccessPolicy);

                // Set the query string.
                item.SasQueryString = qs.stringify(sasQueryUrl.queryString);
                console.log("query string: "+item.SasQueryString);

                // Set the full path on the new new item, 
                // which is used for data binding on the client. 
                item.ImageUri = sasQueryUrl.baseUrl + sasQueryUrl.path;

            } else {
                console.error(error);
                console.log("checkpoint c");
            }
            request.execute();
        });
    } else {
        request.execute();
    }
}