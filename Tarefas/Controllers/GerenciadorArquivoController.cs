using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Tarefas.Models;
using Newtonsoft.Json;

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
        public JsonResult SetExtensao(string arquivo)
        {
            try
            {
                //reconstroi o parametro como GerenciadorArquivoModel
                List<GerenciadorArquivoModel> arquivoModels = JsonConvert.DeserializeObject<List<GerenciadorArquivoModel>>(arquivo);

                foreach (var item in arquivoModels)
                {
                    //Adiciona a Extensao a model
                    item.Extensao = item.Nome.Split('.').Last().ToLower();
                }
                //Define o total de arquivos
                var total = arquivoModels.Count;

                //Ordena por Extensao
                var arquivoReturn = arquivoModels.GroupBy(e => e.Extensao);

                return Json(new { sucesso = true, Dados = arquivoReturn, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }


        }
    }
}