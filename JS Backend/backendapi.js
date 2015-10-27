exports.post = function(request, response) {
    // Use "request.service" to access features of your mobile service, e.g.:
    //   var tables = request.service.tables;
    //   var push = request.service.push;
    var commentTable = request.service.tables.getTable('comment');
    var commentId;
    commentTable.read({
        success:function(results){
            commentId = results[0].Text;
            console.log(commentId);

            response.send(statusCodes.OK, {table: results });
        }
    });
    
};

exports.get = function(request, response) {
    response.send(statusCodes.OK, { message : 'Hello World! Get' });
};