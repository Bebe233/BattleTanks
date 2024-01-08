using BEBE.Engine.Service.Net;
using BEBE.Framework.Attibute;
using BEBE.Framework.Event;
using BEBE.Framework.Managers;
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

    [Location("Canvas/GameStart/Button_Create")]
    public GameObject Button_Create;
    protected override void Awake()
    {
        base.Awake();
        //register
        Button_Join.GetComponent<Button>().onClick.AddListener(event_on_button_join_click);
        Button_Create.GetComponent<Button>().onClick.AddListener(event_on_button_create_click);
    }

    private string id;
    private bool check_id()
    {
        //得到inputfield的内容
        id = InputField_Name.GetComponent<TMPro.TMP_InputField>().text;
        if (string.IsNullOrEmpty(id))
        {
            BEBE.Engine.Logging.Debug.LogWarning("name is empty!");
            MgrsContainer.GetMgr<UIMgr>().LoadCanvasUI<AlertUIView>().SetText("ID CAN'T BE NULL");
            return false;
        }
        else return true;
    }

    private void event_on_button_create_click()
    {
        if (check_id())
        {
            MgrsContainer.GetMgr<CommonStatusMgr>().PlayerId = id;
            ByteBuf buffer = new ByteBuf();
            buffer.WriteString(id);
            //通知服务器创建房间
            Dispatchor.Dispatch(EventCode.CALL_CREATE_ROOM_REQUEST_METHOD, buffer.Data);
        }
    }

    protected void event_on_button_join_click()
    {
        if (check_id())
        {
            MgrsContainer.GetMgr<CommonStatusMgr>().PlayerId = id;
            ByteBuf buffer = new ByteBuf();
            buffer.WriteString(id);
            //call join in method of client
            Dispatchor.Dispatch(EventCode.CALL_JOIN_IN_REQUEST_METHOD, buffer.Data);
        }
    }
}
