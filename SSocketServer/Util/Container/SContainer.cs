using SSocketServer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSocketServer.Util.Container
{
    public static class SContainer
    {
        static Dictionary<Type, object> InstanceDict = new Dictionary<Type, object>();

        static void CreateInstance(Type key, Type valueType) => InstanceDict.Add(key, Activator.CreateInstance(valueType));

        static SContainer()
        {
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes()) { AutoRegister(type); }
            // 进行依赖注入
            DependencyInjection();
        }
        /// <summary>
        /// 自动注册
        /// </summary>
        /// <param name="type"></param>
        static void AutoRegister(Type type)
        {
            var attributes = type.GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case ServiceAttribute service:
                    case DaoAttribute dao:
                        // 这里推荐使用接口,如果有需要的话，也可以按照实现类的type进行注册
                        CreateInstance(type.GetInterfaces()[0], type);
                        return;
                    case ControllerAttribute controller:
                        CreateInstance(type, type);
                        return;
                }
            }
        }
        /// <summary>
        /// 依赖注入
        /// </summary>
        static void DependencyInjection()
        {
            foreach (var instance in InstanceDict)
            {
                // 私有的实例属性, 因为依赖往往只应该内部依赖而不提供给外界
                var properties = instance.Key.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes(typeof(AutowiredAttribute), false);
                    if (attributes.Any())
                    {
                        Console.WriteLine(property.PropertyType);
                        property.SetValue(instance.Value, Resolve(property.PropertyType));
                    }
                }
            }
        }

        /// <summary>
        /// 根据type获取一个实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static object Resolve(Type type)
        {
            if (InstanceDict.TryGetValue(type, out var obj)) return obj;
            throw new NullReferenceException($"没有找到{type.Name}的实现,请检查是否忘记添加特性");
        }

        public static T Resolve<T>() where T : class
        {
            var type = typeof(T);
            if (InstanceDict.TryGetValue(type, out var obj)) return obj as T;
            throw new NullReferenceException($"没有找到{type.Name}的实现,请检查是否忘记添加特性");
        }

        /// <summary>
        /// 此方法需要消耗一定时间，完全可以单独使用若干个字典存放相应类型的实例
        /// 但是为了扩展性，这里决定这样写了
        /// 不应该在初始化以外的地方调用此方法，如有特殊需要，应该缓存结果。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<(Type type, T instance)> Resolves<T>() where T : class
            => InstanceDict.Select(x => (x.Key, x.Value as T))
                            .Where(x => x.Item2 != null);



        public static void Test()
        {

        }
    }
}
