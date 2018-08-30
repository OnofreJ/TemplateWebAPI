"use strict";

(function () {

    var app = angular.module("ajusteApp", []);
    var urlApi = "http://localhost:8800/api/ajustes/";
    var urlApiPesquisar = null;
    var direcaoOrdem = "+";
    var campoOrdem = "";
    var modalExclusao = document.getElementById("modalExclusao");

    var MainController = function ($scope, $http, $interval) {

        //Valor default para a inicialização da ordenação no grid.
        $scope.OrdenacaoAjuste = "-Codigo";

        //Inicializa objetos da tela.
        $scope.Ajuste = null;
        $scope.CodigoSituacao = "";
        $scope.CodigoGrupoResponsavel = "";
        $scope.IndicadorPeriodicidade = "";

        //Inicializa propriedade (flag) responsável por exibir o preloader.
        $scope.PreloaderFlag = null;

        //Tratamento de erro - caso ocorra.
        var TratarErroConsultar = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemErro = "Erro ao consultar os ajustes. Entre em contato com o administrador do sistema.";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroExcluir = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemInfo = null;
            $scope.MensagemSucesso = null;
            $scope.MensagemErro = "Erro ao realizar a exclusão do ajuste " + $scope.NomeExclusao + ".";
            modalExclusao.style.display = "none";
        };

        $scope.Status = [
            { Codigo: "", Nome: "(Selecione)" },
            { Codigo: "14", Nome: "Executado" },
            { Codigo: "13", Nome: "Pendente" },
            { Codigo: "18", Nome: "Capturando" }
        ];

        $scope.Periodicidade = [
            { Codigo: "", Nome: "(Selecione)" },
            { Codigo: "S", Nome: "Periodicos" },
            { Codigo: "N", Nome: "Pontuais" }
        ];

        $scope.GrupoResponsavel = [
            { Codigo: "", Nome: "(Selecione)" },
            { Codigo: "1", Nome: "Grupo 1" },
            { Codigo: "2", Nome: "Grupo 2" },
            { Codigo: "3", Nome: "Grupo 3" },
            { Codigo: "4", Nome: "Grupo 4" }
        ];

        $scope.Limpar = function () {
            $scope.MensagemInfo = null;
            $scope.MensagemErro = null;
            $scope.MensagemSucesso = null;

            $scope.frmAjuste.$setPristine();
        };

        //Realiza a chamada (GET) na API endereça os métodos de retorno e erro.
        $scope.Consultar = function () {
            $scope.PreloaderFlag = true;
            $http.get(urlApiPesquisar ? urlApiPesquisar : urlApi).then(function (response) {
                $scope.Ajuste = response.data.ModelList;
                $scope.PreloaderFlag = null;
            }, TratarErroConsultar);
        };

        //Realiza a chamada (GET) na API endereça os métodos de retorno e erro.
        $scope.Pesquisar = function () {
            urlApiPesquisar = urlApi +
                "?numeroMotivoAjuste=&codigoSituacao=" + $scope.CodigoSituacao +
                "&nomeMotivoAjuste=&codigoGrupoResponsavel=" + $scope.CodigoGrupoResponsavel +
                "&indicadorPeriodicidade=" + $scope.IndicadorPeriodicidade + "&racf=";

            $scope.PreloaderFlag = true;
            $http.get(urlApiPesquisar).then(function (response) {
                $scope.Ajuste = response.data.ModelList;
                $scope.PreloaderFlag = null;
            }, TratarErroConsultar);
        };

        //Realiza a chamada (DELETE) na API endereça os métodos de retorno e erro.
        $scope.Excluir = function () {
            $scope.PreloaderFlag = true;
            var urlApiDelete = urlApi + "/?numeroMotivoAjuste=" + $scope.CodigoExclusao;
            $http.delete(urlApiDelete).then(function (response) {
                modalExclusao.style.display = "none";
                $scope.Limpar();
                $scope.Consultar();
                $scope.MensagemInfo = "O ajuste " + $scope.NomeExclusao + " foi excluído com sucesso!";
            }, TratarErroExcluir);
        };

        $scope.IniciarExclusao = function (codigo, nome) {
            modalExclusao.style.display = "block";
            $scope.NomeExclusao = nome;
            $scope.CodigoExclusao = codigo;
        };

        $scope.IniciarAlteracao = function (numeroMotivoAjuste) {
            if (window.sessionStorage) {
                window.sessionStorage.setItem("NumeroMotivoAjuste", numeroMotivoAjuste);
                window.location.href = "Ajuste/Novo";
            }
            else {
                $scope.MensagemErro = "Erro ao realizar a alteração do ajuste: seu navegador não suporta alguns componentes.";
            }
        };

        //Realiza a ordenação dos campos no grid.
        $scope.SortOrder = function (ordem) {
            if (campoOrdem !== ordem)
                campoOrdem = ordem;

            if (direcaoOrdem === "+")
                direcaoOrdem = "-";
            else
                direcaoOrdem = "+";

            $scope.OrdenacaoAjuste = direcaoOrdem + campoOrdem;
        };

        //Realiza a chamada na api assim que a página é carregada.
        $scope.Consultar();
    };

    //Configura a aplicação Angular e define os serviços utilizados.
    app.controller("MainController", ["$scope", "$http", "$interval", MainController]);

}());