"use strict";

(function () {

    var app = angular.module("dominioApp", []);
    var urlApi = "http://localhost:8800/api/Dominio";
    var direcaoOrdem = "+";
    var campoOrdem = "";

    var modalExclusao = document.getElementById("modalExclusao");
    var btnCancelar = document.getElementById("btnCancelar");
    var btnAlterar = document.getElementById("btnAlterar");
    var btnIncluir = document.getElementById("btnIncluir");

    var MainController = function ($scope, $http, $interval) {

        $scope.TipoDominio = [
            { Codigo: "", Nome: "(Selecione)" },
            { Codigo: "1", Nome: "Técnico" },
            { Codigo: "2", Nome: "Funcional" }
        ];

        $scope.Limpar = function () {
            $scope.MensagemInfo = null;
            $scope.MensagemErro = null;
            $scope.MensagemSucesso = null;
            $scope.NovoDominio = null;

            $scope.frmDominio.$setPristine();
            
            btnCancelar.style.display = "none";
            btnAlterar.style.display = "none";
            btnIncluir.style.display = "block";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroConsultar = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemErro = "Erro ao consultar do domínios. Entre em contato com o administrador do sistema.";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroInserir = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemInfo = null;
            $scope.MensagemSucesso = null;
            $scope.MensagemErro = "Erro ao realizar a inclusão do domínio.";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroAlterar = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemInfo = null;
            $scope.MensagemSucesso = null;
            $scope.MensagemErro = "Erro ao realizar a alteração do domínio " + $scope.NovoDominio.Nome + ".";
        };

        //Tratamento de erro - caso ocorra.
        var TratarErroExcluir = function (reason) {
            $scope.PreloaderFlag = null;
            $scope.MensagemInfo = null;
            $scope.MensagemSucesso = null;
            $scope.MensagemErro = "Erro ao realizar a exclusão do domínio " + $scope.NomeExclusao + ".";
            modalExclusao.style.display = "none";
        };

        //Realiza a chamada (GET) na API endereça os métodos de retorno e erro.
        $scope.Consultar = function () {
            $scope.PreloaderFlag = true;
            $http.get(urlApi).then(function (response) {
                $scope.Dominio = response.data;
                $scope.PreloaderFlag = null;
            }, TratarErroConsultar);
        };

        //Realiza a chamada (DELETE) na API endereça os métodos de retorno e erro.
        $scope.Excluir = function () {
            $scope.PreloaderFlag = true;
            var urlApiDelete = urlApi + "/?codigo=" + $scope.CodigoExclusao;
            $http.delete(urlApiDelete).then(function (response) {
                modalExclusao.style.display = "none";
                $scope.Limpar();
                $scope.Consultar();
                $scope.MensagemInfo = "O domínio " + $scope.NomeExclusao + " foi excluído com sucesso!";
            }, TratarErroExcluir);
        };

        //Realiza a chamada (POST) na API endereça os métodos de retorno e erro.
        $scope.Inserir = function () {
            $scope.PreloaderFlag = true;
            $http.post(urlApi, $scope.NovoDominio, {
                headers: { "Content-Type": "application/json" }
            }).then(function (response) {
                $scope.Limpar();
                $scope.Consultar();
                $scope.MensagemSucesso = "Domínio foi incluído com sucesso!";
            }, TratarErroInserir);
        };

        //Realiza a chamada (PUT) na API endereça os métodos de retorno e erro.
        $scope.Alterar = function () {
            $scope.PreloaderFlag = true;
            $http.put(urlApi, $scope.NovoDominio, {
                headers: { "Content-Type": "application/json" }
            }).then(function (response) {
                $scope.Limpar();
                $scope.Consultar();
                $scope.MensagemInfo = "Domínio foi alterado com sucesso!";
            }, TratarErroAlterar);
        };

        $scope.IniciarExclusao = function (codigo, nome) {
            modalExclusao.style.display = "block";
            $scope.NomeExclusao = nome;
            $scope.CodigoExclusao = codigo;
        };

        $scope.IniciarAlteracao = function (codigo, nome, codigoTipo) {
            $scope.NovoDominio = {
                Codigo: codigo,
                Nome: nome,
                CodigoTipo: codigoTipo
            };

            btnCancelar.style.display = "block";
            btnAlterar.style.display = "block";
            btnIncluir.style.display = "none";
        };

        //Realiza a ordenação dos campos no grid.
        $scope.SortOrder = function (ordem) {
            if (campoOrdem !== ordem)
                campoOrdem = ordem;

            if (direcaoOrdem === "+")
                direcaoOrdem = "-";
            else
                direcaoOrdem = "+";

            $scope.OrdenacaoDominio = direcaoOrdem + campoOrdem;
        };

        //Valor default para a inicialização da ordenação no grid.
        $scope.OrdenacaoDominio = "-Codigo";

        //Inicializa objeto principal da tela.
        $scope.Dominio = null;

        //Inicializa propriedade (flag) responsável por exibir o preloader.
        $scope.PreloaderFlag = null;

        //Realiza a chamada na api assim que a página é carregada.
        $scope.Consultar();
    };

    //Configura a aplicação Angular e define os serviços utilizados.
    app.controller("MainController", ["$scope", "$http", "$interval", MainController]);

}());