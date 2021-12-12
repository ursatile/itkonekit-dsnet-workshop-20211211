// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(connectToSignalR);

function connectToSignalR() {
    console.log("Connecting to signalR...");
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("ShowPopupMessageOnAutobarnWebsite", displayNotification);
    conn.start().then(function () {
        console.log("SignalR has started! Yay!");
    }).catch(function (err) {
        console.log("Error starting signalR:", err);
    });
}

function displayNotification(user, json) {
    const $target = $("div#signalr-notifications");
    console.log(json);
    const data = JSON.parse(json);
    const message = `NEW VEHICLE! <a href="/vehicles/details/${data.registration}">${data.registration}</a> 
        (${data.manufacturer} ${data.modelName},
        ${data.color}, ${data.year}). 
        Price ${data.currency} ${data.price}`;
    var $div = $(`<div>${message}</div>`);
    $target.prepend($div);
    window.setTimeout(function () { $div.fadeOut(2000, function () { $div.remove(); }); }, 8000);
}
