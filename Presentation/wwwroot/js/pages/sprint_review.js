function openModal(id, remained, completed) {
    $('#myModal #Id').val(id);
    $('#myModal #Remained').val(remained);
    $('#myModal #Completed').val(completed);
    $('#myModal #modalTitle').text(editTitle);
    $('#myModal').modal('show');
};

function openDescModal(id) {
    $.ajax({
        type: "GET",
        url: "/SprintReview/GetDescription?id=" + id,
        success: function (data) {
            $('#task-description').html(data);
            $('#descriptionModal').modal('show');
        },
    });

};


$("#saveForm").submit(function (e) {
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: "POST",
        url: "/SprintReview/Save",
        data: form.serialize(),
        success: function (data) {
            if (data.Type == 1) {
                window.location.reload();
            }
            else if (data.Type == 2) {
                console.log(data);
                Swal.fire({
                    icon: 'error',
                    iconColor: 'crimson',
                    title: 'error',
                    html: data.Message,
                    confirmButtonText: 'OK',
                    confirmButtonColor: 'crimson',
                })
            }
            else {
                console.log(data);
            }
        },
    });
});
