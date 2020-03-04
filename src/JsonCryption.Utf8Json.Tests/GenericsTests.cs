using Shouldly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Utf8Json;
using Utf8Json.Resolvers;
using Xunit;

namespace JsonCryption.Utf8Json.Tests
{
    public class GenericsTests
    {
        [Fact]
        public void Primitives()
        {
            var instance = new FooPrimitives
            {
                MyInt = 75,
                MyNullInt = null,
                My2DInts = new[] { int.MinValue, 0, int.MaxValue },
                My3DInts = new[,] { { int.MinValue, int.MaxValue } },
                MyKvp = new KeyValuePair<string, int>("key", 75),
                MyConcurrentDictionary = GetConcurrentDictionary(),
                MyDictionary = GetDictionary(),
                MyDoubleConcurrentBag = new ConcurrentBag<double>(GetDoubleIEnumerable()),
                MyDoubleConcurrentQueue = new ConcurrentQueue<double>(GetDoubleIEnumerable()),
                MyDoubleConcurrentStack = new ConcurrentStack<double>(GetDoubleIEnumerable()),
                MyDoubleHashSet = new HashSet<double>(GetDoubleIEnumerable()),
                MyDoubleICollection = GetDoubleIEnumerable().ToList(),
                MyDoubleIEnumerable = GetDoubleIEnumerable(),
                MyDoubleIList = GetDoubleIEnumerable().ToList(),
                MyDoubleLinkedList = new LinkedList<double>(GetDoubleIEnumerable().ToList()),
                MyDoubleList = GetDoubleIEnumerable().ToList(),
                MyDoubleObservableCollection = new ObservableCollection<double>(GetDoubleIEnumerable()),
                MyDoubleQueue = new Queue<double>(GetDoubleIEnumerable()),
                MyDoubleReadOnlyCollection = new ReadOnlyCollection<double>(GetDoubleIEnumerable().ToList()),
                MyDoubleReadOnlyList = GetDoubleIEnumerable().ToList(),
                MyDoubleReadOnlyObservableCollection = new ReadOnlyObservableCollection<double>(new ObservableCollection<double>(GetDoubleIEnumerable())),
                MyDoubleSet = new HashSet<double>(GetDoubleIEnumerable()),
                MyDoubleStack = new Stack<double>(GetDoubleIEnumerable()),
                MyDoubleTuple = new Tuple<string, int>("key", 75),
                MyDoubleValueTuple = ("key", 75),
                MyGrouping = GetDictionary().GroupBy(d => d.Key, d => d.Value).First(),
                MyIDictionary = GetDictionary(),
                MyIReadOnlyDictionary = GetDictionary(),
                MyLazyDouble = new Lazy<double>(double.MaxValue),
                MyLazyString = new Lazy<string>("key"),
                MyLookup = GetStringIEnumerable().ToLookup(s => s, s => 1.2345),
                MyReadOnlyCollectionOfDoubles = new ReadOnlyCollection<double>(GetDoubleIEnumerable().ToList()),
                MyReadOnlyCollectionOfStrings = new ReadOnlyCollection<string>(GetStringIEnumerable().ToList()),
                MyReadOnlyDictionary = new ReadOnlyDictionary<string, double>(GetDictionary()),
                MySortedDictionary = new SortedDictionary<string, double>(GetDictionary()),
                MySortedList = new SortedList<string, double>(GetDictionary()),
                MyStringConcurrentBag = new ConcurrentBag<string>(GetStringIEnumerable()),
                MyStringConcurrentQueue = new ConcurrentQueue<string>(GetStringIEnumerable()),
                MyStringConcurrentStack = new ConcurrentStack<string>(GetStringIEnumerable()),
                MyStringHashSet = new HashSet<string>(GetStringIEnumerable()),
                MyStringICollection = GetStringIEnumerable().ToList(),
                MyStringIEnumerable = GetStringIEnumerable(),
                MyStringIList = GetStringIEnumerable().ToList(),
                MyStringLinkedList = new LinkedList<string>(GetStringIEnumerable().ToList()),
                MyStringList = GetStringIEnumerable().ToList(),
                MyStringObservableCollection = new ObservableCollection<string>(GetStringIEnumerable()),
                MyStringQueue = new Queue<string>(GetStringIEnumerable()),
                MyStringReadOnlyCollection = GetStringIEnumerable().ToList(),
                MyStringReadOnlyList = GetStringIEnumerable().ToList(),
                MyStringReadOnlyObservableCollection = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(GetStringIEnumerable())),
                MyStringSet = new HashSet<string>(GetStringIEnumerable()),
                MyStringStack = new Stack<string>(GetStringIEnumerable()),
                MyTripleTuple = new Tuple<string, int, decimal>("key", 75, decimal.MaxValue)
            };

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            var resolver = StandardResolver.AllowPrivate;
            var allProperties = GetAllPropertyValues(instance, resolver);

