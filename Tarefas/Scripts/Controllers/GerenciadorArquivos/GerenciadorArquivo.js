$("#ctrl").change(function (e) {
    //Limpa a tabela
    $("#conteudo tr").remove();
    $("#qtdArquivos").empty();

    var arquivo = new FormData();

    for (var i = 0; i < e.currentTarget.files.length; i++) {

        //Põe todos os arquivos em uma lista
        arquivo.append(e.currentTarget.files[i].name, e.currentTarget.files[i]);
    }
    EnviarArquivo(arquivo);
});

//Envia os arquivos para serem tratados na controller
function EnviarArquivo(arquivo) {

    //Exibe o load na tela
    $('.load').modal();

    $.ajax({
        url: '/GerenciadorArquivo/Upload',
        data: arquivo,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (data) {
            if (data.sucesso && data.Dados.length > 0) {
                MontarHtml(data.Dados, data.total); //Chama a função para montar a tabela

            }
            else {
                toastr.error("Vish, deu ruim");
                //Fechar o load caso der ruim
                $('.load').modal('hide');
            }
        }
    });
}

//Função para montar a Tabela
function MontarHtml(arquivosOrder, total) {
    var html = '', count = 1;

    //Percorre os agrupamentos devolvidos pela controller
    arquivosOrder.forEach(function (values) {
        var ex, qtd;

        //pega a extensao e o total de cada agrupamento
        ex = values[0].Extensao;
        qtd = values.length;

        //Cria as linhas da tabelas
        html += "<tr><th>" + count + "</th><td>" + qtd + "</td><td>" + ex + "</td><td><button id=" + ex + " class='btn btn-sm btn-outline-roxo' onclick='Listar(this)'>Listar Arquivo(s)</button></tr>";
        count = count + 1;
    });
    //Seta os dados na tabela
    $("#conteudo").append(html);
    //Seta o total de arquivos
    $("#qtdArquivos").append("Total: " + total);
    //Exibi a tabela
    $(".collapse").collapse('show');

    //Fecha o load quando da bom
    $('.load').modal('hide');
}

//Função para a fazer a requisição dos arquivos
function Listar(e) {
    $('.load').modal();
    var extensao = e.id;
    $.ajax({
        url: "/GerenciadorArquivo/Listar",
        type: 'POST',
        data: { extensao: extensao },
        success: function (data) {
            if (data.sucesso) {

                //Chama a função que joga os arquivos na tela
                ListarModal(data.Arquivo);
                $('.load').modal('hide');
            }
            else
                toastr.error("Vish, Deu ruim...");
        }
    });
}

function ListarModal(Arquivo) {
    var html = '';

    //Limpa a modal
    $("#Arquivoaqui").empty();

    //Insere a lista de arquivos na variavel html
    Arquivo.forEach(function (e) {

        var caminho = e.Url;
        var nome = e.Nome;

        //verifica se a extensão é pdf
        if (e.Extensao == '.pdf')
            //Verifica se não é um pdf editável
            if (!e.Pdf)
                html += "<a title='Não foi possível ler o pdf' style='color: red !important;' href=" + caminho + " target='_blank' class='list-group-item btn-outline-roxo'>" + nome + "</a>";
            else {

                html += "<div class='input-group'><a href=" + caminho + " target='_blank' class='list-group-item btn-outline-roxo'>" + nome + "</a><div class='input-group-prepend'><button onclick='MostrarTexto(this)' data-id=" + e.Nome + " id='btn-text' class='btn btn-outline-roxo' type='button'>Texto</button></div><div style='display: none;'>" + e.Texto + "</div></div>";

                $('#textoaqui').append
            }
        else
            html += "<a href=" + caminho + " target='_blank' class='list-group-item btn-outline-roxo'>" + nome + "</a>";
    });
    //Insere a lista na modal
    $("#Arquivoaqui").append(html);

    //Exibe a modal
    $("#modal-ListarArquivos").modal();


}

function MostrarTexto(btn) {
    $("#modal-ListarArquivos").modal('hide');

    $("#textoaqui").empty();

    //Pega o texto do pdf
    var texto = btn.parentElement.parentElement.lastElementChild.innerHTML;

    $("#textoaqui").append(texto);

    $("#modal-text").modal();

}

