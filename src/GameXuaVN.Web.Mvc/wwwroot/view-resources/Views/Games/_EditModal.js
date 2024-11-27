(function ($) {
    var _gameService = abp.services.app.game,
        l = abp.localization.getSource('GameXuaVN'),
        _$modal = $('#GameEditModal'),
        _$form = _$modal.find('form');
    update = function (_$form, input, ajaxParams) {
        var formData = new FormData();

        // Thêm dữ liệu từ input vào formData
        for (var key in input) {
            if (input.hasOwnProperty(key)) {
                formData.append(key, input[key]);
            }
        }
     
        // Lấy file từ các input type="file" và thêm vào formData
        var thumbnailFile = _$form.find('input[name="ThumbnailFromFile"]:first')[0].files[0];
        if (thumbnailFile) {
            formData.append('ThumbnailFromFile', thumbnailFile);
        }
        var dataFile = _$form.find('input[name="DataFromFile"]:first')[0].files[0];
        if (dataFile) {
            formData.append('DataFromFile', dataFile);
        }

        return abp.ajax($.extend(true, {
            url: abp.appPath + 'Game/Update',
            type: 'PUT',
            processData: false, // Không xử lý dữ liệu form thành chuỗi
            contentType: false, // Đặt content type là multipart/form-data
            data: formData
        }, ajaxParams));
    };
    function save() {
        if (!_$form.valid()) {
            return;
        }
        var game = _$form.serializeFormToObject();
        game.Page = game.Name.substring(0, 1);
       
        abp.ui.setBusy(_$form);
        update(_$form, game).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('game.edited', game);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);
