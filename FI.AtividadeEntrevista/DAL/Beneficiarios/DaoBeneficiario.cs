using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL.Beneficiarios
{
    internal class DaoBeneficiario : AcessoDados
    {
        internal long Incluir(Beneficiario beneficiario)
        {
            var parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new SqlParameter("Cpf", beneficiario.Cpf));
            parametros.Add(new SqlParameter("IdCliente", beneficiario.IdCliente));

            DataSet ds = base.Consultar("FI_SP_InsBenef", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        internal List<Beneficiario> ListarPorCliente(long IdCliente)
        {
            var parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("IdCliente", IdCliente));

            DataSet ds = base.Consultar("FI_SP_ListarBenefPorCliente", parametros);
            List<Beneficiario> cli = Converter(ds);

            return cli;
        }

        internal Beneficiario ObterPorId(long id)
        {
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("Id", id));

            DataSet ds = base.Consultar("FI_SP_ObterBenefPorId", parametros);

            List<Beneficiario> lista = Converter(ds);

            return lista.FirstOrDefault();
        }

        internal void Alterar(Beneficiario beneficiario)
        {
            var parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new SqlParameter("Cpf", beneficiario.Cpf));
            parametros.Add(new SqlParameter("Id", beneficiario.Id));

            base.Consultar("FI_SP_AltBenef", parametros);
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            var lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var cli = new Beneficiario();
                    cli.Id = row.Field<long>("Id");
                    cli.Nome = row.Field<string>("Nome");
                    cli.Cpf = row.Field<string>("Cpf");
                    cli.IdCliente = row.Field<long>("IdCliente");
                    lista.Add(cli);
                }
            }
            return lista;
        }

        internal void Excluir(long id)
        {
            var parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Id", id));

            base.Consultar("FI_SP_DelBenef", parametros);
        }

        internal bool VerificarExistencia(string CPF, long idCliente, long? id = null)
        {
            var parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("CPF", CPF));
            parametros.Add(new SqlParameter("idCliente", idCliente));
            parametros.Add(new SqlParameter("IdExcluir", (object)id ?? DBNull.Value));

            var ds = base.Consultar("FI_SP_VerificaBeneficiario", parametros);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;
            var existe = Convert.ToInt32(ds.Tables[0].Rows[0]["Existe"]);
            return existe == 1;
        }
    }
}
