function openModal(id, title, customerId) {
    $('#myModal #Id').val(id);
    $('#myModal #Title').val(title);
    if (customerId) {
        $("#CustomerId").select2("val", customerId);
    }
    else {
        $("#CustomerId").val("").change();
    }
    if (id === '') {
        $('#myModal #modalTitle').text(createTitle);
    }
    else {
        $('#myModal #modalTitle').text(editTitle);
    }
    $('#myModal').modal('show');
};

$("#saveForm").submit(function (e) {
    if (!$("#saveForm #Title").val() || !$("#saveForm #CustomerId").val()) {
        return false;
    }
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: "POST",
        url: "/Project/Save",
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
                url: '/Project/Delete',
                data: { id: id },
                success: function (result) {
                    window.location.reload();
                }
            });
        }
    });
}