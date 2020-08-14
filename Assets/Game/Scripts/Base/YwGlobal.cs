/**
 * The YwGlobal class.
 *
 * @filename  YwGlobal.cs
 * @copyright Copyright (c) 2015 Yaukey/yaukeywang/WangYaoqi (yaukeywang@gmail.com) all rights reserved.
 * @license   The MIT License (MIT)
 * @author    Yaukey
 * @date      2015-08-23
 */

using UnityEngine;
using System.Collections;
using UnityEngine.Profiling;
using System.Reflection;

// The global object that not be destroyed.
public class YwGlobal : MonoBehaviour
{
    // The lite update fps.
    public int m_liteUpdateFps = 10;

    // The lite upate flag.
    private bool m_bLiteUpdate = true;

    // The lite update timer.
    private WaitForSeconds m_cLiteUpdateTimer = null;

    // The instance.
    private static YwGlobal m_cInstance = null;

    // Get the global instance.
	public static YwGlobal Instance
    {
        get
        {
            // Alloc one if have not.
            if (null == m_cInstance)
            {
                GameObject cObj = new GameObject("YwGlobal");
                m_cInstance = cObj.AddComponent<YwGlobal>();
            }

            return m_cInstance;
        }
    }

    private MethodInfo m = null;

    // Awake to init.
    void Awake()
    {
        // Set property.
        DontDestroyOnLoad(gameObject);

        // Init manager.
        InitGlobalManager();

        //Profiler.SetAreaEnabled(ProfilerArea.Memory, true);

        m = typeof(YwGlobal).GetMethod("ThrowExp");

        //UWAEngine.StaticInit();
        //UWAEngine.SetOverrideLuaLib("libempty.so");
        //UWAEngine.StaticInit();

        //UWAEngine.Start(UWAEngine.Mode.Overview);
    }

    // Start to init.
    IEnumerator Start()
    {
        // Wait the script environment init ok.
        while (!YwLuaScriptMng.Instance.Initialized)
        {
            yield return null;
        }

        // Init lite update settings.
        m_cLiteUpdateTimer = new WaitForSeconds(1.0f / (float)m_liteUpdateFps);
        StartCoroutine(LiteUpdateControler());
    }

    // Update is called once per frame.
    void Update()
    {
        // Check if init ok.
        if (!YwLuaScriptMng.Instance.Initialized)
        {
            return;
        }

        // The main update.
        //UWAEngine.PushSample("YwLuaScriptMng.Update");
        YwLuaScriptMng.Instance.Update();
        //UWAEngine.PopSample();

        // The lite update.
        if (m_bLiteUpdate)
        {
            m_bLiteUpdate = false;
            YwLuaScriptMng.Instance.LiteUpdate();
        }

        if (_doException)
        {
            _doException = false;
            A();
        }
    }

    public void A()
    {
        B();
    }

    public void B()
    {
        m.Invoke(this, null);
    }

    public void ThrowExp()
    {
        Debug.Log("Before Exception");

        Instantiate(null);

        Debug.Log("After Exception");
    }

    // LateUpdate is called every frame.
    void LateUpdate()
    {
        // Check if init ok.
        if (!YwLuaScriptMng.Instance.Initialized)
        {
            return;
        }

        //UWAEngine.PushSample("YwLuaScriptMng.Update");
        YwLuaScriptMng.Instance.LateUpdate();
        //UWAEngine.PopSample();
    }

    // FixedUpdate is called every fixed framerate frame.
    void FixedUpdate()
    {
        // Check if init ok.
        if (!YwLuaScriptMng.Instance.Initialized)
        {
            return;
        }

        YwLuaScriptMng.Instance.FixedUpdate();

        //UWAEngine.AddMarker("FixedUpdateCount");
    }

    // Init the global lua script manager.
    private void InitGlobalManager()
    {
        // The base support.
        YwLuaScriptMng.Instance.Initialize();
    }

    // The coroutine to control lite update.
    private IEnumerator LiteUpdateControler()
    {
        while (true)
        {
            m_bLiteUpdate = true;
            yield return m_cLiteUpdateTimer;
        }
    }

    private bool _doException = false;
    private void OnGUI()
    {
        //if (GUILayout.Button("\n\tException\t\n"))
        //{
        //    _doException = true;
        //}
    }
}