            // make sure everything is encrypted
            foreach (var kvp in allProperties)
            {
                dynamic value = kvp.Value.GetValue(instance);

                var method = typeof(Helpers)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .First(m => m.Name == nameof(Helpers.SerializedValueOf))
                    .MakeGenericMethod(kvp.Value.PropertyType);

                var serialized = (string)method.Invoke(this, new object[] { value, resolver, kvp.Key });
                
                json.ShouldNotContain(serialized);
            }

            var deserialized = JsonSerializer.Deserialize<FooPrimitives>(json);

            // check after deserialization
            var normalProperties = allProperties
                .Where(p => !p.Value.PropertyType.Name.Contains("Lazy"))
                .Where(p => !p.Value.PropertyType.Name.Contains("ConcurrentDictionary"))
                .Where(p => !p.Value.PropertyType.Name.Contains("ConcurrentBag"));

            foreach (var kvp in normalProperties)
            {
                var instanceValue = kvp.Value.GetValue(instance);
                var deserializedValue = kvp.Value.GetValue(deserialized);

                deserializedValue.ShouldBe(instanceValue, kvp.Value.Name);
            }

            // special check for lazy
            deserialized.MyLazyDouble.Value.ShouldBe(instance.MyLazyDouble.Value);
            deserialized.MyLazyString.Value.ShouldBe(instance.MyLazyString.Value);

            // ConcurrentBag, ConcurrentDictionary should be used in unordered manner
            var instanceBagDoubles = instance.MyDoubleConcurrentBag.ToHashSet();
            deserialized.MyDoubleConcurrentBag.All(d => instanceBagDoubles.Contains(d));
            var instanceBagStrings = instance.MyStringConcurrentBag.ToHashSet();
            deserialized.MyStringConcurrentBag.All(s => instanceBagStrings.Contains(s));

            var instanceDictionaryValues = instance.MyConcurrentDictionary.ToHashSet();
            deserialized.MyConcurrentDictionary.All(kvp => instanceDictionaryValues.Contains(kvp));
        }

