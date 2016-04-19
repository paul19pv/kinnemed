using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kinnemed05.Models
{
    public partial class paciente
    {
        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La longitud debe ser de 10 caracteres")]
        public string pac_cedula { get; set; }
        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9 .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string pac_nombres { get; set; }
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Campo Requerido")]
        [RegularExpression("^([A-Za-z0-9 .&'-]+)$", ErrorMessage = "Solo se aceptan caracteres alfabéticos")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "La longitud mínima es 4 caracteres y la máxima 150")]
        public string pac_apellidos { get; set; }
        [Display(Name = "Género")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public string pac_genero { get; set; }
        [Display(Name = "Estado Civil")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public string pac_estadocivil { get; set; }
        [Display(Name = "Nacionalidad/Pais")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public Nullable<int> pac_pais { get; set; }
        [Display(Name = "Fecha Nacimiento")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string pac_fechanacimiento { get; set; }
        [Display(Name = "Edad")]
        [Required(ErrorMessage = "Campo Requerido")]
        public Nullable<int> pac_edad { get; set; }
        [Display(Name = "Teléfono")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(9, MinimumLength = 7, ErrorMessage = "La longitud debe ser de 9 caracteres")]
        public string pac_telefono { get; set; }
        [Display(Name = "Celular")]
        [RegularExpression("^([0-9.&'-]+)$", ErrorMessage = "Solo se aceptan números")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "La longitud debe ser de 10 caracteres")]
        public string pac_celular { get; set; }
        [Display(Name = "Correo electrónico")]
        //[Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El correo no tiene el formato correcto")]
        [StringLength(100, ErrorMessage = "La longitud máxima es 100 caracteres")]
        public string pac_correo { get; set; }
        [Display(Name = "Provincia")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public Nullable<int> pac_provincia { get; set; }
        [Display(Name = "Cantón")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public Nullable<int> pac_canton { get; set; }
        [Display(Name = "Dirección")]
        //[Required(ErrorMessage = "Campo Requerido")]
        [DataType(DataType.Text, ErrorMessage = "El campo no tiene el formato correcto")]
        public string pac_direccion { get; set; }
        [Display(Name = "Nivel de Instrucción")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public string pac_instruccion { get; set; }
        [Display(Name = "Profesión/Ocupación")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public Nullable<int> pac_profesion { get; set; }
        [Display(Name = "Tipo de Discapacidad")]
        //[Required(ErrorMessage = "Campo Requerido")]
        public string pac_tipodiscapacidad { get; set; }
        [Display(Name = "Porcentaje Discapacidad")]
        public Nullable<int> pac_porcentajediscapacidad { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int pac_empresa { get; set; }
        
        [Display(Name = "Firma")]
        public string pac_firma { get; set; }

        public bool pac_estado { get; set; }

    }
}