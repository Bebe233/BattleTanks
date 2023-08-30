using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;

[EditorTool("ChunkEditor")]
public class ChunkEditorTool : EditorTool
{
    protected RectTransform parent, initTemp;
    protected GameObject prefabBlock;
    public GameObject PrefabBlock
    {
        get { return prefabBlock; }
        set
        {
            GameObject temp = value;
            if (temp != prefabBlock)
            {
                //不是同一个, 获取预制件的尺寸
                prefabBlock = temp;
                chunk_width = prefabBlock.GetComponent<RectTransform>().sizeDelta.x;
                chunk_height = prefabBlock.GetComponent<RectTransform>().sizeDelta.y;
            }
        }
    }
    protected float chunk_width, chunk_height;
    protected bool draw = false;
    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView sceneView)) return;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Handles.BeginGUI();
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;
        style.padding = new RectOffset(50, 0, 0, 0);
        using (new GUILayout.HorizontalScope(style, GUILayout.MaxWidth(400)))
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                //获取Chunk预制体
                PrefabBlock = (GameObject)EditorGUILayout.ObjectField("BlockChunk", prefabBlock, typeof(GameObject));
                EditorGUILayout.Vector2Field("ChunkSize", new Vector2(chunk_width, chunk_height));
                //获取Parent
                parent = (RectTransform)EditorGUILayout.ObjectField("Parent", parent, typeof(RectTransform));
                draw = EditorGUILayout.Toggle("Draw", draw);
            }
        }

        Handles.EndGUI();

        if (prefabBlock != null && parent != null && draw)
        {
            //Normalize Cursor Coord
            Vector2 musPos = GetMousePosToScene(sceneView, parent);
            //Debug.Log(musPos);
            if (initTemp == null)
            {
                initTemp = Instantiate(prefabBlock, parent).GetComponent<RectTransform>();
                Undo.RecordObject(initTemp, "Instantiate");
            }
            initTemp.anchoredPosition = new Vector2((int)(musPos.x / chunk_width) * chunk_width, (int)(musPos.y / chunk_height) * chunk_height);
            //Debug.Log(initTemp.anchoredPosition);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                //Debug.Log("Init");
                initTemp = null;
            }
        }
        else
        {
            if (initTemp != null)
            {
                DestroyImmediate(initTemp.gameObject);
                initTemp = null;
            }
        }
    }

    public static Vector2 GetMousePosToScene(SceneView sceneView, RectTransform parent)
    {
        var temp = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
        temp = Camera.main.WorldToScreenPoint(temp);

        return temp;
    }

    // Called when the active tool is set to this tool instance. Global tools are persisted by the ToolManager,
    // so usually you would use OnEnable and OnDisable to manage native resources, and OnActivated/OnWillBeDeactivated
    // to set up state. See also `EditorTools.{ activeToolChanged, activeToolChanged }` events.
    public override void OnActivated()
    {
        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Entering ChunkEditor Tool"), .1f);
    }

    // Called before the active tool is changed, or destroyed. The exception to this rule is if you have manually
    // destroyed this tool (ex, calling `Destroy(this)` will skip the OnWillBeDeactivated invocation).
    public override void OnWillBeDeactivated()
    {
        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting ChunkEditor Tool"), .1f);
    }


}

