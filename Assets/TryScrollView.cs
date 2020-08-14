using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TryScrollView : MonoBehaviour
{
    //Sampler s = null;
    // Start is called before the first frame update
    void Start()
    {
        //s = Sampler.Get("Camera.Render");
    }

    // Update is called once per frame
    void Update()
    {
        //float time = s.GetRecorder().elapsedNanoseconds / 1000.0f / 1000.0f;
        //Debug.Log(time);

        //UWAEngine.LogValue(s.name, time);
    }

    Vector2 pos = Vector2.zero;
    private void OnGUI()
    {
        float old = GUI.skin.verticalScrollbarThumb.fixedWidth;
        float old2 = GUI.skin.verticalScrollbar.fixedWidth;
        GUI.skin.verticalScrollbarThumb.fixedWidth = 50;
        GUI.skin.verticalScrollbar.fixedWidth = 50;
        pos = GUILayout.BeginScrollView(pos, new GUILayoutOption[] { GUILayout.Height(200), GUILayout.Width(200) });
        GUI.skin.verticalScrollbarThumb.fixedWidth = old;
        GUI.skin.verticalScrollbar.fixedWidth = old;
        for (int i = 0; i < 10; i++)
        {
            GUILayout.Button("Test" + i);
        }

        GUILayout.EndScrollView();
    }
}
