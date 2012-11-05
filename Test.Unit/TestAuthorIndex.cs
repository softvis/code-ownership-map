using Lib;
using NUnit.Framework;

namespace Test.Unit
{
    [TestFixture]
    public class TestAuthorIndex
    {
        private AuthorIndex authorIndex;

        [SetUp]
        public void Setup()
        {
            authorIndex = new AuthorIndex();
        }

        [Test]
        public void Should_Assign_First_Author_Index_1()
        {
            int index = authorIndex.IndexForAuthor("Alice");

            Assert.AreEqual(1, index);
        }

        [Test]
        public void Should_Assign_Second_Author_Index_2()
        {
            authorIndex.IndexForAuthor("Alice");
            int index = authorIndex.IndexForAuthor("Bob");

            Assert.AreEqual(2, index);
        }

        [Test]
        public void Should_Return_Previously_Assigned_Index_When_Author_Repeats()
        {
            authorIndex.IndexForAuthor("Alice");
            authorIndex.IndexForAuthor("Bob");
            int index = authorIndex.IndexForAuthor("Alice");

            Assert.AreEqual(1, index);
        }
    }
}

