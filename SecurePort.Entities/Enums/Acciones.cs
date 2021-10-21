namespace SecurePort.Entities.Enums
{
    using System;
    using System.ComponentModel;
    
    public enum Usuario
    {
        [Description("CON_ALTA_USU")]
        ALTA_USUARIO = 1,
        [Description("CON_MODIF_USU")]
        MODIFICAR_USUARIO = 2,
        [Description("CON_BORRA_USU")]
        BORRAR_USUARIO = 3,
        [Description("CON_CONSULTA_USU")]
        CONSULTAR_USUARIO = 4,
        [Description("CON_LISTA_USU")]
        LISTAR_OPCION_USUARIO = 5,
        [Description("CON_MODIF_PASS_USU")]
        MODIFICAR_PASS_USUARIO = 6,
        [Description("CON_RESET_PASS_USU")]
        RESETEAR_PASS_USUARIO = 7,
        [Description("CON_ACTIVA_USU")]
        ACTIVAR_USUARIO = 8,
        [Description("CON_ADJUNTA_DOC_USU")]
        ADJUNTAR_DOCUMENTO_USUARIO = 11,
        [Description("CON_BORRA_DOC_USU")]
        BORRAR_DOCUMENTO_USUARIO = 12,
        [Description("CON_MODIF_DOC_USU")]
        MODIFICAR_DOCUMENTO_USUARIO = 13,
        [Description("CON_VISUALIZAR_TODOS_USU")]
        VISUALIZAR_TODOS_USUARIOS = 14,
        [Description("CON_MODIF_DEPEN_USU")]
        MODIFICAR_DEPEN_USUARIO = 15

    }

    public enum Perfil
    {
        [Description("CON_ALTA_PERFIL")]
        ALTA_PERFIL = 1,
        [Description("CON_MODIF_PERFIL")]
        MODIFICAR_PERFIL = 2,
        [Description("CON_BORRA_PERFIL")]
        BORRAR_PERFIL = 3,
        [Description("CON_CONSULTA_PERFIL")]
        CONSULTAR_PERFIL = 4,
        [Description("CON_LISTA_PERFIL")]
        LISTAR_OPCION_PERFIL = 5

    }

    public enum Grupo
    {
        [Description("CON_ALTA_GRUPO")]
        ALTA_GRUPO = 1,
        [Description("CON_MODIF_GRUPO")]
        MODIFICAR_GRUPO = 2,
        [Description("CON_BORRA_GRUPO")]
        BORRAR_GRUPO = 3,
        [Description("CON_CONSULTA_GRUPO")]
        CONSULTAR_GRUPO = 4,
        [Description("CON_LISTA_GRUPO")]
        LISTAR_OPCION_GRUPO = 5

    }

    public enum Organismo
    {
        [Description("CON_ALTA_ORGANISMO")]
        ALTA_ORGANISMO = 1,
        [Description("CON_LISTA_ORGANISMO")]
        LISTA_ORGANISMO = 2,
        [Description("CON_CONSULTA_ORGANISMO")]
        CONSULTA_ORGANISMO = 3,
        [Description("CON_BORRA_ORGANISMO")]
        BORRA_ORGANISMO = 4,
        [Description("CON_MODIF_ORGANISMO")]
        MODIF_ORGANISMO = 5,
        [Description("CON_ALTA_CONT_OGP")]
        ALTA_CONT_OGP = 6,
        [Description("CON_CONSULTA_CONT_OGP")]
        CONSULTA_CONT_OGP = 7,
        [Description("CON_BORRA_CONT_OGP")]
        BORRA_CONT_OGP = 8,
        [Description("CON_MODIF_CONT_OGP")]
        MODIF_CONT_OGP = 9

    }

    public enum Pais
    {
        [Description("CON_ALTA_PAIS")]
        ALTA_PAIS = 1,
        [Description("CON_LISTA_PAIS")]
        LISTA_PAIS = 2,
        [Description("CON_CONSULTA_PAIS")]
        CONSULTA_PAIS = 3,
        [Description("CON_BORRA_PAIS")]
        BORRA_PAIS = 4,
        [Description("CON_MODIF_PAIS")]
        MODIF_PAIS = 5
    }
    
    public enum Comunidad
    {
        [Description("CON_ALTA_COMAUT")]
        ALTA_COMAUT = 1,
        [Description("CON_CONSULTA_COMAUT")]
        CONSULTA_COMAUT = 2,
        [Description("CON_BORRA_COMAUT")]
        BORRA_COMAUT = 3,
        [Description("CON_MODIF_COMAUT")]
        MODIF_COMAUT = 4,
        [Description("CON_LISTA_COMAUT")]
        LISTA_COMAUT = 5
    }

    public enum Bien
    {
        [Description("CON_ALTA_BIEN")]
        ALTA_BIEN = 1,
        [Description("CON_BORRA_BIEN")]
        BORRA_BIEN = 2,
        [Description("CON_MODIF_BIEN")]
        MODIF_BIEN = 3,
        [Description("CON_LISTA_BIEN")]
        LISTA_BIEN = 4
    }

     public enum TipoInstalacion
     {
         [Description("CON_LISTA_TIPO_IIPP")]
         LISTA_TIPO_IIPP = 1,
         [Description("CON_ALTA_TIPO_IIPP")]
         ALTA_TIPO_IIPP = 2,
         [Description("CON_BORRA_TIPO_IIPP")]
         BORRA_TIPO_IIPP = 3,
         [Description("CON_MODIF_TIPO_IIPP")]
         MODIF_TIPO_IIPP = 4
     }

     public enum Provincia
     {
         [Description("CON_ALTA_PROVINCIA")]
         ALTA_PROVINCIA = 1,
         [Description("CON_LISTA_PROVINCIA")]
         LISTA_PROVINCIA = 2,
         [Description("CON_CONSULTA_PROVINCIA")]
         CONSULTA_PROVINCIA = 3,
         [Description("CON_BORRA_PROVINCIA")]
         BORRA_PROVINCIA = 4,
         [Description("CON_MODIF_PROVINCIA")]
         MODIF_PROVINCIA = 5

     }

     public enum Capitania
     {
         [Description("CON_ALTA_CAPMARIT")]
         ALTA_CAPITANIA = 1,
         [Description("CON_LISTA_CAPMARIT")]
         LISTA_CAPITANIA = 2,
         [Description("CON_CONSULTA_CAPMARIT")]
         CONSULTA_CAPITANIA = 3,
         [Description("CON_BORRA_CAPMARIT")]
         BORRA_CAPITANIA = 4,
         [Description("CON_MODIF_CAPMARIT")]
         MODIF_CAPITANIA = 5

     }
  
    public enum Puerto
    {
        [Description("CON_LISTA_PUERTO")]
        LISTAR_OPCION_PUERTO = 1,
        [Description("CON_ALTA_PUERTO")]
        ALTA_PUERTO = 2,
        [Description("CON_CONSULTA_PUERTO")]
        CONSULTA_PUERTO = 3,
        [Description("CON_BORRA_PUERTO")]
        BORRA_PUERTO = 4,
        [Description("CON_MODIF_PUERTO")]
        MODIF_PUERTO = 5,
        [Description("CON_ALTA_OPP_PUERTO")]
        ALTA_OPP_PUERTO = 6,
        [Description("CON_CONSULTA_OPP_PUERTO")]
        CONSULTA_OPP_PUERTO = 7,
        [Description("CON_BORRA_OPP_PUERTO")]
        BORRA_OPP_PUERTO = 8,
        [Description("CON_MODIF_OPP_PUERTO")]
        MODIF_OPP_PUERTO = 9
    }

     public enum Amenaza
     {
         [Description("CON_LISTA_AMENAZA")]
         LISTAR_OPCION_AMENAZA = 1,
         [Description("CON_ALTA_AMENAZA")]
         ALTA_AMENAZA = 2,
         [Description("CON_BORRA_AMENAZA")]
         BORRA_AMENAZA = 3,
         [Description("CON_MODIF_AMENAZA")]
         MODIF_AMENAZA = 4

     }

    public enum Instalacion_Operadores
    {
        [Description("CON_LISTA_IIPP")]
        LISTA_IIPP = 1,
        [Description("CON_ALTA_IIPP")]
        ALTA_IIPP = 2,
        [Description("CON_CONSULTA_IIPP")]
        CONSULTA_IIPP = 3,
        [Description("CON_BORRA_IIPP")]
        BORRA_IIPP = 4,
        [Description("CON_MODIF_IIPP")]
        MODIF_IIPP = 5,
        [Description("CON_ALTA_OPIP_IIPP")]
        ALTA_OPIP_IIPP = 6,
        [Description("CON_CONSULTA_OPIP_IIPP")]
        CONSULTA_OPIP_IIPP = 7,
        [Description("CON_BORRA_OPIP_IIPP")]
        BORRA_OPIP_IIPP = 8,
        [Description("CON_MODIF_OPIP_IIPP")]
        MODIF_OPIP_IIPP = 9,
        [Description("CON_ADJUNTA_DOC_IIPP")]
        ADJUNTA_DOC_IIPP = 10,
        [Description("CON_BORRA_DOC_IIPP")]
        BORRA_DOC_IIPP = 11,
        [Description("CON_MODIF_DOC_IIPP")]
        MODIF_DOC_IIPP = 12
    }

    public enum Centro
    {
         [Description("CON_ALTA_CENTRO_24H")]
         ALTA_CENTRO   = 1,
         [Description("CON_LISTA_CENTRO_24H")]
         LISTAR_CENTRO = 2,
         [Description("CON_CONSULTA_CENTRO_24H")]
         CONSULTA_CENTRO = 3,
         [Description("CON_BORRA_CENTRO_24H")]
         BORRA_CENTRO = 4,
         [Description("CON_MODIF_CENTRO_24H")]
         MODIF_CENTRO = 5

    }

    public enum Comite
    {
        [Description("CON_LISTA_COMITE")]
        LISTAR_OPCION_COMITES = 1,
        [Description("CON_ALTA_COMITE")]
        ALTA_COMITE = 2,
        [Description("CON_CONSULTA_COMITE")]
        CONSULTA_COMITE = 3,
        [Description("CON_MODIF_COMITE")]
        MODIF_COMITE = 4,
        [Description("CON_BORRA_COMITE")]
        BORRA_COMITE = 5,
        [Description("CON_ADJUNTA_DOC_COMITE")]
        ADJUNTA_DOC_COMITE = 6,
        [Description("CON_BORRA_DOC_COMITE")]
        BORRA_DOC_COMITE = 7,
        [Description("CON_MODIF_DOC_COMITE")]
        MODIF_DOC_COMITE = 8

    }

    public enum Motivo
    {
        [Description("CON_LISTA_MOTIVO_DECLARA")]
        LISTAR_OPCION_MOTIVO_DECLARA = 1,
        [Description("CON_ALTA_MOTIVO_DECLARA")]
        ALTA_MOTIVO_DECLARA = 2,
        [Description("CON_BORRA_MOTIVO_DECLARA")]
        BORRA_MOTIVO_DECLARA = 3,
        [Description("CON_MODIF_MOTIVO_DECLARA")]
        MODIF_MOTIVO_DECLARA = 4

    }

    public enum Organizacion
    {
        [Description(" ")]
        LISTAR_OPCION_ORGANIZACION = 1
    }


    public enum Formacion
    {
        [Description("CON_LISTA_FORMACION")]
        LISTA_FORMACION = 1,
        [Description("CON_ALTA_FORMACION")]
        ALTA_FORMACION = 2,
        [Description("CON_MODIF_FORMACION")]
        MODIF_FORMACION = 3,
        [Description("CON_BORRA_FORMACION")]
        BORRA_FORMACION = 4,
        [Description("CON_CONSULTA_FORMACION")]
        CONSULTA_FORMACION = 5,
        [Description("CON_ADJUNTA_DOC_FORMACION")]
        ADJUNTA_DOC_FORMACION = 6,
        [Description("CON_BORRA_DOC_FORMACION")]
        BORRA_DOC_FORMACION = 7,
        [Description("CON_MODIF_DOC_FORMACION")]
        MODIF_DOC_FORMACION = 8

     }

    public enum Ciudad
    {
        [Description("CON_ALTA_CIUDAD")]
        ALTA_CIUDAD = 1,
        [Description("CON_LISTA_CIUDAD")]
        LISTA_CIUDAD = 2,
        [Description("CON_CONSULTA_CIUDAD")]
        CONSULTA_CIUDAD = 3,
        [Description("CON_BORRA_CIUDAD")]
        BORRA_CIUDAD = 4,
        [Description("CON_MODIF_CIUDAD")]
        MODIF_CIUDAD = 5
    }

    public enum Practica
    {
        [Description("CON_ALTA_PRACTICA")]
        ALTA_PRACTICA = 1,
        [Description("CON_CONSULTA_PRACTICA")]
        CONSULTA_PRACTICA = 2,
        [Description("CON_BORRA_PRACTICA")]
        BORRA_PRACTICA = 3,
        [Description("CON_MODIF_PRACTICA")]
        MODIF_PRACTICA = 4,
        [Description("CON_ADJUNTA_DOC_PRACTICA")]
        ADJUNTA_DOC_PRACTICA = 5,
        [Description("CON_BORRA_DOC_PRACTICA")]
        BORRA_DOC_PRACTICA = 6,
        [Description("CON_MODIF_DOC_PRACTICA")]
        MODIF_DOC_PRACTICA = 7,
        [Description("CON_LISTA_PRACTICA")]
        LISTA_PRACTICA = 8
    }

    public enum Incidente
    {
        [Description("CON_ALTA_INCIDENTE")]
        ALTA_INCIDENTE = 1,
        [Description("CON_CONSULTA_INCIDENTE")]
        CONSULTA_INCIDENTE = 2,
        [Description("CON_BORRA_INCIDENTE")]
        BORRA_INCIDENTE = 3,
        [Description("CON_MODIF_INCIDENTE")]
        MODIF_INCIDENTE = 4,
        [Description("CON_LISTA_INCIDENTE")]
        LISTA_INCIDENTE = 5,
        [Description("CON_ADJUNTA_DOC_INCIDENTE")]
        ADJUNTA_DOC_INCIDENTE = 6,
        [Description("CON_BORRA_DOC_INCIDENTE")]
        BORRA_DOC_INCIDENTE = 7,
        [Description("CON_MODIF_DOC_INCIDENTE")]
        MODIF_DOC_INCIDENTE = 8
    }

    public enum Auditoria
    {
        [Description("CON_ALTA_AUDITORIA")]
        ALTA_AUDITORIA = 1,
        [Description("CON_CONSULTA_AUDITORIA")]
        CONSULTA_AUDITORIA = 2,
        [Description("CON_BORRA_AUDITORIA")]
        BORRA_AUDITORIA = 3,
        [Description("CON_MODIF_AUDITORIA")]
        MODIF_AUDITORIA = 4,
        [Description("CON_ADJUNTA_DOC_AUDITORIA")]
        ADJUNTA_DOC_AUDITORIA = 5,
        [Description("CON_BORRA_DOC_AUDITORIA")]
        BORRA_DOC_AUDITORIA = 6,
        [Description("CON_MODIF_DOC_AUDITORIA")]
        MODIF_DOC_AUDITORIA = 7,
        [Description("CON_LISTA_AUDITORIA")]
        LISTA_AUDITORIA = 8
    }

    public enum Nivel
    {
        [Description("CON_LISTA_NIVEL")]
        LISTA_NIVEL = 1,
        [Description("CON_ALTA_NIVEL")]
        ALTA_NIVEL = 2,
        [Description("CON_MODIF_NIVEL")]
        MODIF_NIVEL = 3,
        [Description("CON_BORRA_NIVEL")]
        BORRA_NIVEL = 4,
        [Description("CON_CONSULTA_NIVEL")]
        CONSULTA_NIVEL = 5,
        [Description("CON_ADJUNTA_DOC_NIVEL")]
        ADJUNTA_DOC_NIVEL = 6,
        [Description("CON_BORRA_DOC_NIVEL")]
        BORRA_DOC_NIVEL = 7,
        [Description("CON_MODIF_DOC_NIVEL")]
        MODIF_DOC_NIVEL = 8

    }

    public enum Mantenimiento
    {
        [Description("CON_LISTA_MANTEN")]
        LISTA_MANTEN = 1,
        [Description("CON_ALTA_MANTEN")]
        ALTA_MANTEN = 2,
        [Description("CON_MODIF_MANTEN")]
        MODIF_MANTEN = 3,
        [Description("CON_BORRA_MANTEN")]
        BORRA_MANTEN = 4,
        [Description("CON_CONSULTA_MANTEN")]
        CONSULTA_MANTEN = 5,
        [Description("CON_ADJUNTA_DOC_MANTEN")]
        ADJUNTA_DOC_MANTEN = 6,
        [Description("CON_BORRA_DOC_MANTEN")]
        BORRA_DOC_MANTEN = 7,
        [Description("CON_MODIF_DOC_MANTEN")]
        MODIF_DOC_MANTEN = 8

    }

    public enum Comunicacion
    {
        [Description("CON_LISTA_COMUNICA")]
        LISTA_COMUNICA = 1,
        [Description("CON_ALTA_COMUNICA")]
        ALTA_COMUNICA = 2,
        [Description("CON_MODIF_COMUNICA")]
        MODIF_COMUNICA = 3,
        [Description("CON_BORRA_COMUNICA")]
        BORRA_COMUNICA = 4,
        [Description("CON_CONSULTA_COMUNICA")]
        CONSULTA_COMUNICA = 5,
        [Description("CON_ADJUNTA_DOC_COMUNICA")]
        ADJUNTA_DOC_COMUNICA = 6,
        [Description("CON_BORRA_DOC_COMUNICA")]
        BORRA_DOC_COMUNICA = 7,
        [Description("CON_MODIF_DOC_COMUNICA")]
        MODIF_DOC_COMUNICA = 8

    }

    public enum Declara_Maritima
    {
        [Description("CON_LISTA_DECLARA_MARIT")]
        LISTA_DECLARA = 1,
        [Description("CON_ALTA_DECLARA_MARIT")]
        ALTA_DECLARA = 2,
        [Description("CON_MODIF_DECLARA_MARIT")]
        MODIF_DECLARA = 3,
        [Description("CON_BORRA_DECLARA_MARIT")]
        BORRA_DECLARA = 4,
        [Description("CON_CONSULTA_DECLARA_MARIT")]
        CONSULTA_DECLARA = 5,
        [Description("CON_ADJUNTA_DOC_DECLARA")]
        ADJUNTA_DOC_DECLARA = 6,
        [Description("CON_BORRA_DOC_DECLARA")]
        BORRA_DOC_DECLARA = 7,
        [Description("CON_MODIF_DOC_DECLARA")]
        MODIF_DOC_DECLARA = 8

    }

    public enum Gisis
    {
        [Description("CON_LISTA_GISIS")]
        LISTA_GISIS = 1,
        [Description("CON_ALTA_GISIS")]
        ALTA_GISIS = 2,
        [Description("CON_MODIF_GISIS")]
        MODIF_GISIS = 3,
        [Description("CON_BORRA_GISIS")]
        BORRA_GISIS = 4,
        [Description("CON_CONSULTA_GISIS")]
        CONSULTA_GISIS = 5,
        
    }

    public enum Gestion_Plantilla
    {
        [Description("CON_LISTA_PLANTILLA")]
        LISTA_PLANTILLA = 1     
    }

    public enum Gestion_Documento
    {
        [Description("CON_LISTA_DOCUMENTO")]
        LISTA_DOCUMENTO = 1
    }

    public enum Procedimiento_AP
    {
        [Description("CON_LISTA_PROCEDIM_AP")]
        LISTA_PROCEDIM_AP = 1,
        [Description("CON_ALTA_PROCEDIM_AP")]
        ALTA_PROCEDIM_AP = 2,
        [Description("CON_MODIF_PROCEDIM_AP")]
        MODIF_PROCEDIM_AP = 3,
        [Description("CON_BORRA_PROCEDIM_AP")]
        BORRA_PROCEDIM_AP = 4,
        [Description("CON_CONSULTA_PROCEDIM_AP")]
        CONSULTA_PROCEDIM_AP = 5,
        [Description("CON_ADJUNTA_DOC_PROCEDIM_AP")]
        ADJUNTA_DOC_PROCEDIM_AP = 6,
        [Description("CON_BORRA_DOC_PROCEDIM_AP")]
        BORRA_DOC_PROCEDIM_AP = 7,
        [Description("CON_MODIF_DOC_PROCEDIM_AP")]
        MODIF_DOC_PROCEDIM_AP = 8

    }

    public enum Procedimiento_OPPE
    {
        [Description("CON_LISTA_PROCEDIM_OPPE")]
        LISTA_PROCEDIM_OPPE = 1,
        [Description("CON_ALTA_PROCEDIM_OPPE")]
        ALTA_PROCEDIM_OPPE = 2,
        [Description("CON_MODIF_PROCEDIM_OPPE")]
        MODIF_PROCEDIM_OPPE = 3,
        [Description("CON_BORRA_PROCEDIM_OPPE")]
        BORRA_PROCEDIM_OPPE = 4,
        [Description("CON_CONSULTA_PROCEDIM_OPPE")]
        CONSULTA_PROCEDIM_OPPE = 5,
        [Description("CON_ADJUNTA_DOC_PROCEDIM_OPPE")]
        ADJUNTA_DOC_PROCEDIM_OPPE = 6,
        [Description("CON_BORRA_DOC_PROCEDIM_OPPE")]
        BORRA_DOC_PROCEDIM_OPPE = 7,
        [Description("CON_MODIF_DOC_PROCEDIM_OPPE")]
        MODIF_DOC_PROCEDIM_OPPE = 8

    }

    public enum Procedimiento_OPPE_AP
    {
        [Description("CON_LISTA_PROCEDIM_OPPE_AP")]
        LISTA_PROCEDIM_OPPE_AP = 1,
        [Description("CON_ALTA_PROCEDIM_OPPE_AP")]
        ALTA_PROCEDIM_OPPE_AP = 2,
        [Description("CON_MODIF_PROCEDIM_OPPE_AP")]
        MODIF_PROCEDIM_OPPE_AP = 3,
        [Description("CON_BORRA_PROCEDIM_OPPE_AP")]
        BORRA_PROCEDIM_OPPE_AP = 4,
        [Description("CON_CONSULTA_PROCEDIM_OPPE_AP")]
        CONSULTA_PROCEDIM_OPPE_AP = 5,
        [Description("CON_ADJUNTA_DOC_PROCEDIM_OPPE_AP")]
        ADJUNTA_DOC_PROCEDIM_OPPE_AP = 6,
        [Description("CON_BORRA_DOC_PROCEDIM_OPPE_AP")]
        BORRA_DOC_PROCEDIM_OPPE_AP = 7,
        [Description("CON_MODIF_DOC_PROCEDIM_OPPE_AP")]
        MODIF_DOC_PROCEDIM_OPPE_AP = 8

    }

    public enum Mov_Evaluaciones
    {
        [Description("EVAL_LISTA_IIPP")]
        LISTA_IIPP = 1,
        [Description("EVAL_GESTION_IIPP")]
        GESTION_IIPP = 2,
        [Description("EVAL_DETALLE_IIPP")]
        DETALLE_IIPP = 3,
        [Description("EVAL_LISTA_EVAL")]
        LISTA_EVAL = 4,
        [Description("EVAL_ALTA_EVAL")]
        ALTA_EVAL = 5,
        [Description("EVAL_MODIF_EVAL")]
        MODIF_EVAL = 6 ,
        [Description("EVAL_CONSULTA_EVAL")]
        CONSULTA_EVAL = 7,
        [Description("EVAL_ADJUNTA_DOC_EVAL")]
        ADJUNTA_DOC_EVAL = 8,
        [Description("EVAL_BORRA_DOC_EVAL")]
        BORRA_DOC_EVAL = 9,
        [Description("EVAL_MODIF_DOC_EVAL")]
        MODIF_DOC_EVAL = 10,
        [Description("EVAL_BORRA_EVAL")]
        BORRA_EVAL = 11,
        [Description("EVAL_COMP_EVAL")]
        COMP_EVAL = 12,
        [Description("EVAL_RIESGOS_EVAL")]
        RIESGOS_EVAL = 13
        
    }
  


    public enum General
    {
        [Description("OPIP")]
        OPIP = 1,
        [Description("OPP")]
        OPP = 2,       
        [Description("Ver")]
        AltaVer = 3,
        [Description("Alta")]
        AltaGeneral = 4,
        [Description("Edit")]
        EditGeneral = 5

    }

    public enum Message
    {
        [Description("Datos guardados correctamente")]
        Alta = 1,
        [Description("Otros")]
        Otros = 2

    }

    public enum Maestros
    {
        [Description("CON_LISTA_USU")]
        CON_LISTA_USU = 1,
        [Description("CON_LISTA_PERFIL")]
        CON_LISTA_PERFIL = 2,
        [Description("CON_LISTA_GRUPO")]
        CON_LISTA_GRUPO = 3,
        [Description("CON_LISTA_PROCEDIM_OPPE")]
        CON_LISTA_PROCEDIM_OPPE = 4,
        [Description("CON_LISTA_PROCEDIM_OPPE_AP")]
        CON_LISTA_PROCEDIM_OPPE_AP = 5,
        [Description("CON_LISTA_PROCEDIM_AP")]
        CON_LISTA_PROCEDIM_AP = 6,
        [Description("CON_LISTA_FORMACION")]
        CON_LISTA_FORMACION = 7,
        [Description("CON_LISTA_PUERTO")]
        CON_LISTA_PUERTO = 8,
        [Description("EVAL_LISTA_IIPP")]
        EVAL_LISTA_IIPP = 9,
        [Description("CON_LISTA_PAIS")]
        CON_LISTA_PAIS = 10,
        [Description("CON_LISTA_PROVINCIA")]
        CON_LISTA_PROVINCIA = 11,
        [Description("CON_LISTA_CIUDAD")]
        CON_LISTA_CIUDAD = 12,
        [Description("CON_LISTA_COMAUT")]
        CON_LISTA_COMAUT = 13,
        [Description("CON_LISTA_CAPMARIT")]
        CON_LISTA_CAPMARIT = 14,
        [Description("CON_LISTA_TIPO_IIPP")]
        CON_LISTA_TIPO_IIPP = 15,
        [Description("CON_LISTA_BIEN")]
        CON_LISTA_BIEN = 16,
        [Description("CON_LISTA_AMENAZA")]
        CON_LISTA_AMENAZA = 17,
        [Description("CON_LISTA_MOTIVO_DECLARA")]
        CON_LISTA_MOTIVO_DECLARA = 18,
        [Description("CON_LISTA_DECLARA_MARIT")]
        CON_LISTA_DECLARA_MARIT = 19,
        [Description("CON_LISTA_PRACTICA")]
        CON_LISTA_PRACTICA = 20,
        [Description("CON_LISTA_INCIDENTE")]
        CON_LISTA_INCIDENTE = 21,
        [Description("CON_LISTA_AUDITORIA")]
        CON_LISTA_AUDITORIA = 22,
        [Description("CON_LISTA_NIVEL")]
        CON_LISTA_NIVEL = 23,
        [Description("CON_LISTA_MANTEN")]
        CON_LISTA_MANTEN = 24,
        [Description("CON_LISTA_COMUNICA")]
        CON_LISTA_COMUNICA = 25,
        [Description("CON_LISTA_CENTROF")]
        CON_LISTA_CENTROF = 26,
        [Description("CON_LISTA_GISIS")]
        CON_LISTA_GISIS = 27,
        [Description("CON_LISTA_COMITE")]
        CON_LISTA_COMITE = 28,
        [Description("CON_LISTA_ORGANISMO")]
        CON_LISTA_ORGANISMO = 29,
        [Description("CON_LISTA_IIPP")]
        CON_LISTA_IIPP = 30,
        [Description("CON_LISTA_CENTRO_24H")]
        CON_LISTA_CENTRO_24H = 31,
        [Description("CON_LISTA_DOCUMENTO")]
        CON_LISTA_DOCUMENTO = 32,
        [Description("CON_LISTA_PLANTILLA")]
        CON_LISTA_PLANTILLA = 33,
        [Description("EVAL_LISTA_EVAL")]
        EVAL_LISTA_EVAL = 34
        
    }


    public static class AttributesHelperExtension
    {
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}


    
