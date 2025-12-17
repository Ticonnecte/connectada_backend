function ViewModel() {
    var self = this;

    var tokenKey = 'accessToken';
    var expiration = 'expiresIn';

    self.result = ko.observable();
    self.user = ko.observable();


    //Cards
    self.cardId = ko.observable(2);
    self.cardGuid = ko.observable('0635a43c-c99d-48e0-86bd-38edcd2f0065');

    //reset
    self.resetUsername = ko.observable();

    //update
    self.newUserName = ko.observable();
    self.newEmail = ko.observable();
    self.oldPassword = ko.observable();
    self.newPassword = ko.observable();
    self.newPassword2 = ko.observable();


    //register
    self.userKey = ko.observable();
    self.name = ko.observable('Alexandre');
    self.userName = ko.observable('alex.moller@pmjg');
    self.registerEmail = ko.observable('alexandre@zenite.inf.br');
    self.registerPassword = ko.observable('');
    self.registerPassword2 = ko.observable('');
    self.cliente = ko.observable();
    self.role = ko.observable();
    self.profile = ko.observable();
    self.clienteOptions = ko.observableArray();
    self.rolesOptions = ko.observableArray();
    self.profileOptions = ko.observableArray();

    var ListObj = function (id, name) {
        this.id = id;
        this.name = name;
    };

    //var Role = function (id, name) {
    //    this.id = id;
    //    this.name = name;
    //}

    //var Cliente = function (id, name) {
    //    this.id = id;
    //    this.name = name;
    //}

    //var Profile = function (id, name) {
    //    this.id = id;
    //    this.name = name;
    //}

    //login
    self.loginUsername = ko.observable('alex.moller@pmjg');
    self.loginPassword = ko.observable('');
    self.tenant = ko.observable('pmjg');
    self.errors = ko.observableArray([]);

    function showError(jqXHR) {

        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState)
                {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    self.callApi = function () {
        self.result('');
        self.errors.removeAll();

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: '/api/values',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    };

    self.resetPass = function () {
        self.result('');
        self.errors.removeAll();
        debugger;
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        var messageHandle = new CustomMessageApp('msg-account');
        messageHandle.clear();

        $.ajax({
            type: 'PUT',
            url: '/api/Account/ResetPasswordAdminRequest?userName=' + self.resetUsername(),
            contentType: 'application/json; charset=utf-8',
            headers: headers,

        }).done(function (data) {
            messageHandle.displayMessage(JSON.stringify(data), 'success', -1);

        }).fail(function (jqXHR, textStatus, errorThrown) {
            debugger;
            messageHandle.displayMessage('Erro {0}. Details: {1}.'.format(jqXHR.status, jqXHR.responseText), 'danger', -1);
        });
    };

    self.updatePass = function () {
        self.result('');
        self.errors.removeAll();
        debugger;
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        var messageHandle = new CustomMessageApp('msg-account');
        messageHandle.clear();

        var data = {};
        data["OldPassword"] = self.oldPassword();
        data["NewPassword"] = self.newPassword();
        data["ConfirmPassword"] = self.newPassword2();

        $.ajax({
            type: 'PUT',
            url: '/api/Account/UpdatePass',
            contentType: 'application/json; charset=utf-8',
            headers: headers,
            data: JSON.stringify(data)
        }).fail(function (jqXHR, textStatus, errorThrown) {
            debugger;
            messageHandle.displayMessage('Erro {0}. Details: {1}.'.format(jqXHR.status, jqXHR.responseText), 'danger', -1);
        });
    };

    self.updateInfo = function () {
        self.result('');
        self.errors.removeAll();
        debugger;
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        var messageHandle = new CustomMessageApp('msg-account');
        messageHandle.clear();

        var data = {};
        data["NewUserName"] = "";
        data["NewEmail"] = self.newEmail();

        $.ajax({
            type: 'PUT',
            url: '/api/Account/UpdateInfo',
            contentType: 'application/json; charset=utf-8',
            headers: headers,
            data: JSON.stringify(data)
        }).fail(function (jqXHR, textStatus, errorThrown) {
            debugger;
            messageHandle.displayMessage('Erro {0}. Details: {1}.'.format(jqXHR.status, jqXHR.responseText), 'danger', -1);
        });
    };

    self.register = function () {
        debugger;
        self.result('');
        self.errors.removeAll();
        var messageHandle = new CustomMessageApp('msg-Cadastros');
        messageHandle.clear();

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var data = {
            Nome: self.name(),
            UserName: self.userName(),
            Email: self.registerEmail(),
            Senha: self.registerPassword(),
            //ConfirmPassword: self.registerPassword2(),
            //ClienteKey: self.cliente(),
            RoleName: self.role()
        };

        $.ajax({
            type: 'POST',
            url: '/api/Account/Register',
            contentType: 'application/json; charset=utf-8',
            headers: headers,
            data: JSON.stringify(data)
        }).done(function (data) {
            self.result("Done!");
            messageHandle.displayMessage('Usuário {0} adicionado com sucesso.'.format(self.name()), 'success', -1);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            debugger;
            messageHandle.displayMessage('Erro {0}. Details: {1}.'.format(jqXHR.status, jqXHR.responseText), 'danger', -1);
        });
    };

    self.login = function () {
        self.result('');
        self.errors.removeAll();
        var messageHandle = new CustomMessageApp('msg-security');
        messageHandle.clear();
        var loginData = {
            grant_type: 'password',
            username: self.loginUsername(),
            password: self.loginPassword(),
            tenant: self.tenant()
        };
        debugger;
        $.ajax({
            type: 'POST',
            url: '/Token',
            contentType: "application/x-www-form-urlencoded",
            data: loginData
        }).done(function (data) {
            debugger;
            self.user(data.userName);
            console.log(data.access_token);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);
            var mEpoch = parseInt(data.expires_in);
            //if (mEpoch < 10000000000) mEpoch *= 1000; 
            var eDate = new Date();
            eDate.setSeconds(eDate.getSeconds() + mEpoch);
            sessionStorage.setItem(expiration, eDate);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            debugger;
            messageHandle.displayMessage('Erro {0}. Details: {1}.'.format(jqXHR.status, jqXHR.responseText), 'danger', -1);
        });
    };

    self.getRegisterOptions = function (option, role, cliente) {

        var messageHandle = new CustomMessageApp('msg-message');
        messageHandle.clear();
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        debugger;
        switch (option) {

            case 1:
                $.ajax({
                    type: 'GET',
                    url: '/api/account/getallroles',
                    contentType: "application/x-www-form-urlencoded",
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    headers: headers
                }).done(function (data) {
                    debugger;
                    self.rolesOptions(data);
                }).fail(showError);
                break;

            case 2:
                $.ajax({
                    type: 'GET',
                    url: '/api/cliente/getalllisted',
                    contentType: "application/x-www-form-urlencoded",
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    headers: headers
                }).done(function (data) {
                    debugger;
                    self.clienteOptions.removeAll();
                    data.forEach(function (result) {
                        self.clienteOptions.push(new ListObj(result['Value'], result['Text']));
                    });
                    document.getElementById('getProfilesButton').style.visibility = 'visible';
                    document.getElementById('getVehiclesButton').style.visibility = 'visible';
                })
                break;

            case 3:

                $.ajax({
                    type: 'GET',
                    url: '/api/perfil/getprofileslisted?role=' + role + '&client=' + cliente,
                    contentType: "application/x-www-form-urlencoded",
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    headers: headers
                }).done(function (data) {
                    debugger;
                    self.profileOptions.removeAll();
                    data.forEach(function (result) {
                        self.profileOptions.push(new ListObj(result['Value'], result['Text']));

                    });
                })

                break;

            case 4:
                $.ajax({
                    type: 'GET',
                    url: '/api/veiculo/GetVehiclesListed?role=' + role + '&cliente=' + cliente,
                    contentType: "application/x-www-form-urlencoded",
                    //contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    headers: headers
                }).done(function (data) {
                    debugger;
                    self.vehicleOptions.removeAll();
                    data.forEach(function (result) {
                        self.vehicleOptions.push(new ListObj(result['Value'], result['Text']));

                    });
                })
                break;
        }
    };

    self.logout = function () {
        // Log out from the cookie based logon.
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'POST',
            url: '/api/Account/Logout',
            contentType: "application/x-www-form-urlencoded",
            headers: headers
        }).done(function (data) {
            // Successfully logged out. Delete the token.
            $('#tenant-name').html('');
            $('#tenant-logo').prop('src', '#');
            $('#store-list').html('');
            self.user('');
            sessionStorage.removeItem(tokenKey);
        }).fail(showError);
    };



    // WA APi...    
    self.phone = ko.observable('5581973189341');
    self.checkPhone = ko.observable(false);
    self.message = ko.observable('Olá, ~{Assinante.Nome}!\n\n*EDUCAÇÃO* ✍️ Inscrições para o SSA da UPE têm início na próxima segunda https://bit.ly/2PSm5B3 \n\n*CULTURA*  🎬 Projeto de formação para cineastas negros tem inscrições abertas até o dia 20 https://bit.ly/3kI9b6Q \n\n*ISSO É UM TESTE!!*\nDesculpa!\nFavor desconsiderar, old news.');
    self.barramento = ko.observable(8);
    self.sendMessage = function () {
        self.result('');
        self.errors.removeAll();
        var messageHandle = new CustomMessageApp('msg-message');
        messageHandle.clear();
        var messageData = {
            //phone: self.phone(),
            BusSize: self.barramento(),
            CheckExists: self.checkPhone,
            Text: self.message()
        };
        $.ajax({
            type: 'POST',
            url: '/api/cliente/sendtextmessage',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(messageData)
        }).done(function (data) {
            console.log(data);
            messageHandle.displayMessage('Mensagem enviada com sucesso.\n' + data.toString(), 'success', -1);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            messageHandle.displayMessage('Erro {0}. Details: {1}.'.format(jqXHR.status, jqXHR.responseText), 'danger', -1);
        });
    };

    self.getClientesCount = function () {
        self.result('');
        self.errors.removeAll();
        var messageHandle = new CustomMessageApp('msg-message');
        messageHandle.clear();
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        $.ajax({
            type: 'GET',
            url: '/api/cliente/assinantescount',
            contentType: "application/x-www-form-urlencoded",
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            headers: headers
        }).done(function (data) {
            console.log(data);
            messageHandle.displayMessage(`Total de '${data}' cliente(s).`, 'success', -1);
        }).fail(showError);
    };

    self.getContact = function () {
        self.result('');
        self.errors.removeAll();
        var messageHandle = new CustomMessageApp('msg-message');
        messageHandle.clear();
        //var token = sessionStorage.getItem(tokenKey);
        //var headers = {};
        //if (token) {
        //    headers.Authorization = 'Bearer ' + token;
        //}
        $.ajax({
            type: 'GET',
            url: `/api/cliente/getcontact?phone=${self.phone()}`,
            contentType: "application/x-www-form-urlencoded",
            //contentType: "application/json; charset=utf-8",
            dataType: "json"
            //headers: headers
        }).done(function (data) {
            console.log(data);
            messageHandle.displayMessage(data.toString(), 'success', -1);
        });
    };

    self.getContact = function () {
        self.result('');
        self.errors.removeAll();
        var messageHandle = new CustomMessageApp('msg-message');
        messageHandle.clear();
        //var token = sessionStorage.getItem(tokenKey);
        //var headers = {};
        //if (token) {
        //    headers.Authorization = 'Bearer ' + token;
        //}
        $.ajax({
            type: 'GET',
            url: `/api/cliente/getcontact?phone=${self.phone()}`,
            contentType: "application/x-www-form-urlencoded",
            //contentType: "application/json; charset=utf-8",
            dataType: "json"
            //headers: headers
        }).done(function (data) {
            console.log(data);
            messageHandle.displayMessage(data.toString(), 'success', -1);
        });
    };


}

function deleteCookie(name) {
    if (searchCookie(name)) {
        document.cookie = name + "=" + "; expires=Thu, 01-Jan-70 00:00:01 GMT";
    }
}

function searchCookie(name) {
    var cookies = document.cookie;
    var prefix = name + "=";
    var begin = cookies.indexOf("; " + prefix);

    if (begin === -1) {

        begin = cookies.indexOf(prefix);

        if (begin !== 0) {
            return null;
        }

    } else {
        begin += 2;
    }

    var end = cookies.indexOf(";", begin);

    if (end === -1) {
        end = cookies.length;
    }

    return decodeURI(cookies.substring(begin + prefix.length, end));
}

var app = new ViewModel();
ko.applyBindings(app);