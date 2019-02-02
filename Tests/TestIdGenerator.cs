using BasicIdSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class TestIdGenerator : IGenerateId<string>
    {
        private string[] ids;

        private int counter;

        public TestIdGenerator(params string[] ids)
        {
            this.ids = ids;
            counter = 0;
        }

        public string GenerateId(string entity)
        {
            int index = counter;
            counter++;
            return ids[index];
        }
    }
}
