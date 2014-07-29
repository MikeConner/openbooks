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
    var range = $('.input-range')
      , value = $('.range-value');

    value.html(range.attr('value'));

    range.on('change', function () {
        value.html(this.value);
    });



});
