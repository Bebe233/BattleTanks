using System;
using System.Reflection;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace BEBE.Framework.Editor
{
    public class CreateEventCode
    {
        const string ClassHeaderPattern =
               @"namespace BEBE.Framework.Event
{
    public enum EventCode
    {
";
        static string writepath => Path.Combine(Application.dataPath, "Scripts/Framework/Event/");
        [MenuItem("Utils/Event/CreateEventCode")]
        public static void OnClickToCreateEventCode()
        {
            Debug.Log("OnClickToCreateEventCode");

            create("EVENT_");

        }

        private static void create(string prefix)
        {
            StringBuilder sb = new StringBuilder();
            //添加头
            sb.Append(ClassHeaderPattern);

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type[] types = currentAssembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                // Debug.Log($"type {t.Name}");
                MethodInfo[] methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                for (int j = 0; j < methods.Length; j++)
                {
                    MethodInfo method = methods[j];
                    string method_name = method.Name;
                    // Debug.Log($"type {t.Name} method {method_name}");
                    if (method_name.StartsWith(prefix))
                    {
                        string event_type = method_name.Substring(prefix.Length);
                        sb.Append(event_type + ",\n");
                    }
                }
            }

            sb.Append("ENUM_COUNT\n }\n }");
            string url = writepath + "EventCode.cs";
            if (!Directory.Exists(writepath))
            {
                Directory.CreateDirectory(writepath);
            }
            if (File.Exists(url))
            {
                Debug.LogError("文件已经存在，覆盖原文件！");
            }
            File.WriteAllText(url, sb.ToString());
            Debug.LogWarning("生成完毕！");
        }
    }
}