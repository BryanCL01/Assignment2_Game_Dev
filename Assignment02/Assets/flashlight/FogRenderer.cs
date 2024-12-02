using UnityEngine;

[ExecuteInEditMode]
public class FogRenderer : MonoBehaviour
{
    public Material fogMaterial; // Assign the material using the fog shader

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (fogMaterial != null)
        {
            // Apply the fog material as a post-processing effect
            Graphics.Blit(src, dest, fogMaterial);
        }
        else
        {
            // Fallback to default rendering
            Graphics.Blit(src, dest);
        }
    }
}
