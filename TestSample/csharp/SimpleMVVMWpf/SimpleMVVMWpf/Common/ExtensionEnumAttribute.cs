namespace SimpleMVVMWpf.Common
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Enum에 추가적으로 속성을 설정 합니다.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property,
                   AllowMultiple = true,
                   Inherited = false)]
    public class ExtensionEnumAttribute : Attribute
    {
        private object[] _objs;
        private Type _resourceType;
        private string _resourceName;

        /// <summary>
        /// Object형식의 상수 값 할당
        /// </summary>
        /// <param name="objs">Object형식의 상수 값</param>
        public ExtensionEnumAttribute(params object[] objs)
        {
            _objs = objs;
        }

        /// <summary>
        /// 리소스 파일 속성 값 할당
        /// </summary>
        /// <param name="resourceType">리소스 파일 타입</param>
        /// <param name="resourceName">리소스 파일 속성 이름</param>
        public ExtensionEnumAttribute(Type resourceType, string resourceName)
        {
            _resourceType = resourceType;
            _resourceName = resourceName;
        }

        public object[] Objs
        {
            get
            {
                return _objs;
            }
        }

        public Type ResourceType
        {
            get
            {
                return _resourceType;
            }
        }

        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
        }
    }

    public static class ExtensionEnumAttributeHelper
    {
        public static T GetEnumAttribute<T>(this Enum pEnum)
        {
            Type tType = pEnum.GetType();
            FieldInfo fi = tType.GetField(pEnum.ToString());
            object[] objCustomAttributes = fi.GetCustomAttributes(false);

            if (objCustomAttributes != null && objCustomAttributes.Length <= 0) return default(T);
            foreach (object Obj in objCustomAttributes)
            {
                if (Obj is T)
                {
                    return (T)Obj;
                }
            }
            return default(T);
        }

        public static T GetEnumAttributeValue<T>(this Enum pEnum, int valIdx)
        {
            Type tType = pEnum.GetType();
            FieldInfo fi = tType.GetField(pEnum.ToString());

            ExtensionEnumAttribute tObj = GetEnumAttribute<ExtensionEnumAttribute>(pEnum);
            if (tObj == null)
            {
                return default(T);
            }

            if (tObj.Objs != null && tObj.Objs.Length > 0)
            {
                if (tObj.Objs[valIdx] is IConvertible)
                {
                    return (T)Convert.ChangeType(tObj.Objs[valIdx], typeof(T));
                }
                else
                {
                    return (T)tObj.Objs[valIdx];
                }
            }
            else if (tObj.ResourceType != null &&
                string.IsNullOrWhiteSpace(tObj.ResourceName) == false)
            {
                PropertyInfo property =
                    tObj.ResourceType.GetProperty(
                        tObj.ResourceName,
                        BindingFlags.Public | BindingFlags.Static);
                if (property == null)
                {
                    throw new Exception("Resource Type Does Not Have Property");
                }
                if (property.PropertyType != typeof(string))
                {
                    throw new Exception("Resource Property is Not String Type");
                }
                return (T)property.GetValue(null, null);
            }
            else
            {
                return default(T);
            }
        }

        public static bool CheckValIdx(this Enum pEnum, int valIdx)
        {
            Type tType = pEnum.GetType();
            FieldInfo fi = tType.GetField(pEnum.ToString());

            ExtensionEnumAttribute tObj = GetEnumAttribute<ExtensionEnumAttribute>(pEnum);
            if (tObj == null)
            {
                return false;
            }

            if (tObj.Objs != null && tObj.Objs.Length > valIdx)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
