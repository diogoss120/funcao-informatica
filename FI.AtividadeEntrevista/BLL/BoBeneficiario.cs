using FI.AtividadeEntrevista.DAL.Beneficiarios;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        public long Incluir(Beneficiario beneficiario)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Incluir(beneficiario);
        }

        public bool Alterar(Beneficiario beneficiario)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Alterar(beneficiario);
        }

        public List<Beneficiario> ListarPorCliente(long idCliente)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.ListarPorCliente(idCliente);   
        }

        public Beneficiario ObterPorId(long idCliente)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.ObterPorId(idCliente);
        }
    }
}
