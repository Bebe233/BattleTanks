using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using BEBE.Framework.Event;
namespace BEBE.Framework.Managers
{
    delegate void EventHandler(object param);
    //消息分发管理类
    public class DispatchMgr : IMgr
    {
        static Dictionary<EventCode, EventHandler> all_handlers = new Dictionary<EventCode, EventHandler>();

        public override void Awake()
        {
            //通过反射注册所有 EVENT_ 方法,在EDITOR模式下自动生成 ENUM 类消息键
            //register_all_events("EVENT_");
        }

        private void register_all_events(string prefix)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type[] types = currentAssembly.GetTypes();
            Debug.Log($"Assembly types count {types.Length}");
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                //Debug.Log($"type {t.Name}");
                MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                for (int j = 0; j < methods.Length; j++)
                {
                    MethodInfo method = methods[j];
                    string method_name = method.Name;
                    if (method_name.StartsWith(prefix))
                    {
                        string event_type = method_name.Substring(prefix.Length);
                        if (Enum.TryParse(event_type, out EventCode eCode))
                        {
                            try
                            {
                                Debug.LogWarning($"CreateDelegate {eCode.ToString()} {method_name}");
                                EventHandler handler = Delegate.CreateDelegate(typeof(EventHandler), method) as EventHandler;
                                if (!all_handlers.ContainsKey(eCode))
                                    all_handlers.Add(eCode, handler);
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    }
                }

            }
        }

        public void Register(object obj, string prefix)
        {
            Type t = obj.GetType();
            //Debug.Log($"type {t.Name}");
            MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            for (int j = 0; j < methods.Length; j++)
            {
                MethodInfo method = methods[j];
                string method_name = method.Name;
                if (method_name.StartsWith(prefix))
                {
                    string event_type = method_name.Substring(prefix.Length);
                    if (Enum.TryParse(event_type, out EventCode eCode))
                    {
                        try
                        {
                            // Debug.LogWarning($"CreateDelegate {eCode.ToString()} {method_name}");
                            EventHandler handler = Delegate.CreateDelegate(typeof(EventHandler), obj, method) as EventHandler;
                            if (!all_handlers.ContainsKey(eCode))
                                all_handlers.Add(eCode, handler);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
            }
        }

        public static void Dispatch(EventCode eCode, object param)
        {
            if (all_handlers.TryGetValue(eCode, out EventHandler handler))
            {
                handler.Invoke(param);
            }
            else
            {
                Debug.LogError("请求未注册！");
            }
        }
    }
}

