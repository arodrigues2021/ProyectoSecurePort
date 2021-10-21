using System;
using System.ComponentModel;
namespace SecurePort.Entities.Enums
{
    public enum EstadosPEA
    {
        [Description("Programación")]
        PRO = 1,
        [Description("Planificación")]
        PLA = 2,
        [Description("Resultado")]
        REL = 3
    }

    public enum MensajeError
    {
        [Description("OK")]
        OK = 1,
        [Description("No puede darse de alta el usuario al no existir ninguna dependencia asociada a la categoría ")]
        NOK = 2
    }

    public enum ValoracionPE
    {
        [Description("Satisfactorio")]
        OK = 1,
        [Description("No satisfactorio")]
        NOOK = 2
    }

    public enum NivelCOM
    {
        [Description("Comité")]
        CMT = 1,
        [Description("SubComité")]
        SCMT = 2
    }

    public enum TipoPE
    {
        [Description("Prácticas")]
        PRAC = 1,
        [Description("Ejercicios")]
        EJER = 2
    }

    public enum TipoINC
    {
        [Description("Violación")]
        VIOL = 1,
        [Description("Amenaza e incidente")]
        AMEN = 2
    }

    public enum TipoAUD
    {
        [Description("Externa")]
        EXT = 1,
        [Description("Interna")]
        INTER = 2
    }

    public enum TipoORG
    {
        [Description("Interés General")]
        OIG = 1,
        [Description("Gestión Autornómica")]
        OGA = 2,
        [Description("Otros")]
        OOT = 3
    }

    public enum CanalTipo
    {
        [Description("Teléfono")]
        TEL = 1,
        [Description("Fax")]
        FAX = 2,
        [Description("Correo")]
        MAI = 3
    }


    public enum TipoCategoria
    {
        [Description("Organismo Gestión Portuaria")]
        Organismo = 1,
        [Description("Puerto")]
        Puerto = 2,
        [Description("Instalación")]
        Instalacion = 3
    }

    public enum TipoRegistro
    {
        [Description("Alta Registro")]
        ALT = 1,
        [Description("Modificación registro")]
        MOD = 2,
        [Description("Baja registro")]
        BAJ = 3
    }

    public enum TipoPublico
    {
        [Description("Privado")]
        PRI = 1,
        [Description("Público")]
        PUB = 2,
        [Description("Organismos")]
        ORG = 3,
        [Description("Ministerio Interior")]
        MI = 4
    }

    public enum EliminarCategoria{
        [Description("No se puede eliminar porque su categoria es diferente a la del registro seleccionado")]
        MessageCategoria = 1
    
    }

    public enum ModificarCategoria
    {
        [Description("No se puede modificar porque su categoria es diferente a la del registro seleccionado")]
        MessageCategoria = 1

    }

    public enum MensajeBienPadreHijo
    {
        [Description("Datos guardados correctamente")]
        MessageCategoria = 1
    }

    public enum EstadoEvaluacion
    {
        [Description("En proceso")]
        ALT = 1,
        [Description("Procesada")]
        PRO = 2,
        [Description("Conforme")]
        CON = 3,
        [Description("Tramitada")]
        TRA = 4,
        [Description("Aprobada")]
        APR = 5,
        [Description("Rechazada")]
        REC = 6
    }

    public enum BajoRiesgo
    {
        [Description("Bajo Riesgo")]
        MessageBajoRiesgo = 1
    }

}
