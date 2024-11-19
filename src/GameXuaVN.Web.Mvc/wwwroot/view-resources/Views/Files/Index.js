(function ($) {
    var _fileService = abp.services.app.file,
        l = abp.localization.getSource('GameXuaVN'),
        _$modal = $('#FileCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#FilesTable');

    var _$filesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _fileService.getAll,
            inputFilter: function () {
                return $('#FilesSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$filesTable.draw(false)
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
                data: 'fileName',
                sortable: false
            },
            {
                targets: 2,
                data: 'description',
                sortable: false
            },
            {
                targets: 3,
                data: 'downloadCount',
                sortable: false
            },
            {
                targets: 4,
                data: 'totalRate',
                sortable: false,
                
            },
            {
                targets: 5,
                data: 'Thumbnail',
                sortable: false,
                render: data => `<img alt="photo" />`
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                width: 300,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-file" data-file-id="${row.id}" data-toggle="modal" data-target="#FileEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-primary edit-file" data-file-id="${row.id}" data-toggle="modal" data-target="#FileEditModal">`,
                        `       <i class="fas fa-download"></i> ${l('Download')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-file" data-file-id="${row.id}" data-file-name="${row.name}">`,
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

        var file = _$form.serializeFormToObject();
        file.roleNames = [];
        var _$roleCheckboxes = _$form[0].querySelectorAll("input[name='role']:checked");
        if (_$roleCheckboxes) {
            for (var roleIndex = 0; roleIndex < _$roleCheckboxes.length; roleIndex++) {
                var _$roleCheckbox = $(_$roleCheckboxes[roleIndex]);
                file.roleNames.push(_$roleCheckbox.val());
            }
        }

        abp.ui.setBusy(_$modal);
        _fileService.create(file).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            _$filesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-file', function () {
        var fileId = $(this).attr("data-file-id");
        var fileName = $(this).attr('data-file-name');

        deleteFile(fileId, fileName);
    });

    function deleteFile(fileId, fileName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                fileName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _fileService.delete({
                        id: fileId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$filesTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-file', function (e) {
        var fileId = $(this).attr("data-file-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Files/EditModal?fileId=' + fileId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#FileEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[data-target="#FileCreateModal"]', (e) => {
        $('.nav-tabs a[href="#file-details"]').tab('show')
    });

    abp.event.on('file.edited', (data) => {
        _$filesTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$filesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$filesTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
