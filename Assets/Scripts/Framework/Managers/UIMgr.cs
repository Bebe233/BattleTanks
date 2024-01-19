using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using BEBE.Framework.UI;
using BEBE.Framework.Attibute;

namespace BEBE.Framework.Managers
{
    //UI管理类
    public class UIMgr : IMgr
    {
        protected Stack<UIView> uiStack = new Stack<UIView>();
        public T LoadCanvasUI<T>() where T : UIView
        {
            //将栈顶元素设为隐藏或不可互动
            UIView view;
            if (uiStack.TryPeek(out view))
            {
                if (view is T) return view as T;
                else
                    view.SetInteractable(false);
            }
            Type t = typeof(T);
            var prefabLocationAttr = t.GetCustomAttribute<PrefabLocationAttribute>();
            if (prefabLocationAttr == null) return default(T);
#if UNITY_EDITOR
            GameObject temp = MgrsContainer.GetMgr<SrcMgr>().GetPrefabAsset(prefabLocationAttr.Path);
#elif UNITY_STANDALONE
            GameObject temp = MgrsContainer.GetMgr<SrcMgr>().GetPrefabAsset(prefabLocationAttr.Path, "ui");
#endif
            var obj = MonoBehaviour.Instantiate(temp, GameObject.Find("Canvas").transform);
            obj.name = temp.name;
            var cmpt = obj.AddComponent<T>();
            //进栈
            uiStack.Push(cmpt);
            return cmpt;
        }

        public void UnloadCanvasUI<T>() where T : UIView
        {
            UIView view;
            if (uiStack.TryPeek(out view))
            {
                if (view is T)
                {
                    uiStack.Pop();
                    GameObject.Destroy(view.gameObject);
                }
            }
            if (uiStack.TryPeek(out view))
            {
                view.SetInteractable(true);
            }

        }

        public void UnloadTopLayer()
        {
            UIView view;
            if (uiStack.TryPeek(out view))
            {
                uiStack.Pop();
                GameObject.Destroy(view.gameObject);
            }
            if (uiStack.TryPeek(out view))
            {
                view.SetInteractable(true);
            }
        }

        public void UnloadAll()
        {
            UIView view;
            while (uiStack.TryPeek(out view))
            {
                MonoBehaviour.Destroy(uiStack.Pop().gameObject);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            MgrsContainer.GetMgr<SrcMgr>().UnloadAssetBundle("ui");
        }

    }
}
