using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;

namespace CodeDOM_Example
{
    public class SampleClass
    {
        // CodeDOM 객체 그래프 생성을 위한 컴파일 단위 정의
        CodeCompileUnit _targetUnit;
        // 클래스, 구조체, 인터페이스 또는 열거형 선언을 위한 
        CodeTypeDeclaration _targetClass;
        private const string _outputFileName = "SampleCode.cs";

        public SampleClass()
        {
            _targetUnit = new CodeCompileUnit();

            // 네임스페이스 설정
            CodeNamespace samples = new CodeNamespace("CodeDOMSample");
            // Import 설정
            samples.Imports.Add(new CodeNamespaceImport("System"));

            // 클래스 이름 설정
            _targetClass = new CodeTypeDeclaration("CodeDOMCreatedClass");
            _targetClass.IsClass = true;
            // public 접근자로 설정
            _targetClass.TypeAttributes =
                TypeAttributes.Public;
            samples.Types.Add(_targetClass);

            // 객체 그래프에 네임스페이스 추가
            _targetUnit.Namespaces.Add(samples);
        }

        // 필드 추가
        public void AddFields()
        {
            // Private 필드 생성
            CodeMemberField ageValueField = new CodeMemberField();
            // Private 접근자로 설정
            ageValueField.Attributes = MemberAttributes.Private;
            // 필드 이름
            ageValueField.Name = "_ageValue";
            // Int16 타입으로 설정
            ageValueField.Type = new CodeTypeReference(typeof(System.Int16));
            // 주석 추가
            ageValueField.Comments.Add(new CodeCommentStatement("나이"));
            // 클래스에 맴버 필드로 추가
            _targetClass.Members.Add(ageValueField);

            // Private 필드 생성
            CodeMemberField nameValueField = new CodeMemberField();
            // Private 접근자로 설정
            nameValueField.Attributes = MemberAttributes.Private;
            // 필드 이름
            nameValueField.Name = "_nameValue";
            // String 타입으로 설정
            nameValueField.Type =
                new CodeTypeReference(typeof(System.String));
            // 주석 추가
            nameValueField.Comments.Add(new CodeCommentStatement("이름"));
            // 클래스에 맴버 필드로 추가
            _targetClass.Members.Add(nameValueField);
        }

