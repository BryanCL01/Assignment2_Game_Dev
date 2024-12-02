using UnityEngine;

public class FlashlightManager : MonoBehaviour
{
    public Material clippingMaskMaterial;
    public Transform flashlightTransform;
    public float coneAngle = 30f;

    void Update()
    {
        // Update the flashlight's position and direction in the material
        clippingMaskMaterial.SetVector("_FlashlightPosition", flashlightTransform.position);
        clippingMaskMaterial.SetVector("_FlashlightDirection", flashlightTransform.forward);
        clippingMaskMaterial.SetFloat("_ConeAngle", coneAngle);
    }
}
