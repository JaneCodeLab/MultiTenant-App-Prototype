function openModal(id, remained, completed) {
    $('#myModal #Id').val(id);
    $('#myModal #Remained').val(remained);
    $('#myModal #Completed').val(completed);
    $('#myModal #modalTitle').text((id === '0') ? createTitle : editTitle);
    $('#myModal').modal('show');
};

$("#saveForm").submit(function (e) {
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: "POST",
        url: "/MyTasks/Save",
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
