// Description
// Make a write request to an external API

// Add axios 0.20.0 as a dependency under Functions Global Config, Dependencies
const axios = require('axios');
axios.defaults.baseURL = 'https://was-qna-prod.azurewebsites.net/qnamaker';
axios.defaults.headers.common['Authorization'] = "EndpointKey 8eaa7ec5-ce8f-4546-9673-5d9642707c63";
axios.defaults.headers.post['Content-Type'] = 'application/json';

exports.handler = function (context, event, callback) {
    
  // JSONPlaceholder: https://jsonplaceholder.typicode.com/
  // Fake Online REST API for Testing and Prototyping
  const instance = axios.create({
    timeout: 5000
  });

console.log(JSON.stringify(event.question));

  instance
     .post('/knowledgebases/aa52ebd2-6c1b-4a38-81c8-99df64060d8c/generateAnswer',{ question: JSON.stringify(event.question)} )
    .then((response) => {
      console.log(JSON.stringify(response.data));
      return callback(null, response.data);
    })
    .catch((error) => {
      console.log(error);
      return callback(error);
    });
};