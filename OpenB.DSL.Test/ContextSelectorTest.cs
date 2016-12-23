//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using NUnit.Framework;
//using OpenB.Core;

//namespace OpenB.DSL.Test
//{
//    [TestFixture]
//    public class ContextSelectorTest
//    {
//        [Test]
//        public void DoSomething()
//        {
//            var mockedFamilyRepository = new Mock<IRepository<Family>>();


//            ContextSelector contextSelector = new ContextSelector(mockedFamilyRepository.Object, nameof(Family.Members));

//            Person father = new Person { Age = 35 };
//            Person mother = new Person { Age = 31 };
//            Person childOne = new Person { Age = 3 };
//            Person childTwo = new Person { Age = 1 };

//            Family family = new Family()
//            {
//                Members = new[] { father, mother, childOne, childTwo }
//            };


//            ExpressionParser parser = ExpressionParser.GetInstance(family);



//            //Queue queue = parser.Parse(expression);
//            bool result = parser.Parse(expression).Outcome;

//            Assert.That(result.Equals(true));
//        }
//    }

//    internal class Family : IModel
//    {
       

//        public string Description
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public bool IsActive
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }

//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public string Key
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public string Name
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        internal IList<Person> Members
//        {
//            get
//            {                
//                IRepository<Person> personRepository = RepositoryFactory.GetRepository<Person>();
//                IList<ICondition> conditions = new List<ICondition>();
//                conditions.Add(p => p.Family.Key == this.Key);

//                return personRepository.Read(conditions);
//            }

//        }
//    }

//    internal static class RepositoryFactory
//    {
//        internal static IRepository<T> GetRepository<T>()
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class ContextSelector
//    {
//        readonly IRepository repository;
//        readonly string modelProperty;

//        public ContextSelector(IRepository repository, string modelProperty)
//        {
//            if (modelProperty == null)
//                throw new ArgumentNullException(nameof(modelProperty));
//            if (repository == null)
//                throw new ArgumentNullException(nameof(repository));

//            this.modelProperty = modelProperty;
//            this.repository = repository;
//        }

//        public IEnumerable GetContext()
//        {
//            Type[] repositoryType = repository.GetType().GetGenericArguments();
//            if (repositoryType == null || repositoryType.Length != 1)
//            {
//                throw new NotSupportedException("Repository is not supported.");
//            }


//        }
//    }

//    public interface IRepository<TModel> : IRepository where TModel : IModel
//    {
//        IModel Read(string key);        
//        IList<TModel> Read(IList<ICondition> conditions);
//        void CreateOrUpdate(IModel model);
//        void Remove(IList<ICondition> conditions);
//        void Remove(string key);      
//    }

//    public interface ICondition
//    {
//    }

//    public interface IRepository
//    {
//    }

//}
