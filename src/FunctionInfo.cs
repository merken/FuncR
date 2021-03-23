using System;
using System.Reflection;

namespace FuncR
{
    public class FunctionInfo
    {
        public MethodInfo MethodInfo { get; set; }
        public Delegate Function { get; set; }
    }
}