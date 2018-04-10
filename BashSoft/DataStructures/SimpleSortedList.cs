using BashSoft.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.DataStructures
{
    public class SimpleSortedList<T> : ISimpleOrderedBag<T> where T : IComparable<T>
    {
        private const int DefaultSize = 16;

        private T[] innerCollection;
        private int size;
        private IComparer<T> comparison;

        /// <summary>
        /// Custom comparer constructor
        /// </summary>
        /// <param name="comparer">Provide your custom comparer</param>
        /// <param name="capacity"></param>
        public SimpleSortedList(IComparer<T> comparer, int capacity)
        {
            this.comparison = comparer;

            InitializeInnerCollection(capacity);
        }

        /// <summary>
        /// Default comparer constructor
        /// </summary>
        /// <param name="capacity"></param>
        public SimpleSortedList(int capacity) : this(Comparer<T>.Create((x, y) => x.CompareTo(y)), capacity) { }

        /// <summary>
        /// Default capacity constructor
        /// </summary>
        /// <param name="comparer"></param>
        public SimpleSortedList(IComparer<T> comparer) : this(comparer, DefaultSize) { }

        /// <summary>
        /// Default size and capacity consructor
        /// </summary>
        public SimpleSortedList() : this(Comparer<T>.Create((x, y) => x.CompareTo(y)), DefaultSize) { }

        public int Size => this.size;

        /// <summary>
        /// Adds an item to the list, resizes if needed and sorts the collection
        /// </summary>
        /// <param name="element"></param>
        public void Add(T element)
        {
            if (this.innerCollection.Length == this.size)
                Resize(); 

            this.innerCollection[size] = element;
            this.size++;
            QuickSorter.Sort(this.innerCollection, 0, size - 1, this.comparison);
        }

        /// <summary>
        /// Adds the items to the list, resizes if needed and finally sorts the list
        /// </summary>
        /// <param name="collection"></param>
        public void AddAll(ICollection<T> collection)
        {
            if (this.size + collection.Count >= this.innerCollection.Length)
                MultiResize(collection);

            foreach (var element in collection)
            {
                this.innerCollection[this.size] = element;
                this.size++;
            }

            QuickSorter.Sort(this.innerCollection, 0, this.size - 1, this.comparison);
        }

        public string JoinWth(string joiner)
        {
            var joinedElements = string.Join(joiner, this);
            return joinedElements;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.size; i++)
                yield return this.innerCollection[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void InitializeInnerCollection(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("Capacity cannot be negatve!");

            else this.innerCollection = new T[capacity];
        }

        private void Resize()
        {
            Array.Resize(ref this.innerCollection, this.innerCollection.Length * 2);
        }

        private void MultiResize(ICollection<T> collection)
        {
            int newSize = this.innerCollection.Length * 2;

            while (this.size + collection.Count >= newSize)
                newSize *= 2;

            Array.Resize(ref this.innerCollection, this.innerCollection.Length * newSize);
        }
    }
}
