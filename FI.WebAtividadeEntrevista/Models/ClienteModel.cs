using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Cliente
    /// </summary>
    public class ClienteModel
    {
        public long Id { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        public string CEP { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string Cidade { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        public string Email { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        [Required]
        public string Logradouro { get; set; }

        /// <summary>
        /// Nacionalidade
        /// </summary>
        [Required]
        public string Nacionalidade { get; set; }

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
        /// Sobrenome
        /// </summary>
        [Required]
        public string Sobrenome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        public static implicit operator ClienteModel(FI.AtividadeEntrevista.DML.Cliente cliente)
        {
            if (cliente == null)
                return null;

            return new ClienteModel
            {
                Id = cliente.Id,
                CEP = cliente.CEP,
                Cidade = cliente.Cidade,
                Email = cliente.Email,
                Estado = cliente.Estado,
                Logradouro = cliente.Logradouro,
                Nacionalidade = cliente.Nacionalidade,
                Cpf = cliente.Cpf, 
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome,
                Telefone = cliente.Telefone
            };
        }

        public static implicit operator FI.AtividadeEntrevista.DML.Cliente(ClienteModel model)
        {
            if (model == null)
                return null;

            return new FI.AtividadeEntrevista.DML.Cliente
            {
                Id = model.Id,
                CEP = model.CEP,
                Cidade = model.Cidade,
                Email = model.Email,
                Estado = model.Estado,
                Logradouro = model.Logradouro,
                Nacionalidade = model.Nacionalidade,
                Cpf = model.Cpf, 
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Telefone = model.Telefone
            };
        }


    }
}