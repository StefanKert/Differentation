using Rota.PayrollInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Differentation.Diff.Version_3
{
    public interface IComparisionStrategy<T>
    {
        bool ShouldCallComparisionStrategyForProperty(PropertyInfo propertyToCompare);
        bool Equals(T first, T second, PropertyInfo propertyToCompare);
        bool Equals(T first, T second, PropertyInfo propertyToCompare, Func<T, object> getFunc);
    }

    public class BasicComparisionStrategy<T> : IComparisionStrategy<T>
    {
        public static Dictionary<Type, Func<object, object, bool>> IsEqualMethods = new Dictionary<Type, Func<object, object, bool>>
        {
            { typeof(DateTime), (first, second) => ((DateTime)first).CompareTo(((DateTime)second)) == 0 },
            { typeof(bool), (first, second) => ((bool)first).CompareTo(((bool)second)) == 0},
            { typeof(int), (first, second) => ((int)first).CompareTo(((int)second)) == 0 },
            { typeof(double), (first, second) => Math.Abs((double) first - (double) second) < 0.0001 }
        };

        public bool Equals(T first, T second, PropertyInfo propertyToCompare) {
            return Equals(first, second, propertyToCompare, x => x.GetType().GetProperty(propertyToCompare.Name).GetValue(x, null));
        }

        public bool Equals(T first, T second, PropertyInfo propertyToCompare, Func<T, object> getFunc) {
            var firstValue = first.GetType().GetProperty(propertyToCompare.Name).GetValue(first, null);
            var secondValue = second.GetType().GetProperty(propertyToCompare.Name).GetValue(first, null);
            Func<object, object, bool> isEqualMethod;

            if (IsEqualMethods.TryGetValue(propertyToCompare.PropertyType, out isEqualMethod))
                return isEqualMethod(firstValue, secondValue);

            throw new NotSupportedException($"Type {propertyToCompare.PropertyType} for given field {propertyToCompare.Name} not supported with {nameof(BasicComparisionStrategy<T>)}");
        }

        public bool ShouldCallComparisionStrategyForProperty(PropertyInfo propertyToCompare) => IsEqualMethods.ContainsKey(propertyToCompare.PropertyType);
    }


    public class ContractComparisionStrategy : IComparisionStrategy<PiContract>
    {
        public bool Equals(PiContract first, PiContract second, PropertyInfo propertyToCompare){
            return Equals(first, second, propertyToCompare, x => x.GetType().GetProperty(propertyToCompare.Name).GetValue(x, null));
        }

        public bool Equals(PiContract first, PiContract second, PropertyInfo propertyToCompare, Func<PiContract, object> getFunc) {
            if (propertyToCompare.Name == nameof(PiContract.EndOfContract))
            {
                if (first.EndOfContract == second.EndOfContract)
                    return true;
                else if (first.EndOfContract == new DateTime(2200, 12, 31) && second.EndOfContract == new DateTime(9999, 12, 31))
                    return true;
                else if (second.EndOfContract == new DateTime(2200, 12, 31) && first.EndOfContract == new DateTime(9999, 12, 31))
                    return true;
                else
                    return false;
            }
            throw new NotSupportedException($"Type {propertyToCompare.PropertyType} for given field {propertyToCompare.Name} not supported with {nameof(ContractComparisionStrategy)}");
        }

        public bool ShouldCallComparisionStrategyForProperty(PropertyInfo propertyToCompare){
            if (propertyToCompare.Name == nameof(PiContract.EndOfContract))
                return true;
            return false;
        }
    }
}
