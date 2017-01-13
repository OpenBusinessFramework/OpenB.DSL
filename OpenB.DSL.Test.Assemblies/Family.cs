using System;
using System.Collections.Generic;
using OpenB.Core;

namespace OpenB.DSL.Test.Assemblies
{

    public class Family : IModel
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

            public IList<Person> Members { get; private set; }
            public Family()
            {
                Members = new List<Person>();
            }
        }
}