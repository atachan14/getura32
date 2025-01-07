using UnityEngine;
using UnityEngine.UIElements;

public class DebugUI : MonoBehaviour
{
    private GUIStyle customStyle;
    public TentacleController tentacleController;

    private void Start()
    {
        customStyle = new GUIStyle();
        customStyle.fontSize = 15;
        customStyle.normal.textColor = Color.black;
    }

    private void OnGUI()
    {
        if (tentacleController.TargetPlayer == null)
        {
            return;
        }
        
        GUI.Label(new Rect(10, 390, 700, 20), "TargetPlayer:" + tentacleController.TargetPlayer.transform.position, customStyle);
        GUI.Label(new Rect(10, 410, 700, 20), "TentaclePosition:" + tentacleController.TentaclePosition.Value, customStyle);
        GUI.Label(new Rect(10, 430, 700, 20), "thisTransformPosition:" + this.transform.position, customStyle);
        GUI.Label(new Rect(10, 450, 700, 20), "TentacleRotation:" + tentacleController.TentacleRotation.Value, customStyle);
        GUI.Label(new Rect(10, 470, 700, 20), "tentacleScaleY:" + tentacleController.TentacleScaleY.Value, customStyle);
    }
}
