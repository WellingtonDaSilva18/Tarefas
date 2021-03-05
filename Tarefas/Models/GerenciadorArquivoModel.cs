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
            Pdf = false;
        }

        public GerenciadorArquivoModel(string nome, string extensao, string url, string caminhofisico)
        {
            Nome = nome;
            Extensao = extensao;
            Url = url;
            CaminhoFisico = caminhofisico;
            Pdf = false;

        }

        public GerenciadorArquivoModel()
        {
            Pdf = false;
        }

        public string Nome { get; set; }
        public string Extensao { get; set; }
        public string Url { get; set; }
        public string CaminhoFisico { get; set; }
        public bool Pdf { get; set; }
        public string Texto { get; set; }
    }
}