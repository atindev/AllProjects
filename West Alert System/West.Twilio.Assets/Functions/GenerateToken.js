const adal = require('adal-node').AuthenticationContext;

exports.handler = function(context, event, callback) {
	let token = '';
	const adalcontext = new adal(context.authorityUrl);
    adalcontext.acquireTokenWithClientCredentials(
    context.resource,
    context.clientId,
    context.clientSecret,
    (err, tokenResponse) => {
    if (err) {
      console.log(`Token generation failed due to ${err}`);
    } else {
      token = tokenResponse.accessToken;
      return callback(null, token);
    }
  });
};