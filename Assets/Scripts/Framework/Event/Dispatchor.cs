using System;
using System.Collections.Generic;
using System.Reflection;
using BEBE.Engine.Logging;

namespace BEBE.Framework.Event
{
    delegate void EventHandler(object param);
    //消息分发管理类
    public class Dispatchor
    {
        static Dictionary<EventCode, Dictionary<object, EventHandler>> eCode2handler = new Dictionary<EventCode, Dictionary<object, EventHandler>>();

        public static void Register(object sender, string prefix)
        {
            Type t = sender.GetType();
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
                            Debug.LogWarning($"CreateDelegate --> {t.Name} :: {eCode.ToString()} :: {method_name}");
                            EventHandler handler = Delegate.CreateDelegate(typeof(EventHandler), sender, method) as EventHandler;
                            //TODO
                            if (eCode2handler.ContainsKey(eCode))
                            {
                                if (eCode2handler.TryGetValue(eCode, out Dictionary<object, EventHandler> dict))
                                {
                                    if (!dict.ContainsKey(sender))
                                    {
                                        dict.Add(sender, handler);
                                    }
                                    else
                                    {
                                        dict[sender] = handler;
                                    }
                                }
                                else
                                {
                                    var dict_new = new Dictionary<object, EventHandler>();
                                    dict_new.Add(sender, handler);
                                    eCode2handler[eCode] = dict_new;
                                }
                            }
                            else
                            {
                                var dict = new Dictionary<object, EventHandler>();
                                dict.Add(sender, handler);
                                eCode2handler.Add(eCode, dict);
                            }

                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
            }
        }

        public static void Dispatch(object sender, EventCode eCode, object param)
        {
            //TODO
            if (eCode2handler.TryGetValue(eCode, out Dictionary<object, EventHandler> dict))
            {
                if (sender != null)
                {
                    if (dict.TryGetValue(sender, out EventHandler handler))
                    {
                        handler?.Invoke(param);
                    }
                }
                else
                {
                    //Broadcast
                    foreach (var handler in dict.Values)
                    {
                        handler?.Invoke(param);
                    }
                }
            }
        }

        public static void Dispatch(EventCode eCode, object param)
        {
            Dispatch(null, eCode, param);
        }

        public static void Dispatch(EventCode eCode)
        {
            Dispatch(eCode, null);
        }
    }
}

