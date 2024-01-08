using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;

namespace BEBE.Framework.Editor
{
    public class CreateUIClass
    {
        [MenuItem("Utils/UI/CreateUIClass")]
        public static void OnClickToCreateUIClass()
        {
            Debug.Log("OnClickToCreateUIClass");
            GameObject obj = Selection.activeGameObject;
            if (obj == null) return;
            Debug.Log($"Selecting name is {obj.name}");
            //判断选择的对象是不是属于 Layer UI 
            if (!(obj.layer == 5)) return;
            Debug.Log($"Selecting obj is in UI layer");
            string classname = String.Concat(obj.name, "UIView");
            CreateClassInstance(classname, obj.transform);
        }

        const string ClassHeaderPattern =
        @"using BEBE.Framework.Attibute;
using BEBE.Framework.UI;
using UnityEngine;
using UnityEngine.UI;
[PrefabLocation(""ui/"")]
public class {0} : UIView {{
    
";
        const string ClassTrailerPattern = "}\n";
        static string writepath => Path.Combine(Application.dataPath, "Scripts/Game/UI/");
        private static void CreateClassInstance(string className, Transform parent)
        {
            StringBuilder sb = new StringBuilder();
            Debug.Log(className);
            //添加头
            sb.Append(String.Format(ClassHeaderPattern, className));
            //添加成员
            append_children_component(parent, sb);
            //添加尾
            sb.Append(ClassTrailerPattern);
            string url = writepath + className + ".cs";
            if (!Directory.Exists(writepath))
            {
                Directory.CreateDirectory(writepath);
            }
            if (File.Exists(url))
            {
                Debug.LogError("文件已经存在，请删除后再试！");
                return;
            }
            File.WriteAllText(url, sb.ToString());
        }

        const string memberPattern = @"
[Location(""{0}"")]
public GameObject {1};
";
        private static void append_children_component(Transform parent, StringBuilder sb)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = parent.GetChild(i);
                string hierarchy_location = get_hierarchy_location(child);
                sb.Append(String.Format(memberPattern, hierarchy_location, child.name));
            }
        }

        private static string get_hierarchy_location(Transform child)
        {
            string location = child.name;
            Transform current = child;
            while (current.parent != null)
            {
                location = string.Concat(current.parent.name, "/", location);
                current = current.parent;
            }
            return location;
        }
    }
}
