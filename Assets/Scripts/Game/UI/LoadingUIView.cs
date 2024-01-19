using BEBE.Framework.Attibute;
using BEBE.Framework.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using BEBE.Engine.Math;

[PrefabLocation("ui/Loading")]
public class LoadingUIView : UIView
{
    [Location("Canvas/Loading/Grid")]
    public GameObject Grid;
    [SerializeField]
    private Slider[] progress_units = new Slider[10];
    [SerializeField]
    private TMP_Text[] text_player_ids = new TMP_Text[10];

    [Location("Canvas/Loading/MainProgress")]
    public GameObject MainProgress;
    [SerializeField]
    private Slider main_progress;
    [SerializeField]
    private TMP_Text text_progress;
    protected override void Awake()
    {
        base.Awake();
        main_progress = MainProgress.GetComponent<Slider>();
        text_progress = main_progress.transform.Find("Text (TMP)").GetComponent<TMP_Text>();
        //units
        int amount = Grid.transform.childCount;
        for (int i = 0; i < amount; i++)
        {
            progress_units[i] = Grid.transform.GetChild(i).GetComponent<Slider>();
            text_player_ids[i] = progress_units[i].transform.Find("Text (TMP)").GetComponent<TMP_Text>();
            progress_units[i].gameObject.SetActive(false);
        }
    }

    internal void InActiveAllUnits()
    {
        for (int i = 0; i < progress_units.Length; i++)
        {
            progress_units[i].gameObject.SetActive(false);
        }
    }

    internal void SetUnit(byte i, string player_id, LFloat load_progress)
    {
        progress_units[i].gameObject.SetActive(true);
        progress_units[i].value = load_progress;
        text_player_ids[i].text = player_id;
    }

    internal void SetMainProgress(LFloat progress)
    {
        main_progress.value = progress;
    }
}

