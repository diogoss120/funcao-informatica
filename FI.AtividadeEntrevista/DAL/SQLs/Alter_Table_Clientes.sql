
-- inclui a coluna de CPF na tabela de clientes
alter table Clientes
add Cpf varchar(11) NOT NULL;


CREATE UNIQUE INDEX UX_CLIENTES_CPF
    ON CLIENTES (CPF);

