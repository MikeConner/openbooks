$(function () {
    $(document).foundation();

    // Toggle Search Navigation
    $('.search-toggle').on('click', function () {
        $('.search-menu').slideToggle();
        return false;
    });

    // Filter toggle
    $('.filter-toggle').on('click', function () {
        $('.filter-controls').slideToggle();
    });

    // Range Slider
    var range = $('.input-range'),
        value = $('.range-value');

    value.html(range.attr('value'));
    // Set range max (server sets a hidden field; copy it to the control). Cannot do on the server without breaking client JS.
    var contractMax = $('#ctl00_ContentPlaceHolder1_MaxContractField').val();

    $('#dblMaxContract').attr('max', contractMax);
    $('span#maxContract').html(contractMax);

    range.on('change', function () {
        // ctl00_ContentPlaceHolder1_
        if ("dblMinContract" == this.id) {
            $('span#minContract').html(this.value);
        }
        else {
            $('span#maxContract').html(this.value);
        }
    });
});
