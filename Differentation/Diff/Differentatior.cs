using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Differentation.Diff
{
    public class Differentatior<T>
    {
        public T First { get; set; }
        public T Second { get; set; }

        public IList<PropertyInfo> Fields { get; set; }

        public Differentatior()
        {
            Fields = typeof(T).GetProperties();
        }

        public IEnumerable<MemberInformation> GetDifferences(T first, T second)
        {
            foreach(var field in Fields)
            {
                var firstValue = GetPropValue(first, field.Name);
                var secondValue = GetPropValue(first, field.Name);

                if (firstValue == secondValue)
                    continue;

                yield return new MemberInformation
                {
                    FirstValue = firstValue,
                    SecondValue = secondValue,
                    FieldName = field.Name
                };
            }
        }

        public IEnumerable<MemberInformation> GetEqualMembers(T first, T second)
        {
            foreach (var field in Fields)
            {
                var firstValue = GetPropValue(first, field.Name);
                var secondtValue = GetPropValue(first, field.Name);

                if (firstValue != secondtValue)
                    continue;

                yield return new MemberInformation
                {
                    FirstValue = firstValue,
                    SecondValue = secondtValue,
                    FieldName = field.Name
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
