using System;
using System.Linq;
using System.Reflection;

public static class ReflectionUtil
{
    public static Type[] FindAllSubslasses<T>()
    {
        Type baseType = typeof(T);
        Assembly assembly = Assembly.GetAssembly(baseType);

        Type[] types = assembly.GetTypes();
        Type[] subclasses = types.Where(type => type.IsSubclassOf(baseType) && !type.IsAbstract).ToArray();

        return subclasses;
    }
}