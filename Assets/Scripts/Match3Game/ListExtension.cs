using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Smith.Extensions {



    static class ListExtension {
        /// <summary>
        /// возвращает последний элемент
        /// </summary>
        public static T Pop<T>(this List<T> list) {
            T item = list[list.Count - 1];
            list.Remove(item);
            return item;
        }

        /// <summary>
        /// возвращает первый элемент
        /// </summary>
        public static T Shift<T>(this List<T> list) {
            T item = list[0];
            list.Remove(item);
            return item;
        }
        /// <summary>
        /// перемешивает список
        /// </summary>
        public static List<T> Shuffle<T>(this List<T> list) {
            for (var i = 0; i < list.Count; i++) {
                var temp = list[i];
                var randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
            return list;
        }
    }

}