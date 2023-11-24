using System.ComponentModel.DataAnnotations;

namespace SprintAPI.Models
{
    public class EstudianteModel
    {
        [Key]
        [MaxLength(12, ErrorMessage ="El campo Identificacion solo puede tener máximo 12 dígitos.")]
        [RegularExpression("([0-9]+)", ErrorMessage ="El campo Identificacion debe contener únicamente números.")]
        public string Identificacion { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "El campo Nombre1 debe contener únicamente letras.")]
        public string Nombre1 { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "El campo Nombre2 debe contener únicamente letras.")]
        public string? Nombre2 { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "El campo Apellido1 debe contener únicamente letras.")]
        public string Apellido1 { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "El campo Apellido2 debe contener únicamente letras.")]
        public string? Apellido2 { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo Estado Civil debe contener únicamente números.")]
        public string EstadoCivil { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "El campo FechaInicio solo puede ser una fecha.")]
        public DateTime FechaNacimiento { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo Genero debe contener únicamente números.")]
        public string Genero { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo Ocupacion debe contener únicamente números.")]
        public string Ocupacion { get; set; }
        [DataType(DataType.DateTime, ErrorMessage ="El campo FechaInicio solo puede ser una fecha.")]
        public DateTime FechaInicio { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo ViveCon debe contener únicamente números.")]
        public string ViveCon { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo Municipio debe contener únicamente números.")]
        public string Municipio { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El campo NroHijos debe ser un número entero.")]
        public int NroHijos { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo NivelFormacion debe contener únicamente números.")]
        public string NivelFormacion { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "El campo Estrato debe contener únicamente números.")]
        public string? Estrato { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "El campo IngresosMonetarios debe ser un número entero.")]
        public int IngresosMonetarios { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "El campo EmpresaDondeTrabaja debe contener únicamente letras.")]
        public string? EmpresaDondeTrabaja { get; set; }
        //[Range(typeof(bool), "true", "true", ErrorMessage = "El campo Retencion debe ser 'true' o 'false'.")]
        public Boolean Retencion { get; set; }
    }
}
