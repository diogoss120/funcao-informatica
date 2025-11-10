using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FI.WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }


        private string _cpf;

        /// <summary>
        /// Cpf
        /// </summary>
        [Required]
        public string Cpf
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_cpf))
                    return _cpf;

                // pega apenas dígitos
                var digits = new string(_cpf.Where(char.IsDigit).ToArray());

                // se não tem 11 dígitos, retorna o valor original (evita mascarar valores inválidos)
                if (digits.Length != 11)
                    return _cpf;

                // formata: 000.000.000-00
                return $"{digits.Substring(0, 3)}.{digits.Substring(3, 3)}.{digits.Substring(6, 3)}-{digits.Substring(9, 2)}";
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _cpf = value;
                    return;
                }

                // armazena apenas os dígitos (útil para persistir sem máscara)
                _cpf = new string(value.Where(char.IsDigit).ToArray());
            }
        }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Id do cliente 
        /// </summary>
        [Required]
        public long IdCliente { get; set; }


        // ---- Operators de conversão ----
        public static implicit operator BeneficiarioModel(FI.AtividadeEntrevista.DML.Beneficiario b)
        {
            if (b == null) return null;

            return new BeneficiarioModel
            {
                Id = b.Id,
                Cpf = b.Cpf,
                Nome = b.Nome,
                IdCliente = b.IdCliente
            };
        }

        public static implicit operator FI.AtividadeEntrevista.DML.Beneficiario(BeneficiarioModel m)
        {
            if (m == null) return null;

            return new FI.AtividadeEntrevista.DML.Beneficiario
            {
                Id = m.Id,
                Cpf = new string((m.Cpf ?? "").Where(char.IsDigit).ToArray()),
                Nome = m.Nome,
                IdCliente = m.IdCliente
            };
        }
    }
}