using System;
using System.Collections.Generic;

namespace BasicIdSystem
{
    public class IdSystem<T>
    {
        private Dictionary<string, T> idToObject = new Dictionary<string, T>();

        private Dictionary<T, string> objectToId = new Dictionary<T, string>();

        private Dictionary<string, Tag> tags = new Dictionary<string, Tag>();

        public string Register(T entity)
        {
            string id = GenerateId(entity);
            idToObject.Add(id, entity);
            objectToId.Add(entity, id);
            return id;
        }

        public virtual string GenerateId(T entity)
        {
            return Guid.NewGuid().ToString();
        }

        public T UnregisterId(string id)
        {
            T entity = idToObject[id];
            idToObject.Remove(id);
            objectToId.Remove(entity);
            return entity;
        }

        public string Unregister(T entity)
        {
            string id = objectToId[entity];
            objectToId.Remove(entity);
            idToObject.Remove(id);
            return id;
        }

        public void Tag(string tag, string id)
        {
            Tag newTag = new Tag(tag, id);
            tags.Add(tag, newTag);
        }

        public T this[string idOrTag]
        {
            get
            {
                if (idToObject.ContainsKey(idOrTag))
                {
                    return idToObject[idOrTag];
                }

                if (tags.ContainsKey(idOrTag))
                {
                    Tag tag = tags[idOrTag];
                    return idToObject[tag.ReferencedId];
                }

                throw new KeyNotFoundException("The given id or tag was not registered in the IdSystem");
            }
        }

        
    }
}
