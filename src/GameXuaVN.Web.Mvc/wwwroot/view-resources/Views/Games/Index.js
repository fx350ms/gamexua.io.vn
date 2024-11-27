(function ($) {
    var _gameService = abp.services.app.game,
        l = abp.localization.getSource('GameXuaVN'),
        _$modal = $('#GameCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#GamesTable');
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
            url: abp.appPath + 'Game/Create',
            type: 'POST',
            processData: false, // Không xử lý dữ liệu form thành chuỗi
            contentType: false, // Đặt content type là multipart/form-data
            data: formData
        }, ajaxParams));
    };

  

    var _$gamesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
       
        listAction: {
            ajaxFunction: _gameService.getAll,
            inputFilter: function () {
                return $('#GamesSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$gamesTable.draw(false)
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
                data: 'name',
                sortable: false
            },
            {
                targets: 2,
                data: 'description',
                sortable: false
            },
            {
                targets: 3,
                data: 'totalPlay',
                sortable: false
            },
            {
                targets: 4,
                data: 'totalLike',
                sortable: false,
                render: (data, type, row, meta) => {
                    return `${row.totalLike} / ${row.totalDislike}`
                }
            },
            {
                targets: 5,
                data: 'thumnail',
                sortable: false,
                render: (data, type, row, meta) => {
                    return `<img src="${row.thumbnailBase64}"  style="width:133px; height:80px;" />`
                }
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                width: 300,
                defaultContent: '',
                render: (data, type, row, meta) => {

                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-game" data-game-id="${row.id}" data-toggle="modal" data-target="#GameEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-success edit-game" data-game-id="${row.id}" data-toggle="modal" data-target="#GameEditModal">`,
                        `       <i class="fas fa-play"></i> ${l('Test')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-primary edit-game" data-game-id="${row.id}" data-toggle="modal" data-target="#GameEditModal">`,
                        `       <i class="fas fa-download"></i> ${l('Download')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-game" data-game-id="${row.id}" data-game-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.validate({
        rules: {
            Password: "required",
            ConfirmPassword: {
                equalTo: "#Password"
            }
        }
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        var game = _$form.serializeFormToObject();
        game.Page = game.Name.substring(0, 1);

        abp.ui.setBusy(_$modal);
        create(game).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            _$gamesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-game', function () {
        var gameId = $(this).attr("data-game-id");
        var gameName = $(this).attr('data-game-name');

        deleteGame(gameId, gameName);
    });

    function deleteGame(gameId, gameName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                gameName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _gameService.delete({
                        id: gameId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$gamesTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-game', function (e) {
        var gameId = $(this).attr("data-game-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Game/EditModal?gameId=' + gameId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#GameEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[data-target="#GameCreateModal"]', (e) => {
        $('.nav-tabs a[href="#game-details"]').tab('show')
    });

    abp.event.on('game.edited', (data) => {
        _$gamesTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$gamesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$gamesTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
