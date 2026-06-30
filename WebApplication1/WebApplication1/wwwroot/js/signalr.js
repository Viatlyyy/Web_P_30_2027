"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/apphub").build();


connection.on("ReceiveMessage", function (userName, message, timestamp) {
    updateChat(userName, message, timestamp);
});


connection.on("DataChanged", function (entityName, action) {
    
    const currentPage = window.location.pathname.toLowerCase();
    let needReload = false;
    if (entityName === "Student" && (currentPage.includes("/students") || currentPage === "/")) needReload = true;
    if (entityName === "Course" && (currentPage.includes("/courses") || currentPage === "/")) needReload = true;
    if (entityName === "Instructor" && (currentPage.includes("/instructors") || currentPage === "/")) needReload = true;
    if (entityName === "Enrollment" && (currentPage.includes("/enrollments") || currentPage === "/")) needReload = true;
    if (needReload) {
        location.reload();
    }
});

connection.start().catch(function (err) {
    console.error(err.toString());
});


function updateChat(userName, message, timestamp) {
    if (typeof window.appendChatMessage === "function") {
        window.appendChatMessage(userName, message, timestamp);
    }
}