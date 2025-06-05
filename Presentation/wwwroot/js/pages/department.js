function openModal(id, title) {
    $('#myModal #Id').val(id);
    $('#myModal #Title').val(title);
    $('#myModal #modalTitle').text((id === '0') ? createTitle : editTitle);
    $('#myModal').modal('show');
};

$("#saveForm").submit(function (e) {
    if (!$("#saveForm #Title").val()) {
        return false;
    }
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: "POST",
        url: "/Department/Save",
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

function deleteConfirm(id) {
    Swal.fire({
        icon: 'question',
        iconColor: 'crimson',
        title: deleteMboxTitle,
        html: sureToDelete,
        showCancelButton: true,
        confirmButtonText: yesDeleteButton,
        confirmButtonColor: 'crimson',
        cancelButtonText: noButton,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: '/Department/Delete',
                data: { id: id },
                success: function (result) {
                    window.location.reload();
                }
            });
        }
    });
}