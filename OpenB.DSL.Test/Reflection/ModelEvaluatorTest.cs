//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NUnit.Framework;
//using OpenB.DSL.Reflection;

//namespace OpenB.DSL.Test.Reflection
//{
//    public class Person : IModel
//    {
//        public string Name { get; set; }
//        public DateTime DateOfBirth { get; set; }
//        public IList<Person> Children { get; set; }
//        public string Key { get; internal set; }

//        public Person()
//        {
//            Children = new List<Person>();
//        }
//    }

//    public class PersonRepository : IRepository<Person>
//    {
//        public PersonRepository(IDataStore dataStore)
//        {

//        }

//        public void CreateOrUpdate(Person model)
//        {
//            throw new NotImplementedException();
//        }

//        public void Delete(string key)
//        {
//            throw new NotImplementedException();
//        }

//        public Person Read(IList<ICondition<Person>> conditions)
//        {
//            throw new NotImplementedException();
//        }

//        public Person Read(string key)
//        {
//            throw new NotImplementedException();
//        }

//        internal void Read(Func<Person, bool> p)
//        {
//            var query = "select from person where";

//            foreach(var thing in p.Method.GetParameters())
//            {

//            }


//        }
//    }

//    public interface IContext
//    {
//        IUser User { get; }

//    }

//    public interface IUser
//    {
//        string Username { get; }
//    }

//    public interface IRepository<T> where T : IModel
//    {
//        void CreateOrUpdate(T model);
//        void Delete(string key);
//        T Read(string key);
//        T Read(IList<ICondition<T>> conditions);
//    }

//    public interface ICondition<T> where T : IModel
//    {

//    }

//    public interface IModel
//    {
//    }

//    [TestFixture]
//    public class ModelEvaluatorTest
//    {
//        [Test]
//        public void DoSomething()
//        {
//            Person firstChild = new Person
//            {
//                Key = "97AA329F-9EBA-4AE3-9E23-E5D5EFB958ED",
//                Name = "Max Doe",
//                DateOfBirth = DateTime.Parse("2012-03-15", CultureInfo.InvariantCulture)
//            };

//            Person person = new Person
//            //{
//                Key = "97AA329F-9EBA-4AE3-9E23-E5D5EFB958DA",
//                DateOfBirth = DateTime.Parse("1981-11-25", CultureInfo.InvariantCulture),
//                Name = "John Doe",
//            };

//            person.Children.Add(firstChild);

//            PersonRepository repository = new PersonRepository();
//            repository.Read(p => p.Name == "Henk" && p.DateOfBirth.Year > 1980);

//            ModelEvaluator evaluator = new ModelEvaluator();
//            evaluator.Evaluate("[Name]");

            
//        }



//    }
//}
