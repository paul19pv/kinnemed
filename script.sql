USE [master]
GO
/****** Object:  Database [DB_9D0010_kinnemeddb]    Script Date: 09/10/2018 14:07:34 ******/
CREATE DATABASE [DB_9D0010_kinnemeddb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'bd_kinnemed02', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\DB_9D0010_kinnemeddb_data.mdf' , SIZE = 1531648KB , MAXSIZE = 3072000KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'bd_kinnemed02_log', FILENAME = N'H:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\DB_9D0010_kinnemeddb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_9D0010_kinnemeddb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET  MULTI_USER 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET DELAYED_DURABILITY = DISABLED 
GO
USE [DB_9D0010_kinnemeddb]
GO
/****** Object:  UserDefinedFunction [dbo].[getFirma]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza
-- Create date: 24/06/2016
-- Description:	Obtener la firma del responsable
-- =============================================
CREATE FUNCTION [dbo].[getFirma]
(
	-- Add the parameters for the function here
	@responsable int,
	@perfil int
)
RETURNS varbinary(max)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @firma varbinary(max)

	if @responsable is not null
	begin 
		if @perfil=2
			select @firma=ISNULL(med_firma, CONVERT(VARBINARY(MAX), 0)) from medico where med_id=@responsable
			/*select @firma=CAST(med_firma as varbinary(max)) from medico where med_id=@responsable*/
		else if @perfil=5
			select @firma=ISNULL(lab_firma, CONVERT(VARBINARY(MAX), 0)) from laboratorista where lab_id=@responsable
		else 
			set @firma=CONVERT(VARBINARY(MAX), 1)
	end
	else
		set @firma=CONVERT(VARBINARY(MAX), 1)

	
	-- Return the result of the function
	
	RETURN @firma

END

GO
/****** Object:  UserDefinedFunction [dbo].[getHistoria]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[getHistoria] 
(
	-- Add the parameters for the function here
	@paciente int,
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @estado bit

	-- Add the T-SQL statements to compute the return value here
	SELECT @estado= cast(
		case when 
			(select  count(*) from historia where CONVERT(date,his_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  and his_paciente=@paciente) >0
		 then 1 else 0 end as bit)
	from paciente
	where pac_id=@paciente

	-- Return the result of the function
	RETURN @estado

END

GO
/****** Object:  UserDefinedFunction [dbo].[getResponsable]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza	
-- Create date: 24/06/2016
-- Description:	obtener el nombre del responsable
-- =============================================
CREATE FUNCTION [dbo].[getResponsable]
(
	-- Add the parameters for the function here
	@responsable int,
	@perfil int
)
RETURNS varchar(150)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @nombre varchar(150)

	-- Add the T-SQL statements to compute the return value here
	if @responsable is not null
	begin
		if @perfil=2
			SELECT @nombre=(med_nombres+ ' '+med_apellidos) from medico where med_id=@responsable
		else if @perfil=5
			select @nombre=('Lic. '+lab_nombres+' '+lab_apellidos) from laboratorista where lab_id=@responsable
		else 
		set @nombre=''
	end
	else
		set @nombre='Sin Responsable'
	-- Return the result of the function
	RETURN @nombre

END

GO
/****** Object:  UserDefinedFunction [dbo].[thereHistorias]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[thereHistorias]
(
	-- Add the parameters for the function here
	@fecha varchar(10)
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	declare @estado bit

	-- Add the T-SQL statements to compute the return value here
	if @fecha!= null
set @estado=1
else 
set @estado=0
return @estado
END

GO
/****** Object:  Table [dbo].[accidente]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[accidente](
	[acc_id] [int] NOT NULL,
	[acc_descripción] [varchar](200) NOT NULL,
	[acc_capacidad] [varchar](200) NOT NULL,
	[acc_fecha] [varchar](10) NOT NULL,
	[acc_empresa] [varchar](200) NOT NULL,
	[acc_paciente] [int] NOT NULL,
 CONSTRAINT [PK_accidente] PRIMARY KEY CLUSTERED 
(
	[acc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[actividad]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[actividad](
	[act_id] [int] NOT NULL,
	[act_enf_estado] [varchar](2) NULL,
	[act_enf_descripcion] [varchar](200) NULL,
	[act_enf_fecha] [varchar](10) NULL,
	[act_enf_empresa] [varchar](200) NULL,
	[act_acc_estado] [varchar](2) NULL,
	[act_acc_descripcion] [varchar](200) NULL,
	[act_acc_capacidad] [varchar](200) NULL,
	[act_acc_fecha] [varchar](10) NULL,
	[act_acc_empresa] [varchar](200) NULL,
 CONSTRAINT [PK_actividad] PRIMARY KEY CLUSTERED 
(
	[act_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[area]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[area](
	[are_id] [int] NOT NULL,
	[are_nombre] [varchar](50) NOT NULL,
	[are_tipo] [varchar](50) NOT NULL,
 CONSTRAINT [PK_area] PRIMARY KEY CLUSTERED 
(
	[are_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[audiometria]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[audiometria](
	[aud_id] [int] IDENTITY(1,1) NOT NULL,
	[aud_paciente] [int] NOT NULL,
	[aud_archivo] [varchar](256) NOT NULL,
	[aud_observacion] [nvarchar](max) NULL,
	[aud_fecha] [varchar](10) NOT NULL,
	[aud_responsable] [int] NULL,
	[aud_perfil] [int] NULL,
	[aud_laboratorista] [int] NOT NULL,
	[aud_orden] [int] NULL,
 CONSTRAINT [PK_audiometria] PRIMARY KEY CLUSTERED 
(
	[aud_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[canton]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[canton](
	[can_id] [int] NOT NULL,
	[can_nombre] [varchar](150) NULL,
	[can_provincia] [int] NULL,
 CONSTRAINT [PK_canton] PRIMARY KEY CLUSTERED 
(
	[can_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[cie10]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cie10](
	[cie_id] [int] NOT NULL,
	[cie_codigo] [varchar](10) NOT NULL,
	[cie_descripcion] [varchar](200) NOT NULL,
 CONSTRAINT [PK_cie10] PRIMARY KEY CLUSTERED 
(
	[cie_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[codigo]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[codigo](
	[cod_id] [int] IDENTITY(1,1) NOT NULL,
	[cod_codigo] [varchar](12) NOT NULL,
	[cod_imagen] [image] NULL,
	[cod_registro] [int] NOT NULL,
	[cod_area] [int] NOT NULL,
 CONSTRAINT [PK_codigo] PRIMARY KEY CLUSTERED 
(
	[cod_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[concepto]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[concepto](
	[con_id] [int] NOT NULL,
	[con_resultado] [varchar](50) NOT NULL,
	[con_observacion] [nvarchar](max) NULL,
	[con_valor] [varchar](50) NULL,
	[con_seguimiento] [varchar](2) NULL,
	[con_periodo] [int] NULL,
 CONSTRAINT [PK_concepto] PRIMARY KEY CLUSTERED 
(
	[con_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[control]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[control](
	[con_id] [int] IDENTITY(1,1) NOT NULL,
	[con_perfil] [int] NOT NULL,
	[con_examen] [int] NOT NULL,
 CONSTRAINT [PK_control] PRIMARY KEY CLUSTERED 
(
	[con_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[diagnostico]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[diagnostico](
	[dia_id] [int] IDENTITY(1,1) NOT NULL,
	[dia_historia] [int] NOT NULL,
	[dia_descripcion] [varchar](255) NULL,
	[dia_subcie10] [int] NOT NULL,
	[dia_tipo] [varchar](3) NOT NULL,
 CONSTRAINT [PK_diagnostico] PRIMARY KEY CLUSTERED 
(
	[dia_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[doctor]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[doctor](
	[doc_id] [int] IDENTITY(1,1) NOT NULL,
	[doc_cedula] [varchar](150) NOT NULL,
	[doc_nombres] [varchar](150) NOT NULL,
	[doc_apellidos] [varchar](150) NOT NULL,
	[doc_codigo] [varchar](50) NULL,
	[doc_especialidad] [int] NOT NULL,
	[doc_correo] [varchar](150) NOT NULL,
	[doc_firma] [image] NULL,
	[doc_estado] [bit] NOT NULL,
	[doc_empresa] [int] NOT NULL,
 CONSTRAINT [PK_doctor] PRIMARY KEY CLUSTERED 
(
	[doc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_doc_cedula] UNIQUE NONCLUSTERED 
(
	[doc_cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[empresa]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[empresa](
	[emp_id] [int] IDENTITY(1,1) NOT NULL,
	[emp_cedula] [varchar](15) NOT NULL,
	[emp_nombre] [varchar](50) NOT NULL,
	[emp_direccion] [varchar](200) NOT NULL,
	[emp_telefono] [varchar](10) NOT NULL,
	[emp_mail] [varchar](100) NOT NULL,
	[emp_estado] [varchar](25) NOT NULL,
 CONSTRAINT [PK_empresa] PRIMARY KEY CLUSTERED 
(
	[emp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_CEDULA] UNIQUE NONCLUSTERED 
(
	[emp_cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[enfermedad]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[enfermedad](
	[enf_id] [int] NOT NULL,
	[enf_descripcion] [varchar](200) NOT NULL,
	[enf_fecha] [varchar](10) NOT NULL,
	[enf_empresa] [varchar](200) NOT NULL,
	[enf_paciente] [int] NOT NULL,
 CONSTRAINT [PK_enfermedad] PRIMARY KEY CLUSTERED 
(
	[enf_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[especialidad]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[especialidad](
	[esp_id] [int] NOT NULL,
	[esp_nombre] [varchar](100) NULL,
 CONSTRAINT [PK_especialidad] PRIMARY KEY CLUSTERED 
(
	[esp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[espirometria]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[espirometria](
	[esp_id] [int] IDENTITY(1,1) NOT NULL,
	[esp_paciente] [int] NOT NULL,
	[esp_archivo] [varchar](256) NOT NULL,
	[esp_observacion] [nvarchar](max) NULL,
	[esp_fecha] [varchar](10) NOT NULL,
	[esp_responsable] [int] NULL,
	[esp_perfil] [int] NULL,
	[esp_laboratorista] [int] NOT NULL,
	[esp_orden] [int] NULL,
 CONSTRAINT [PK_espirometria] PRIMARY KEY CLUSTERED 
(
	[esp_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[examen]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[examen](
	[exa_id] [int] IDENTITY(1,1) NOT NULL,
	[exa_nombre] [varchar](50) NOT NULL,
	[exa_unidad] [varchar](15) NULL,
	[exa_tipo] [varchar](15) NOT NULL,
	[exa_area] [int] NOT NULL,
	[exa_item] [int] NULL,
	[exa_valores] [nvarchar](max) NULL,
	[exa_inicial] [varchar](50) NULL,
	[exa_estado] [varchar](20) NOT NULL,
	[exa_orden] [int] NOT NULL,
 CONSTRAINT [PK_examen] PRIMARY KEY CLUSTERED 
(
	[exa_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[familiar]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[familiar](
	[fam_id] [int] NOT NULL,
	[fam_cardiopatia] [bit] NOT NULL,
	[fam_car_txt] [nvarchar](max) NULL,
	[fam_diabetes] [bit] NOT NULL,
	[fam_dia_txt] [nvarchar](max) NULL,
	[fam_cardiovascular] [bit] NOT NULL,
	[fam_vas_txt] [nvarchar](max) NULL,
	[fam_hipertension] [bit] NOT NULL,
	[fam_hip_txt] [nvarchar](max) NULL,
	[fam_cancer] [bit] NOT NULL,
	[fam_can_txt] [nvarchar](max) NULL,
	[fam_tuberculosis] [bit] NOT NULL,
	[fam_tub_txt] [nvarchar](max) NULL,
	[fam_mental] [bit] NOT NULL,
	[fam_men_txt] [nvarchar](max) NULL,
	[fam_infecciosa] [bit] NOT NULL,
	[fam_inf_txt] [nvarchar](max) NULL,
	[fam_malformacion] [bit] NOT NULL,
	[fam_mal_txt] [nvarchar](max) NULL,
	[fam_otro] [bit] NOT NULL,
	[fam_otr_txt] [nvarchar](max) NULL,
	[fam_hipotiroidismo] [bit] NOT NULL,
	[fam_hit_txt] [nvarchar](max) NULL,
 CONSTRAINT [PK_familiar] PRIMARY KEY CLUSTERED 
(
	[fam_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[fisico]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[fisico](
	[fis_id] [int] NOT NULL,
	[fis_piel] [bit] NOT NULL,
	[fis_pie_txt] [nvarchar](max) NULL,
	[fis_cabeza] [bit] NOT NULL,
	[fis_cab_txt] [nvarchar](max) NULL,
	[fis_ojos] [bit] NOT NULL,
	[fis_ojo_txt] [nvarchar](max) NULL,
	[fis_oidos] [bit] NOT NULL,
	[fis_oid_txt] [nvarchar](max) NULL,
	[fis_nariz] [bit] NOT NULL,
	[fis_nar_txt] [nvarchar](max) NULL,
	[fis_boca] [bit] NOT NULL,
	[fis_boc_txt] [nvarchar](max) NULL,
	[fis_faringe] [bit] NOT NULL,
	[fis_far_txt] [nvarchar](max) NULL,
	[fis_cuello] [bit] NOT NULL,
	[fis_cue_txt] [nvarchar](max) NULL,
	[fis_axilas] [bit] NOT NULL,
	[fis_axi_txt] [nvarchar](max) NULL,
	[fis_torax] [bit] NOT NULL,
	[fis_tor_txt] [nvarchar](max) NULL,
	[fis_abdomen] [bit] NOT NULL,
	[fis_abd_txt] [nvarchar](max) NULL,
	[fis_columna] [bit] NOT NULL,
	[fis_col_txt] [nvarchar](max) NULL,
	[fis_ingle] [bit] NOT NULL,
	[fis_ing_txt] [nvarchar](max) NULL,
	[fis_mie_sup] [bit] NOT NULL,
	[fis_msp_txt] [nvarchar](max) NULL,
	[fis_mie_inf] [bit] NOT NULL,
	[fis_mif_txt] [nvarchar](max) NULL,
	[fis_organos] [bit] NOT NULL,
	[fis_org_txt] [nvarchar](max) NULL,
	[fis_respiratorio] [bit] NOT NULL,
	[fis_res_txt] [nvarchar](max) NULL,
	[fis_cardiovascular] [bit] NOT NULL,
	[fis_car_txt] [nvarchar](max) NULL,
	[fis_digestivo] [bit] NOT NULL,
	[fis_dig_txt] [nvarchar](max) NULL,
	[fis_genital] [bit] NOT NULL,
	[fis_fen_txt] [nvarchar](max) NULL,
	[fis_urinario] [bit] NOT NULL,
	[fis_uri_txt] [nvarchar](max) NULL,
	[fis_esqueletico] [bit] NOT NULL,
	[fis_esq_txt] [nvarchar](max) NULL,
	[fis_endocrino] [bit] NOT NULL,
	[fis_end_txt] [nvarchar](max) NULL,
	[fis_linfatico] [bit] NOT NULL,
	[fis_lin_txt] [nvarchar](max) NULL,
	[fis_neurologico] [bit] NOT NULL,
	[fis_neu_txt] [nvarchar](max) NULL,
 CONSTRAINT [PK_fisico] PRIMARY KEY CLUSTERED 
(
	[fis_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ginecologico]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ginecologico](
	[gin_id] [int] NOT NULL,
	[gin_fum] [varchar](10) NULL,
	[gin_ciclos] [varchar](25) NULL,
	[gin_gestas] [int] NULL,
	[gin_partos] [int] NULL,
	[gin_cesarea] [int] NULL,
	[gin_abortos] [int] NULL,
	[gin_hijos] [int] NULL,
	[gin_planificacion] [bit] NOT NULL,
	[gin_pla_txt] [nvarchar](max) NULL,
	[gin_paptest] [varchar](10) NULL,
 CONSTRAINT [PK_ginecologico] PRIMARY KEY CLUSTERED 
(
	[gin_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[habitos]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[habitos](
	[hab_id] [int] NOT NULL,
	[hab_fumo] [varchar](2) NOT NULL,
	[hab_fuma] [varchar](2) NOT NULL,
	[hab_cigarillos] [varchar](150) NULL,
	[hab_alcohol] [varchar](2) NOT NULL,
	[hab_alc_txt] [varchar](150) NULL,
	[hab_drogas] [varchar](2) NOT NULL,
	[hab_dro_txt] [varchar](150) NULL,
	[hab_ejercicio] [varchar](2) NOT NULL,
 CONSTRAINT [PK_habitos] PRIMARY KEY CLUSTERED 
(
	[hab_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[historia]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[historia](
	[his_id] [int] IDENTITY(1,1) NOT NULL,
	[his_paciente] [int] NOT NULL,
	[his_motivo] [nvarchar](max) NULL,
	[his_problema] [nvarchar](max) NULL,
	[his_observacion] [nvarchar](max) NULL,
	[his_tipo] [int] NOT NULL,
	[his_fecha] [varchar](10) NOT NULL,
	[his_numero] [int] NOT NULL,
	[his_medico] [int] NOT NULL,
	[his_firma] [image] NULL,
 CONSTRAINT [PK_historia] PRIMARY KEY CLUSTERED 
(
	[his_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[inmunizacion]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[inmunizacion](
	[inm_id] [int] IDENTITY(1,1) NOT NULL,
	[inm_vacuna] [int] NOT NULL,
	[inm_fecha] [varchar](10) NOT NULL,
	[inm_tipo] [varchar](25) NOT NULL,
	[inm_paciente] [int] NOT NULL,
 CONSTRAINT [PK_inmunizacion] PRIMARY KEY CLUSTERED 
(
	[inm_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[laboral]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[laboral](
	[lab_id] [int] IDENTITY(1,1) NOT NULL,
	[lab_ocupacional] [int] NOT NULL,
	[lab_riesgo] [int] NOT NULL,
	[lab_estado] [bit] NOT NULL,
 CONSTRAINT [PK_rielab] PRIMARY KEY CLUSTERED 
(
	[lab_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[laboratorista]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[laboratorista](
	[lab_id] [int] IDENTITY(1,1) NOT NULL,
	[lab_cedula] [varchar](10) NOT NULL,
	[lab_nombres] [varchar](150) NOT NULL,
	[lab_apellidos] [varchar](150) NOT NULL,
	[lab_correo] [varchar](150) NOT NULL,
	[lab_firma] [image] NULL,
	[lab_estado] [bit] NOT NULL,
 CONSTRAINT [PK_laboratorista] PRIMARY KEY CLUSTERED 
(
	[lab_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[medico]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[medico](
	[med_id] [int] IDENTITY(1,1) NOT NULL,
	[med_cedula] [varchar](10) NOT NULL,
	[med_nombres] [varchar](150) NOT NULL,
	[med_apellidos] [varchar](150) NOT NULL,
	[med_ci] [varchar](50) NULL,
	[med_codigo] [varchar](50) NULL,
	[med_especialidad] [int] NOT NULL,
	[med_correo] [varchar](150) NOT NULL,
	[med_firma] [image] NULL,
	[med_estado] [bit] NOT NULL,
 CONSTRAINT [PK_medico] PRIMARY KEY CLUSTERED 
(
	[med_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_med_cedula] UNIQUE NONCLUSTERED 
(
	[med_cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ocupacional]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ocupacional](
	[ocu_id] [int] IDENTITY(1,1) NOT NULL,
	[ocu_empresa] [varchar](200) NULL,
	[ocu_seccion] [varchar](200) NOT NULL,
	[ocu_cargo] [varchar](200) NOT NULL,
	[ocu_descripcion] [nvarchar](max) NOT NULL,
	[ocu_jornada] [varchar](25) NOT NULL,
	[ocu_inicio] [varchar](10) NOT NULL,
	[ocu_fin] [varchar](10) NOT NULL,
	[ocu_tiempo] [decimal](5, 2) NOT NULL,
	[ocu_maquinaria] [nvarchar](max) NULL,
	[ocu_materiales] [nvarchar](max) NULL,
	[ocu_sustancias] [nvarchar](max) NULL,
	[ocu_equipo] [nvarchar](max) NULL,
	[ocu_tipo] [varchar](25) NOT NULL,
	[ocu_estado] [bit] NOT NULL,
	[ocu_paciente] [int] NOT NULL,
 CONSTRAINT [PK_riesgo] PRIMARY KEY CLUSTERED 
(
	[ocu_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[oftalmologia]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[oftalmologia](
	[oft_id] [int] IDENTITY(1,1) NOT NULL,
	[oft_paciente] [int] NOT NULL,
	[oft_con_od] [varchar](10) NOT NULL,
	[oft_con_oi] [varchar](10) NOT NULL,
	[oft_sin_od] [varchar](10) NOT NULL,
	[oft_sin_oi] [varchar](10) NOT NULL,
	[oft_ref_od] [varchar](150) NOT NULL,
	[oft_ref_oi] [varchar](150) NOT NULL,
	[oft_biomiscroscopia] [varchar](25) NOT NULL,
	[oft_bio_txt] [varchar](150) NULL,
	[oft_fondo] [varchar](25) NOT NULL,
	[oft_fon_txt] [varchar](150) NULL,
	[oft_colores] [varchar](150) NOT NULL,
	[oft_diagnostico] [varchar](150) NOT NULL,
	[oft_dia_txt] [varchar](max) NULL,
	[oft_indicaciones] [varchar](150) NOT NULL,
	[oft_ind_txt] [varchar](max) NULL,
	[oft_otros] [varchar](250) NULL,
	[oft_fecha] [varchar](10) NULL,
	[oft_responsable] [int] NULL,
	[oft_perfil] [int] NULL,
	[oft_orden] [int] NULL,
 CONSTRAINT [PK_oftalmologia] PRIMARY KEY CLUSTERED 
(
	[oft_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[orden]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[orden](
	[ord_id] [int] NOT NULL,
	[ord_paciente] [int] NOT NULL,
	[ord_examen] [int] NOT NULL,
	[ord_fecha] [varchar](10) NOT NULL,
 CONSTRAINT [PK_orden] PRIMARY KEY CLUSTERED 
(
	[ord_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[paciente]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[paciente](
	[pac_id] [int] IDENTITY(1,1) NOT NULL,
	[pac_cedula] [varchar](10) NOT NULL,
	[pac_nombres] [varchar](150) NOT NULL,
	[pac_apellidos] [varchar](150) NOT NULL,
	[pac_genero] [varchar](50) NULL,
	[pac_estadocivil] [varchar](50) NULL,
	[pac_pais] [int] NULL,
	[pac_fechanacimiento] [varchar](10) NULL,
	[pac_edad] [int] NOT NULL,
	[pac_telefono] [varchar](15) NULL,
	[pac_celular] [varchar](15) NULL,
	[pac_correo] [varchar](150) NULL,
	[pac_provincia] [int] NULL,
	[pac_canton] [int] NULL,
	[pac_direccion] [varchar](250) NULL,
	[pac_instruccion] [varchar](50) NULL,
	[pac_profesion] [int] NULL,
	[pac_tipodiscapacidad] [varchar](50) NULL,
	[pac_porcentajediscapacidad] [int] NULL,
	[pac_empresa] [int] NOT NULL,
	[pac_estado] [bit] NOT NULL,
	[pac_varios] [varchar](500) NULL,
	[pac_actividad] [varchar](150) NULL,
 CONSTRAINT [PK_paciente] PRIMARY KEY CLUSTERED 
(
	[pac_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_pac_cedula] UNIQUE NONCLUSTERED 
(
	[pac_cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[pais]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[pais](
	[pais_id] [int] NOT NULL,
	[pais_nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_pais] PRIMARY KEY CLUSTERED 
(
	[pais_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[perfil]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[perfil](
	[per_id] [int] IDENTITY(1,1) NOT NULL,
	[per_nombre] [varchar](50) NOT NULL,
	[per_descripcion] [varchar](150) NOT NULL,
 CONSTRAINT [PK_perfil] PRIMARY KEY CLUSTERED 
(
	[per_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[personal]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[personal](
	[per_id] [int] NOT NULL,
	[per_patologicas] [bit] NOT NULL,
	[per_pat_txt] [nvarchar](max) NULL,
	[per_quirurgicas] [bit] NOT NULL,
	[per_qui_txt] [nvarchar](max) NULL,
	[per_traumaticos] [bit] NOT NULL,
	[per_tra_txt] [nvarchar](max) NULL,
	[per_alergias] [bit] NOT NULL,
	[per_ale_txt] [nvarchar](max) NULL,
	[per_vacunas] [bit] NOT NULL,
	[per_vac_txt] [nvarchar](max) NULL,
	[per_lateralidad] [varchar](25) NULL,
	[per_otros] [bit] NOT NULL,
	[per_otr_txt] [nvarchar](max) NULL,
 CONSTRAINT [PK_personal_1] PRIMARY KEY CLUSTERED 
(
	[per_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[plan]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[plan](
	[pla_id] [int] NOT NULL,
	[pla_texto1] [nvarchar](max) NOT NULL,
	[pla_texto2] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_plan] PRIMARY KEY CLUSTERED 
(
	[pla_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[profesion]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[profesion](
	[pro_id] [int] NOT NULL,
	[pro_nombre] [varchar](100) NULL,
 CONSTRAINT [PK_profesion] PRIMARY KEY CLUSTERED 
(
	[pro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[provincia]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[provincia](
	[pro_id] [int] NOT NULL,
	[pro_nombre] [varchar](100) NULL,
 CONSTRAINT [PK_provincia] PRIMARY KEY CLUSTERED 
(
	[pro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[prueba]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[prueba](
	[pru_id] [int] IDENTITY(1,1) NOT NULL,
	[pru_examen] [int] NOT NULL,
	[pru_registro] [int] NOT NULL,
	[pru_resultado] [varchar](40) NULL,
	[pru_valor] [varchar](20) NULL,
	[pru_fuera] [varchar](2) NULL,
 CONSTRAINT [PK_resultado] PRIMARY KEY CLUSTERED 
(
	[pru_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[rayos]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[rayos](
	[ray_id] [int] IDENTITY(1,1) NOT NULL,
	[ray_paciente] [int] NOT NULL,
	[ray_imagen] [varchar](256) NOT NULL,
	[ray_observacion] [nvarchar](max) NULL,
	[ray_fecha] [varchar](10) NULL,
	[ray_responsable] [int] NULL,
	[ray_perfil] [int] NULL,
	[ray_laboratorista] [int] NOT NULL,
	[ray_orden] [int] NULL,
 CONSTRAINT [PK_rayos] PRIMARY KEY CLUSTERED 
(
	[ray_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[registro]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[registro](
	[reg_id] [int] IDENTITY(1,1) NOT NULL,
	[reg_paciente] [int] NOT NULL,
	[reg_orden] [int] NOT NULL,
	[reg_fecha] [varchar](10) NOT NULL,
	[reg_estado] [bit] NULL,
	[reg_laboratorista] [int] NOT NULL,
	[reg_validacion] [varchar](10) NULL,
 CONSTRAINT [PK_registro] PRIMARY KEY CLUSTERED 
(
	[reg_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[reposo]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[reposo](
	[rep_id] [int] NOT NULL,
	[rep_inicio] [varchar](10) NOT NULL,
	[rep_fin] [varchar](10) NOT NULL,
	[rep_tiempo] [int] NOT NULL,
	[rep_ini_txt] [varchar](500) NULL,
	[rep_fin_txt] [varchar](500) NULL,
 CONSTRAINT [PK_reposo] PRIMARY KEY CLUSTERED 
(
	[rep_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[revision]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[revision](
	[rev_id] [int] NOT NULL,
	[rev_organos] [bit] NOT NULL,
	[rev_org_txt] [nvarchar](max) NULL,
	[rev_respiratorio] [bit] NOT NULL,
	[rev_res_txt] [nvarchar](max) NULL,
	[rev_cardiovascular] [bit] NOT NULL,
	[rev_car_txt] [nvarchar](max) NULL,
	[rev_digestivo] [bit] NOT NULL,
	[rev_dig_txt] [nvarchar](max) NULL,
	[rev_genital] [bit] NOT NULL,
	[rev_gen_txt] [nvarchar](max) NULL,
	[rev_urinario] [bit] NOT NULL,
	[rev_uri_txt] [nvarchar](max) NULL,
	[rev_musculo] [bit] NOT NULL,
	[rev_mus_txt] [nvarchar](max) NULL,
	[rev_endocrino] [bit] NOT NULL,
	[rev_end_txt] [nvarchar](max) NULL,
	[rev_linfatico] [bit] NOT NULL,
	[rev_lin_txt] [nvarchar](max) NULL,
	[rev_nervioso] [bit] NOT NULL,
	[rev_ner_txt] [nvarchar](max) NULL,
 CONSTRAINT [PK_revision] PRIMARY KEY CLUSTERED 
(
	[rev_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[riesgo]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[riesgo](
	[rie_id] [int] NOT NULL,
	[rie_grupo] [varchar](100) NOT NULL,
	[rie_nombre] [varchar](100) NOT NULL,
 CONSTRAINT [PK_riesgo_1] PRIMARY KEY CLUSTERED 
(
	[rie_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[signos]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[signos](
	[sig_id] [int] NOT NULL,
	[sig_presion] [varchar](25) NULL,
	[sig_cardiaca] [decimal](10, 2) NULL,
	[sig_respiratoria] [decimal](10, 2) NULL,
	[sig_temperatura] [decimal](10, 2) NULL,
	[sig_peso] [decimal](10, 2) NULL,
	[sig_talla] [decimal](10, 2) NULL,
	[sig_masa] [decimal](10, 2) NULL,
	[sig_perimetro] [decimal](10, 2) NULL,
	[sig_viceral] [decimal](10, 2) NULL,
	[sig_corporal] [decimal](10, 2) NULL,
	[sig_kilocalorias] [decimal](10, 2) NULL,
	[sig_edadpeso] [varchar](25) NULL,
	[sig_masamuscular] [decimal](10, 2) NULL,
 CONSTRAINT [PK_signos] PRIMARY KEY CLUSTERED 
(
	[sig_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sub_cie10]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sub_cie10](
	[sub_id] [int] NOT NULL,
	[sub_codigo] [varchar](50) NOT NULL,
	[sub_descripcion] [varchar](350) NOT NULL,
	[sub_cie10] [int] NOT NULL,
 CONSTRAINT [PK_sub_cie10] PRIMARY KEY CLUSTERED 
(
	[sub_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[subsecuente]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[subsecuente](
	[sub_id] [int] IDENTITY(1,1) NOT NULL,
	[sub_historia] [int] NOT NULL,
	[sub_fecha] [varchar](10) NOT NULL,
	[sub_hora] [varchar](10) NOT NULL,
	[sub_subjetivo] [nvarchar](max) NULL,
	[sub_objetivo] [nvarchar](max) NULL,
	[sub_analisis] [nvarchar](max) NULL,
	[sub_plan] [nvarchar](max) NULL,
 CONSTRAINT [PK_subsecuente] PRIMARY KEY CLUSTERED 
(
	[sub_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[trabajador]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[trabajador](
	[tra_id] [int] IDENTITY(1,1) NOT NULL,
	[tra_cedula] [varchar](10) NOT NULL,
	[tra_nombres] [varchar](150) NOT NULL,
	[tra_apellidos] [varchar](150) NOT NULL,
	[tra_correo] [varchar](150) NOT NULL,
	[tra_empresa] [int] NOT NULL,
	[tra_estado] [bit] NOT NULL,
 CONSTRAINT [PK_trabajador] PRIMARY KEY CLUSTERED 
(
	[tra_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
	[UserMedico] [int] NULL,
	[UserPaciente] [int] NULL,
	[UserEmpresa] [int] NULL,
	[UserLaboratorista] [int] NULL,
	[UserTrabajador] [int] NULL,
	[UserEstado] [bit] NOT NULL CONSTRAINT [DF_UserProfile_UserEstado]  DEFAULT ((1)),
 CONSTRAINT [PK__UserProf__1788CC4C2C3393D0] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__UserProf__C9F284562F10007B] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[vacuna]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vacuna](
	[vac_id] [int] NOT NULL,
	[vac_nombre] [varchar](50) NOT NULL,
	[vac_dosis] [int] NOT NULL,
	[vac_esquema] [varchar](200) NOT NULL,
 CONSTRAINT [PK_vacuna] PRIMARY KEY CLUSTERED 
(
	[vac_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[valores]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[valores](
	[val_id] [int] NOT NULL,
	[val_minimo] [decimal](10, 2) NULL,
	[val_maximo] [decimal](10, 2) NULL,
	[val_categoria] [varchar](50) NOT NULL,
	[val_examen] [int] NOT NULL,
 CONSTRAINT [PK_valores] PRIMARY KEY CLUSTERED 
(
	[val_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL DEFAULT ((0)),
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL DEFAULT ((0)),
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK__webpages__8AFACE1A3C69FB99] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__webpages__8A2B61603F466844] UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[getHistorias]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza
-- Create date: 21/06/2015
-- Description:	Determinar si un paciente tiene historias en un periodo de tiempo
-- =============================================
CREATE FUNCTION [dbo].[getHistorias] 
(	
	-- Add the parameters for the function here
	@paciente int,
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT  cast(
		case when 
			(select  count(*) from historia where CONVERT(date,his_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  and his_paciente=@paciente) >0
		 then 1 else 0 end as bit) as h
	from paciente
	where pac_id=@paciente
	/*where his_paciente=@paciente*/
)

