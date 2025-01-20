using System;
using System.Reflection;

namespace YSharp.Utility;

/*
To expose Variables or methods use the Expose attribute and they need to be public
you need to define itfor your library yourself like this:

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
public class ExposeAttribute : Attribute { }

To compile a class to the required dll file run: dotnet build -o ./

the .csproj file should look similar to this:

<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
</Project>

*/

public static class ImportUtil
{
    private const string ExposeAttributeName = "Expose";

    public static void Load(string filePath)
    {
        if (!TryGetAssamblyFromPath(filePath, out Assembly assembly))
        {
            return;
        }

        // Get the first class
        Type[] classType = assembly
            .GetTypes()
            .Where(cls => HasAttribute(cls, ExposeAttributeName))
            .ToArray();

        List<ExposedClassData> data = [];
        foreach (Type type in classType){
            data.Add(GetExposedClassData(type));
        }
    }

    private static ExposedClassData GetExposedClassData(Type classType)
    {
        // Create an instance of the class
        object? classInstance = Activator.CreateInstance(classType);

        // List public methods
        MethodInfo[] publicMethods = classType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(meth => HasAttribute(meth, ExposeAttributeName))
            .ToArray();

        // List public variables (fields)
        FieldInfo[] publicFields = classType
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(field => HasAttribute(field, ExposeAttributeName))
            .ToArray();

        // List public properties
        PropertyInfo[] publicProperties = classType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => HasAttribute(prop, ExposeAttributeName) && prop.CanRead)
            .ToArray();

        return new ExposedClassData(publicMethods, publicFields, publicProperties, classInstance);
    }

    private static bool TryGetAssamblyFromPath(string filePath, out Assembly assembly)
    {
        assembly = null;

        if (!File.Exists(filePath) || !filePath.EndsWith(".dll"))
        {
            Console.WriteLine("File not found: " + filePath);
            return false;
        }

        try
        {
            // Load the assembly
            assembly = Assembly.LoadFrom(filePath);
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
        }
    }

    private static bool HasAttribute(ICustomAttributeProvider provider, string attribute)
    {
        return provider.GetCustomAttributes(false).Any(attr => attr.GetType().Name == attribute);
    }
}


public class ExposedClassData(MethodInfo[] Methods, FieldInfo[] Fields, PropertyInfo[] Properties, object? instance = null)
{
    public readonly MethodInfo[] methods = Methods;
    public readonly FieldInfo[] fields = Fields;
    public readonly PropertyInfo[] properties = Properties;
    public readonly object? instance = instance;
}