        // 속성 추가
        public void AddProperties()
        {
            // Age 속성 생성
            CodeMemberProperty ageProperty = new CodeMemberProperty();
            // public 접근자로 설정
            ageProperty.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            // 속성 이름
            ageProperty.Name = "Age";
            // get 지정
            ageProperty.HasGet = true;
            // Int16 타입으로 설정
            ageProperty.Type = new CodeTypeReference(typeof(System.Int16));
            // 주석 추가
            ageProperty.Comments.Add(new CodeCommentStatement("나이"));

            // get 반환 필드 지정 [_ageValue]
            ageProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "_ageValue")));

            // 클래스에 속성 추가
            _targetClass.Members.Add(ageProperty);

            // Name 속성 생성
            CodeMemberProperty nameProperty = new CodeMemberProperty();
            // public 접근자로 설정
            nameProperty.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            // 속성 이름
            nameProperty.Name = "Name";
            nameProperty.HasGet = true;
            // String 타입으로 설정
            nameProperty.Type = new CodeTypeReference(typeof(System.String));
            // 주석 추가
            nameProperty.Comments.Add(new CodeCommentStatement("이름"));

            // get 반환 필드 지정 [_nameValue]
            nameProperty.GetStatements.Add(new CodeMethodReturnStatement(
                new CodeFieldReferenceExpression(
                new CodeThisReferenceExpression(), "_nameValue")));

            // 클래스에 속성 추가
            _targetClass.Members.Add(nameProperty);

            // 읽기 전용 속성 생성
            CodeMemberProperty introductionProperty = new CodeMemberProperty();
            introductionProperty.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;
            introductionProperty.Name = "Introduction";
            introductionProperty.HasGet = true;
            introductionProperty.Type = new CodeTypeReference(typeof(System.String));
            introductionProperty.Comments.Add(new CodeCommentStatement("읽기 전용 속성 입니다."));

            // get 코드 연산
            CodeBinaryOperatorExpression introductionExpression =
                new CodeBinaryOperatorExpression(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "_ageValue"),
                    CodeBinaryOperatorType.Add,
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "_nameValue"));

            // get 반환 필드 지정 [위에서 처리한 연산 코드 introductionExpression]
            introductionProperty.GetStatements.Add(
                new CodeMethodReturnStatement(introductionExpression));

            // 클래스에 속성 추가
            _targetClass.Members.Add(introductionProperty);
        }

        // 메서드 추가
        public void AddMethod()
        {
            // Result 메서드 생성
            CodeMemberMethod resultMethod = new CodeMemberMethod();
            // public 접근자로 설정
            resultMethod.Attributes =
                MemberAttributes.Public;
            // 메서드 이름
            resultMethod.Name = "Result";
            // Return 타입 String 지정
            resultMethod.ReturnType =
                new CodeTypeReference(typeof(System.String));

            // 반환 값
            string formattedOutput = "설정된 이름과 나이는" + Environment.NewLine +
                "{0}, {1} 입니다.";

            // 로컬 변수 정의
            CodeFieldReferenceExpression ageReference =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Age");

            CodeFieldReferenceExpression nameReference =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Name");

            CodeMethodReturnStatement returnStatement = new CodeMethodReturnStatement();

            // Return 코드 연산
            returnStatement.Expression =
                new CodeMethodInvokeExpression(
                    new CodeTypeReferenceExpression("System.String"), "Format",
                    new CodePrimitiveExpression(formattedOutput), ageReference, nameReference);

            resultMethod.Statements.Add(returnStatement);
            _targetClass.Members.Add(resultMethod);
        }

        // 생성자 추가
        public void AddConstructor()
        {
            // 생성자 생성
            CodeConstructor constructor = new CodeConstructor();
            // public 접근자로 설정
            constructor.Attributes =
                MemberAttributes.Public | MemberAttributes.Final;

            // Add parameters.
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(System.Int16), "age"));
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                typeof(System.String), "name"));

            // 맴버 필드 초기화
            // 생성자 파라메터 age 설정 후 ageReference[_age]에 대입
            CodeFieldReferenceExpression ageReference =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_ageValue");

            constructor.Statements.Add(new CodeAssignStatement(ageReference, new CodeArgumentReferenceExpression("age")));

            // 생성자 파라메터 name 설정 후 nameReference[_name]에 대입
            CodeFieldReferenceExpression nameReference =
                new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_nameValue");

            constructor.Statements.Add(new CodeAssignStatement(nameReference, new CodeArgumentReferenceExpression("name")));
            _targetClass.Members.Add(constructor);
        }

        // 진입점 추가
        public void AddEntryPoint(Int16 age, string name)
        {
            CodeEntryPointMethod start = new CodeEntryPointMethod();

            // 객체 생성
            CodeObjectCreateExpression objectCreate =
                new CodeObjectCreateExpression(
                    new CodeTypeReference("CodeDOMCreatedClass"),
                    new CodePrimitiveExpression(age),
                    new CodePrimitiveExpression(name));

            // 코드 생성 다음과 같은 코드를 생성합니다.
            // CodeDOMCreatedClass testClass =
            //     new CodeDOMCreatedClass({age}, {name});"
            start.Statements.Add(new CodeVariableDeclarationStatement(new CodeTypeReference("CodeDOMCreatedClass"), "testClass", objectCreate));

            // 메서드 호출
            CodeMethodInvokeExpression resultInvoke =
                new CodeMethodInvokeExpression(
                new CodeVariableReferenceExpression("testClass"), "Result");

            // 결과 콘솔 출력
            start.Statements.Add(new CodeMethodInvokeExpression(
                new CodeTypeReferenceExpression("System.Console"), "WriteLine", resultInvoke));

            _targetClass.Members.Add(start);
        }

        public void GenerateCSharpCodeFile()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(_outputFileName))
            {
                provider.GenerateCodeFromCompileUnit(_targetUnit, sourceWriter, options);
            }
        }

        public string GenerateCSharpCode()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            StringBuilder builder = new StringBuilder();
            using (StringWriter sourceWriter = new StringWriter(builder))
            {
                provider.GenerateCodeFromCompileUnit(_targetUnit, sourceWriter, options);
            }

            return builder.ToString();
        }
    }
}
