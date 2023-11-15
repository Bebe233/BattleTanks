using BEBE.Framework.Attibute;
using BEBE.Framework.UI;
using UnityEngine;
using UnityEngine.UI;

[PrefabLocation("ui/GameStart")]
public class GameStartUIView : UIView
{

    [Location("Canvas/GameStart/Bottom")]
    public GameObject Bottom;

    [Location("Canvas/GameStart/ToggleGroup")]
    public GameObject ToggleGroup;

    [SerializeField]
    protected Toggle[] toggles;

    protected override void Awake()
    {
        base.Awake();

        int count = ToggleGroup.transform.childCount;
        toggles = new Toggle[count];
        for (int i = 0; i < count; i++)
        {
            toggles[i] = ToggleGroup.transform.GetChild(i).GetComponent<Toggle>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //按回车健,进入游戏
            int index = get_toggle_on_index();
            Game.Instance.LoadSceneLevel(index);
        }
    }

    private int get_toggle_on_index()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn) return i;
        }
        return 0;
    }
}
