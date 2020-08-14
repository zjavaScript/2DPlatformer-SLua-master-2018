﻿/**
 * The lua mono behavior class.
 *
 * @filename  YwLuaMonoBehaviour.cs
 * @copyright Copyright (c) 2015 Yaukey/yaukeywang/WangYaoqi (yaukeywang@gmail.com) all rights reserved.
 * @license   The MIT License (MIT)
 * @author    Yaukey
 * @date      2015-11-27
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

// The params of class YwLuaMonoBehaviour.
[System.Serializable]
public class YwLuaMonoBehaviourParams
{
    // The mono method types.
    public enum EMonoMethod
    {
        Start,
        Update,
        LateUpdate,
        FixedUpdate,
        LiteUpdate,
        OnEnable,
        OnDisable,
        OnDestroy,
        OnTriggerEnter,
        OnTriggerEnter2D,
        OnTriggerExit,
        OnTriggerExit2D,
        OnCollisionEnter,
        OnCollisionEnter2D,
        OnCollisionExit,
        OnCollisionExit2D,
        // Add more if your need.
    }

    // The lua class name.
    public string m_strLuaClassName = string.Empty;

    // All mono method we have.
    public List<EMonoMethod> m_cMonoMethods = new List<EMonoMethod>();

    // The parameters used to pass to lua.
    public GameObject[] m_aParameters = null;
}

// The lua mono behaviour class.
public class YwLuaMonoBehaviour : YwLuaMonoBehaviourBase
{
    // The mono behaviour params to lua.
    [DoNotToLua]
    public YwLuaMonoBehaviourParams m_luaParameters = new YwLuaMonoBehaviourParams();

    // The static method array.
    private bool[] m_aMonoMethodFlags = new bool[System.Enum.GetValues(typeof(YwLuaMonoBehaviourParams.EMonoMethod)).Length];

    // Name of enable base lua function.
    private static readonly string LUA_FUNC_NAME_ON_ENABLE_BASE = "OnEnableBase";

    // Name of disable base lua function.
    private static readonly string LUA_FUNC_NAME_ON_DISENABLE_BASE = "OnDisableBase";

    // The On enable base function.
    private LuaFunction m_cOnEnableBase = null;

    // The On disable base function.
    private LuaFunction m_cOnDisableBase = null;

    // Use this for initialization.
    void Awake()
    {
        if (!YwLuaScriptMng.Instance.Initialized)
        {
            return;
        }

        // Get lua class name.
        string strLuaClassName = m_luaParameters.m_strLuaClassName;

        // If no class name is specified, use name of game object instead.
        if (string.IsNullOrEmpty(strLuaClassName))
        {
            strLuaClassName = gameObject.name;
            Debug.LogWarning("You do not set lua class name, use GameObject's name \'" + gameObject.name + "\' instead!");
        }

        // Directly create a lua class instance to associate with this monobehavior.
        if (string.IsNullOrEmpty(strLuaClassName) || !CreateClassInstance(strLuaClassName))
        {
            return;
        }

        // Initialize parameters need to be passed to lua.
        if ((null != m_luaParameters.m_aParameters) && (m_luaParameters.m_aParameters.Length > 0))
        {
            m_cBehavior.SetData("m_aParameters", m_luaParameters.m_aParameters);
        }

        // Init all method flags.
        for (int i = 0; i < m_aMonoMethodFlags.Length; i++)
        {
            m_aMonoMethodFlags[i] = m_luaParameters.m_cMonoMethods.Contains((YwLuaMonoBehaviourParams.EMonoMethod)i);
        }

        // Init update flags.
        m_cBehavior.SetData("m_bUpdate", m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.Update]);
        m_cBehavior.SetData("m_bLateUpdate", m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.LateUpdate]);
        m_cBehavior.SetData("m_bFixedUpdate", m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.FixedUpdate]);
        m_cBehavior.SetData("m_bLiteUpdate", m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.LiteUpdate]);

        // To set custom parameters need to be passed to lua.
        OnSetCustomParameters();

        // Call awake base.
        LuaFunction cAwakeBase = null;
        m_cBehavior.CallMethod(ref cAwakeBase, "AwakeBase", m_cBehavior.GetChunk());

        // Call awake.
        m_cBehavior.Awake();
    }

	// Use this for initialization.
	void Start()
    {
	    if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.Start])
        {
            return;
        }

        m_cBehavior.Start();
    }

    // This function is called when the behaviour becomes disabled or inactive.
    void OnEnable()
    {
        if (!m_bReady)
        {
            return;
        }

        // Call on enable base anyway.
        m_cBehavior.CallMethod(ref m_cOnEnableBase, LUA_FUNC_NAME_ON_ENABLE_BASE, m_cBehavior.GetChunk());

        // Call enable.
        if (m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnEnable])
        {
            m_cBehavior.OnEnable();
        }
    }

    // This function is called when the object becomes enabled and active.
    void OnDisable()
    {
        if (!m_bReady)
        {
            return;
        }

        // Call on disable base anyway.
        m_cBehavior.CallMethod(ref m_cOnDisableBase, LUA_FUNC_NAME_ON_DISENABLE_BASE, m_cBehavior.GetChunk());

        // Call disable.
        if (m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnDisable])
        {
            m_cBehavior.OnDisable();
        }
    }

    // On destroy method.
    void OnDestroy()
    {
        if (!m_bReady)
        {
            return;
        }

        // Call on destroy base anyway.
        LuaFunction cOnDestroyBase = null;
        m_cBehavior.CallMethod(ref cOnDestroyBase, "OnDestroyBase", m_cBehavior.GetChunk());

        // Call on destroy.
        if (m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnDestroy])
        {
            m_cBehavior.OnDestroy();
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger.
    void OnTriggerEnter(Collider cOther)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnTriggerEnter])
        {
            return;
        }

        m_cBehavior.OnTriggerEnter(cOther);
    }

    // Sent when another object enters a trigger collider attached to this object (2D physics only).
    void OnTriggerEnter2D(Collider2D cOther)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnTriggerEnter2D])
        {
            return;
        }

        m_cBehavior.OnTriggerEnter2D(cOther);
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger.
    void OnTriggerExit(Collider cOther)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnTriggerExit])
        {
            return;
        }

        m_cBehavior.OnTriggerExit(cOther);
    }

    // Sent when another object leaves a trigger collider attached to this object (2D physics only).
    void OnTriggerExit2D(Collider2D cOther)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnTriggerExit2D])
        {
            return;
        }

        m_cBehavior.OnTriggerExit2D(cOther);
    }

    // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
    void OnCollisionEnter(Collision cCollision)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnCollisionEnter])
        {
            return;
        }

        m_cBehavior.OnCollisionEnter(cCollision);
    }

    // Sent when an incoming collider makes contact with this object's collider (2D physics only).
    void OnCollisionEnter2D(Collision2D cCollision)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnCollisionEnter2D])
        {
            return;
        }

        m_cBehavior.OnCollisionEnter2D(cCollision);
    }

    // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
    void OnCollisionExit(Collision cCollision)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnCollisionExit])
        {
            return;
        }

        m_cBehavior.OnCollisionExit(cCollision);
    }

    // Sent when a collider on another object stops touching this object's collider (2D physics only).
    void OnCollisionExit2D(Collision2D cCollision)
    {
        if (!m_bReady || !m_aMonoMethodFlags[(int)YwLuaMonoBehaviourParams.EMonoMethod.OnCollisionExit2D])
        {
            return;
        }

        m_cBehavior.OnCollisionExit2D(cCollision);
    }

    /**
     * Get lua class name.
     * 
     * @param void.
     * @return string - The name of the associated lua class.
     */
    public string GetLuaClassName()
    {
        return m_luaParameters.m_strLuaClassName;
    }

    /**
     * To set custom parameters need to be passed to lua.
     * 
     * @param void.
     * @return void.
     */
    protected virtual void OnSetCustomParameters()
    {
    }
}
