using System;
using System.ComponentModel;

namespace ICustomTypeDescriptorEx;

public class CustomPropertyDescriptor<T> : PropertyDescriptor
{
    #region Member Fields
    private Type _propertyType;
    private Type _componentType;
    private T? _propertyValue;
    #endregion

    #region Constructor
    public CustomPropertyDescriptor(string propertyName, Type componentType)
        : base(propertyName, new Attribute[] { })
    {
        _propertyType = typeof(T?);
        _componentType = componentType;
    }
    #endregion

    #region PropertyDescriptor Implementation Overriden Methods
    public override bool CanResetValue(object component) { return true; }
    public override Type ComponentType
    {
        get
        {
            return _componentType;
        }
    }

    public override object? GetValue(object? component)
    {
        return _propertyValue;
    }

    public override bool IsReadOnly { get { return false; } }
    public override Type PropertyType { get { return _propertyType; } }
    public override void ResetValue(object component)
    {
        SetValue(component, default(T));
    }
    public override void SetValue(object? component, object? value)
    {
        if (value != null && value.GetType().IsAssignableFrom(_propertyType) == false)
        {
            throw new System.Exception("잘못된 타입입니다.");
        }

        _propertyValue = (T)value!;
    }

    public override bool ShouldSerializeValue(object component) { return true; }
    #endregion
}