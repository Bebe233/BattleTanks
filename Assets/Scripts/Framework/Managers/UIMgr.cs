using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BEBE.Framework.UI;
using BEBE.Framework.Attibute;

namespace BEBE.Framework.Managers
{   
    //UI管理类
    public class UIMgr : IMgr
    {
        protected Stack<UIView> uiStack = new Stack<UIView>();
        public void LoadCanvasUI<T>() where T : UIView
        {
            //将栈顶元素设为隐藏或不可互动
            UIView view;
            if (uiStack.TryPeek(out view))
            {
                view.gameObject.SetActive(false);
            }
            Type t = typeof(T);
            var prefabLocationAttr = t.GetCustomAttribute<PrefabLocationAttribute>();
            if (prefabLocationAttr == null) return;
            GameObject temp = GameLaucher.Instance.Container.GetMgr<SrcMgr>().GetPrefabAsset(prefabLocationAttr.Path);
            var gamestart = MonoBehaviour.Instantiate(temp, GameObject.Find("Canvas").transform);
            gamestart.name = temp.name;
            var cmpt = gamestart.AddComponent<GameStartUIView>();
            //进栈
            uiStack.Push(cmpt);
        }

        public void UnloadCanvasUI<T>() where T : UIView
        {
            //判断栈内是否存在指定类型的元素
            if (uiStack.Any(x => x.GetType() is T))
            {
                UIView view;
                while (uiStack.TryPop(out view))
                {
                    if (view is T)
                    {
                        MonoBehaviour.Destroy(view.gameObject);
                        break;
                    }
                    else
                    {
                        MonoBehaviour.Destroy(view.gameObject);
                    }
                }
                if (uiStack.TryPeek(out view))
                {
                    view.gameObject.SetActive(true);
                }
            }
        }

        public void ClearStack()
        {
            UIView view;
            while (uiStack.TryPeek(out view))
            {
                MonoBehaviour.Destroy(uiStack.Pop().gameObject);
            }
        }
    }
}
