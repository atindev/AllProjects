const adal = require('adal-node').AuthenticationContext;

exports.handler = function(context, event, callback) {
	let token = '';
	console.log(context.authorityUrl);
	console.log(context.graphresource);
	console.log(context.clientId);
	console.log(context.clientSecret);
	const adalcontext = new adal(context.authorityUrl);
    adalcontext.acquireTokenWithClientCredentials(
    context.graphresource,
    context.clientId,
    context.clientSecret,
    (err, tokenResponse) => {
    if (err) {
      console.log(`Token generation failed due to ${err}`);
    } else {
      token = tokenResponse.accessToken;
      console.log(token);
      return callback(null, token);
    }
  });
};