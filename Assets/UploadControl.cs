using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UploadControl : MonoBehaviour {

    // Use this for initialization
    void Start() {
        UWAEngine.StaticInit();
    }

    // Update is called once per frame
    void Update() {

    }

    string projectId = "";
    string projectName = "";
    string timeLimit = "";
    string userName = "13701649509";
    string pwd = "uwa123456";
    string tag = "";

    class A{}

    List<A> temp = new List<A>();

    Stopwatch sw = new Stopwatch();

    long ms = 0;

    private void OnGUI()
    {
        int id;
        int time = 100000;

        GUILayoutOption[] oo = new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), };

        if(GUILayout.Button("\n\t===========================================================================\t\n"))
        {
            temp.Clear();

            sw.Reset();
            sw.Start();

            for (int i = 0; i < 100000; i++)
            {
                temp.Add(new A());
            }

            ms = sw.ElapsedMilliseconds;
        }

        if (ms > 0) GUILayout.Button(ms.ToString());

        GUILayout.BeginHorizontal();
        GUILayout.Button("\n\t项目ID\t\n");
        projectId = GUILayout.TextField(projectId, oo);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button("\n\t项目名\t\n");
        projectName = GUILayout.TextField(projectName, oo);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button("\n\t时间上限\t\n");
        timeLimit = GUILayout.TextField(timeLimit, oo);
        if (!int.TryParse(timeLimit, out time)) time = 100000;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button("\n\t用户名\t\n");
        userName = GUILayout.TextField(userName, oo);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Button("\n\t密码\t\n");
        pwd = GUILayout.TextField(pwd, oo);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("\n\t改变TAG\t\n"))
        {
            UWAEngine.Tag(tag);
        }
        tag = GUILayout.TextField(tag, oo);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("\n\t用项目ID上传\t\n") && int.TryParse(projectId, out id))
        {
            UWAEngine.Upload((bool x) => { UnityEngine.Debug.Log(x); }, userName, pwd, id, time);
        }

        if (GUILayout.Button("\n\t用项目名上传\t\n"))
        {
            UWAEngine.Upload((bool x) => { UnityEngine.Debug.Log(x); }, userName, pwd, projectName, time);
        }



        if (GUILayout.Button("\n\t开启Overview模式\t\n"))
        {
            UWAEngine.Start(UWAEngine.Mode.Overview);
        }

        if (GUILayout.Button("\n\t结束测试\t\n"))
        {
            UWAEngine.Stop();
        }
    }
}
