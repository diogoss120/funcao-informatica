using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model)
        {
            try
            {
                if (model == null)
                {
                    Response.StatusCode = 400;
                    return Json("Requisição inválida.");
                }

                var cpfLimpo = model.Cpf?.GetCpfLimpo();
                if (!string.IsNullOrEmpty(cpfLimpo))
                {
                    var cpfExistente = CpfValidador.VerificarExistencia(cpfLimpo);
                    if (cpfExistente) ModelState.AddModelError("Cpf", "O CPF informado já está sendo usado");

                    var cpfValido = CpfValidador.ValidarCpf(cpfLimpo);
                    if (!cpfValido) ModelState.AddModelError("Cpf", "O CPF informado é inválido");
                }

                if (!ModelState.IsValid)
                {
                    var erros = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }

                var bo = new BoBeneficiario();
                var beneficiario = new Beneficiario
                {
                    Nome = model.Nome,
                    Cpf = cpfLimpo,    
                    IdCliente = model.IdCliente
                };

                model.Id = bo.Incluir(beneficiario);

                return Json("Cadastro de beneficiário efetuado com sucesso");
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return Json("Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                {
                    Response.StatusCode = 400;
                    return Json("Requisição inválida.");
                }

                var cpfLimpo = model.Cpf?.GetCpfLimpo();
                if (!string.IsNullOrEmpty(cpfLimpo))
                {
                    var cpfExistente = CpfValidador.VerificarExistencia(cpfLimpo);
                    if (cpfExistente) ModelState.AddModelError("Cpf", "O CPF informado já está sendo usado");

                    var cpfValido = CpfValidador.ValidarCpf(cpfLimpo);
                    if (!cpfValido) ModelState.AddModelError("Cpf", "O CPF informado é inválido");
                }

                if (!ModelState.IsValid)
                {
                    var erros = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }

                var beneficiario = new Beneficiario
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Cpf = cpfLimpo,   
                    IdCliente = model.IdCliente
                };

                var bo = new BoBeneficiario();
                bo.Alterar(beneficiario);

                return Json("Beneficiário atualizado com sucesso");
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return Json("Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public JsonResult ListarPorCliente(long idCliente)
        {
            try
            {
                if (idCliente <= 0)
                {
                    Response.StatusCode = 400;
                    return Json("Id do cliente inválido.");
                }

                var bo = new BoBeneficiario();
                var beneficiarios = bo.ListarPorCliente(idCliente);
                return Json(beneficiarios);
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return Json("Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public JsonResult Obter(long id)
        {
            try
            {
                if (id <= 0)
                {
                    Response.StatusCode = 400;
                    return Json("Id inválido.");
                }

                var bo = new BoBeneficiario();
                var beneficiario = bo.ObterPorId(id);

                if (beneficiario == null)
                {
                    Response.StatusCode = 404;
                    return Json("Beneficiário não encontrado.");
                }

                return Json(beneficiario);
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return Json("Ocorreu um erro interno no servidor.");
            }
        }
    }
}
