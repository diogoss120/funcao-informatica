CREATE PROC FI_SP_ListarBenefPorCliente
    @IdCliente BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        ID,
        NOME,
        CPF,
        IdCliente
    FROM BENEFICIARIOS
    WHERE IdCliente = @IdCliente
END
