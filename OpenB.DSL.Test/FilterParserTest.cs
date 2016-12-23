using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenB.Core;
using static OpenB.DSL.Test.FilterParserTest;

namespace OpenB.DSL.Test
{
    [TestFixture]
    public class FilterParserTest
    {
        [Test]
        public void DoSomething()
        {
            IRepository<Person> personRepository = new Repository<Person>();

            string expression = "GetYearFromDate([DateOfBirth]) > 1980 and [Gender] = 'Male'";

            ExpressionParser parser = ExpressionParser.GetInstance(personRepository);
            parser.Parse(expression);
        }

        public class Person : IModel
        {
            public string Description
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsActive
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public string Key
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string Name
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public GenderType Gender { get; set; }
            public Date DateOfBirth { get; set; }
        }

        public interface IRepository<TModel> where TModel : IModel
        {
            IEnumerable<TModel> Read(IList<ICondition> conditions);
            
        }

        public interface IRepository
        {

        }

        public enum GenderType
        {
            Male,
            Female
        }
    }

    internal class Repository<T> : IRepository<T> where T : IModel
    {
        public IEnumerable<T> Read(IList<ICondition> conditions)
        {
            throw new NotImplementedException();
        }

        
    }

    public interface ICondition
    {
    }

    public class Date
    {
        private DateTime dateTime;

        public Date(int year, int month, int day)
        {
            dateTime = new DateTime(year, month, day);
        }

        

        public override string ToString()
        {
            return dateTime.ToString();
        }
    }
}
