using System;
using System.Collections.Generic;
using OpenB.Core;

namespace OpenB.DSL
{

    public interface IRepository<TModel> : IRepository where TModel : IModel
    {
        IEnumerable<TModel> Read(IList<IFilter> conditions);

    }

    public interface IFilter
    {
    }

    public interface IRepository
    {

    }

    public enum GenderType
    {
        Male,
        Female
    }



    public class Repository<T> : IRepository<T> where T : IModel
    {

        public IEnumerable<T> Read(IList<IFilter> conditions)
        {
            throw new NotImplementedException();
        }
    }
}