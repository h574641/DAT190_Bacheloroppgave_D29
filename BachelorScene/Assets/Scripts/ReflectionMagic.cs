using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


public class ReflectionMagic
{
    public static ConstructorInfo GetDefaultConstructor(Type type)
    {
        if (type == null) throw new ArgumentNullException("type");
        var constructor = type.GetConstructor(Type.EmptyTypes);
        if (constructor == null)
        {
            var ctors =
                from ctor in type.GetConstructors()
                let prms = ctor.GetParameters()
                where prms.All(p => p.IsOptional)
                orderby prms.Length
                select ctor;
            constructor = ctors.FirstOrDefault();
        }

        return constructor;
    }

    public static List<ConstructorInfo> GetAllTypeDefaultConstructors(Type t)
    {
        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => t.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(x => GetDefaultConstructor(x)).ToList();
    }
}
