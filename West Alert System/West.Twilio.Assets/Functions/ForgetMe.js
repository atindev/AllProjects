// Add axios 0.20.0 as a dependency under Functions Global Config, Dependencies
const axios = require('axios');

exports.handler = function (context, event, callback) {
    console.log(JSON.stringify(context));
    console.log(JSON.stringify(event));   

    axios.defaults.baseURL = context.devBaseURL;
    axios.defaults.headers.common['Authorization'] = `Bearer ${event.Token}`;
    axios.defaults.headers.post['Content-Type'] = 'application/json';

    const instance = axios.create({
        timeout: 30000
    });

    instance
        .post('/api/v1/Subscription/UnsubscribeMobile', JSON.stringify(event))
        .then((response) => {
          console.log(`response : ${JSON.stringify(response.data)}`);
          return callback(null, response.data);
        })
        .catch((error) => {
          console.log(`Exception : ${error}`);
          return callback(error);
        });
};