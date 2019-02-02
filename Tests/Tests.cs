using System;
using System.Collections.Generic;
using BasicIdSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestRegisterUnregister()
        {
            //Has a user I want to (un)register an object
            var idSystem = new IdSystem<string>();

            Assert.AreEqual(0, idSystem.Count);

            string entity = "someEntity";
            string id = idSystem.Register(entity);
            Assert.AreEqual(1, idSystem.Count);

            string unregisteredEntity = idSystem.UnregisterId(id);

            Assert.AreEqual(0, idSystem.Count);
            Assert.AreEqual(entity, unregisteredEntity);
            Assert.AreSame(entity, unregisteredEntity);

            

            id = idSystem.Register(entity);
            Assert.AreEqual(1, idSystem.Count);
            string unregisteredId = idSystem.Unregister(entity);

            Assert.AreEqual(0, idSystem.Count);
            Assert.AreEqual(id, unregisteredId);
        }

        [TestMethod]
        public void TestGetByIdOrTag()
        {
            //Has a user I want to find an object by id or by tag
            var idSystem = GetTestSimple();

            Assert.AreEqual("1", idSystem["aaaaa"]);
            Assert.AreEqual("1", idSystem["myTag"]);
        }

        [TestMethod]
        public void TestAddRemoveTag()
        {
            var idSystem = new IdSystem<string>();

            string testValue = "element";

            string tag = "tag";
            string id = idSystem.Register(testValue);
            idSystem.Tag(tag, id);

            Assert.IsTrue(idSystem.IsRegisteredTag(tag));
            Assert.AreEqual(testValue, idSystem[tag]);
            Assert.AreEqual(idSystem[id], idSystem[tag]);

            idSystem.Untag(testValue);
            Assert.IsFalse(idSystem.IsRegisteredTag(tag));

            var values = idSystem.Find(tag);
            Assert.AreEqual(0, values.Count);
        }

            private IdSystem<string> GetTestSimple()
        {
            var idSystem = new IdSystem<string>(new TestIdGenerator("aaaaa", "aaaab", "aaaac", "aabbb"));

            idSystem.Register("1");
            idSystem.Register("2");
            idSystem.Register("3");
            idSystem.Register("4");

            idSystem.Tag("myTag", "aaaaa");
            idSystem.Tag("aatag", "aaaaa");

            Assert.AreEqual(4, idSystem.Count);
            return idSystem;
        }

        [TestMethod]
        public void TestFind()
        {
            //Has a user I want to list all object (filtered)
            var idSystem = GetTestSimple();

            IEnumerable<string> retrieved = idSystem.Find("");
            AssertContainsAll(retrieved, "1", "2", "3", "4");

            retrieved = idSystem.Find("aa");
            AssertContainsAll(retrieved, "1", "2", "3", "4");

            retrieved = idSystem.Find("aaa");
            AssertContainsAll(retrieved, "1", "2", "3");

            retrieved = idSystem.Find("aaaaa");
            AssertContainsAll(retrieved, "1");

            retrieved = idSystem.Find("b");
            AssertContainsAll(retrieved);

            retrieved = idSystem.Find("b");
            AssertContainsAll(retrieved);

            retrieved = idSystem.Find("my");
            AssertContainsAll(retrieved, "1");
        }

        public void AssertContainsAll(IEnumerable<string> tested, params string[] expectedValues)
        {
            var values = new HashSet<string>(expectedValues);
            var testedValues = new List<string>(tested);
            Assert.AreEqual(values.Count, testedValues.Count);
            foreach(var value in testedValues)
            {
                Assert.IsTrue(values.Contains(value));
            }
        }
    }
}
