using System;
using System.Collections.Generic;
using System.Text;

namespace BasicIdSystem
{
    public class Tag
    {
        public string Name { get; private set; }

        public string ReferencedId { get; private set; }

        public Tag(string name, string referencedId)
        {
            this.Name = name;
            this.ReferencedId = referencedId;
        }
    }
}
