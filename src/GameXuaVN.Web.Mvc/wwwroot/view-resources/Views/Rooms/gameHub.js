var gameHub = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

gameHub.start().then(function () {
    console.log("Done");
}).catch(function (err) {
    console.log(err);
});


gameHub.on('UserCountUpdated', function (NumberOfUsers) {
    $('#txt-user-online').html(NumberOfUsers + ' người đang online.');
});





//gameHub.on('roomCreated', function (roomId, roomName) {
//    alert(`Room created: ${roomName} (ID: ${roomId})`);
//});

//gameHub.on('playerJoined', function (userName) {
//    alert(`${userName} has joined the room!`);
//});

//gameHub.on('playerLeft', function (userName) {
//    alert(`${userName} has left the room.`);
//});

//gameHub.on('receiveGameState', function (gameState) {
//    console.log('Game State:', gameState);
//});

//// Kết nối SignalR
//connection.start().done(function () {
//    console.log('SignalR connected');
//}).fail(function (error) {
//    console.error('SignalR connection failed:', error);
//});

//// Gửi yêu cầu tạo room
//function createRoom(roomName, maxPlayers) {
//    gameHub.invoke('createRoom', roomName, maxPlayers);
//}

//// Tham gia room
//function joinRoom(roomId) {
//    gameHub.invoke('joinRoom', roomId);
//}

//// Rời room
//function leaveRoom(roomId) {
//    gameHub.invoke('leaveRoom', roomId);
//}
