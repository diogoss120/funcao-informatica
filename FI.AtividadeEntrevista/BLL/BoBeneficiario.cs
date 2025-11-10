using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto do beneficiário</param>
        public long Incluir(Beneficiario beneficiario)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto do beneficiário</param>
        public void Alterar(Beneficiario beneficiario)
        {
            var daoBeneficiario = new DaoBeneficiario();
            daoBeneficiario.Alterar(beneficiario);
        }

        /// <summary>
        /// Lista beneficiários por id de cliente
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        public List<Beneficiario> ListarPorCliente(long idCliente)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.ListarPorCliente(idCliente);   
        }

        /// <summary>
        /// Obtêm beneficiário por id
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        public Beneficiario ObterPorId(long idCliente)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.ObterPorId(idCliente);
        }

        /// <summary>
        /// Excluir beneficiário por id
        /// </summary>
        /// <param name="id">Id do beneficiário</param>
        public void Excluir(long id)
        {
            var daoBeneficiario = new DaoBeneficiario();
            daoBeneficiario.Excluir(id);
        }
    }
}
