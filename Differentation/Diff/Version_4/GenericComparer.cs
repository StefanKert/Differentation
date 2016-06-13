using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Differentation.Diff.Version_4
{
    public class GenericComparer<T>
    {
        private readonly IComparisionStrategy<T> _comparisionStrategy;
        private PropertyInfo[] _fields;

        public T First { get; set; }
        public T Second { get; set; }

        public static ConcurrentDictionary<int, Func<T, object>> getMethods = new ConcurrentDictionary<int, Func<T, object>>();

        public GenericComparer(IComparisionStrategy<T>  comparisionStrategy){
            _comparisionStrategy = comparisionStrategy;
            _fields = typeof(T).GetProperties();
            foreach (var field in _fields) {
                if (!getMethods.ContainsKey(field.Name.GetHashCode()))
                    getMethods.TryAdd(field.Name.GetHashCode(), CreateGetterMethodForField(field));
            }
        }

        public IEnumerable<MemberInformation> GetDifferences(T first, T second)
        {
            var results = new List<MemberInformation>();
            foreach(var field in _fields) {
                var fieldName = field.Name;
                var fieldType = field.PropertyType;
                var readerFunc = getMethods[fieldName.GetHashCode()];
                var firstValue = readerFunc(first);
                var secondValue = readerFunc(second);

                if (_comparisionStrategy.ShouldCallComparisionStrategyForProperty(fieldName, fieldType))
                {
                    if (_comparisionStrategy.Equals(first, second, field, readerFunc))
                        continue;
                }


                if (firstValue == secondValue)
                    continue;

                results.Add(new MemberInformation
                {
                    FirstValue = firstValue,
                    SecondValue = secondValue,
                    FieldName = fieldName,
                    FieldType = fieldType
                });
            }
            return results;
        }

        private Func<T, object> CreateGetterMethodForField(PropertyInfo field) {
            var getterMethodInfo = field.GetGetMethod();
            var entity = Expression.Parameter(typeof(T));
            var getterCall = Expression.Call(entity, getterMethodInfo);
            var castToObject = Expression.Convert(getterCall, typeof(object));
            var lambda = Expression.Lambda(castToObject, entity);
            var functionThatGetsValue = (Func<T, object>)lambda.Compile();
            return functionThatGetsValue;
        }
    }

    public class MemberInformation
    {
        public Type FieldType { get; set; }
        public object FirstValue { get; set; }
        public object SecondValue { get; set; }
        public string FieldName { get; set; }

        public override string ToString()
        {
            return $"FieldName: {FieldName}, Type: {FieldType} , FirstValue: {FirstValue}, SecondValue: {SecondValue}";
        }
    }
}

