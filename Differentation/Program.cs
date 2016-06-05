using Differentation.Diff;
using Rota.PayrollInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Differentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstObj = new PayrollInterfacePerson();
            var secondObj = new PayrollInterfacePerson();


            var differentiator = new Differentatior<PayrollInterfacePerson>();
            var differences = differentiator.GetDifferences(firstObj, secondObj);
        }
    }


    public class TestObject
    {
        public int FirstProp { get; set; }
        public int SecondProp { get; set; }
    }
}
