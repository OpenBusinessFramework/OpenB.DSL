using System;
using OpenB.Core;

namespace OpenB.DSL.Test
{
    internal class Person : IModel
    {
        public int Age { get; set; }

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

        public bool IsMarried { get; set; }

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
    }

    public enum MarriageStatus
    {
        Single,
        Married
    }
}