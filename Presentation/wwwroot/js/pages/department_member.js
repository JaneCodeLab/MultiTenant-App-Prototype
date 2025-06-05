$(function () {
    if (no_records == 0) {
        openModal('0', '', '', '');
    }

    org_chart = $('#orgChart').orgChart({
        data: treeData,
        showControls: true,
        allowEdit: false,
        onAddNode: function (node) {
        },
        onDeleteNode: function (node) {
        },
        onClickNode: function (node) {
        }
    });
});

function changeView(isGrid) {
    if (isGrid) {
        $('#gridView').show();
        $('#treeView').hide();
    }
    else {
        $('#gridView').hide();
        $('#treeView').show();
    }
};

function changeDepartment(departmentId) {
    window.location.href = '/DepartmentMember?departmentId=' + departmentId;
};

function openModal(memberId, userId, supervisorId, roleId) {
    $('#myModal #Id').val(memberId);
    $('#myModal #UserId').val(userId).change();
    $('#myModal #SupervisorMemberId').val(supervisorId).change();
    $('#myModal #DepartmentRoleId').val(roleId).change();
    $('#myModal #modalTitle').text((memberId === '0') ? createTitle : editTitle);
    $('#myModal').modal('show');
};

$("#saveForm").submit(function (e) {
    if (!$("#saveForm #UserId").val() || !$("#saveForm #DepartmentRoleId").val()) {
        return false;
    }
    e.preventDefault();
    var form = $(this);
    $.ajax({
        type: "POST",
        url: "/DepartmentMember/Save",
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
                url: '/DepartmentMember/Delete',
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