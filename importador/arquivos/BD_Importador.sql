CREATE TABLE CLIENTE
(
	ID_Cliente int identity,
	Cnpj varchar(14),
	Razao_Social varchar(100),
	Endereco varchar(200),
	Cidade varchar(60),
	Bairro varchar(50),
	Numero int,
	Foto varchar(300),
	CONSTRAINT PK_ID_Cliente PRIMARY KEY (ID_Cliente)
)

CREATE TABLE USUARIO
(
	ID_Usuario int identity,
	Usuario varchar(32),
	Senha varchar(32),
	IDCliente int,
	CONSTRAINT PK_ID_Usuario PRIMARY KEY (ID_Usuario),
	CONSTRAINT FK_ID_Cliente_Usuario FOREIGN KEY (IDCliente) REFERENCES CLIENTE	
)

CREATE TABLE EMPRESA
(
	ID_Empresa int identity,
	Foto varchar(300),
	Nome varchar(100),
	Cnpj varchar(14),
	ID_Cliente int,
    CONSTRAINT PK_ID_Empresa PRIMARY KEY (ID_Empresa),
    CONSTRAINT FK_ID_Cliente_Empresa FOREIGN KEY (ID_Cliente) REFERENCES CLIENTE	
)

CREATE TABLE ARQUIVO
(
	ID_Arquivo int identity,
	Nome_Arquivo varchar(60),
	Caminho varchar(300),
	Tipo char(1),
	IDEmpresa int,
	Data date,
	CONSTRAINT PK_ID_Arquivo PRIMARY KEY (ID_Arquivo),
    CONSTRAINT FK_IDEmpresa_Arquivo FOREIGN KEY (IDEmpresa) REFERENCES EMPRESA		                                   
)

CREATE TABLE DEPARA
(
	ID_DePara int identity,
	Historico_Banco varchar(100),
	Conta_Debito varchar(10),
	Conta_Credito varchar(10),
	Historico_Contabil varchar(100),
	ID_Empresa_Depara int,
	Cnpj varchar(14),
	Centro_Custo varchar(7),
	CONSTRAINT PK_ID_DePara PRIMARY KEY (ID_DePara),
    CONSTRAINT FK_IDEmpresa_DePara FOREIGN KEY (ID_Empresa_Depara) REFERENCES EMPRESA		                                   	
)

CREATE TABLE DEPARA_TEMPORARIO
(
	Historico_Banco varchar(100),
	ID_Empresa_Depara_Temporario int,
	CONSTRAINT FK_ID_Empresa_Depara_Temporario FOREIGN KEY (ID_Empresa_Depara_Temporario) REFERENCES EMPRESA		                                   		
)

CREATE TABLE TERCEIRO
(
	Cnpj varchar(14),
	Nome_Terceiro varchar(100),
	ID_Empresa int,
	CONSTRAINT PK_Cnpj PRIMARY KEY (Cnpj,ID_Empresa),
	CONSTRAINT FK_ID_Empresa_Terceiro FOREIGN KEY (ID_Empresa) REFERENCES EMPRESA		                                   		
)

create table ARQUIVO_CONTASNULL
(
	ID int identity,
	ID_Arquivo int,
	Historico varchar(100),
	CONSTRAINT PK_ID PRIMARY KEY (ID)	                                   		
)


INSERT INTO CLIENTE 
VALUES ('00000000000000','CGC - Contabilidade','Rua dos ventos uivantes','Montenegro','Centro','1263','~/Arquivos/cgc.jpg')

INSERT INTO EMPRESA
VALUES ('~/Arquivos/bradesco.png','Bradesco','00000000000000',1)

INSERT INTO EMPRESA
VALUES ('~/Arquivos/banrisul.jpg','Banrisul','00000000000000',1)
