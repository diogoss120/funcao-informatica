CREATE PROC FI_SP_ObterBenefPorId
    @Id BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ID,
        NOME,
        CPF,
        IdCliente
    FROM BENEFICIARIOS
    WHERE ID = @Id
END
