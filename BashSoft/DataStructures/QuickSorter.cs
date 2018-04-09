using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.DataStructures
{
    /// <summary>
    /// Implementation taken from https://www.geeksforgeeks.org/quick-sort/
    /// </summary>
    public class QuickSorter
    {
        public static void Sort<T>(T[] arr, int startIndex, int endIndex, IComparer<T> comparer) where T : IComparable<T>
        {
            if (startIndex < endIndex)
            {
                //pi is partitioning index, arr[p] is now at right place
                var pi = Partition(arr, startIndex, endIndex, comparer);

                Sort(arr, startIndex, pi - 1, comparer);  //Before pi
                Sort(arr, pi + 1, endIndex, comparer);    //After pi
            }
        }

        /// <summary>
        /// This function takes last element as pivot, places the pivot element at its correct 
        /// position in sorted array, and places all smaller(smaller than pivot) to left of pivot and all 
        /// greater elements to right of pivot
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        private static int Partition<T>(T[] arr, int startIndex, int endIndex, IComparer<T> comparer) where T : IComparable<T>
        {
            //The last element on the array
            var pivot = arr[endIndex];

            var i = startIndex - 1;

            for (int j = startIndex; j <= endIndex - 1; j++)
            {
                if (comparer.Compare(arr[j], pivot) <= 0)
                {
                    i++;                 // increment index of smaller element
                    Swap(arr, i, j);
                }
            }

            Swap(arr, i + 1, endIndex);

            return i + 1;
        }

        private static void Swap<T>(T[] array, int index1, int index2)
        {
            var tmp = array[index1];
            array[index1] = array[index2];
            array[index2] = tmp;
        }
    }
}
