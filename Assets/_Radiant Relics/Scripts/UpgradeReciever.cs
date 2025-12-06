using UnityEngine;
using DevionGames;
using System.Reflection;
using System.Collections.Generic;

public class UpgradeReceiver : MonoBehaviour
{
    public int level = 0;

    public void UpgradeTo1(CallbackEventData data = null)
    {
        level = 1;
        Debug.Log($"업그레이드 1단계!");

        // data가 들어왔을 때만 출력
        if (data != null)
        {
            // private Dictionary<string, object> properties 접근
            var field = typeof(CallbackEventData).GetField("properties",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                var dict = field.GetValue(data) as Dictionary<string, object>;
                if (dict != null)
                {
                    foreach (var pair in dict)
                    {
                        Debug.Log($"data[{pair.Key}] = {pair.Value}");
                    }
                }
            }
        }
    }

    public void UpgradeTo2(CallbackEventData data = null)
    {
        level = 2;
        Debug.Log($"업그레이드 2단계!");
    }

    public void UpgradeTo3(CallbackEventData data = null)
    {
        level = 3;
        Debug.Log($"업그레이드 3단계!");
    }

    public void UpgradeFinal(CallbackEventData data = null)
    {
        level = 4;
        Debug.Log($"최종 업그레이드!");
    }
}
