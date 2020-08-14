#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;

[InitializeOnLoad]
public static class PrefabStageListener
{
    static PrefabStageListener()
    {
        // 打开Prefab编辑界面回调
        //PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        // Prefab被保存之前回调
        //PrefabStage.prefabSaving += OnPrefabSaving;
        // Prefab被保存之后回调
        PrefabStage.prefabSaved += OnPrefabSaved;
        // 关闭Prefab编辑界面回调
        //PrefabStage.prefabStageClosing += OnPrefabStageClosing;
    }

    public static void OnPrefabSaved(GameObject go)
    {
        Debug.Log(go.name);
    }
    //....
}
#endif