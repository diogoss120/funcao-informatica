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

        public List<Beneficiario> Consultar(long idCliente)
        {
            var daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Consultar(idCliente);   
        }
    }
}
