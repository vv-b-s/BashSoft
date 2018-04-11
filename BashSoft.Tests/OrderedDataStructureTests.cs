using BashSoft.Contracts;
using BashSoft.DataStructures;
using NUnit.Framework;
using System;
using System.Linq;

namespace BashSoft.Tests
{
    [TestFixture]
    public class OrderedDataStructureTests
    {
        private const int RequiredCapacity = 16;
        private const int RequiredInitialSize = 0;

        private ISimpleOrderedBag<string> names;

        [SetUp]
        public void Init()
        {
            this.names = new SimpleSortedList<string>();
        }

        [Test]
        public void TestEmptyCtor()
        {
            this.names = new SimpleSortedList<string>();
            Assert.That(this.names.Capacity, Is.EqualTo(RequiredCapacity));
            Assert.That(this.names.Size, Is.EqualTo(RequiredInitialSize));
        }

        [Test]
        public void TestCtorWithAllParams()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase, 30);
            Assert.That(this.names.Capacity, Is.EqualTo(30));
            Assert.That(this.names.Size, Is.EqualTo(RequiredInitialSize));
        }

        [Test]
        public void TestCtorWithInitialCapacity()
        {
            this.names = new SimpleSortedList<string>(20);
            Assert.That(this.names.Capacity, Is.EqualTo(20));
            Assert.That(this.names.Size, Is.EqualTo(RequiredInitialSize));
        }

        [Test]
        public void TestCtorWithInitialComparer()
        {
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase);
            Assert.That(this.names.Capacity, Is.EqualTo(RequiredCapacity));
            Assert.That(this.names.Size, Is.EqualTo(RequiredInitialSize));
        }

        [Test]
        public void TestAddIncreasesSize()
        {
            this.names.Add("Nasko");
            Assert.That(this.names.Size, Is.EqualTo(1));
        }

        [Test]
        public void TestAddNullThrowsException() =>
            Assert.That(() => this.names.Add(null), Throws.ArgumentNullException);

        [Test]
        public void TestAddUnsortedDetailsHeldSorted()
        {
            var unsoretedItems = new string[] { "Roesen", "Georgi", "Balkan" };
            var sortedItems = unsoretedItems.OrderBy(s => s).ToArray();

            this.names.AddAll(unsoretedItems);

            for (int i = 0; i < this.names.Size; i++)
                Assert.That(this.names[i], Is.EqualTo(sortedItems[i]));
        }

        [Test]
        public void TestAddingMoreThanInitialCapacity()
        {
            var items = new string[17];
            for (int i = 0; i < items.Length; i++)
                items[i] = $"item{i}";

            this.names.AddAll(items);

            Assert.That(RequiredCapacity, Is.LessThan(this.names.Capacity));
        }

        [Test]
        public void TestAddingAllFromCollectionIncreasesSize()
        {
            var items = new string[] { "Pesho", "Mesho" };

            this.names.AddAll(items);

            Assert.That(RequiredInitialSize, Is.LessThan(this.names.Size));
        }

        [Test]
        public void TestAddingAllFromNullThrowsException()
        {
            var itemsToAdd = new string[] { "Pesho", "Ke$ho", null, "Boriska" };

            Assert.That(() => this.names.AddAll(itemsToAdd), Throws.ArgumentNullException);
        }

        [Test]
        public void TestAddAllKeepsSorted()
        {
            var items = new string[] { "Linda", "Ben", "Josh" };
            var moreItems = new string[] { "Trisha", "Pisha", "Ne disha" };

            var expectedItems = items.Concat(moreItems).OrderBy(s => s).ToArray();

            //Add items normally
            for (int i = 0; i < items.Length; i++)
                this.names.Add(items[i]);

            this.names.AddAll(moreItems);

            //Check if items remain sorted
            for (int i = 0; i < this.names.Size; i++)
                Assert.That(this.names[i], Is.EqualTo(expectedItems[i]));
        }

        [Test]
        public void TestRemoveValidElementDecreasesSize()
        {
            var initialSize = 13;

            for (int i = 0; i < initialSize; i++)
                this.names.Add($"Name{i}");

            var expectedSizeAfterRemoval = initialSize - 1;

            this.names.Remove(this.names[0]);

            Assert.That(this.names.Size, Is.EqualTo(expectedSizeAfterRemoval));
        }

        [Test]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            var elements = new string[] { "Ivanchaka", "Chereshko" };
            this.names.AddAll(elements);

            var expectedRemainingElement = elements[1];

            this.names.Remove(elements[0]);

            Assert.That(expectedRemainingElement, Is.EqualTo(expectedRemainingElement));
        }

        [Test]
        public void TestRemovingNullThrowsException()
        {
            Assert.That(() => this.names.Remove(null), Throws.ArgumentNullException);
        }

        [Test]
        public void TestJoinWithNull()
        {
            this.names.AddAll(new string[] { "John", "Joker", "Jesper" });

            Assert.That(() => this.names.JoinWth(null), Throws.ArgumentNullException);
        }

        [Test]
        public void TestJoinWorksFine()
        {
            var data = new string[] { "John", "Joker", "Jesper" };
            this.names.AddAll(data);

            Array.Sort(data);

            var joiner = " - ";
            var expectedString = string.Join(joiner, data);
            var actualString = this.names.JoinWth(joiner);

            Assert.That(actualString, Is.EqualTo(expectedString));
        }
    }
}
