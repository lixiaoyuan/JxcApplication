using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Utilities
{
   public static class Utilities
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> inputEnumerable)
        {
            try
            {
                return new ObservableCollection<T>(inputEnumerable);
            }
            catch (Exception)
            {
                return null;
            }
        }

       public static string GetLocalIpAddress()
       {
           var address = System.Net.Dns.GetHostAddresses(Dns.GetHostName());
           foreach (IPAddress ipAddress in address)
           {
               if (ipAddress.ToString().Contains(".")&&ipAddress.AddressFamily==AddressFamily.InterNetwork)
               {
                   return ipAddress.ToString();
               }
           }
           return "127.0.0.1";
       }

       public static T DynamicConvert<T, TSource>(TSource source)
       {
            Type toType = typeof(T);
            Type sourceType = typeof(TSource);
            T returnT = default(T);
            foreach (PropertyInfo property in toType.GetProperties())
            {
                var sourceProperty = sourceType.GetProperty(property.Name, property.PropertyType);
                if (sourceProperty == null)
                {
                    continue;
                }
                if (returnT == null)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    returnT = (T)toType.GetConstructor(Type.EmptyTypes).Invoke(null);
                }
                property.SetValue(returnT,sourceProperty.GetValue(source,null),null);
            }
            GC.Collect();
           return returnT;
       }
    }
}
