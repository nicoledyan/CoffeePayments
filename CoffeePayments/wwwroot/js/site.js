// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


function confirmPay() {
    return confirm('Are you sure you want to mark this coworker as paid?');
}
function confirmSkip() {
    return confirm('Are you sure you want to skip this coworker?');
}
function deleteCoworker(id, name) {
    if (!confirm(`Are you sure you want to remove ${name}? This won't delete their payment history.`)) return;
    
    $.ajax({
        url: '/Coworker/RemoveCoworker',
        type: 'POST',
        data: {
            id: id,
        },
        success: function (html) {
            $('#coworker-list').html(html);
        },
        error: function () {
            alert('Failed to remove coworker.');
        }
    });
}

$(document).ready(function () {
    $('#add-coworker-form').on('submit', function (e) {
        e.preventDefault();
        
        const data = {
            name: $('#coworker-name').val(),
            favoriteDrink: $('#favorite-drink').val(),
            drinkCost: $('#drink-cost').val(),
            joinDate: $('#join-date').val(),
        };

        $.ajax({
            url: '/Coworker/AddCoworker',
            type: 'POST',
            data: data,
            success: function (html) {
                $('#coworker-list').html(html);
                $('#add-coworker-form')[0].reset();
                $('#addCoworkerForm').collapse('hide');
            },
            error: function () {
                alert('Failed to add coworker.');
            }
        });
    });
});