        private Dictionary<string, PropertyInfo> GetAllPropertyValues<T>(T foo, IJsonFormatterResolver resolver)
        {
            return typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => (p.Name, PropertyInfo: p))
                .ToDictionary(x => x.Name, x => x.PropertyInfo);
        }

        private IEnumerable<string> GetStringIEnumerable() => new[] { "min", "rando", "max" };
        private IEnumerable<double> GetDoubleIEnumerable() => new[] { double.MinValue, 1.2345, double.MaxValue };
        private Dictionary<string, double> GetDictionary() => new Dictionary<string, double>
        {
            { "min", double.MinValue},
            { "rando", 1.2345},
            { "max", double.MaxValue},
        };
        private ConcurrentDictionary<string, double> GetConcurrentDictionary()
        {
            var dict = new ConcurrentDictionary<string, double>();
            dict["min"] = double.MinValue;
            dict["rando"] = 1.2345;
            dict["max"] = double.MaxValue;
            return dict;
        }

        class FooPrimitives
        {
            [Encrypt]
            public int? MyInt { get; set; }
            [Encrypt]
            public int? MyNullInt { get; set; }
            [Encrypt]
            public int[] My2DInts { get; set; }
            [Encrypt]
            public int[,] My3DInts { get; set; }
            [Encrypt]
            public ConcurrentDictionary<string, double> MyConcurrentDictionary { get; set; }
            [Encrypt]
            public Dictionary<string, double> MyDictionary { get; set; }
            [Encrypt]
            public ConcurrentBag<double> MyDoubleConcurrentBag { get; set; }
            [Encrypt]
            public ConcurrentQueue<double> MyDoubleConcurrentQueue { get; set; }
            [Encrypt]
            public ConcurrentStack<double> MyDoubleConcurrentStack { get; set; }
            [Encrypt]
            public HashSet<double> MyDoubleHashSet { get; set; }
            [Encrypt]
            public ICollection<double> MyDoubleICollection { get; set; }
            [Encrypt]
            public IEnumerable<double> MyDoubleIEnumerable { get; set; }
            [Encrypt]
            public IList<double> MyDoubleIList { get; set; }
            [Encrypt]
            public LinkedList<double> MyDoubleLinkedList { get; set; }
            [Encrypt]
            public List<double> MyDoubleList { get; set; }
            [Encrypt]
            public KeyValuePair<string, int> MyKvp { get; set; }
            [Encrypt]
            public Tuple<string, int> MyDoubleTuple { get; set; }
            [Encrypt]
            public Tuple<string, int, decimal> MyTripleTuple { get; set; }
            [Encrypt]
            public ValueTuple<string, int> MyDoubleValueTuple { get; set; }
            [Encrypt]
            public List<string> MyStringList { get; set; }
            [Encrypt]
            public LinkedList<string> MyStringLinkedList { get; set; }
            [Encrypt]
            public Queue<string> MyStringQueue { get; set; }
            [Encrypt]
            public Queue<double> MyDoubleQueue { get; set; }
            [Encrypt]
            public Stack<string> MyStringStack { get; set; }
            [Encrypt]
            public Stack<double> MyDoubleStack { get; set; }
            [Encrypt]
            public HashSet<string> MyStringHashSet { get; set; }
            [Encrypt]
            public ReadOnlyCollection<string> MyReadOnlyCollectionOfStrings { get; set; }
            [Encrypt]
            public ReadOnlyCollection<double> MyReadOnlyCollectionOfDoubles { get; set; }
            [Encrypt]
            public IList<string> MyStringIList { get; set; }
            [Encrypt]
            public ICollection<string> MyStringICollection { get; set; }
            [Encrypt]
            public IEnumerable<string> MyStringIEnumerable { get; set; }
            [Encrypt]
            public IDictionary<string, double> MyIDictionary { get; set; }
            [Encrypt]
            public SortedDictionary<string, double> MySortedDictionary { get; set; }
            [Encrypt]
            public SortedList<string, double> MySortedList { get; set; }
            [Encrypt]
            public ILookup<string, double> MyLookup { get; set; }
            [Encrypt]
            public IGrouping<string, double> MyGrouping { get; set; }
            [Encrypt]
            public ObservableCollection<string> MyStringObservableCollection { get; set; }
            [Encrypt]
            public ObservableCollection<double> MyDoubleObservableCollection { get; set; }
            [Encrypt]
            public ReadOnlyObservableCollection<string> MyStringReadOnlyObservableCollection { get; set; }
            [Encrypt]
            public ReadOnlyObservableCollection<double> MyDoubleReadOnlyObservableCollection { get; set; }
            [Encrypt]
            public IReadOnlyList<string> MyStringReadOnlyList { get; set; }
            [Encrypt]
            public IReadOnlyList<double> MyDoubleReadOnlyList { get; set; }
            [Encrypt]
            public IReadOnlyCollection<string> MyStringReadOnlyCollection { get; set; }
            [Encrypt]
            public IReadOnlyCollection<double> MyDoubleReadOnlyCollection { get; set; }
            [Encrypt]
            public ISet<string> MyStringSet { get; set; }
            [Encrypt]
            public ISet<double> MyDoubleSet { get; set; }
            [Encrypt]
            public ConcurrentBag<string> MyStringConcurrentBag { get; set; }
            [Encrypt]
            public ConcurrentQueue<string> MyStringConcurrentQueue { get; set; }
            [Encrypt]
            public ConcurrentStack<string> MyStringConcurrentStack { get; set; }
            [Encrypt]
            public ReadOnlyDictionary<string, double> MyReadOnlyDictionary { get; set; }
            [Encrypt]
            public IReadOnlyDictionary<string, double> MyIReadOnlyDictionary { get; set; }
            [Encrypt]
            public Lazy<string> MyLazyString { get; set; }
            [Encrypt]
            public Lazy<double> MyLazyDouble { get; set; }
        }

        [Fact]
        public void Complex_types()
        {
            var instance = new FooComplex<Bar>
            {
                MyBarDictionary = GetComplexDictionary(),
                MyConcurrentBarDictionary = new ConcurrentDictionary<string, Bar>(GetComplexDictionary()),
                MyBars = GetComplexArray(),
                MyListOfBars = new List<Bar>(GetComplexArray()),
                MyLazyBar = new Lazy<Bar>(new Bar { MyInt = 75, MyString = "something public" })
            };

            Helpers.SetJsonSerializerResolver();

            var bytes = JsonSerializer.Serialize(instance);
            var json = Encoding.UTF8.GetString(bytes);

            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyBarDictionary, StandardResolver.AllowPrivate, nameof(instance.MyBarDictionary)));
            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyConcurrentBarDictionary, StandardResolver.AllowPrivate, nameof(instance.MyConcurrentBarDictionary)));
            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyBars, StandardResolver.AllowPrivate, nameof(instance.MyBars)));
            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyListOfBars, StandardResolver.AllowPrivate, nameof(instance.MyListOfBars)));
            json.ShouldNotContain(Helpers.SerializedValueOf(instance.MyLazyBar, StandardResolver.AllowPrivate, nameof(instance.MyLazyBar)));

            var deserialized = JsonSerializer.Deserialize<FooComplex<Bar>>(json);

            deserialized.MyBarDictionary.ShouldBe(instance.MyBarDictionary);
            deserialized.MyBars.ShouldBe(instance.MyBars);
            deserialized.MyListOfBars.ShouldBe(instance.MyListOfBars);

            // lazy is special
            deserialized.MyLazyBar.Value.ShouldBe(instance.MyLazyBar.Value);

            // ConcurrentDictionary should be unordered
            var instanceConcurrentDictionaryStrings = instance.MyConcurrentBarDictionary.Keys.ToHashSet();
            deserialized.MyConcurrentBarDictionary.Keys.All(key => instanceConcurrentDictionaryStrings.Contains(key));
            deserialized.MyConcurrentBarDictionary.Keys.All(key => deserialized.MyConcurrentBarDictionary[key] == instance.MyConcurrentBarDictionary[key]);
        }

        private Bar[] GetComplexArray()
            => new Bar[]
            {
                new Bar{ MyInt = 75, MyString = "something public" },
                new Bar{ MyInt = 17, MyString = "blah blah" }
            };

        private Dictionary<string, Bar> GetComplexDictionary()
            => new Dictionary<string, Bar>
                {
                    { "first", new Bar{ MyInt = 75, MyString = "something public" } },
                    { "second", new Bar{ MyInt = 17, MyString = "blah blah" } }
                };

        class FooComplex<T>
        {
            [Encrypt]
            public Dictionary<string, T> MyBarDictionary { get; set; }
            [Encrypt]
            public ConcurrentDictionary<string, T> MyConcurrentBarDictionary { get; set; }
            [Encrypt]
            public T[] MyBars { get; set; }
            [Encrypt]
            public List<T> MyListOfBars { get; set; }
            [Encrypt]
            public Lazy<T> MyLazyBar { get; set; }
        }

        class Bar
        {
            public int MyInt { get; set; }
            public string MyString { get; set; }

            public override bool Equals(object obj) => obj is Bar other && MyInt.Equals(other.MyInt) && MyString.Equals(other.MyString);
            public override int GetHashCode() => MyInt.GetHashCode() ^ MyString.GetHashCode();
        }
    }
}
