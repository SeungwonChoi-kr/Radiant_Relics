using UnityEngine;
using DevionGames;
using System.Collections.Generic;

public class ToolData : MonoBehaviour
{
    public string toolName = "Default Tool";
    public Sprite toolIcon;

    public void DebugCallbackData(CallbackEventData data)
    {
        Debug.Log("===== CallbackEventData =====");

        if (data == null)
        {
            Debug.Log("데이터 없음");
            return;
        }

        Dictionary<string, object> properties =
            data.GetType().GetField("properties",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
            .GetValue(data) as Dictionary<string, object>;

        foreach (var pair in properties)
        {
            Debug.Log($"{pair.Key} : {pair.Value}");
        }
    }
}