GO
/****** Object:  View [dbo].[view_audiometria]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_audiometria]
AS
SELECT        dbo.audiometria.aud_id, dbo.paciente.pac_cedula, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.paciente.pac_edad, dbo.paciente.pac_genero, dbo.empresa.emp_nombre, 
                         dbo.audiometria.aud_observacion, dbo.audiometria.aud_fecha, dbo.audiometria.aud_responsable, dbo.audiometria.aud_perfil, dbo.getResponsable(dbo.audiometria.aud_responsable, dbo.audiometria.aud_perfil) 
                         AS responsable, dbo.getFirma(dbo.audiometria.aud_responsable, dbo.audiometria.aud_perfil) AS firma, dbo.audiometria.aud_orden
FROM            dbo.audiometria INNER JOIN
                         dbo.paciente ON dbo.audiometria.aud_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.empresa ON dbo.paciente.pac_empresa = dbo.empresa.emp_id

GO
/****** Object:  View [dbo].[view_codigo]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_codigo]
AS
SELECT        dbo.codigo.cod_codigo, dbo.codigo.cod_imagen, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.registro.reg_id
FROM            dbo.codigo INNER JOIN
                         dbo.registro ON dbo.codigo.cod_registro = dbo.registro.reg_id INNER JOIN
                         dbo.paciente ON dbo.registro.reg_paciente = dbo.paciente.pac_id

GO
/****** Object:  View [dbo].[view_diagnostico]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_diagnostico]
AS
SELECT        dbo.cie10.cie_codigo, dbo.sub_cie10.sub_codigo, dbo.sub_cie10.sub_descripcion, dbo.diagnostico.dia_descripcion, dbo.diagnostico.dia_tipo, dbo.diagnostico.dia_historia
FROM            dbo.diagnostico INNER JOIN
                         dbo.sub_cie10 ON dbo.diagnostico.dia_subcie10 = dbo.sub_cie10.sub_id INNER JOIN
                         dbo.cie10 ON dbo.sub_cie10.sub_cie10 = dbo.cie10.cie_id

GO
/****** Object:  View [dbo].[view_espirometria]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_espirometria]
AS
SELECT        dbo.espirometria.esp_id, dbo.paciente.pac_cedula, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.paciente.pac_genero, dbo.paciente.pac_edad, dbo.empresa.emp_nombre, 
                         dbo.espirometria.esp_fecha, dbo.espirometria.esp_responsable, dbo.espirometria.esp_perfil, dbo.espirometria.esp_observacion, dbo.getResponsable(dbo.espirometria.esp_responsable, 
                         dbo.espirometria.esp_perfil) AS responsable, dbo.getFirma(dbo.espirometria.esp_responsable, dbo.espirometria.esp_perfil) AS firma, dbo.espirometria.esp_orden
FROM            dbo.espirometria INNER JOIN
                         dbo.paciente ON dbo.espirometria.esp_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.empresa ON dbo.paciente.pac_empresa = dbo.empresa.emp_id

GO
/****** Object:  View [dbo].[view_historia]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_historia]
AS
SELECT        dbo.historia.his_id, dbo.paciente.pac_cedula, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.paciente.pac_genero, dbo.paciente.pac_edad, dbo.familiar.fam_car_txt, dbo.familiar.fam_dia_txt, 
                         dbo.familiar.fam_vas_txt, dbo.familiar.fam_hip_txt, dbo.familiar.fam_can_txt, dbo.familiar.fam_tub_txt, dbo.familiar.fam_men_txt, dbo.familiar.fam_inf_txt, dbo.familiar.fam_mal_txt, dbo.familiar.fam_hit_txt, 
                         dbo.revision.rev_org_txt, dbo.revision.rev_res_txt, dbo.revision.rev_car_txt, dbo.revision.rev_dig_txt, dbo.revision.rev_gen_txt, dbo.revision.rev_uri_txt, dbo.revision.rev_mus_txt, dbo.revision.rev_end_txt, 
                         dbo.revision.rev_lin_txt, dbo.revision.rev_ner_txt, dbo.familiar.fam_otr_txt, dbo.personal.per_pat_txt, dbo.personal.per_qui_txt, dbo.personal.per_tra_txt, dbo.personal.per_ale_txt, dbo.personal.per_vac_txt, 
                         dbo.personal.per_otr_txt, dbo.fisico.fis_pie_txt, dbo.fisico.fis_cab_txt, dbo.fisico.fis_ojo_txt, dbo.fisico.fis_oid_txt, dbo.fisico.fis_nar_txt, dbo.fisico.fis_boc_txt, dbo.fisico.fis_far_txt, dbo.fisico.fis_cue_txt, 
                         dbo.fisico.fis_axi_txt, dbo.fisico.fis_tor_txt, dbo.fisico.fis_abd_txt, dbo.fisico.fis_col_txt, dbo.fisico.fis_ing_txt, dbo.fisico.fis_msp_txt, dbo.fisico.fis_mif_txt, dbo.fisico.fis_org_txt, dbo.fisico.fis_res_txt, 
                         dbo.fisico.fis_car_txt, dbo.fisico.fis_dig_txt, dbo.fisico.fis_fen_txt, dbo.fisico.fis_uri_txt, dbo.fisico.fis_esq_txt, dbo.fisico.fis_end_txt, dbo.fisico.fis_lin_txt, dbo.fisico.fis_neu_txt, dbo.signos.sig_presion, 
                         dbo.signos.sig_cardiaca, dbo.signos.sig_respiratoria, dbo.signos.sig_temperatura, dbo.signos.sig_peso, dbo.signos.sig_talla, dbo.signos.sig_masa, dbo.signos.sig_perimetro, dbo.signos.sig_viceral, 
                         dbo.signos.sig_corporal, dbo.signos.sig_kilocalorias, dbo.signos.sig_edadpeso, dbo.signos.sig_masamuscular, dbo.[plan].pla_texto1, dbo.[plan].pla_texto2, dbo.historia.his_problema, dbo.historia.his_motivo, 
                         dbo.historia.his_fecha, dbo.historia.his_tipo, dbo.historia.his_firma, dbo.medico.med_firma, dbo.medico.med_nombres, dbo.medico.med_apellidos, dbo.medico.med_codigo, dbo.historia.his_numero, 
                         dbo.medico.med_cedula, dbo.paciente.pac_fechanacimiento, dbo.paciente.pac_celular, dbo.paciente.pac_telefono, dbo.paciente.pac_instruccion, dbo.profesion.pro_nombre, dbo.paciente.pac_estadocivil, 
                         dbo.habitos.hab_fumo, dbo.habitos.hab_fuma, dbo.habitos.hab_cigarillos, dbo.habitos.hab_alcohol, dbo.habitos.hab_drogas, dbo.habitos.hab_ejercicio, dbo.actividad.act_enf_estado, 
                         dbo.actividad.act_enf_descripcion, dbo.actividad.act_enf_fecha, dbo.actividad.act_enf_empresa, dbo.actividad.act_acc_estado, dbo.actividad.act_acc_descripcion, dbo.actividad.act_acc_capacidad, 
                         dbo.actividad.act_acc_fecha, dbo.actividad.act_acc_empresa, dbo.habitos.hab_alc_txt, dbo.habitos.hab_dro_txt
FROM            dbo.historia INNER JOIN
                         dbo.paciente ON dbo.historia.his_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.familiar ON dbo.paciente.pac_id = dbo.familiar.fam_id INNER JOIN
                         dbo.personal ON dbo.paciente.pac_id = dbo.personal.per_id INNER JOIN
                         dbo.fisico ON dbo.historia.his_id = dbo.fisico.fis_id INNER JOIN
                         dbo.signos ON dbo.historia.his_id = dbo.signos.sig_id INNER JOIN
                         dbo.revision ON dbo.historia.his_id = dbo.revision.rev_id INNER JOIN
                         dbo.[plan] ON dbo.historia.his_id = dbo.[plan].pla_id INNER JOIN
                         dbo.medico ON dbo.historia.his_medico = dbo.medico.med_id LEFT OUTER JOIN
                         dbo.actividad ON dbo.paciente.pac_id = dbo.actividad.act_id LEFT OUTER JOIN
                         dbo.habitos ON dbo.paciente.pac_id = dbo.habitos.hab_id LEFT OUTER JOIN
                         dbo.profesion ON dbo.paciente.pac_profesion = dbo.profesion.pro_id

GO
/****** Object:  View [dbo].[view_inmunizacion]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_inmunizacion]
AS
SELECT        dbo.vacuna.vac_nombre, dbo.inmunizacion.inm_fecha, dbo.inmunizacion.inm_tipo, dbo.inmunizacion.inm_paciente
FROM            dbo.inmunizacion INNER JOIN
                         dbo.vacuna ON dbo.inmunizacion.inm_vacuna = dbo.vacuna.vac_id

GO
/****** Object:  View [dbo].[view_oftalmologia]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_oftalmologia]
AS
SELECT        dbo.oftalmologia.oft_id, dbo.oftalmologia.oft_con_od, dbo.oftalmologia.oft_con_oi, dbo.oftalmologia.oft_sin_od, dbo.oftalmologia.oft_sin_oi, dbo.oftalmologia.oft_ref_od, dbo.oftalmologia.oft_ref_oi, 
                         dbo.oftalmologia.oft_biomiscroscopia, dbo.oftalmologia.oft_bio_txt, dbo.oftalmologia.oft_fondo, dbo.oftalmologia.oft_fon_txt, dbo.oftalmologia.oft_colores, dbo.oftalmologia.oft_diagnostico, 
                         dbo.oftalmologia.oft_dia_txt, dbo.oftalmologia.oft_indicaciones, dbo.oftalmologia.oft_ind_txt, dbo.oftalmologia.oft_fecha, dbo.oftalmologia.oft_otros, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, 
                         dbo.paciente.pac_genero, dbo.paciente.pac_edad, dbo.getResponsable(dbo.oftalmologia.oft_responsable, dbo.oftalmologia.oft_perfil) AS responsable, dbo.getFirma(dbo.oftalmologia.oft_responsable, 
                         dbo.oftalmologia.oft_perfil) AS firma, dbo.empresa.emp_nombre, dbo.paciente.pac_cedula, dbo.oftalmologia.oft_orden
FROM            dbo.oftalmologia INNER JOIN
                         dbo.paciente ON dbo.oftalmologia.oft_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.empresa ON dbo.paciente.pac_empresa = dbo.empresa.emp_id

GO
/****** Object:  View [dbo].[view_prueba_paciente]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_prueba_paciente]
AS
SELECT        dbo.prueba.pru_id, dbo.examen.exa_nombre, dbo.prueba.pru_resultado, dbo.examen.exa_unidad, dbo.examen.exa_valores, dbo.area.are_nombre, dbo.registro.reg_paciente, dbo.registro.reg_fecha, 
                         dbo.prueba.pru_valor, dbo.registro.reg_id, dbo.examen.exa_id, dbo.laboratorista.lab_cedula, dbo.laboratorista.lab_nombres, dbo.laboratorista.lab_apellidos, dbo.laboratorista.lab_firma, dbo.paciente.pac_cedula,
                          dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.empresa.emp_nombre
FROM            dbo.examen INNER JOIN
                         dbo.area ON dbo.examen.exa_area = dbo.area.are_id INNER JOIN
                         dbo.prueba ON dbo.examen.exa_id = dbo.prueba.pru_examen INNER JOIN
                         dbo.registro ON dbo.prueba.pru_registro = dbo.registro.reg_id INNER JOIN
                         dbo.laboratorista ON dbo.registro.reg_laboratorista = dbo.laboratorista.lab_id INNER JOIN
                         dbo.paciente ON dbo.registro.reg_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.empresa ON dbo.paciente.pac_empresa = dbo.empresa.emp_id
WHERE        (dbo.prueba.pru_resultado IS NOT NULL OR
                         dbo.prueba.pru_resultado <> '') AND (dbo.registro.reg_validacion = 'valido')

GO
/****** Object:  View [dbo].[view_rayos]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_rayos]
AS
SELECT        dbo.rayos.ray_id, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.paciente.pac_genero, dbo.paciente.pac_edad, dbo.rayos.ray_observacion, dbo.rayos.ray_fecha, 
                         dbo.getResponsable(dbo.rayos.ray_responsable, dbo.rayos.ray_perfil) AS responsable, dbo.getFirma(dbo.rayos.ray_responsable, dbo.rayos.ray_perfil) AS firma, dbo.empresa.emp_nombre, 
                         dbo.paciente.pac_cedula, dbo.rayos.ray_orden
FROM            dbo.rayos INNER JOIN
                         dbo.paciente ON dbo.rayos.ray_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.empresa ON dbo.paciente.pac_empresa = dbo.empresa.emp_id

GO
/****** Object:  View [dbo].[view_reporte01]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_reporte01]
AS
SELECT        dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, historia_1.his_fecha, audiometria_1.aud_fecha, espirometria_1.esp_fecha, oftalmologia_1.oft_fecha, rayos_1.ray_fecha, registro_1.reg_fecha
FROM            dbo.paciente LEFT OUTER JOIN
                         dbo.audiometria AS audiometria_1 ON dbo.paciente.pac_id = audiometria_1.aud_paciente LEFT OUTER JOIN
                         dbo.espirometria AS espirometria_1 ON dbo.paciente.pac_id = espirometria_1.esp_paciente LEFT OUTER JOIN
                         dbo.oftalmologia AS oftalmologia_1 ON dbo.paciente.pac_id = oftalmologia_1.oft_paciente LEFT OUTER JOIN
                         dbo.rayos AS rayos_1 ON dbo.paciente.pac_id = rayos_1.ray_paciente LEFT OUTER JOIN
                         dbo.registro AS registro_1 ON dbo.paciente.pac_id = registro_1.reg_paciente LEFT OUTER JOIN
                         dbo.historia AS historia_1 ON dbo.paciente.pac_id = historia_1.his_paciente

GO
/****** Object:  View [dbo].[view_reporte02]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_reporte02]
AS
SELECT        dbo.paciente.pac_id, dbo.historia.his_fecha, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.paciente.pac_edad, dbo.paciente.pac_genero, dbo.medico.med_nombres, dbo.medico.med_apellidos,
                             (SELECT        ocu_cargo
                               FROM            dbo.ocupacional
                               WHERE        (ocu_paciente = dbo.paciente.pac_id) AND (ocu_tipo = 'actual')) AS Puesto,
                             (SELECT        ocu_seccion
                               FROM            dbo.ocupacional AS ocupacional_1
                               WHERE        (ocu_paciente = dbo.paciente.pac_id) AND (ocu_tipo = 'actual')) AS Area, 
                         CASE historia.his_tipo WHEN 1 THEN 'GENERAL' WHEN 2 THEN 'PREOCUPACIONAL' WHEN 3 THEN 'OCUPACIONAL' WHEN 4 THEN 'RETIRO' END AS Tipo, dbo.registro.reg_fecha
FROM            dbo.historia INNER JOIN
                         dbo.paciente ON dbo.historia.his_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.medico ON dbo.historia.his_medico = dbo.medico.med_id LEFT OUTER JOIN
                         dbo.registro ON dbo.paciente.pac_id = dbo.registro.reg_paciente

GO
/****** Object:  View [dbo].[view_reposo]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_reposo]
AS
SELECT        dbo.historia.his_id, dbo.paciente.pac_cedula, dbo.paciente.pac_nombres, dbo.paciente.pac_apellidos, dbo.diagnostico.dia_subcie10, dbo.sub_cie10.sub_descripcion, dbo.sub_cie10.sub_cie10, 
                         dbo.cie10.cie_descripcion, dbo.reposo.rep_inicio, dbo.reposo.rep_ini_txt,dbo.reposo.rep_fin,dbo.reposo.rep_fin_txt, dbo.reposo.rep_tiempo, dbo.medico.med_cedula, dbo.medico.med_nombres, dbo.medico.med_apellidos, dbo.medico.med_codigo, 
                         dbo.medico.med_firma
FROM            dbo.historia INNER JOIN
                         dbo.reposo ON dbo.historia.his_id = dbo.reposo.rep_id INNER JOIN
                         dbo.diagnostico ON dbo.historia.his_id = dbo.diagnostico.dia_historia INNER JOIN
                         dbo.sub_cie10 ON dbo.diagnostico.dia_subcie10 = dbo.sub_cie10.sub_id INNER JOIN
                         dbo.cie10 ON dbo.sub_cie10.sub_cie10 = dbo.cie10.cie_id INNER JOIN
                         dbo.paciente ON dbo.historia.his_paciente = dbo.paciente.pac_id INNER JOIN
                         dbo.medico ON dbo.historia.his_medico = dbo.medico.med_id

GO
/****** Object:  View [dbo].[view_riesgo]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_riesgo]
AS
SELECT        dbo.riesgo.rie_grupo, dbo.riesgo.rie_nombre, dbo.laboral.lab_estado, dbo.ocupacional.ocu_tipo, dbo.ocupacional.ocu_paciente
FROM            dbo.riesgo INNER JOIN
                         dbo.laboral ON dbo.riesgo.rie_id = dbo.laboral.lab_riesgo INNER JOIN
                         dbo.ocupacional ON dbo.laboral.lab_ocupacional = dbo.ocupacional.ocu_id
WHERE        (dbo.laboral.lab_estado = 1) AND (dbo.ocupacional.ocu_tipo = 'actual')
GO
ALTER TABLE [dbo].[accidente]  WITH CHECK ADD  CONSTRAINT [FK_accidente_paciente] FOREIGN KEY([acc_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[accidente] CHECK CONSTRAINT [FK_accidente_paciente]
GO
ALTER TABLE [dbo].[actividad]  WITH CHECK ADD  CONSTRAINT [FK_actividad_paciente] FOREIGN KEY([act_id])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[actividad] CHECK CONSTRAINT [FK_actividad_paciente]
GO
ALTER TABLE [dbo].[audiometria]  WITH CHECK ADD  CONSTRAINT [FK_audiometria_laboratorista] FOREIGN KEY([aud_laboratorista])
REFERENCES [dbo].[laboratorista] ([lab_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[audiometria] CHECK CONSTRAINT [FK_audiometria_laboratorista]
GO
ALTER TABLE [dbo].[audiometria]  WITH CHECK ADD  CONSTRAINT [FK_audiometria_paciente] FOREIGN KEY([aud_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[audiometria] CHECK CONSTRAINT [FK_audiometria_paciente]
GO
ALTER TABLE [dbo].[canton]  WITH CHECK ADD  CONSTRAINT [FK_canton_provincia] FOREIGN KEY([can_provincia])
REFERENCES [dbo].[provincia] ([pro_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[canton] CHECK CONSTRAINT [FK_canton_provincia]
GO
ALTER TABLE [dbo].[codigo]  WITH CHECK ADD  CONSTRAINT [FK_codigo_area] FOREIGN KEY([cod_area])
REFERENCES [dbo].[area] ([are_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[codigo] CHECK CONSTRAINT [FK_codigo_area]
GO
ALTER TABLE [dbo].[codigo]  WITH CHECK ADD  CONSTRAINT [FK_codigo_registro] FOREIGN KEY([cod_registro])
REFERENCES [dbo].[registro] ([reg_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[codigo] CHECK CONSTRAINT [FK_codigo_registro]
GO
ALTER TABLE [dbo].[concepto]  WITH CHECK ADD  CONSTRAINT [FK_concepto_historia] FOREIGN KEY([con_id])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[concepto] CHECK CONSTRAINT [FK_concepto_historia]
GO
ALTER TABLE [dbo].[control]  WITH CHECK ADD  CONSTRAINT [FK_control_examen] FOREIGN KEY([con_examen])
REFERENCES [dbo].[examen] ([exa_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[control] CHECK CONSTRAINT [FK_control_examen]
GO
ALTER TABLE [dbo].[control]  WITH CHECK ADD  CONSTRAINT [FK_control_perfil] FOREIGN KEY([con_perfil])
REFERENCES [dbo].[perfil] ([per_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[control] CHECK CONSTRAINT [FK_control_perfil]
GO
ALTER TABLE [dbo].[diagnostico]  WITH CHECK ADD  CONSTRAINT [FK_diagnostico_historia] FOREIGN KEY([dia_historia])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[diagnostico] CHECK CONSTRAINT [FK_diagnostico_historia]
GO
ALTER TABLE [dbo].[diagnostico]  WITH CHECK ADD  CONSTRAINT [FK_diagnostico_sub_cie10] FOREIGN KEY([dia_subcie10])
REFERENCES [dbo].[sub_cie10] ([sub_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[diagnostico] CHECK CONSTRAINT [FK_diagnostico_sub_cie10]
GO
ALTER TABLE [dbo].[doctor]  WITH CHECK ADD  CONSTRAINT [FK_doctor_empresa] FOREIGN KEY([doc_empresa])
REFERENCES [dbo].[empresa] ([emp_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[doctor] CHECK CONSTRAINT [FK_doctor_empresa]
GO
ALTER TABLE [dbo].[doctor]  WITH CHECK ADD  CONSTRAINT [FK_doctor_especialidad] FOREIGN KEY([doc_especialidad])
REFERENCES [dbo].[especialidad] ([esp_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[doctor] CHECK CONSTRAINT [FK_doctor_especialidad]
GO
ALTER TABLE [dbo].[enfermedad]  WITH CHECK ADD  CONSTRAINT [FK_enfermedad_paciente] FOREIGN KEY([enf_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[enfermedad] CHECK CONSTRAINT [FK_enfermedad_paciente]
GO
ALTER TABLE [dbo].[espirometria]  WITH CHECK ADD  CONSTRAINT [FK_espirometria_laboratorista] FOREIGN KEY([esp_laboratorista])
REFERENCES [dbo].[laboratorista] ([lab_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[espirometria] CHECK CONSTRAINT [FK_espirometria_laboratorista]
GO
ALTER TABLE [dbo].[espirometria]  WITH CHECK ADD  CONSTRAINT [FK_espirometria_paciente] FOREIGN KEY([esp_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[espirometria] CHECK CONSTRAINT [FK_espirometria_paciente]
GO
ALTER TABLE [dbo].[examen]  WITH CHECK ADD  CONSTRAINT [FK_examen_area] FOREIGN KEY([exa_area])
REFERENCES [dbo].[area] ([are_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[examen] CHECK CONSTRAINT [FK_examen_area]
GO
ALTER TABLE [dbo].[familiar]  WITH CHECK ADD  CONSTRAINT [FK_familiar_paciente] FOREIGN KEY([fam_id])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[familiar] CHECK CONSTRAINT [FK_familiar_paciente]
GO
ALTER TABLE [dbo].[fisico]  WITH CHECK ADD  CONSTRAINT [FK_fisico_historia] FOREIGN KEY([fis_id])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[fisico] CHECK CONSTRAINT [FK_fisico_historia]
GO
ALTER TABLE [dbo].[ginecologico]  WITH CHECK ADD  CONSTRAINT [FK_ginecologico_paciente] FOREIGN KEY([gin_id])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ginecologico] CHECK CONSTRAINT [FK_ginecologico_paciente]
GO
ALTER TABLE [dbo].[habitos]  WITH CHECK ADD  CONSTRAINT [FK_habitos_paciente] FOREIGN KEY([hab_id])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[habitos] CHECK CONSTRAINT [FK_habitos_paciente]
GO
ALTER TABLE [dbo].[historia]  WITH CHECK ADD  CONSTRAINT [FK_historia_medico] FOREIGN KEY([his_medico])
REFERENCES [dbo].[medico] ([med_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[historia] CHECK CONSTRAINT [FK_historia_medico]
GO
ALTER TABLE [dbo].[historia]  WITH CHECK ADD  CONSTRAINT [FK_historia_paciente] FOREIGN KEY([his_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[historia] CHECK CONSTRAINT [FK_historia_paciente]
GO
ALTER TABLE [dbo].[inmunizacion]  WITH CHECK ADD  CONSTRAINT [FK_inmunizacion_paciente] FOREIGN KEY([inm_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[inmunizacion] CHECK CONSTRAINT [FK_inmunizacion_paciente]
GO
ALTER TABLE [dbo].[inmunizacion]  WITH CHECK ADD  CONSTRAINT [FK_inmunizacion_vacuna] FOREIGN KEY([inm_vacuna])
REFERENCES [dbo].[vacuna] ([vac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[inmunizacion] CHECK CONSTRAINT [FK_inmunizacion_vacuna]
GO
ALTER TABLE [dbo].[laboral]  WITH CHECK ADD  CONSTRAINT [FK_laboral_ocupacional] FOREIGN KEY([lab_ocupacional])
REFERENCES [dbo].[ocupacional] ([ocu_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[laboral] CHECK CONSTRAINT [FK_laboral_ocupacional]
GO
ALTER TABLE [dbo].[laboral]  WITH CHECK ADD  CONSTRAINT [FK_laboral_riesgo] FOREIGN KEY([lab_riesgo])
REFERENCES [dbo].[riesgo] ([rie_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[laboral] CHECK CONSTRAINT [FK_laboral_riesgo]
GO
ALTER TABLE [dbo].[medico]  WITH CHECK ADD  CONSTRAINT [FK_medico_especialidad] FOREIGN KEY([med_especialidad])
REFERENCES [dbo].[especialidad] ([esp_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[medico] CHECK CONSTRAINT [FK_medico_especialidad]
GO
ALTER TABLE [dbo].[ocupacional]  WITH CHECK ADD  CONSTRAINT [FK_ocupacional_paciente] FOREIGN KEY([ocu_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ocupacional] CHECK CONSTRAINT [FK_ocupacional_paciente]
GO
ALTER TABLE [dbo].[oftalmologia]  WITH CHECK ADD  CONSTRAINT [FK_oftalmologia_paciente] FOREIGN KEY([oft_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[oftalmologia] CHECK CONSTRAINT [FK_oftalmologia_paciente]
GO
ALTER TABLE [dbo].[orden]  WITH CHECK ADD  CONSTRAINT [FK_orden_examen] FOREIGN KEY([ord_examen])
REFERENCES [dbo].[examen] ([exa_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[orden] CHECK CONSTRAINT [FK_orden_examen]
GO
ALTER TABLE [dbo].[orden]  WITH CHECK ADD  CONSTRAINT [FK_orden_paciente] FOREIGN KEY([ord_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[orden] CHECK CONSTRAINT [FK_orden_paciente]
GO
ALTER TABLE [dbo].[paciente]  WITH CHECK ADD  CONSTRAINT [FK_paciente_canton] FOREIGN KEY([pac_canton])
REFERENCES [dbo].[canton] ([can_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[paciente] CHECK CONSTRAINT [FK_paciente_canton]
GO
ALTER TABLE [dbo].[paciente]  WITH CHECK ADD  CONSTRAINT [FK_paciente_empresa] FOREIGN KEY([pac_empresa])
REFERENCES [dbo].[empresa] ([emp_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[paciente] CHECK CONSTRAINT [FK_paciente_empresa]
GO
ALTER TABLE [dbo].[paciente]  WITH CHECK ADD  CONSTRAINT [FK_paciente_pais] FOREIGN KEY([pac_pais])
REFERENCES [dbo].[pais] ([pais_id])
GO
ALTER TABLE [dbo].[paciente] CHECK CONSTRAINT [FK_paciente_pais]
GO
ALTER TABLE [dbo].[paciente]  WITH CHECK ADD  CONSTRAINT [FK_paciente_profesion] FOREIGN KEY([pac_profesion])
REFERENCES [dbo].[profesion] ([pro_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[paciente] CHECK CONSTRAINT [FK_paciente_profesion]
GO
ALTER TABLE [dbo].[personal]  WITH CHECK ADD  CONSTRAINT [FK_personal_paciente] FOREIGN KEY([per_id])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[personal] CHECK CONSTRAINT [FK_personal_paciente]
GO
ALTER TABLE [dbo].[plan]  WITH CHECK ADD  CONSTRAINT [FK_plan_historia] FOREIGN KEY([pla_id])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[plan] CHECK CONSTRAINT [FK_plan_historia]
GO
ALTER TABLE [dbo].[prueba]  WITH CHECK ADD  CONSTRAINT [FK_prueba_examen] FOREIGN KEY([pru_examen])
REFERENCES [dbo].[examen] ([exa_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[prueba] CHECK CONSTRAINT [FK_prueba_examen]
GO
ALTER TABLE [dbo].[prueba]  WITH CHECK ADD  CONSTRAINT [FK_prueba_registro] FOREIGN KEY([pru_registro])
REFERENCES [dbo].[registro] ([reg_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[prueba] CHECK CONSTRAINT [FK_prueba_registro]
GO
ALTER TABLE [dbo].[rayos]  WITH CHECK ADD  CONSTRAINT [FK_rayos_laboratorista] FOREIGN KEY([ray_laboratorista])
REFERENCES [dbo].[laboratorista] ([lab_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rayos] CHECK CONSTRAINT [FK_rayos_laboratorista]
GO
ALTER TABLE [dbo].[rayos]  WITH CHECK ADD  CONSTRAINT [FK_rayos_paciente] FOREIGN KEY([ray_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[rayos] CHECK CONSTRAINT [FK_rayos_paciente]
GO
ALTER TABLE [dbo].[registro]  WITH CHECK ADD  CONSTRAINT [FK_registro_laboratorista] FOREIGN KEY([reg_laboratorista])
REFERENCES [dbo].[laboratorista] ([lab_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[registro] CHECK CONSTRAINT [FK_registro_laboratorista]
GO
ALTER TABLE [dbo].[registro]  WITH CHECK ADD  CONSTRAINT [FK_registro_paciente] FOREIGN KEY([reg_paciente])
REFERENCES [dbo].[paciente] ([pac_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[registro] CHECK CONSTRAINT [FK_registro_paciente]
GO
ALTER TABLE [dbo].[reposo]  WITH CHECK ADD  CONSTRAINT [FK_reposo_historia] FOREIGN KEY([rep_id])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[reposo] CHECK CONSTRAINT [FK_reposo_historia]
GO
ALTER TABLE [dbo].[revision]  WITH CHECK ADD  CONSTRAINT [FK_revision_historia] FOREIGN KEY([rev_id])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[revision] CHECK CONSTRAINT [FK_revision_historia]
GO
ALTER TABLE [dbo].[signos]  WITH CHECK ADD  CONSTRAINT [FK_signos_historia] FOREIGN KEY([sig_id])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[signos] CHECK CONSTRAINT [FK_signos_historia]
GO
ALTER TABLE [dbo].[sub_cie10]  WITH CHECK ADD  CONSTRAINT [FK_sub_cie10_cie10] FOREIGN KEY([sub_cie10])
REFERENCES [dbo].[cie10] ([cie_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[sub_cie10] CHECK CONSTRAINT [FK_sub_cie10_cie10]
GO
ALTER TABLE [dbo].[subsecuente]  WITH CHECK ADD  CONSTRAINT [FK_subsecuente_historia] FOREIGN KEY([sub_historia])
REFERENCES [dbo].[historia] ([his_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[subsecuente] CHECK CONSTRAINT [FK_subsecuente_historia]
GO
ALTER TABLE [dbo].[trabajador]  WITH CHECK ADD  CONSTRAINT [FK_trabajador_empresa] FOREIGN KEY([tra_empresa])
REFERENCES [dbo].[empresa] ([emp_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[trabajador] CHECK CONSTRAINT [FK_trabajador_empresa]
GO
ALTER TABLE [dbo].[valores]  WITH CHECK ADD  CONSTRAINT [FK_valores_examen] FOREIGN KEY([val_examen])
REFERENCES [dbo].[examen] ([exa_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[valores] CHECK CONSTRAINT [FK_valores_examen]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
/****** Object:  StoredProcedure [dbo].[getReporte01]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte01] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ROW_NUMBER() over(order by emp_nombre) as id,  pac_apellidos, pac_nombres, emp_nombre,

	cast(
		case when 
			(select  count(*) from historia 
			where CONVERT(date,his_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and his_paciente=pac_id) >0
		 then 1 else 0 end as int
		 ) as historia,
	CAST(
			case when (select count(*) from registro 
			where CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and reg_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Laboratorio,
	CAST(
		case when (select count(*) from audiometria 
		where CONVERT(date,aud_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
		and aud_paciente=pac_id)>0
		then 1 else 0 end as int
	) AS Audiometria,
	CAST(
		case when (select count(*) from oftalmologia 
		where CONVERT(date,oft_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
		and oft_paciente=pac_id)>0
		then 1 else 0 end as int
	) AS Oftalmologia,
	CAST(
		case when (select count(*) from espirometria 
		where CONVERT(date,esp_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
		and esp_paciente=pac_id)>0
		then 1 else 0 end as int
	) AS Espirometria,
	CAST (
		case when (select count(*) from rayos 
		where CONVERT(date,ray_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
		and ray_paciente=pac_id)>0
		then 1 else 0 end as int
	) AS Rayos
	
	from paciente
	
	inner join empresa on pac_empresa=emp_id
	inner join registro on reg_paciente=pac_id
	where CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
	order by emp_nombre
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte02]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte02]
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select ROW_NUMBER() over(order by emp_nombre) as Numero, empresa.emp_nombre,
		historia.his_fecha as Fecha, (paciente.pac_apellidos+' '+paciente.pac_nombres) as Paciente, paciente.pac_edad as Edad, paciente.pac_genero as Genero, 
		paciente.pac_instruccion as Instruccion, paciente.pac_estadocivil as EstadoCivil,
		(med_nombres+' '+med_apellidos) as Medico,
		(Select top 1 ocu_cargo from ocupacional where ocu_paciente=paciente.pac_id and  ocu_tipo='actual') as Puesto,
		(Select top 1 ocu_seccion from ocupacional where ocu_paciente=paciente.pac_id and  ocu_tipo='actual') as Area,
		actividad.act_enf_descripcion as Enfermedad, actividad.act_acc_descripcion as Accidente,
		habitos.hab_fuma as Fuma, habitos.hab_alcohol as Alcohol, habitos.hab_ejercicio as Ejercicio,
		ginecologico.gin_paptest as PapTest,
		CAST(
			case when (select count(*) from inmunizacion 
			where inm_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Inmunizacion,
		signos.sig_peso as Peso, signos.sig_talla as Talla, signos.sig_corporal as IMC,
		

		Tipo= case historia.his_tipo
			when 1 then 'GENERAL'
			when 2 then 'PREOCUPACIONAL'
			when 3 then 'OCUPACIONAL'
			when 4 then 'RETIRO'
			end,
		CAST(
			case when (select count(*) from registro 
			where CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and reg_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Laboratorio,
		CAST(
			case when (select count(*) from audiometria 
			where CONVERT(date,aud_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and aud_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Audiometria,
		CAST(
			case when (select count(*) from oftalmologia 
			where CONVERT(date,oft_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and oft_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Oftalmologia,
		CAST(
			case when (select count(*) from espirometria 
			where CONVERT(date,esp_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and esp_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Espirometria,
		CAST (
			case when (select count(*) from rayos 
			where CONVERT(date,ray_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and ray_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Rayos,
		
		cast(
			case when 
			(select count(*) from registro 
			where CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and reg_paciente=pac_id)>0
			OR
			(select count(*) from audiometria 
			where CONVERT(date,aud_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and aud_paciente=pac_id)>0
			OR
			(select count(*) from oftalmologia 
			where CONVERT(date,oft_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and oft_paciente=pac_id)>0
			OR
			(select count(*) from espirometria 
			where CONVERT(date,esp_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and esp_paciente=pac_id)>0
			OR
			(select count(*) from rayos 
			where CONVERT(date,ray_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and ray_paciente=pac_id)>0
			AND
			(select  count(*) from historia 
			where CONVERT(date,his_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and his_paciente=pac_id) >0
		then 1 else 0 end as int) as Chequeo,
		(Select con_resultado from concepto where con_id=historia.his_id) as Certificado,

		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  0 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico1,
		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  1 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico2,
		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  2 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico3,
		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  3 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico4,
		concepto.con_seguimiento as Seguimiento,
		concepto.con_periodo as Periodicidad
		
		
		 
	from historia
	left join paciente on pac_id=his_paciente
	inner join medico on med_id=his_medico
	left join habitos on hab_id=pac_id
	left join ginecologico on gin_id=pac_id
	left join signos on sig_id=pac_id
	left join concepto on con_id=his_id
	left join actividad on act_id=pac_id
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,his_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte03]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza
-- Create date: 27/06/2016
-- Description:	Numero de registros de laboratorio por exámen
-- =============================================
CREATE PROCEDURE [dbo].[getReporte03] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select empresa.emp_nombre as Empresa, examen.exa_nombre as Examen,  count(*) Total
	from prueba
	join examen on pru_examen=exa_id
	join registro on pru_registro=reg_id
	join paciente on reg_paciente=pac_id
	join empresa on emp_id=pac_empresa
	where exa_estado!='subgrupo'  and exa_estado!='INACTIVO' and reg_estado=1 
	and CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
	group by empresa.emp_nombre, examen.exa_nombre
	order by empresa.emp_nombre, examen.exa_nombre
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte04]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza
-- Create date: 27/06/2016
-- Description:	Numero de audiometrias por empresa
-- =============================================
CREATE PROCEDURE [dbo].[getReporte04] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select empresa.emp_nombre as Empresa, count(*) Audiometrias
	from audiometria
	inner join paciente on aud_paciente=pac_id
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,aud_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
	group by emp_nombre
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte05]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza
-- Create date: 27/06/2016
-- Description:	Numero de espirometrias por empresa
-- =============================================
CREATE PROCEDURE [dbo].[getReporte05] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select empresa.emp_nombre as Empresa, count(*) Espirometrias
	from espirometria
	join paciente on esp_paciente=pac_id
	join empresa on emp_id=pac_empresa
	where CONVERT(date,esp_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
	group by emp_nombre
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte06]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza
-- Create date: 27/06/2016
-- Description:	Numero de oftalmologias por empresa
-- =============================================
CREATE PROCEDURE [dbo].[getReporte06]
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select empresa.emp_nombre as Empresa, count(*) Oftalmologias
	from oftalmologia
	join paciente on oft_paciente=pac_id
	join empresa on emp_id=pac_empresa
	where CONVERT(date,oft_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
	group by emp_nombre
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte07]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Chicaiza
-- Create date: 27/06/2016
-- Description:	Numero de rayos x por empresa
-- =============================================
CREATE PROCEDURE [dbo].[getReporte07] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select empresa.emp_nombre as Empresa, count(*) Rayos
	from rayos
	join paciente on ray_paciente=pac_id
	join empresa on emp_id=pac_empresa
	where CONVERT(date,ray_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
	group by emp_nombre
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte08]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte08] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ROW_NUMBER() over (order by emp_nombre) as Numero, 
	reg_fecha as Fecha, (pac_apellidos+' '+pac_nombres) as Paciente, (lab_apellidos+' '+lab_nombres) as Responsable, emp_nombre as Empresa
	from registro
	inner join laboratorista on lab_id=reg_laboratorista
	inner join paciente on pac_id=reg_paciente
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte09]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte09] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ROW_NUMBER() over (order by emp_nombre) as Numero,
	aud_fecha as Fecha, (pac_apellidos+' '+pac_nombres) as Paciente, dbo.getResponsable(aud_responsable,aud_perfil) as Responsable, emp_nombre as Empresa
	from audiometria
	inner join paciente on pac_id=aud_paciente
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,aud_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte10]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte10] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ROW_NUMBER() over (order by emp_nombre) as Numero,
	esp_fecha as Fecha, (pac_apellidos+' '+pac_nombres) as Paciente, dbo.getResponsable(esp_responsable,esp_perfil) as Responsable, emp_nombre as Empresa
	from espirometria
	inner join paciente on pac_id=esp_paciente
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,esp_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte11]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte11] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ROW_NUMBER() over (order by emp_nombre) as Numero,
	oft_fecha as Fecha, (pac_apellidos+' '+pac_nombres) as Paciente, dbo.getResponsable(oft_responsable,oft_perfil) as Responsable, emp_nombre as Empresa
	from oftalmologia
	inner join paciente on pac_id=oft_paciente
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,oft_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte12]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte12] 
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select ROW_NUMBER() over (order by emp_nombre) as Numero,
	ray_fecha as Fecha, (pac_apellidos+' '+pac_nombres) as Paciente, dbo.getResponsable(ray_responsable,ray_perfil) as Responsable,  emp_nombre as Empresa
	from rayos
	inner join paciente on pac_id=ray_paciente
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,ray_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
END

GO
/****** Object:  StoredProcedure [dbo].[getReporte13]    Script Date: 09/10/2018 14:07:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getReporte13]
	-- Add the parameters for the stored procedure here
	@fecha_ini varchar(10),
	@fecha_fin varchar(10),
	@empresa int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select ROW_NUMBER() over(order by emp_nombre) as Numero, empresa.emp_nombre,
		historia.his_fecha as Fecha, (paciente.pac_apellidos+' '+paciente.pac_nombres) as Paciente, paciente.pac_edad as Edad, paciente.pac_genero as Genero, 
		paciente.pac_instruccion as Instruccion, paciente.pac_estadocivil as EstadoCivil,
		(med_nombres+' '+med_apellidos) as Medico,
		(Select ocu_cargo from ocupacional where ocu_paciente=paciente.pac_id and  ocu_tipo='actual') as Puesto,
		(Select ocu_seccion from ocupacional where ocu_paciente=paciente.pac_id and  ocu_tipo='actual') as Area,
		actividad.act_enf_descripcion as Enfermedad, actividad.act_acc_descripcion as Accidente,
		habitos.hab_fuma as Fuma, habitos.hab_alcohol as Alcohol, habitos.hab_ejercicio as Ejercicio,
		ginecologico.gin_paptest as PapTest,
		CAST(
			case when (select count(*) from inmunizacion 
			where inm_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Inmunizacion,
		signos.sig_peso as Peso, signos.sig_talla as Talla, signos.sig_corporal as IMC,
		

		Tipo= case historia.his_tipo
			when 1 then 'GENERAL'
			when 2 then 'PREOCUPACIONAL'
			when 3 then 'OCUPACIONAL'
			when 4 then 'RETIRO'
			end,
		CAST(
			case when (select count(*) from registro 
			where CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and reg_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Laboratorio,
		CAST(
			case when (select count(*) from audiometria 
			where CONVERT(date,aud_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and aud_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Audiometria,
		CAST(
			case when (select count(*) from oftalmologia 
			where CONVERT(date,oft_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and oft_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Oftalmologia,
		CAST(
			case when (select count(*) from espirometria 
			where CONVERT(date,esp_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and esp_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Espirometria,
		CAST (
			case when (select count(*) from rayos 
			where CONVERT(date,ray_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and ray_paciente=pac_id)>0
			then 1 else 0 end as int
		) AS Rayos,
		
		cast(
			case when 
			(select count(*) from registro 
			where CONVERT(date,reg_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and reg_paciente=pac_id)>0
			OR
			(select count(*) from audiometria 
			where CONVERT(date,aud_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and aud_paciente=pac_id)>0
			OR
			(select count(*) from oftalmologia 
			where CONVERT(date,oft_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and oft_paciente=pac_id)>0
			OR
			(select count(*) from espirometria 
			where CONVERT(date,esp_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and esp_paciente=pac_id)>0
			OR
			(select count(*) from rayos 
			where CONVERT(date,ray_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and ray_paciente=pac_id)>0
			AND
			(select  count(*) from historia 
			where CONVERT(date,his_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)  
			and his_paciente=pac_id) >0
		then 1 else 0 end as int) as Chequeo,
		(Select con_resultado from concepto where con_id=historia.his_id) as Certificado,

		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  0 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico1,
		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  1 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico2,
		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  2 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico3,
		(SELECT sub_descripcion FROM diagnostico
			inner join dbo.sub_cie10 on dia_subcie10=sub_id
			where diagnostico.dia_historia=historia.his_id
			order by diagnostico.dia_id
			OFFSET  3 ROWS 
			FETCH NEXT 1 ROWS ONLY 
		) as Diagnostico4,
		concepto.con_seguimiento as Seguimiento,
		concepto.con_periodo as Periodicidad
		
		
		 
	from historia
	left join paciente on pac_id=his_paciente
	inner join medico on med_id=his_medico
	left join habitos on hab_id=pac_id
	left join ginecologico on gin_id=pac_id
	left join signos on sig_id=pac_id
	left join concepto on con_id=his_id
	left join actividad on act_id=pac_id
	inner join empresa on emp_id=pac_empresa
	where CONVERT(date,his_fecha,103) between CONVERT(date,@fecha_ini,103)  and CONVERT(date,@fecha_fin,103)
	and paciente.pac_empresa=@empresa
END


GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "audiometria"
            Begin Extent = 
               Top = 64
               Left = 469
               Bottom = 194
               Right = 678
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 37
               Left = 136
               Bottom = 167
               Right = 374
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "empresa"
            Begin Extent = 
               Top = 168
               Left = 38
               Bottom = 298
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_audiometria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_audiometria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "codigo"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "registro"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 136
               Right = 494
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 6
               Left = 532
               Bottom = 136
               Right = 770
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_codigo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_codigo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "diagnostico"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "cie10"
            Begin Extent = 
               Top = 24
               Left = 821
               Bottom = 137
               Right = 1030
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "sub_cie10"
            Begin Extent = 
               Top = 14
               Left = 444
               Bottom = 144
               Right = 653
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_diagnostico'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_diagnostico'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "espirometria"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 6
               Left = 532
               Bottom = 136
               Right = 770
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "empresa"
            Begin Extent = 
               Top = 74
               Left = 869
               Bottom = 204
               Right = 1078
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1170
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_espirometria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_espirometria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "historia"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 136
               Right = 523
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "familiar"
            Begin Extent = 
               Top = 6
               Left = 561
               Bottom = 136
               Right = 770
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "personal"
            Begin Extent = 
               Top = 6
               Left = 808
               Bottom = 136
               Right = 1017
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "fisico"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "signos"
            Begin Extent = 
               Top = 138
               Left = 285
               Bottom = 268
               Right = 494
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "revision"
            Begin Extent = 
               Top = 138
               Left = 532
               Bottom = 268
               Right = 741
            End
            DisplayFlags =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_historia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 280
            TopColumn = 17
         End
         Begin Table = "plan"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 383
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "medico"
            Begin Extent = 
               Top = 316
               Left = 450
               Bottom = 446
               Right = 659
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "actividad"
            Begin Extent = 
               Top = 138
               Left = 779
               Bottom = 268
               Right = 988
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "habitos"
            Begin Extent = 
               Top = 102
               Left = 1055
               Bottom = 232
               Right = 1264
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "profesion"
            Begin Extent = 
               Top = 6
               Left = 1055
               Bottom = 102
               Right = 1264
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_historia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_historia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "inmunizacion"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "vacuna"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 136
               Right = 494
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_inmunizacion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_inmunizacion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "oftalmologia"
            Begin Extent = 
               Top = 93
               Left = 654
               Bottom = 223
               Right = 863
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 203
               Left = 270
               Bottom = 333
               Right = 508
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "empresa"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_oftalmologia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_oftalmologia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "examen"
            Begin Extent = 
               Top = 41
               Left = 641
               Bottom = 208
               Right = 910
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "area"
            Begin Extent = 
               Top = 72
               Left = 993
               Bottom = 197
               Right = 1262
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "prueba"
            Begin Extent = 
               Top = 31
               Left = 317
               Bottom = 198
               Right = 586
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "registro"
            Begin Extent = 
               Top = 37
               Left = 3
               Bottom = 204
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "laboratorista"
            Begin Extent = 
               Top = 198
               Left = 310
               Bottom = 328
               Right = 519
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 198
               Left = 948
               Bottom = 328
               Right = 1186
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "empresa"
            Begin Extent = 
               Top = 204
               Left = 38
               Bottom = 334
               Right = 247
            End
            DisplayFlags =' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_prueba_paciente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_prueba_paciente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_prueba_paciente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "rayos"
            Begin Extent = 
               Top = 0
               Left = 750
               Bottom = 130
               Right = 959
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 136
               Right = 523
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "empresa"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_rayos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_rayos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "paciente"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 276
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "historia_1"
            Begin Extent = 
               Top = 6
               Left = 314
               Bottom = 136
               Right = 523
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "audiometria_1"
            Begin Extent = 
               Top = 6
               Left = 561
               Bottom = 136
               Right = 770
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "espirometria_1"
            Begin Extent = 
               Top = 6
               Left = 808
               Bottom = 136
               Right = 1017
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "oftalmologia_1"
            Begin Extent = 
               Top = 6
               Left = 1055
               Bottom = 136
               Right = 1264
            End
            DisplayFlags = 280
            TopColumn = 18
         End
         Begin Table = "rayos_1"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "registro_1"
            Begin Extent = 
               Top = 138
               Left = 285
               Bottom = 268
               Right = 494
            End
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reporte01'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'       DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reporte01'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reporte01'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "historia"
            Begin Extent = 
               Top = 165
               Left = 351
               Bottom = 295
               Right = 560
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 21
               Left = 16
               Bottom = 151
               Right = 254
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "medico"
            Begin Extent = 
               Top = 171
               Left = 669
               Bottom = 301
               Right = 878
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "registro"
            Begin Extent = 
               Top = 1
               Left = 816
               Bottom = 131
               Right = 1025
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reporte02'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reporte02'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "historia"
            Begin Extent = 
               Top = 95
               Left = 405
               Bottom = 225
               Right = 614
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "reposo"
            Begin Extent = 
               Top = 51
               Left = 66
               Bottom = 164
               Right = 275
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "diagnostico"
            Begin Extent = 
               Top = 179
               Left = 658
               Bottom = 309
               Right = 867
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "sub_cie10"
            Begin Extent = 
               Top = 117
               Left = 908
               Bottom = 247
               Right = 1117
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cie10"
            Begin Extent = 
               Top = 40
               Left = 1141
               Bottom = 153
               Right = 1350
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "paciente"
            Begin Extent = 
               Top = 199
               Left = 71
               Bottom = 329
               Right = 309
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
     ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reposo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reposo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_reposo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "riesgo"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "laboral"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 136
               Right = 494
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ocupacional"
            Begin Extent = 
               Top = 6
               Left = 532
               Bottom = 136
               Right = 741
            End
            DisplayFlags = 280
            TopColumn = 12
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_riesgo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_riesgo'
GO
USE [master]
GO
ALTER DATABASE [DB_9D0010_kinnemeddb] SET  READ_WRITE 
GO
