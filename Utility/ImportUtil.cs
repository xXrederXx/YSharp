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
        if(!TryGetAssamblyFromPath(filePath, out Assembly assembly))
        {
            return;
        }

        // Get the first class
        Type classType = assembly.GetTypes()[0];
        System.Console.WriteLine(classType);

        // Create an instance of the class
        object? classInstance = Activator.CreateInstance(classType);
        if (classInstance == null)
        {
            Console.WriteLine("Created instance is null");
            return;
        }

        // List public methods
        MethodInfo[] publicMethods = classType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        Console.WriteLine("Public Methods:");
        foreach (var method in publicMethods)
        {
            if (HasAttribute(method, ExposeAttributeName))
            {
                Console.WriteLine($"- {method.ReturnType} {method.Name}({string.Join(", ", method.GetParameters().Select(x => $"{x.ParameterType} {x.Name}"))})");
            }
        }

        // List public variables (fields)
        FieldInfo[] publicFields = classType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        Console.WriteLine("Public Fields:");
        foreach (var field in publicFields)
        {
            if (HasAttribute(field, ExposeAttributeName))
            {
                Console.WriteLine($"- {field.FieldType} {field.Name}: {field.GetValue(classInstance)}");
            }
        }

        // List public properties
        PropertyInfo[] publicProperties = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        Console.WriteLine("Public Properties:");
        foreach (var property in publicProperties)
        {
            if (property.CanRead && HasAttribute(property, ExposeAttributeName))
            {
                Console.WriteLine($"- {property.PropertyType} {property.Name}: {property.GetValue(classInstance)}");
            }
        }
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

    private static bool HasAttribute(ICustomAttributeProvider provider, string attribute){
        return provider.GetCustomAttributes(false).Any(attr => attr.GetType().Name == attribute);
    }
}
