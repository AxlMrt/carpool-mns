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
    }
}