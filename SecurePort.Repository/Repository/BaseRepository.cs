
namespace SecurePort.Services.Servicios
{
    #region using
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Validation;
    using System.Linq.Expressions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SecurePort.Entities;
    using SecurePort.Services.Interfaces;
    #endregion

    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {

        public List<T> GetAll()
        {
            try
            {
                using (SecurePortContext context = new SecurePortContext())
                {
                    return (List<T>)context.Set<T>().ToList();
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }

        public List<T> GetAll(List<Expression<Func<T, object>>> includes)
        {
            try
            {
                List<string> includelist = new List<string>();

                foreach (var item in includes)
                {
                    MemberExpression body = item.Body as MemberExpression;
                    if (body == null)
                        throw new ArgumentException("The body must be a member expression");

                    includelist.Add(body.Member.Name);
                }

                using (SecurePortContext context = new SecurePortContext())
                {
                    DbQuery<T> query = context.Set<T>();

                    includelist.ForEach(x => query = query.Include(x));

                    return (List<T>)query.ToList();
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            

        }


        public T Single(Expression<Func<T, bool>> predicate)
        {
            try
            {
                using (SecurePortContext context = new SecurePortContext())
                {
                    return context.Set<T>().FirstOrDefault(predicate);
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }

        public T Single(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            try
            {
                List<string> includelist = new List<string>();

                foreach (var item in includes)
                {
                    MemberExpression body = item.Body as MemberExpression;
                    if (body == null)
                        throw new ArgumentException("The body must be a member expression");

                    includelist.Add(body.Member.Name);
                }

                using (SecurePortContext context = new SecurePortContext())
                {
                    DbQuery<T> query = context.Set<T>();

                    includelist.ForEach(x => query = query.Include(x));

                    return query.FirstOrDefault(predicate);
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }


        public List<T> Filter(Expression<Func<T, bool>> predicate)
        {
            try
            {
                using (SecurePortContext context = new SecurePortContext())
                {
                    return (List<T>)context.Set<T>().Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }

        public List<T> Filter(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            try
            {
                List<string> includelist = new List<string>();

                foreach (var item in includes)
                {
                    MemberExpression body = item.Body as MemberExpression;
                    if (body == null)
                        throw new ArgumentException("The body must be a member expression");

                    includelist.Add(body.Member.Name);
                }

                using (SecurePortContext context = new SecurePortContext())
                {
                    DbQuery<T> query = context.Set<T>();

                    includelist.ForEach(x => query = query.Include(x));

                    return (List<T>)query.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }


        public void Create(T entity)
        {
            try
            {
                using (SecurePortContext context = new SecurePortContext())
                {
                    context.Set<T>().Add(entity);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }

        public void Update(T entity)
        {
            try
            {
                using (SecurePortContext context = new SecurePortContext())
                {
                    context.Entry(entity).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }

        public void Delete(T entity)
        {
           try
            {
                using (SecurePortContext context = new SecurePortContext())
                {
                    context.Entry(entity).State = EntityState.Deleted;
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                var MessageError = ex.InnerException.InnerException.Message;

                throw new DbUpdateException(MessageError);
            }
            catch (ModelValidationException ex)
            {
                var MessageError = ex.InnerException.InnerException.Message;

                throw new ModelValidationException(MessageError);
            }
            catch (DbEntityValidationException ex)
            {
                var MessageError = ex.InnerException.InnerException.Message;

                throw new DbEntityValidationException(MessageError);
            }
            catch (Exception ex)
            {
                var MessageError = ex.InnerException.InnerException.Message;

                throw new ArgumentException(MessageError);
                }
           
        }

        
        public void Delete(Expression<Func<T, bool>> predicate)
        {
            try
            {
                using (SecurePortContext context = new SecurePortContext())
                {
                    var entities = context.Set<T>().Where(predicate).ToList();
                    entities.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw new ArgumentException(ex.Message);

            }
            
        }

    }
}
