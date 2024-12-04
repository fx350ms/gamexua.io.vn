const gameHub = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

gameHub.start().then(function () {
    console.log("Done");
}).catch(function (err) {
    console.log(err);
});
 

gameHub.on('onRoomCreated', function (roomId, gameId, roomName, gameName) {
    abp.notify.info('Room created: ' + roomName);
});

function createRoom(roomId, gameId, roomName, gameName) {
    gameHub.invoke('CreateRoom', roomId, gameId, roomName, gameName);
}


(function ($) {
    var _roomService = abp.services.app.room,
        l = abp.localization.getSource('GameXuaVN'),
        _$modal = $('#RoomCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#RoomsTable');
    create = function (input, ajaxParams) {
        var formData = new FormData();

        // Thêm dữ liệu từ input vào formData
        for (var key in input) {
            if (input.hasOwnProperty(key)) {
                formData.append(key, input[key]);
            }
        }

        // Lấy file từ các input type="file" và thêm vào formData
        var thumbnailFile = $('input[name="ThumbnailFromFile"]')[0].files[0];
        if (thumbnailFile) {
            formData.append('ThumbnailFromFile', thumbnailFile);
        }

        var dataFile = $('input[name="DataFromFile"]')[0].files[0];
        if (dataFile) {
            formData.append('DataFromFile', dataFile);
        }

        return abp.ajax($.extend(true, {
            url: abp.appPath + 'Room/Create',
            type: 'POST',
            processData: false, // Không xử lý dữ liệu form thành chuỗi
            contentType: false, // Đặt content type là multipart/form-data
            data: formData
        }, ajaxParams));
    };



    var _$roomsTable = _$table.DataTable({
        paging: true,
        serverSide: true,

        listAction: {
            ajaxFunction: _roomService.getAll,
            inputFilter: function () {
                return $('#RoomsSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$roomsTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'id',
                sortable: false
            },
            {
                targets: 2,
                data: 'roomName',
                sortable: false
            },
            {
                targets: 3,
                data: 'currentPlayers',
                sortable: false,
                render: (data, type, row, meta) => {
                    return '<p data-row-id="' + row.id + '"> ' + row.currentPlayers + ' </p>'
                }
            },

            {
                targets: 4,
                data: null,
                sortable: false,
                width: 300,
                defaultContent: '',
                render: (data, type, row, meta) => {

                    return [
                        `   <a type="button" href="room/join/${row.id}" class="btn btn-sm bg-success " data-room-id="${row.id}" >`,
                        `       <i class="fas fa-play"></i> ${l('Join')}`,
                        '   </a>'
                    ].join('');
                }
            }
        ]
    });


    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        var room = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _roomService.create(room).done(function () {

            createRoom(1, 6, '', '');
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            _$roomsTable.ajax.reload();


        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-room', function () {
        var roomId = $(this).attr("data-room-id");
        var roomName = $(this).attr('data-room-name');

        deleteRoom(roomId, roomName);
    });

    function deleteRoom(roomId, roomName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                roomName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _roomService.delete({
                        id: roomId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$roomsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-room', function (e) {
        var roomId = $(this).attr("data-room-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Room/EditModal?roomId=' + roomId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#RoomEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[data-target="#RoomCreateModal"]', (e) => {
        $('.nav-tabs a[href="#room-details"]').tab('show')
    });

    abp.event.on('room.edited', (data) => {
        _$roomsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$roomsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$roomsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
