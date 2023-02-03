using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDOM_Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SampleClass sample = new SampleClass();
            sample.AddFields();
            sample.AddProperties();
            sample.AddMethod();
            sample.AddConstructor();
            sample.AddEntryPoint(20, "테스트");
            sample.GenerateCSharpCode();

            Console.WriteLine("코드 생성 완료");

            Console.ReadLine();

            CodeDomProvider codeDom = CodeDomProvider.CreateProvider("CSharp");
            // 메모리에 어셈블리 생성
            CompilerParameters param = new CompilerParameters();
            param.GenerateInMemory = true;

            // 생성된 소스코드 컴파일
            CompilerResults results =
                codeDom.CompileAssemblyFromSource(param, System.IO.File.ReadAllText("SampleCode.cs"));

            // 컴파일 실패시
            if (results.Errors.Count > 0)
            {
                foreach (var err in results.Errors)
                {
                    Console.WriteLine(err.ToString());
                }
                return;
            }

            // 어셈블리 로딩            
            Type createdClassType = results.CompiledAssembly.GetType("CodeDOMSample.CodeDOMCreatedClass");
            object createdClassObj = Activator.CreateInstance(createdClassType, (Int16)20, "테스트");

            // 메서드 호출
            var result = createdClassObj.GetType().GetMethod("Result").Invoke(createdClassObj, null);
            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
