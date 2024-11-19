

(function ($) {
  
    var _fileService = abp.services.app.file,
        l = abp.localization.getSource('GameXuaVN'),
        _$modal = $('#FileCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#FilesTable')
        ;

    var vars = {
        divContentList: '#content-list',
        hiddenPage: '#hidden-page'
    };

    _$form.validate({
        rules: {
            Password: "required",
            ConfirmPassword: {
                equalTo: "#Password"
            }
        }
    });

    function loadData() {
        var page = $('#hidden-page').val();
        $.ajax({
            url: '/Nes/ListItem?page=' + page,
            method: 'GET',
        }).done(function (data) {
            $(vars.divContentList).html(data);
        });
            
    }

    //data-item="paging"
    $(document).on('click', '[data-item]', function (a) {
        $('[data-item]').parent().removeClass('active');

        var page = $(this).attr('data-value');

        $('[data-value="' + page + '"]').parent().addClass('active')
        if (page) {
            $(vars.hiddenPage).val(page);
            loadData();
        }
    });

    loadData();

})(jQuery);
