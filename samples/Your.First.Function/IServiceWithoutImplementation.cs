using System.Threading.Tasks;

namespace Your.First.Function
{
    public class Bar
    {
        public override string ToString() => "BAR";
    }
    
    public interface IServiceWithoutImplementation
    {
        string Foo(Bar bar);
    }
}