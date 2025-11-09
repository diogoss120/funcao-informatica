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
                Cpf = model.Cpf,
                IdCliente = model.IdCliente
            };
            model.Id = bo.Incluir(beneficiario);

            return Json("Cadastro de beneficiário efetuado com sucesso");
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model)
        {
            var bo = new BoBeneficiario();

            /// no caso do cpf do beneficiário, por estar em uma tabela diferente, como vou fazer para validar 
            /// o cpf? devo checar se existe cpf igual em ambas as tabelas (Cliente e Beneficiário) ?
            /// esse código abaixo checa o cpf na tabela do cliente
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
                Id = model.Id,
                Nome = model.Nome,
                Cpf = model.Cpf,
                IdCliente = model.IdCliente
            };

            var result = bo.Alterar(beneficiario);

            return Json("Beneficiário atualizado com sucesso");
        }

        [HttpPost]
        public JsonResult ListarPorCliente(long idCliente)
        {
            var bo = new BoBeneficiario();
            var beneficiarios = bo.ListarPorCliente(idCliente);
            return Json(beneficiarios);
        }

        [HttpPost]
        public JsonResult Obter(long id)
        {
            var bo = new BoBeneficiario();
            var beneficiario = bo.ObterPorId(id);
            return Json(beneficiario);
        }
    }
}