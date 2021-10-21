using System.Collections.Generic;
using System;
using System.Linq;
using SecurePort.Entities;
using SecurePort.Entities.Models;


namespace SecurePort.Services.Interfaces
{
    public class ServiciosBienes : IServiciosBienes 
    {
       
        #region Atributos
            private readonly ILogger log;
            protected readonly SecurePortContext db = new SecurePortContext();
            private IList<BienesViewModel> ListBienesPadresActivados = new List<BienesViewModel>();
        #endregion

        #region Métodos públicos
        public ServiciosBienes(ILogger log)
        {
            this.log = log;
        }

        public IEnumerable<BienesViewModel> GetAllBienes()
        {
            try
            {
                IEnumerable<BienesViewModel> result =
                   (from t1 in this.db.Bienes.ToList()
                    join b1 in this.db.Bienes.ToList() on t1.id_Bien_Padre equals (int?)b1.Id into BienEmp
                    from be in BienEmp.DefaultIfEmpty()
                    join c1 in this.db.Tipos_Instalacion.ToList() on t1.id_Tipo_IIPP equals c1.Id into TipoTemp
                    from ti in TipoTemp.DefaultIfEmpty()
                    orderby t1.Descripcion, t1.id_Tipo_IIPP,t1.id_Bien_Padre
                    select new BienesViewModel { Id = t1.Id, 
                                                 Descripcion = t1.Descripcion,
                                                 Tipo_instalacion = (ti == null) ? "" : ti.Descripcion, 
                                                 Bien_Padre = (be == null) ? "" : be.Descripcion, 
                                                 id_Bien_Padre = t1.id_Bien_Padre,
                                                 id_Tipo_IIPP = t1.id_Tipo_IIPP, 
                                                 activado = (t1.es_activo == true? "Si": "No"),
                                                 es_activo = t1.es_activo 
                                                });

               return ListarHijos(result);

               


               // return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        //  //Seleccionar los padre y recorrer la y adicionar los hijos de ese padre
        private IEnumerable<BienesViewModel> ListarHijos(IEnumerable<BienesViewModel> Bienes)
        {
            BienesViewModel Padre;
            IEnumerable<BienesViewModel> resultHijos = new List<BienesViewModel>();

             var resultPadres = from a in Bienes.Where(x => x.id_Bien_Padre == null).ToList()
                               join p in Bienes on a.Id equals p.id_Bien_Padre into Hijos
                               select new
                               {
                                   Padre = a,
                                   resultHijos = Hijos
                               };

             IEnumerable<BienesViewModel> result = new BienesViewModel[]{ };

             List<BienesViewModel> list = result.ToList();

             foreach (var bienPadre in resultPadres)
             {
                 Padre = bienPadre.Padre;

                 list.Add(Padre);

                 foreach (var bienHijo in bienPadre.resultHijos)
                 {
                     Padre = bienHijo;
                     list.Add(Padre);

                 }
             }


             return list;

        }

        public IEnumerable<BienesViewModel> ListBienes()
        {
            try
            {
                IEnumerable<BienesViewModel> result =
                   (from t1 in this.db.Bienes.ToList()
                    orderby t1.Id
                    select new BienesViewModel
                    {
                        Id = t1.Id,
                        Descripcion = t1.Descripcion,
                        id_Bien_Padre = t1.id_Bien_Padre,
                        id_Tipo_IIPP = t1.id_Tipo_IIPP,
                        es_activo = t1.es_activo
                    }).OrderBy(x => x.Id).ThenBy(l => l.Descripcion);

                return result;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }

        }

        #endregion

        public IEnumerable<Bienes> ListBienes(string codigo)
        {
            try
            {
               
                var result = this.db.Bienes.Where(x => x.Descripcion == codigo).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Bienes> ListBienes(string nombre, int Id)
        {
            try
            {
                var result = this.db.Bienes.Where(x => x.Id != Id && x.Descripcion == nombre).ToList();
                return result;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public bool ListBienes(string nombre, int Id, bool estado)
        {
            try
            {
                bool CambioEstado = false;
                if (estado == false)
                {
                    IEnumerable<Bienes> result = this.db.Bienes.Where(x => x.Id == Id && x.Descripcion == nombre).ToList();
                    Bienes bien = result.FirstOrDefault();
                    if (bien.es_activo != estado)
                        CambioEstado = true;
                }
                return CambioEstado;

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<Bienes> GetIdBienes(int id)
        {
            try
            {
                var ListaBienes = db.Bienes.Where(x=> x.Id==id && x.es_activo).ToList();
                return ListaBienes;
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<BienesHijosViewModel> GetAllBienesHijos(int? id)
        {
            try
            {
                IEnumerable<BienesHijosViewModel> result =
                   (from t1 in this.db.Bienes.ToList()
                    join b1 in this.db.Bienes.ToList() on t1.id_Bien_Padre equals (int?)b1.Id into BienEmp
                    from be in BienEmp.DefaultIfEmpty()
                    join c1 in this.db.Tipos_Instalacion.ToList() on t1.id_Tipo_IIPP equals c1.Id into TipoTemp
                    from ti in TipoTemp.DefaultIfEmpty()
                    orderby t1.Descripcion, t1.id_Tipo_IIPP, t1.id_Bien_Padre
                    select new BienesHijosViewModel
                    {
                        Id                = t1.Id,
                        Descripcion       = t1.Descripcion,
                        //Tipo_instalacion  = (ti == null) ? "" : ti.Descripcion,
                        //Bien_Padre        = (be == null) ? "" : be.Descripcion,
                        id_Bien_Padre     = t1.id_Bien_Padre,
                        //id_Tipo_IIPP      = t1.id_Tipo_IIPP,
                        activado          = "No",
                        es_activo         = t1.es_activo
                    });

                return result.Where(x=> !string.IsNullOrEmpty(x.id_Bien_Padre.ToString()) && x.id_Bien_Padre == id && x.es_activo);
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<BienesHijosViewModel> GetAllBienesHijosAmenazas(int? id, int? idInstalacion)
        {
            try
            {
                IEnumerable<BienesHijosViewModel> result = (from o in (from t1 in this.db.Bienes.ToList()
                    orderby t1.Id
                    select
                        new BienesHijosViewModel
                        {
                            Id            = t1.Id,
                            Descripcion   = t1.Descripcion,
                            id_Bien_Padre = t1.id_Bien_Padre,
                            activado      = "No",
                            es_activo     = t1.es_activo
                        }).Where(
                            x => !string.IsNullOrEmpty(x.id_Bien_Padre.ToString()) && x.id_Bien_Padre == id && x.es_activo)
                            join c1 in this.db.MOV_Detalle_Instalacion.Where(x=> x.ID_Instalacion == idInstalacion).ToList() on o.Id equals c1.ID_Bien
                            orderby o.Id
                            select
                                new BienesHijosViewModel
                                {
                                    Id            = o.Id,
                                    Descripcion   = o.Descripcion,
                                    id_Bien_Padre = o.id_Bien_Padre,
                                    activado      = "Si",
                                    es_activo     = o.es_activo
                                });

                return result.GroupBy(c => c.Id).Select(grp => grp.First()).ToList();
            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }

        public IEnumerable<BienesViewModel> GetAllBienesPadre(int? id)
        {
            try
            {


                 IEnumerable<BienesViewModel> resultado = new List<BienesViewModel>();

                 IEnumerable<BienesViewModel> BienesPadres = new List<BienesViewModel>();

                 //Bienes Padres
                 resultado = (from t1 in this.db.Bienes.ToList()
                                join b1 in this.db.Bienes.ToList() on t1.id_Bien_Padre equals (int?)b1.Id into BienEmp
                              from be in BienEmp.DefaultIfEmpty()
                                join c1 in this.db.Tipos_Instalacion.ToList() on t1.id_Tipo_IIPP equals c1.Id into TipoTemp
                              from ti in TipoTemp.DefaultIfEmpty()
                              orderby t1.Descripcion, t1.id_Tipo_IIPP, t1.id_Bien_Padre
                              select new BienesViewModel
                                   {
                                       Id            = t1.Id,
                                       Descripcion   = t1.Descripcion,
                                       Bien_Padre    = (be == null)? "": be.Descripcion,
                                       id_Bien_Padre = t1.id_Bien_Padre,
                                       activado      = "Si",
                                       es_activo     = t1.es_activo
                                   }).Where(b1 => string.IsNullOrEmpty(b1.id_Bien_Padre.ToString()) && b1.es_activo);

                if (this.db.MOV_Detalle_Instalacion.Where(x => x.ID_Instalacion == id).Any())
                {
                    
                    BienesPadres = (from t1 in this.db.Bienes.ToList()
                                        join b1 in this.db.Bienes.ToList() on t1.id_Bien_Padre equals (int?)b1.Id into BienEmp
                                    from be in BienEmp.DefaultIfEmpty()
                                        join c1 in this.db.Tipos_Instalacion.ToList() on t1.id_Tipo_IIPP equals c1.Id into TipoTemp
                                    from ti in TipoTemp.DefaultIfEmpty()
                                        orderby t1.Descripcion, t1.id_Tipo_IIPP, t1.id_Bien_Padre
                                        select new BienesViewModel
                                               {
                                                   Id            = t1.Id,
                                                   Descripcion   = t1.Descripcion,
                                                   Bien_Padre    = (be == null)? "": be.Descripcion,
                                                   id_Bien_Padre = t1.id_Bien_Padre,
                                                   activado      = "No",
                                                   es_activo     = t1.es_activo
                                               }).Where(b1 => string.IsNullOrEmpty(b1.id_Bien_Padre.ToString()) && b1.es_activo);

                    //Padres Activados 
                    var result = (from a1 in this.db.MOV_Detalle_Instalacion.ToList()
                                    join b1 in BienesPadres on a1.ID_Bien equals b1.Id
                                    where a1.ID_Instalacion == id
                                    select
                                        new BienesViewModel
                                        {
                                            Id            = b1.Id,
                                            Descripcion   = b1.Descripcion,
                                            Bien_Padre    = b1.Bien_Padre,
                                            id_Bien_Padre = b1.id_Bien_Padre,
                                            activado      = "Si",
                                            es_activo     = b1.es_activo
                                        });

                    foreach (var bienesViewModel in BienesPadres)
                    {
                        this.ListBienesPadresActivados.Add(bienesViewModel);
                    }

                    this.ListBienesPadresActivados.ToList().ForEach(
                       p =>
                       {
                            if (result.Where(x => x.Id == p.Id).ToList().Any())
                            {
                                this.ListBienesPadresActivados.Remove(p);
                            }
                        }
                    );

                    resultado = this.ListBienesPadresActivados.Union(result).ToList();

                }
                
                return resultado.ToList().OrderBy(x=> x.Id);

            }
            catch (Exception ex)
            {
                this.log.PublishException(ex);
                throw;
            }
        }
        
        public IEnumerable<Bienes> ListBienesHijos(int? IdPadre)
        {
            try
            {
                var result = this.db.Bienes.Where(x => x.id_Bien_Padre == IdPadre).ToList();
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
