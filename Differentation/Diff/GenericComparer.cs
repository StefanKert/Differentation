using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Differentation.Diff
{
    public class GenericComparer<T>
    {
        public T First { get; set; }
        public T Second { get; set; }

        public IList<PropertyInfo> Fields { get; set; }

        public static Dictionary<Type, Func<object, object, bool>> IsEqualMethods = new Dictionary<Type, Func<object, object, bool>>
        {
            { typeof(DateTime), (first, second) => ((DateTime)first).CompareTo(((DateTime)second)) == 0 },
            { typeof(bool), (first, second) => ((bool)first).CompareTo(((bool)second)) == 0},
            { typeof(int), (first, second) => ((int)first).CompareTo(((int)second)) == 0 },
            { typeof(double), (first, second) => Math.Abs((double) first - (double) second) < 0.0001 }
        };

        public GenericComparer()
        {
            Fields = typeof(T).GetProperties();
        }

        public IEnumerable<MemberInformation> GetDifferences(T first, T second)
        {
            foreach(var field in Fields)
            {
                var fieldName = field.Name;
                var fieldType = field.PropertyType;
                if (fieldType.IsEnum)
                    fieldType = typeof(int);

                var firstValue = GetPropValue(first, fieldName);
                var secondValue = GetPropValue(second, fieldName);



                if (IsEqualMethods.ContainsKey(fieldType))
                {
                    if (IsEqualMethods[fieldType](firstValue, secondValue))
                        continue;
                }
                else if (firstValue == secondValue)
                    continue;

                yield return new MemberInformation
                {
                    FirstValue = firstValue,
                    SecondValue = secondValue,
                    FieldName = fieldName
                };
            }
        }

        public IEnumerable<MemberInformation> GetEqualMembers(T first, T second)
        {
            foreach (var field in Fields)
            {
                var fieldName = field.Name;
                var fieldType = field.PropertyType;
                if (fieldType.IsEnum)
                    fieldType = typeof(int);

                var firstValue = GetPropValue(first, fieldName);
                var secondValue = GetPropValue(second, fieldName);

                if (IsEqualMethods.ContainsKey(fieldType))
                {
                    if (!IsEqualMethods[fieldType](firstValue, secondValue))
                        continue;
                }
                else if (firstValue == secondValue)
                    continue;

                yield return new MemberInformation
                {
                    FirstValue = firstValue,
                    SecondValue = secondValue,
                    FieldName = fieldName
                };
            }
        }

        private object GetPropValue(object src, string propName)
        {
            var property = src.GetType().GetProperty(propName);
            return property.GetValue(src, null);
        }
    }

    public class MemberInformation
    {
        public object FirstValue { get; set; }
        public object SecondValue { get; set; }
        public string FieldName { get; set; }
    }
}

