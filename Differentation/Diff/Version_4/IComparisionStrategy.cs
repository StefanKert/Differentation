using Rota.PayrollInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Differentation.Diff.Version_4
{
    public interface IComparisionStrategy<T>
    {
        bool ShouldCallComparisionStrategyForProperty(string fieldName, Type propertyType);
        bool ShouldCallComparisionStrategyForProperty(PropertyInfo propertyToCompare);
        bool Equals(T first, T second, PropertyInfo propertyToCompare);
        bool Equals(T first, T second, PropertyInfo propertyToCompare, Func<T, object> getFunc);
    }

    public class BasicComparisionStrategy<T> : IComparisionStrategy<T>
    {
        public static Dictionary<int, Func<object, object, bool>> IsEqualMethods = new Dictionary<int, Func<object, object, bool>>
        {
            { typeof(DateTime).GetHashCode(), (first, second) => ((DateTime)first).CompareTo(((DateTime)second)) == 0 },
            { typeof(bool).GetHashCode(), (first, second) => ((bool)first).CompareTo(((bool)second)) == 0},
            { typeof(int).GetHashCode(), (first, second) => ((int)first).CompareTo(((int)second)) == 0 },
            { typeof(double).GetHashCode(), (first, second) => Math.Abs((double) first - (double) second) < 0.0001 }
        };

        public bool Equals(T first, T second, PropertyInfo propertyToCompare) {
            return Equals(first, second, propertyToCompare, x => x.GetType().GetProperty(propertyToCompare.Name).GetValue(x, null));
        }

        public bool Equals(T first, T second, PropertyInfo propertyToCompare, Func<T, object> getFunc) {
            var firstValue = getFunc(first);
            var secondValue = getFunc(second);
            Func<object, object, bool> isEqualMethod;

            if (IsEqualMethods.TryGetValue(propertyToCompare.PropertyType.GetHashCode(), out isEqualMethod))
                return isEqualMethod(firstValue, secondValue);

            throw new NotSupportedException($"Type {propertyToCompare.PropertyType} for given field {propertyToCompare.Name} not supported with {nameof(BasicComparisionStrategy<T>)}");
        }

        public bool ShouldCallComparisionStrategyForProperty(string fieldName, Type propertyType) => IsEqualMethods.ContainsKey(propertyType.GetHashCode());
        public bool ShouldCallComparisionStrategyForProperty(PropertyInfo propertyToCompare) => IsEqualMethods.ContainsKey(propertyToCompare.PropertyType.GetHashCode());
    }
}
