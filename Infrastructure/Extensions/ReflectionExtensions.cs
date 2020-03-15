using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QAP4.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// coppy object value
        /// </summary>
        /// <param name="obj1">object input</param>
        /// <param name="obj2">object output</param>
        /// <returns></returns>
        public static Object CopyObjectValue(Object obj1, Object obj2)
        {
            var propInfo = obj1.GetType().GetProperties();
            foreach (var item in propInfo)
            {
                obj2.GetType().GetProperty(item.Name).SetValue(obj2, item.GetValue(obj1, null), null);
            }
            return obj2;
        }
    }
}
