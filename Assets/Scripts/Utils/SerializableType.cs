using System;


[Serializable]
public class SerializableType
{
    public string assemblyQualifiedName;

    public SerializableType(Type type)
    {
        assemblyQualifiedName = type?.AssemblyQualifiedName;
    }

    public Type GetSerializedType()
    {
        return Type.GetType(assemblyQualifiedName);
    }
}