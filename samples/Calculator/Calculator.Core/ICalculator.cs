using System.Threading.Tasks;

namespace Calculator.Core
{
    public interface ICalculator
    {
        Task<int> Add(int a, int b);
        Task<int> Subtract(int a, int b);
    }
}