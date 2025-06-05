
function changeDepartment(departmentId) {
    window.location.href = '/Backlog?departmentId=' + departmentId;
};
function deleteConfirm(id, isIssue) {
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
            var address = isIssue ? "Issue" : "SprintTask";
            $.ajax({
                type: 'POST',
                url: '/' + address + '/Delete',
                data: { id: id },
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
                }
            });
        }
    });
}

function changeSprintConfirm(id) {
    Swal.fire({
        icon: 'question',
        iconColor: 'crimson',
        title: addToCurrentSprintTitle,
        html: sureToAddToCurrentSprint,
        showCancelButton: true,
        confirmButtonText: yesSureButton,
        confirmButtonColor: 'crimson',
        cancelButtonText: noButton,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: '/Backlog/AddToCurrentSprint',
                data: { id: id },
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
                }
            });
        }
    });
}
