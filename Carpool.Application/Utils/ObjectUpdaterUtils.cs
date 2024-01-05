using System;
using System.Reflection;

namespace Carpool.Application.Utils
{
    public static class ObjectUpdater
    {
        public static void UpdateObject<T1, T2>(T1 existingObject, T2 updatedObject)
        {
            var propertiesToUpdate = typeof(T2).GetProperties();

            foreach (var property in propertiesToUpdate)
            {
                var updatedValue = property.GetValue(updatedObject);
                if (updatedValue != null)
                {
                    var existingProperty = typeof(T1).GetProperty(property.Name);
                    existingProperty?.SetValue(existingObject, updatedValue);
                }
            }
        }

        public static T MapObject<T>(object dto)
        {
            if (dto == null)
            {
                return default;
            }

            Type type = typeof(T);
            T obj = (T)Activator.CreateInstance(type);

            PropertyInfo[] dtoProperties = dto.GetType().GetProperties();
            PropertyInfo[] objProperties = type.GetProperties();

            foreach (PropertyInfo objProp in objProperties)
            {
                PropertyInfo dtoProp = dtoProperties.FirstOrDefault(p => p.Name == objProp.Name);

                if (dtoProp != null)
                {
                    object value = dtoProp.GetValue(dto);
                    
                    if (objProp.Name.ToLower() != "password")
                    {
                        if (value != null && value is string v)
                        {
                            value = v.ToLower();
                        }
                    }

                    objProp.SetValue(obj, value);
                }
            }

            return obj;
        }
    }
}
