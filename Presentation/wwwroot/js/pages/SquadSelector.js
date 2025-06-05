function openSquadModal() {
    $('#squadId').empty();
    $.ajax({
        type: "GET",
        url: "/Home/GetSquads",
        success: function (data) {
            $.each(data, function (i, item) {
                $('#squadId').append($('<option>', {
                    value: item.Id,
                    text: item.Title
                }));
            });
        },
    });

    $('#SquadModal #modalTitle').text(SquadSelection);
    $('#SquadModal').modal('show');
};



$("#ChangeSquad").submit(function (e) {
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: "POST",
        url: "/SelectBranch/ChangeSquad",
        data: form.serialize(),
        success: function (data) {
            window.location.reload();
        },
    });
});
