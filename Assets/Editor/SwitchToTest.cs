using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//using UwaProjScan.ScanRule;

public class SwitchToTest : Editor
{
    [MenuItem("Tools/UWA Got Test/Test")]
    public static void Test()
    {
        //WindowTool.Test = true;
        Selection.activeGameObject.name = "Test\nTest";
    }
}