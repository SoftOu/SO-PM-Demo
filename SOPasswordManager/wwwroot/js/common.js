function showNotification(from, align, msg) {
    color = 'success';

    $.notify({
        icon: "now-ui-icons ui-1_bell-53",
        message: msg

    }, {
            type: color,
            timer: 2000,
            placement: {
                from: from,
                align: align
            }
        });
}

function showErrorNotification(from, align, msg) {

    color = 'danger';

    $.notify({
        icon: "now-ui-icons ui-1_bell-53",
        message: msg

    }, {
            type: color,
            timer: 4000,
            placement: {
                from: from,
                align: align
            }
        });
}

//onclick =\"OpenPopup('divProjectUserAdd', 'myModalContent','" + url + "', 'AddProjectUserForm')\"> 
function OpenPopup(modalId, modalContainId, url, FormId) {
    loaderstart();
    $('#' + modalContainId + '').empty();
    $('#' + modalContainId + '').load(url, function (response, status, xhr) {
        $('#' + modalId + '').modal('show');
        bindForm(this, modalId, modalContainId, FormId);
        loaderstop();
    });
}


function GetParameterValues(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}

function bindForm(dialog, modalId, modalContainid, FormId) {
    $('#' + FormId + '').submit(function () {
        if ($('#' + FormId + '').valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                crossDomain: true,
                success: function (html, status, xhr) {
                    if (html.success == true) {
                        $('#' + modalId + '').modal('hide');
                        if (html.url != null) {
                            location.href = html.url;
                        }
                        else {
                            location.reload();
                        }
                    } else {
                        $('#' + modalContainid + '').html(html);

                        bindForm(dialog, modalId, modalContainid, FormId);

                    }
                },
                error: function (e) {
                    console.log(e);
                }
            });
        }
        return false;
    });
}

function loaderstart() {
    $("#loading-image").show();   
}
function loaderstop() {    
    $("#loading-image").hide();
}