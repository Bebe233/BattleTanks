using BEBE.Framework.Attibute;
using BEBE.Framework.UI;
using UnityEngine;
using UnityEngine.UI;
[PrefabLocation("ui/GameStart")]
public class GameStartUIView : UIView
{

    [Location("Canvas/GameStart/Text (TMP)_Name")]
    public GameObject Text_Name;

    [Location("Canvas/GameStart/InputField (TMP)_Name")]
    public GameObject InputField_Name;

    [Location("Canvas/GameStart/Button_Join")]
    public GameObject Button_Join;

    protected override void Awake()
    {
        base.Awake();
        //register
        Button_Join.GetComponent<Button>().onClick.AddListener(event_on_button_click);
    }

    protected void event_on_button_click()
    {
        //得到inputfield的内容
        var name = InputField_Name.GetComponent<TMPro.TMP_InputField>().text;
        if (string.IsNullOrEmpty(name))
        {
            BEBE.Engine.Logging.Debug.LogWarning("name is empty!");
            return;
        }
        BEBE.Engine.Logging.Debug.Log($"event_on_button_click {name}");
        //send order to server
        BEBE.Engine.Managers.Dispatchor.Dispatch(null, BEBE.Engine.Event.EventCode.SEND_JOIN_REQUEST, null);
    }
}
