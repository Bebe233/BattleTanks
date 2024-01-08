using BEBE.Framework.Attibute;
using BEBE.Framework.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BEBE.Framework.Managers;
using BEBE.Framework.Event;

[PrefabLocation("ui/Room")]
public class RoomUIView : UIView
{


    [Location("Canvas/Room/Text_title")]
    public GameObject Text_title;

    [Location("Canvas/Room/Text_id")]
    public GameObject Text_id;

    [Location("Canvas/Room/Container")]
    public GameObject Container;

    [Location("Canvas/Room/Container/Text_players")]
    public GameObject Text_players;

    [Location("Canvas/Room/Container/Grid")]
    public GameObject Grid;
    [SerializeField]
    private GameObject[] units = new GameObject[10];
    [SerializeField]
    private TMP_Text[] unit_player_id = new TMP_Text[10];
    [SerializeField]
    private GameObject[] unit_host_icon = new GameObject[10];
    [SerializeField]
    private GameObject[] unit_ready_icon = new GameObject[10];

    [Location("Canvas/Room/Button_play")]
    public GameObject Button_play;

    [Location("Canvas/Room/Button_ready")]
    public GameObject Button_ready;

    [Location("Canvas/Room/Button_cancel")]
    public GameObject Button_cancel;

    [Location("Canvas/Room/Button_exit")]
    public GameObject Button_exit;
    protected override void Awake()
    {
        base.Awake();
        int length = Grid.transform.childCount;
        for (int i = 0; i < length; i++)
        {
            units[i] = Grid.transform.GetChild(i).gameObject;
            unit_player_id[i] = units[i].transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            unit_host_icon[i] = units[i].transform.Find("Host").gameObject;
            unit_ready_icon[i] = units[i].transform.Find("Ready").gameObject;
            units[i].SetActive(false);
        }
        Button_play.GetComponent<Button>().onClick.AddListener(on_play_clicked);
        Button_exit.GetComponent<Button>().onClick.AddListener(on_exit_clicked);
        Button_ready.GetComponent<Button>().onClick.AddListener(on_ready_clicked);
        Button_cancel.GetComponent<Button>().onClick.AddListener(on_cancel_clicked);
    }

    protected UIMgr ui = MgrsContainer.GetMgr<UIMgr>();
    protected void on_play_clicked()
    {
        Dispatchor.Dispatch(EventCode.CALL_PLAY_METHOD);
    }

    protected void on_ready_clicked()
    {
        Dispatchor.Dispatch(EventCode.CALL_GET_READY_METHOD);
    }

    protected void on_cancel_clicked()
    {
        Dispatchor.Dispatch(EventCode.CALL_CANCEL_READY_METHOD);
    }

    protected void on_exit_clicked()
    {
        //Call exit room method TODO
        Dispatchor.Dispatch(EventCode.CALL_EXIT_ROOM_METHOD);
    }

    public void SetRoomId(int id)
    {
        Text_id.GetComponent<TMP_Text>().text = id.ToString();
    }

    public void InActiveAllUnits()
    {
        for (int i = 0; i < 10; i++)
        {
            units[i].SetActive(false);
        }
    }

    public void SetUnit(int index, string text, bool isHost, bool isReady)
    {
        units[index].SetActive(true);
        unit_player_id[index].text = text;
        unit_host_icon[index].SetActive(isHost);
        if (isHost) unit_ready_icon[index].SetActive(false); //如果是Host，不显示准备图标
        else unit_ready_icon[index].SetActive(isReady);
    }

    public void SetPlayers(byte count, byte capacity)
    {
        Text_players.GetComponent<TMP_Text>().text = $"{count}/{capacity}";
    }
}
