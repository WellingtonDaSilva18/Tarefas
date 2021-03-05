using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarefas.Models
{
    public class GerenciadorArquivoModel
    {
        public GerenciadorArquivoModel(string nome, string extensao)
        {
            Nome = nome;
            Extensao = extensao;
        }

        public GerenciadorArquivoModel()
        {
        }

        public string Nome { get; set; }
        public string Extensao { get; set; }
    }
}