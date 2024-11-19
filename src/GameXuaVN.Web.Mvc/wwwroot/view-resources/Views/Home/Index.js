$(function () {

    'use strict';
   

    var vars = {
        divTopPlayContentList: '#div-top-play-content',
        hiddenPage: '#hidden-page'
    };
    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    //-----------------------
    //- MONTHLY SALES CHART -
    //-----------------------

   
    //---------------------------
    //- END MONTHLY SALES CHART -
    //---------------------------
    TopPlayListData();

    function TopPlayListData() {
       
        $.ajax({
            url: '/Nes/TopPlayListItem',
            method: 'GET',
        }).done(function (data) {
            $(vars.divTopPlayContentList).html(data);
        });

    }
});
