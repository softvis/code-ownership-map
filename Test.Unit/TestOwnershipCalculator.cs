using System.Collections.Generic;
using Lib;
using NUnit.Framework;

namespace Test.Unit
{
    [TestFixture]
    public class TestOwnershipCalculator
    {
        private OwnershipCalculator calculator;
        private Dictionary<string, int> listOfAuthors;
        private decimal COMMIT_THRESHOLD;

        [SetUp]
        public void setup()
        {
            calculator = new OwnershipCalculator();
            listOfAuthors = new Dictionary<string, int>();
            COMMIT_THRESHOLD = 0.50M;
        }

        [Test]
        public void Should_File_That_Has_5_Authors_Who_No_Single_Author_Has_50_Percent_Commits_Is_TEAM()
        {
            listOfAuthors.Add("Joe", 3);
            listOfAuthors.Add("Fred", 2);
            listOfAuthors.Add("Sam", 2);
            listOfAuthors.Add("Betty", 1);
            listOfAuthors.Add("Amanda", 1);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10, COMMIT_THRESHOLD);

            Assert.AreEqual(0, calculator.codeDistribution["foo.cs"]);
        }

        [Test]
        public void Should_File_That_Has_5_Authors_Who_Has_A_Single_Author_Who_Owns_50_Percent_Commits_Is_ONE()
        {
            listOfAuthors.Add("Joe", 2);
            listOfAuthors.Add("Fred", 1);
            listOfAuthors.Add("Sam", 1);
            listOfAuthors.Add("Betty", 1);
            listOfAuthors.Add("Amanda", 5);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10, COMMIT_THRESHOLD);

            Assert.AreEqual(1, calculator.codeDistribution["foo.cs"]);
        }
        
     /*   
        [Test]
        public void Should_File_That_Has_5_Authors_Who_Commit_50_Percent_Commits_Is_TEAM()
        {
            listOfAuthors.Add("Joe", 3);
            listOfAuthors.Add("Fred", 2);
            listOfAuthors.Add("Sam", 2);
            listOfAuthors.Add("Betty", 1);
            listOfAuthors.Add("Amanda", 1);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10);

            Assert.AreEqual(CodeOwnership.Team, 
                calculator.codeDistribution.fileCommitCount["foo.cs"]);
        }

        [Test]
        public void Should_File_That_Has_4_Authors_Who_Commit_50_Percent_Commits_Is_FOUR()
        {
            listOfAuthors.Add("Joe", 3);
            listOfAuthors.Add("Fred", 2);
            listOfAuthors.Add("Sam", 3);
            listOfAuthors.Add("Betty", 2);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10);

            Assert.AreEqual(CodeOwnership.Four,
                calculator.codeDistribution.fileCommitCount["foo.cs"]);
        }

        [Test]
        public void Should_File_That_Has_3_Authors_Who_Commit_50_Percent_Commits_Is_THREE()
        {
            listOfAuthors.Add("Joe", 3);
            listOfAuthors.Add("Fred", 3);
            listOfAuthors.Add("Sam", 4);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10);

            Assert.AreEqual(CodeOwnership.Three,
                calculator.codeDistribution.fileCommitCount["foo.cs"]);
        }

        [Test]
        public void Should_File_That_Has_2_Authors_Who_Commit_50_Percent_Commits_Is_TWO()
        {
            listOfAuthors.Add("Joe", 5);
            listOfAuthors.Add("Fred", 5);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10);

            Assert.AreEqual(CodeOwnership.Two,
                calculator.codeDistribution.fileCommitCount["foo.cs"]);
        }

        [Test]
        public void Should_File_That_Has_1_Authors_Who_Commit_50_Percent_Commits_Is_ONE()
        {
            listOfAuthors.Add("Joe", 10);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10);

            Assert.AreEqual(CodeOwnership.One,
                calculator.codeDistribution.fileCommitCount["foo.cs"]);
        }

        [Test]
        public void Should_File_That_Has_2_Authors_Who_One_Commits_60_Percent_Commits_Is_ONE()
        {
            listOfAuthors.Add("Joe", 6);
            listOfAuthors.Add("Fred", 4);

            calculator.CalculateNumberOfAuthors("foo.cs", listOfAuthors, 10);

            Assert.AreEqual(CodeOwnership.One,
                calculator.codeDistribution.fileCommitCount["foo.cs"]);
        }
      */
    }
}
