using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
    public class ServiciosDocumentos : IServiciosDocumentos 
    {
       
        #region Atributos
            private readonly ILogger log;
            protected readonly SecurePortContext db = new SecurePortContext();
        #endregion

        #region Métodos públicos

        public ServiciosDocumentos(ILogger log)
        {
            this.log = log;
        }
        
        #endregion
   
        public IEnumerable<Doc_Asoc> GetDocs(int? id, int tipo_Servicio)
        {
            try
            {
                IEnumerable<Doc_Asoc> result = (from t1 in this.db.Docs_Usuario.Where(x => x.id_servicio == id && x.id_tipo_serv == tipo_Servicio).ToList()
                                                          join t2 in this.GetTiposDocumento() on t1.id_tipo_doc equals t2.id
                                                          orderby t2.id
                                                          select
                                                              new Doc_Asoc
                                                              {
                                                                  id = t1.id,
                                                                  id_servicio = t1.id_servicio,
                                                                  id_tipo_doc = t1.id_tipo_doc,
                                                                  TipoNombre = t2.documento,
                                                                  id_tipo_serv = t1.id_tipo_serv,
                                                                  documento = t1.documento,
                                                                  descripcion = t1.descripcion,
                                                                  fech_documento = t1.fech_documento
                                                              });

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


        public IEnumerable<Tipos_Documento> GetTiposDocumento()
        {
            try
            {
                IEnumerable<Tipos_Documento> result =
                    (from t1 in this.db.Tipos_Documento.ToList()
                     orderby t1.id
                     select new Tipos_Documento { id = t1.id, documento = t1.documento, descripcion = t1.descripcion });
                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }


    }
}
