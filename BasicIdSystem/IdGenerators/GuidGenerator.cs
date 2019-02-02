using System;
using System.Collections.Generic;
using System.Text;

namespace BasicIdSystem.IdGenerators
{
    public class GuidGenerator<T> : IGenerateId<T>
    {
        public string GenerateId(T entity)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
