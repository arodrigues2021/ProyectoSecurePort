namespace SecurePort.Entities.Models.Json
{
    public class CentrosJson
    {
        public int Id                { get; set; }

        public int ID_Organismo      { get; set; }

        public int? ID_Puerto        { get; set; }

        public string Centro_24H     { get; set; }

        public string action         { get; set; }

        public string Tipo_Validado  { get; set; }

        public string Direccion      { get; set; }

        public int? Id_Ciudad        { get; set; }

        public int? Cod_Postal       { get; set; }

        public int? Id_Provincia     { get; set; }

    }
}


