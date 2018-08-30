"use strict";

(function () {

    var app = angular.module("ajusteApp", []);
    var urlApi = "http://localhost:8800/api/ajustes/";
    var btnIncluir = document.getElementById("btnIncluir");
    var btnAlterar = document.getElementById("btnAlterar");

    var MainController = function ($scope, $http, $interval) {

        $scope.GrupoResponsavel = [
            { Codigo: "", Nome: "(Selecione)" },
            { Codigo: "1", Nome: "Grupo 1" },
            { Codigo: "2", Nome: "Grupo 2" },
            { Codigo: "3", Nome: "Grupo 3" },
            { Codigo: "4", Nome: "Grupo 4" }
        ];

        //Inicializa propriedade (flag) responsável por exibir o preloader.
        $scope.PreloaderFlag = null;

        $scope.Limpar = function () {
            $scope.MensagemInfo = null;
            $scope.MensagemErro = null;
            $scope.MensagemSucesso = null;
            $scope.NovoAjuste = null;

            $scope.frmAjuste.$setPristine();

            btnAlterar.style.display = "none";
            btnIncluir.style.display = "block";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroConsultar = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemErro = "Erro ao consultar os ajustes. Entre em contato com o administrador do sistema.";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroInserir = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemInfo = null;
            $scope.MensagemSucesso = null;
            $scope.MensagemErro = "Erro ao realizar a inclusão do ajuste.";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroAlterar = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemInfo = null;
            $scope.MensagemSucesso = null;
            $scope.MensagemErro = "Erro ao realizar a alteração do ajuste " + $scope.NovoAjuste.Nome + ".";
        };

        //Realiza a chamada (POST) na API endereça os métodos de retorno e erro.
        $scope.Inserir = function () {
            $scope.PreloaderFlag = true;

            $scope.NovoAjuste.CodigoSituacao = 13;
            $scope.NovoAjuste.DataManutencao = new Date().toLocaleString();
            $scope.NovoAjuste.CodigoUsuarioManutencao = "ONOFREJ";
            
            //if (rdbPeriodicidade.SelectedValue != "N")
            //{
            //    voMotivoAjuste.DataFimVigenciaAjuste = Convert.ToDateTime(txtVigenciaAte.Text);
            //    voMotivoAjuste.DataInicioVigenciaAjuste = Convert.ToDateTime(txtVigenciaDe.Text);
            //    voMotivoAjuste.DiaUtilExecucaoAjuste = Convert.ToInt32(txtDataExecucao.Text);
            //}
            
            $http.post(urlApi, $scope.NovoAjuste, {
                headers: { "Content-Type": "application/json" }
            }).then(function (response) {
                $scope.Limpar();
                $scope.MensagemSucesso = "Ajuste foi incluído com sucesso!";
                $scope.PreloaderFlag = null;
            }, TratarErroInserir);
        };

        //Realiza a chamada (PUT) na API endereça os métodos de retorno e erro.
        $scope.Alterar = function () {
            $scope.PreloaderFlag = true;
            $http.put(urlApi, $scope.NovoAjuste, {
                headers: { "Content-Type": "application/json" }
            }).then(function (response) {
                $scope.Limpar();
                $scope.MensagemInfo = "Ajuste foi alterado com sucesso!";
                $scope.PreloaderFlag = null;
            }, TratarErroAlterar);
        };

        $scope.IniciarAlteracao = function (numeroMotivoAjuste ) {
            $scope.PreloaderFlag = true;

            var urlApiPesquisar = urlApi + "?numeroMotivoAjuste=" + numeroMotivoAjuste +
                "&codigoSituacao=&nomeMotivoAjuste=&codigoGrupoResponsavel=&indicadorPeriodicidade=&racf=";

            $http.get(urlApiPesquisar).then(function (response) {
                $scope.NovoAjuste = response.data.ModelList[0];
                $scope.PreloaderFlag = null;
                btnAlterar.style.display = "block";
                btnIncluir.style.display = "none";
                window.sessionStorage.removeItem("NumeroMotivoAjuste");
            }, TratarErroConsultar);
        };

        //Valida se existe um item a ser alterado.
        if (window.sessionStorage.getItem("NumeroMotivoAjuste"))
            $scope.IniciarAlteracao(window.sessionStorage.getItem("NumeroMotivoAjuste"));
    };

    //Configura a aplicação Angular e define os serviços utilizados.
    app.controller("MainController", ["$scope", "$http", "$interval", MainController]);

}());