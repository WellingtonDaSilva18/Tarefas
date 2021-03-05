$("#ctrl").change(function (e) {
    //Limpa a tabela
    $("#conteudo tr").remove();
    $("#qtdArquivos").empty();

    var arquivo = [];

    //Pega todos os arquivos
    var inputfiles = e.currentTarget.files;

    for (var i = 0; i < inputfiles.length; i++) {

        //Põe todos os arquivos em uma lista
        arquivo.push({ Nome: inputfiles[i].name })
    }
    EnviarArquivo(JSON.stringify(arquivo));
});

function EnviarArquivo(arquivo) {
    $.ajax({
        type: 'POST',
        url: '/GerenciadorArquivo/SetExtensao',
        data: { arquivo: arquivo },
        success: function (data) {
            if (data.sucesso && data.Dados.length > 0) {
                MontarHtml(data.Dados, data.total);

            }
            else
                toastr.error("Vish, deu ruim");
        }
    });
}

function MontarHtml(arquivosOrder, total) {
    var html = '', count = 1;

    //Percorre os agrupamentos devolvidos pela controller
    arquivosOrder.forEach(function (values) {
        var ex, qtd;

        //pega a extensao e o total de cada agrupamento
        ex = values[0].Extensao;
        qtd = values.length;

        //Cria as linhas da tabelas
        html += "<tr><th>" + count + "</th><td>" + qtd + "</td><td>" + ex + "</td></tr>";
        count = count + 1;
    });
    //Seta os dados na tabela
    $("#conteudo").append(html);
    //Seta o total de arquivos
    $("#qtdArquivos").append("Total: " + total);
    //Exibi a tabela
    $(".collapse").collapse('show');
}