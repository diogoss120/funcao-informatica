using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model)
        {
            var bo = new BoBeneficiario();

            var cpfExistente = CpfValidador.VerificarExistencia(model.Cpf.GetCpfLimpo());
            if (cpfExistente) ModelState.AddModelError("Cpf", "O CPF informado já está sendo usado");

            var cpfValido = CpfValidador.ValidarCpf(model.Cpf.GetCpfLimpo());
            if (!cpfValido) ModelState.AddModelError("Cpf", "O CPF informado é inválido");

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            var beneficiario = new Beneficiario
            {
                Nome = model.Nome,
                Cpf = model.Cpf
            };
            model.Id = bo.Incluir(beneficiario);

            return Json("Cadastro de beneficiário efetuado com sucesso");
        }

        [HttpPost]
        public JsonResult ListarPorCliente(long idCliente)
        {
            var beneficiarios = new List<BeneficiarioModel> {
                new BeneficiarioModel{Id =1 , Nome = "TEste" , Cpf = "Teste"}
            };
            return Json(beneficiarios);
        }
    }
}