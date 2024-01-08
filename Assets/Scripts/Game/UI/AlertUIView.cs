using System;
using BEBE.Framework.Attibute;
using BEBE.Framework.Managers;
using BEBE.Framework.UI;
using UnityEngine;
using UnityEngine.UI;

[PrefabLocation("ui/Alert")]
public class AlertUIView : UIView
{
    [Location("Canvas/Alert/Text")]
    public GameObject Text;
    private TMPro.TMP_Text text;

    [Location("Canvas/Alert/Button_close")]
    public GameObject Button_close;
    protected override void Awake()
    {
        base.Awake();
        _canvas_group = GetComponent<CanvasGroup>();
        text = Text.GetComponent<TMPro.TMP_Text>();
        Button_close.GetComponent<Button>().onClick.AddListener(event_on_click);
    }

    private void event_on_click()
    {
        MgrsContainer.GetMgr<UIMgr>().UnloadCanvasUI<AlertUIView>();
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
