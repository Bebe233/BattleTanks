using System;
using System.Reflection;
using BEBE.Framework.Attibute;
using UnityEngine;

namespace BEBE.Framework.UI
{

    [RequireComponent(typeof(RectTransform))]
    public class UIView : MonoBehaviour
    {
        protected RectTransform m_RectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }
                return m_RectTransform;
            }
        }

        protected CanvasGroup _canvas_group;

        protected virtual void Awake()
        {
            //利用反射，查找标记有location特性的子类成员变量的路径并赋值
            Type type = this.GetType();

            var members = type.GetMembers();
            for (int i = 0; i < members.Length; i++)
            {
                var loc = members[i].GetCustomAttribute<LocationAttribute>();
                if (loc == null) continue;
                type.GetField(members[i].Name).SetValue(this, loc.Locate());
            }
            //UI交互管理
            _canvas_group = gameObject.AddComponent<CanvasGroup>(); 
        }

        public void SetInteractable(bool isOn)
        {
            _canvas_group.interactable = isOn;
        }

    }

}