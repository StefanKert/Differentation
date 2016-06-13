using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Differentation.Diff.Version_2
{
    public class GenericComparer<T>
    {
        private readonly IComparisionStrategy<T> _comparisionStrategy;
        public T First { get; set; }
        public T Second { get; set; }

        public GenericComparer(IComparisionStrategy<T>  comparisionStrategy)
        {
            _comparisionStrategy = comparisionStrategy;
        }

        public IEnumerable<MemberInformation> GetDifferences(T first, T second)
        {
            var results = new List<MemberInformation>();
            foreach(var field in typeof(T).GetProperties())
            {
                var firstValue = first.GetType().GetProperty(field.Name).GetValue(first, null);
                var secondValue = second.GetType().GetProperty(field.Name).GetValue(second, null);

                if (_comparisionStrategy.ShouldCallComparisionStrategyForProperty(field))
                {
                    if (_comparisionStrategy.Equals(first, second, field))
                        continue;
                }


                if (firstValue == secondValue)
                    continue;

                results.Add(new MemberInformation
                {
                    FirstValue = firstValue,
                    SecondValue = secondValue,
                    FieldName = field.Name,
                    FieldType = field.PropertyType
                });
            }
            return results;
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

