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
    var contractMax = $('#MaxContractField').val();
    var stickyMin = $('#StickyMinContract').val();
    var stickyMax = $('#StickyMaxContract').val();

    if (($('#dblMaxContract').length > 0) && (undefined == $('#dblMaxContract').attr('max'))) {
        $('#dblMaxContract').attr('max', contractMax);
        $('#dblMaxContract').val(contractMax);
        $('span#maxContract').html(formatNumber(contractMax));
    }

    // Only have to check one value; they will either both be 0 or both have values
    if (stickyMin > 0) {
        $('#dblMinContract').val(stickyMin);
        $('span#minContract').html(formatNumber(stickyMin));
        $('#dblMaxContract').val(stickyMax);
        $('span#maxContract').html(formatNumber(stickyMax));
    }

    range.on('change', function () {
        // ctl00_ContentPlaceHolder1_
        if ("dblMinContract" == this.id) {
            $('span#minContract').html(formatNumber(this.value));
        }
        else {
            $('span#maxContract').html(formatNumber(this.value));
        }
    });
});

function formatNumber(num) {
    return undefined == num ? "" : num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}

// Disable individual field validation when uploading Contributions
function validateContributionUpload() {
    var element = document.getElementById('<%= txtAmount.ClientID %>');
    element.enabled = false;
    element = document.getElementById('<%= txtContributor.ClientID %>');
    element.enabled = false;

    return true;
}