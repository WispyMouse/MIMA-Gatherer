using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfigurations : MonoBehaviour
{
    [SerializeField]
    private ConfiguredStatistic<float> CameraXModifier = new ConfiguredStatistic<float>(0, $"{nameof(CameraConfigurations)}.{nameof(CameraXModifier)}");
    [SerializeField]
    private ConfiguredStatistic<float> CameraYModifier = new ConfiguredStatistic<float>(0, $"{nameof(CameraConfigurations)}.{nameof(CameraYModifier)}");
    [SerializeField]
    private ConfiguredStatistic<float> CameraZModifier = new ConfiguredStatistic<float>(0, $"{nameof(CameraConfigurations)}.{nameof(CameraZModifier)}");

    [SerializeField]
    private ConfiguredStatistic<float> CameraXRotation = new ConfiguredStatistic<float>(0, $"{nameof(CameraConfigurations)}.{nameof(CameraXRotation)}");
    [SerializeField]
    private ConfiguredStatistic<float> CameraYRotation = new ConfiguredStatistic<float>(0, $"{nameof(CameraConfigurations)}.{nameof(CameraYRotation)}");
    [SerializeField]
    private ConfiguredStatistic<float> CameraZRotation = new ConfiguredStatistic<float>(0, $"{nameof(CameraConfigurations)}.{nameof(CameraZRotation)}");

    public ConfiguredStatistic<float> CameraPanningMovementPerSecond = new ConfiguredStatistic<float>(10f, $"{nameof(CameraConfigurations)}.{nameof(CameraPanningMovementPerSecond)}");

    private void Awake()
    {
        transform.position = new Vector3(CameraXModifier.Value, CameraYModifier.Value, CameraZModifier.Value);
        transform.rotation = Quaternion.Euler(CameraXRotation.Value, CameraYRotation.Value, CameraZRotation.Value);
    }
}
