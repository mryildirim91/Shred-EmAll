using UnityEngine;

public struct ScreenArea
{
    public static Vector3 screenArea = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    public static float FitHrz { get; private set; } = screenArea.x;
    public static float FitVer { get; private set; } = screenArea.y;
}
