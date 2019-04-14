using BasicIdSystem.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicIdSystem
{
    public class IdSystem<T>
    {
        private Dictionary<string, T> idToObject = new Dictionary<string, T>();

        private Dictionary<T, string> objectToId = new Dictionary<T, string>();

        private Dictionary<string, Tag> tags = new Dictionary<string, Tag>();

        private Dictionary<string, HashSet<string>> idToTags = new Dictionary<string, HashSet<string>>();

        private IGenerateId<T> idGenerator;

        public int Count
        {
            get { return idToObject.Count;}
        }

        public IdSystem(IGenerateId<T> idGenerator)
        {
            this.idGenerator = idGenerator;
        }

        public IdSystem(): this(new GuidGenerator<T>())
        {
        }

        public HashSet<string> ListTags()
        {
            return new HashSet<string>(tags.Keys);
        }

        public HashSet<string> ListTagsFor(string id)
        {
            if (!idToTags.ContainsKey(id))
            {
                return new HashSet<string>();
            }

            return idToTags[id];
        }

        public string Register(T entity)
        {
            string id = GenerateId(entity);
            idToObject.Add(id, entity);
            objectToId.Add(entity, id);
            return id;
        }

        public virtual string GenerateId(T entity)
        {
            return idGenerator.GenerateId(entity);
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
            if (!IsRegisteredId(id))
            {
                throw new KeyNotFoundException("The given id is not registered in the IdSystem");
            }

            Tag newTag = new Tag(tag, id);
            tags.Add(tag, newTag);

            if (!idToTags.ContainsKey(id))
            {
                idToTags[id] = new HashSet<string>();
            }

            idToTags[id].Add(tag);
        }

        public void RemoveTag(string tag)
        {
            if (!tags.ContainsKey(tag))
            {
                return;
            }

            Tag actualTag = tags[tag];
            idToTags[actualTag.ReferencedId].Remove(tag);
            tags.Remove(tag);
        }

        public void Untag(T entity)
        {
            string id = objectToId[entity];
            if (!idToTags.ContainsKey(id))
            {
                return;
            }

            var tagNames = idToTags[id];
            tagNames = new HashSet<string>(tagNames);
            
            foreach(var tagName in tagNames)
            {
                RemoveTag(tagName);
            }
        }

        public string GetIdOf(T entity)
        {
            return objectToId[entity];
        }

        public bool IsRegistered(T entity)
        {
            return objectToId.ContainsKey(entity);
        }

        public bool IsRegisteredIdoOrTag(string idOrTag)
        {
            return IsRegisteredId(idOrTag) || IsRegisteredTag(idOrTag);
        }

        public bool IsRegisteredTag(string tag)
        {
            return tags.ContainsKey(tag);
        }

        public bool IsRegisteredId(string id)
        {
            return idToObject.ContainsKey(id);
        }

        public HashSet<T> Find(string idOrTag)
        {
            var valuesById = new HashSet<T>(idToObject
                .Where(x => x.Key.StartsWith(idOrTag))
                .Select(x => x.Value));

            var valuesByTag = new HashSet<T>(tags
                .Where(x => x.Key.StartsWith(idOrTag))
                .Select(x => idToObject[x.Value.ReferencedId]));

            foreach(T value in valuesByTag)
            {
                valuesById.Add(value);
            }

            return valuesById;
        }

        public T this[string idOrTag]
        {
            get
            {
                if (IsRegisteredId(idOrTag))
                {
                    return idToObject[idOrTag];
                }

                if (IsRegisteredTag(idOrTag))
                {
                    Tag tag = tags[idOrTag];
                    return idToObject[tag.ReferencedId];
                }

                throw new KeyNotFoundException("The given id or tag is not registered in the IdSystem");
            }
        }

        
    }
}
