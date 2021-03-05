using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Tarefas.Models;
using Newtonsoft.Json;
using System.Security.Permissions;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.pdf.parser;
using SautinSoft;
using Microsoft.VisualBasic;
using SelectPdf;

namespace Tarefas.Controllers
{
    public class GerenciadorArquivoController : Controller
    {
        // GET: GerenciadorArquvio
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Upload()
        {
            try
            {
                var C = @"C:\";
                // Dar permissão de acesso ao disco C
                FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.AllAccess, C);

                var classeArquivo = new List<GerenciadorArquivoModel>();

                //diretório que será salvo temporariamente os arquivos
                var path = Server.MapPath("~/Upload/");

                //Verifica se o diretório existe
                if (Directory.Exists(path))
                    //Deleta o diretório e os arquivos
                    Directory.Delete(path, true);


                //Cria o diretório
                Directory.CreateDirectory(path);

                //Percorre os arquivos
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    //Pega o arquivo
                    var arquivo = Request.Files[i];

                    //Pega o nome do arquivo
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(arquivo.FileName).Replace(" ", "");

                    //Pega a extensão do arquivo
                    var extensao = System.IO.Path.GetExtension(arquivo.FileName).ToLower();

                    //Salva o arquivo
                    arquivo.SaveAs(path + fileName + extensao);

                    //Adiciona o arquivo na classe
                    classeArquivo.Add(new GerenciadorArquivoModel(fileName, extensao));
                }

                //Pega o total de arquivos
                var total = classeArquivo.Count;

                //Agrupa os arquivos por extensão
                var retorno = classeArquivo.GroupBy(e => e.Extensao).ToList();

                return Json(new { sucesso = true, Dados = retorno, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { sucesso = false, mensagem = e.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult Listar(string extensao)
        {
            try
            {

                var path = Server.MapPath("~/Upload/");

                // Dar permissão de acesso ao diretório
                FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.AllAccess, path);

                var Arquivo = new List<GerenciadorArquivoModel>();

                //Pega os arquivos de acordo com o parametro
                var files = Directory.GetFiles(path, "*" + extensao);

                foreach (var item in files)
                {
                    var fileName = System.IO.Path.GetFileName(item);

                    //Coloca o caminho lógico para que possa ser acessado
                    var caminho = "/Upload/" + fileName;

                    //Adiciona na classe
                    Arquivo.Add(new GerenciadorArquivoModel(fileName, extensao, caminho, item));
                }

                //verifica se a extensão é pdf
                if (extensao == ".pdf")
                {
                    //chama a função de validação de pdf
                    Arquivo = CheckPdf(Arquivo);

                }

                return Json(new { sucesso = true, Arquivo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { sucesso = false, mensagem = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<GerenciadorArquivoModel> CheckPdf(List<GerenciadorArquivoModel> arquivos)
        {
            try
            {
                foreach (var item in arquivos)
                {
                    #region
                    ////Abre o arquivo pdf
                    //using (PdfReader leitor = new PdfReader(item.CaminhoFisico))
                    //{
                    //    //Cria uma String
                    //    StringBuilder texto = new StringBuilder();
                    //    for (int i = 1; i <= leitor.NumberOfPages; i++)
                    //    {
                    //        //Adiciona o texto que extraiu do pdf
                    //        texto.Append(PdfTextExtractor.GetTextFromPage(leitor, i));
                    //    }
                    #endregion

                    // instantiate a pdf to text converter object
                    PdfToText pdfToText = new PdfToText();

                    // load PDF file
                    pdfToText.Load(item.CaminhoFisico);

                    // set the properties
                    pdfToText.Layout = TextLayout.Original;
                    pdfToText.StartPageNumber = 1;
                    pdfToText.EndPageNumber = 0;

                    // extract the text
                    string text = pdfToText.GetText();

                    string freetrial = "===========================================================================================================\r\n\r\nYou are currently using Demo Version - Select.Pdf SDK. With the free trial version,\r\nonly the first 3 pages of the PDF document are converted to text.\r\n\r\n===========================================================================================================\r\n\r\n\r\n\r\nDemo Version - Select.Pdf SDK - http://selectpdf.com\r\n";

                    if (text == freetrial)
                        text = "";
                    //transforma em string
                    var textopdf = text.ToString();

                    //char teste = ControlChars.Lf;

                    //var vamotropa = text.Split(teste);

                    //verifica se veio algum texto do pdf
                    if (textopdf != "")
                    {
                        //Valida como pdf e insere o texto na classe
                        item.Pdf = true;
                        item.Texto = text;

                    }

                }

                return arquivos;
            }
            catch (Exception ex)
            {
                return new List<GerenciadorArquivoModel>();
            }
        }
    }
}