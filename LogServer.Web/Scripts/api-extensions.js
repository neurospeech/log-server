/// <reference path="web-atoms/web-atoms.js" />

AtomConfig.ajax.jsonPostEncode = function (o) {
    o.dataType = "json";
    return o;
};


AtomPromise.delete = function (url, query, postData) {
    return AtomPromise.ajax(url, query, {
        type: 'DELETE',
        data: postData
    });
};

AtomPromise.put = function (url, query, postData) {
    return AtomPromise.ajax(url, query, {
        type: 'PUT',
        data: postData
    });
};

AtomPromise.patch = function (url, query, postData) {
    return AtomPromise.ajax(url, query, {
        type: 'PATCH',
        data: postData
    });
};