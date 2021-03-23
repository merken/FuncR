using System.Threading.Tasks;

namespace Calculator.Web
{
    public interface IStaticValuesService
    {
        Task<int> GetSubtractionConstant();
    }

    public class StaticValuesService : IStaticValuesService
    {
        private const int subtractionConstant = 1;

        public async Task<int> GetSubtractionConstant()
        {
            await Task.Delay(1000);
            return subtractionConstant;
        }
    }
}