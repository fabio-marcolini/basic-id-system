using System;
using System.Collections.Generic;
using System.Text;

namespace BasicIdSystem
{
    public interface IGenerateId<T>
    {
        string GenerateId(T entity);
    }
}
