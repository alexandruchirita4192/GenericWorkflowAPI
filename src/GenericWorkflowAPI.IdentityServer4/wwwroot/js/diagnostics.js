// Ajax for Vanilla JS
var ajax = {};
ajax.x = function () {
    if (typeof XMLHttpRequest !== 'undefined') {
        return new XMLHttpRequest();
    }
    var versions = [
        "MSXML2.XmlHttp.6.0",
        "MSXML2.XmlHttp.5.0",
        "MSXML2.XmlHttp.4.0",
        "MSXML2.XmlHttp.3.0",
        "MSXML2.XmlHttp.2.0",
        "Microsoft.XmlHttp"
    ];

    var xhr;
    for (var i = 0; i < versions.length; i++) {
        try {
            xhr = new ActiveXObject(versions[i]);
            break;
        } catch (e) {
        }
    }
    return xhr;
};

ajax.send = function (url, callback, method, data, async, headers) {
    if (async === undefined) {
        async = true;
    }
    var x = ajax.x();
    x.open(method, url, async);
    x.onreadystatechange = function () {
        if (x.readyState == 4) {
            callback(x.responseText)
        }
    };
    if (method == 'POST') {
        /*
            var headers = []; // create an empty array
            headers.push({
              key:   "keyName",
              value: "the value"
            });
        */
        if (headers && headers.length > 0) {
            let i = 0;
            while (i < headers.length) {
                console.log('headers[' + i + '] key=[' + headers[i].key + '] value=[' + headers[i].value + ']');
                x.setRequestHeader(headers[i].key, headers[i].value);
                i++;
            }
        } else {
            x.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
            x.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
        }
    }
    x.send(data)
};

ajax.get = function (url, data, callback, async, headers) {
    var query = [];
    if (data) {
        for (var key in data) {
            query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
        }
    }
    ajax.send(url + (query.length ? '?' + query.join('&') : ''), callback, 'GET', null, async, headers)
};

ajax.post = function (url, data, callback, async, headers) {
    var query = [];
    if (data) {
        for (var key in data) {
            query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data[key]));
        }
    }
    ajax.send(url, callback, 'POST', query.join('&'), async, headers)
};

// Function to get the token and append it on the diagnostics page
function getToken() {
    var headers = []; // create an empty array for headers

    // Add X-Requested-With header with it's value (XMLHttpRequest)
    // The so-called way to check Ajax requests doesn't work without doing special Ajax requests
    headers.push({
        key: "X-Requested-With",
        value: "XMLHttpRequest"
    });

    // Add Content-type header with it's value (JSON)
    headers.push({
        key: "Content-type",
        value: "application/json; charset=utf-8"
    });

    // Add AntiForgeryToken XSRF-TOKEN header with it's value
    var requestVerificationTokenElement = document.getElementsByName('__RequestVerificationToken');
    if (requestVerificationTokenElement && requestVerificationTokenElement.length == 1) {
        var value = requestVerificationTokenElement[0].value;
        headers.push({
            key: "XSRF-TOKEN",
            value: value
        });
    }

    // Create a token for the diagnostics page (so it's in the DiagnosticsController with the action name "GetToken")
    ajax.post('/Diagnostics/GetToken', null, function (response) {

        // Set the value of the description list value (dd element) with id 'token-data'
        // from "Views\Diagnostics\Index.cshtml" as the token received in response
        var element = document.getElementById('token-data');
        if (element) {
            element.innerHTML = response;
        }
    }, true, headers);
}

// Append token on the diagnostics page
document.addEventListener("readystatechange", function (event) {
    getToken();
});