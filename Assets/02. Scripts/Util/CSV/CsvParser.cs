using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class CsvParser
{
    public static List<T> Parse<T>(TextAsset csvFile) where T : new()
    {
        var result = new List<T>();
        string[] lines = csvFile.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2) return result;

        string[] headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            T instance = new T();
            foreach (var member in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = member.GetCustomAttribute<CsvColumnAttribute>();
                if (attr == null) continue;

                int index = Array.IndexOf(headers, attr.ColumnName);
                if (index == -1 || index >= values.Length) continue;

                string value = values[index];

                try
                {
                    if (member is FieldInfo field)
                    {
                        object converted = ConvertValue(value, field.FieldType);
                        field.SetValue(instance, converted);
                    }
                    else if (member is PropertyInfo property && property.CanWrite)
                    {
                        object converted = ConvertValue(value, property.PropertyType);
                        property.SetValue(instance, converted);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[CsvParser] Failed to parse {attr.ColumnName}: {ex.Message}");
                }
            }

            result.Add(instance);
        }

        return result;
    }

    private static object ConvertValue(string value, Type targetType)
    {
        if (targetType == typeof(int))
            return int.Parse(value);
        if (targetType == typeof(float))
            return float.Parse(value);
        if (targetType == typeof(bool))
            return bool.Parse(value);
        if (targetType == typeof(string))
            return value;
        if (targetType.IsEnum)
            return Enum.Parse(targetType, value);
        throw new NotSupportedException($"Type {targetType} is not supported");
    }
}