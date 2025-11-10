using FI.AtividadeEntrevista.DAL.Beneficiarios;
using System.Linq;

namespace FI.AtividadeEntrevista.BLL
{
    public static class CpfValidador
    {
        public static bool VerificarExistenciaCliente(string CPF, long? id = null)
        {
            var cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF, id);
        }

        public static bool VerificarExistenciaBeneficiario(string CPF, long idCliente, long? id = null)
        {
            var ben = new DaoBeneficiario();
            return ben.VerificarExistencia(CPF, idCliente, id);
        }

        public static string GetCpfLimpo(this string Cpf)
        {
            if (string.IsNullOrWhiteSpace(Cpf))
                return string.Empty;

            return System.Text.RegularExpressions.Regex
                .Replace(Cpf, @"\D", ""); // remove qualquer coisa que não é número
        }

        public static bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove tudo que não é número
            cpf = System.Text.RegularExpressions.Regex.Replace(cpf, @"\D", "");

            // Verifica se tem 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Elimina CPFs conhecidos inválidos (todos os dígitos iguais)
            var invalidos = new string[]
            {
                "00000000000", "11111111111", "22222222222", "33333333333",
                "44444444444", "55555555555", "66666666666",
                "77777777777", "88888888888", "99999999999"
            };

            if (invalidos.Contains(cpf))
                return false;

            // Calcula primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (cpf[i] - '0') * (10 - i);

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            if (cpf[9] - '0' != digito1)
                return false;

            // Calcula segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (cpf[i] - '0') * (11 - i);

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            if (cpf[10] - '0' != digito2)
                return false;

            return true; // CPF válido
        }

    }
}
