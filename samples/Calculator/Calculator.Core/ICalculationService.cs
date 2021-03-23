using System;
using System.Threading.Tasks;

namespace Calculator.Core
{
    public interface ICalculationService
    {
        Task<int> Calculate(int a, int b);
    }
}
