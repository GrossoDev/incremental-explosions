using System;
using UnityEngine;

public class SubclassOfAttribute : PropertyAttribute
{
    public Type BaseType { get; }

    public SubclassOfAttribute(Type baseType)
    {
        BaseType = baseType;
    }
}