using System.Threading.Tasks;
using Calculator.Core;

namespace Calculator.Domain
{
    public class CalculationService: ICalculationService
    {
        private readonly ICalculator _calculator;

        public CalculationService(ICalculator calculator)
        {
            _calculator = calculator;
        }
        
        public async Task<int> Calculate(int a, int b)
        {
            a = await _calculator.Add(a, b);
            a = await _calculator.Subtract(a, b);
            return a;
        }
    }
}