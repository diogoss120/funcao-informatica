
ALTER TABLE Beneficiarios
ADD CONSTRAINT FK_Beneficiarios_Clientes
FOREIGN KEY (IdCliente)
REFERENCES Clientes (Id);
