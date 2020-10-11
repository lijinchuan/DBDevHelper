using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Linq.Expressions;

namespace Biz.Common
{
    public static class ReflectionHelper
    {
        //public static object Eval(object o, string itemName)
        //{
        //    if (o == null)
        //        return null;
        //    if (string.IsNullOrWhiteSpace(itemName))
        //        return null;
        //    var pros = o.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //    foreach (PropertyInfo p in pros)
        //    {
        //        if (p.Name.Equals(itemName))
        //        {
        //            return p.GetValue(o, null);
        //        }
        //    }
        //    var fields = o.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        //    foreach (FieldInfo f in fields)
        //    {
        //        if (f.Name.Equals(itemName))
        //        {
        //            return f.GetValue(o);
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 反射取值
        /// </summary>
        /// <param name="o"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object Eval(this object o, string property)
        {
            try
            {
                if (o == null)
                    return null;

                var tp = o.GetType();
                var propertyInfo = tp.GetProperty(property);

                if (propertyInfo == null)
                {
                    throw new Exception(string.Format("类型\"{0}\"不在名为\"{1}\"的属性", tp.FullName, property));
                }

                ParameterExpression instance = Expression.Parameter(
                typeof(object), "instance");
                Expression instanceCast = Expression.Convert(
                instance, propertyInfo.ReflectedType);
                Expression propertyAccess = Expression.Property(
                instanceCast, propertyInfo);
                UnaryExpression castPropertyValue = Expression.Convert(
                propertyAccess, typeof(object));

                Expression<Func<object, object>> lamexpress =
                Expression.Lambda<Func<object, object>>(
                    castPropertyValue, instance);

                return lamexpress.Compile()(o);
            }
            catch
            {
                return null;
            }
        }
    }
}
