namespace System
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class Dump
    {
        public static List<(string Name, Type Type, object Value)> Get(object instance)
        {
            var result = new List<(string Name, Type Type, object Value)>();

            if (instance == null)
                return result;

            foreach (var property in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                object value;

                try
                {
                    value = property.GetValue(instance);
                }
                catch (Exception ex)
                {
                    value = $"<Error: {ex.Message}>";
                }

                result.Add((property.Name, property.PropertyType, value));
            }

            return result;
        }
    }
}
