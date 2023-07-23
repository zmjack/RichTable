using RichTable.Test.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RichTable.Test
{
    public class LayeringTests
    {
        public class LayoutGroup<TPrevKey, TKey, TModel>
        {
            private LayoutGroup<TPrevKey, TKey, TModel> PrevGroup { get; set; }

            public TKey Key { get; set; }
            public IEnumerable<TModel> Models { get; set; }
        }

        public abstract class LayeringSource<TSource> : IEnumerable<TSource>
        {
            public object[] Keys { get; set; }
            public IEnumerable<TSource> Enumerable { get; set; }

            public IEnumerator<TSource> GetEnumerator() => Enumerable.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class LayeringSource<TSource, TKey1> : LayeringSource<TSource>
        {
            public TKey1 Key1 => (TKey1)Keys[0];

            private Dictionary<TKey1, IEnumerable<TSource>> Dict = new();
        }

        public class LayeringSource<TSource, TKey1, TKey2> : LayeringSource<TSource>
        {
            public TKey1 Key1 => (TKey1)Keys[0];
            public TKey2 Key2 => (TKey2)Keys[1];

            private Dictionary<TKey1, Dictionary<TKey2, IEnumerable<TSource>>> Dict = new();

            public LayeringSource(TSource[] sources, Func<TSource, TKey1> keySelector1, Func<TSource, TKey2> keySelector2)
            {
                foreach (var group in sources.GroupBy(keySelector1))
                {
                    Dict.Add(group.Key, new Dictionary<TKey2, IEnumerable<TSource>>());

                    foreach (var group2 in sources.GroupBy(keySelector2))
                    {
                        Dict[group.Key].Add(group2.Key, group2);
                    }
                }
            }

            public IEnumerable<TSource> this[TKey1 key]
            {
                get
                {
                    return Dict[key].Values.SelectMany(g => g);
                }
            }

            public IEnumerable<TSource> this[TKey1 key1, TKey2 key2]
            {
                get
                {
                    return Dict[key1][key2];
                }
            }
        }

        [Fact]
        public void ModelTest()
        {
            var models = new[]
            {
                new Table.Model { Book = "Book1", Chapter1 = "A", Chapter2 = "A-1", Chapter3 = "A-1-1", Words = 1011 },
                new Table.Model { Book = "Book1", Chapter1 = "A", Chapter2 = "A-1", Chapter3 = "A-1-2", Words = 1012 },
                new Table.Model { Book = "Book1", Chapter1 = "A", Chapter2 = "A-2", Chapter3 = "A-2-1", Words = 1021 },
                new Table.Model { Book = "Book1", Chapter1 = "A", Chapter2 = "A-2", Chapter3 = "A-2-2", Words = 1021 },

                new Table.Model { Book = "Book1", Chapter1 = "B", Chapter2 = "B-1", Chapter3 = "B-1-1", Words = 1011 },
                new Table.Model { Book = "Book1", Chapter1 = "B", Chapter2 = "B-1", Chapter3 = "B-1-2", Words = 1012 },
                new Table.Model { Book = "Book1", Chapter1 = "B", Chapter2 = "B-2", Chapter3 = "B-2-1", Words = 1021 },
                new Table.Model { Book = "Book1", Chapter1 = "B", Chapter2 = "B-2", Chapter3 = "B-2-2", Words = 1021 },
            };

            //var layeringData =

            //models.GroupBy(x => x.Book).Select(b => new LayeringSource<Table.Model, string>
            //{
            //    Keys = new[] { b.Key },
            //    Enumerable = b,
            //}).GroupBy(x => x.Chapter1));
        }

    }
}
