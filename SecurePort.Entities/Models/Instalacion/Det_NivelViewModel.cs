namespace SecurePort.Entities.Models
{
    #region using

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion

    
    public class Det_NivelViewModel
    {

        
        public int Id { get; set; }

        public int Id_Nivel { get; set; }

        public string Nivel { get; set; }

        public int Id_IIPP { get; set; }

        public string IIPP { get; set; }        

    }
}
