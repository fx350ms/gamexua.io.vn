(function ($) {
    var _gameService = abp.services.app.game,
        l = abp.localization.getSource('GameXuaVN'),
        _$modal = $('#GameCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#GamesTable');

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
                data: null,
                sortable: false,
                width: 300,
                defaultContent: '',
                render: (data, type, row, meta) => {

                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-game" data-game-id="${row.id}" data-toggle="modal" data-target="#GameEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
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
        game.roleNames = [];
        var _$roleCheckboxes = _$form[0].querySelectorAll("input[name='role']:checked");
        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                game.roleNames.push(_$roleCheckbox.val());
            }
        }

        abp.ui.setBusy(_$modal);
        _gameService.create(game).done(function () {
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
