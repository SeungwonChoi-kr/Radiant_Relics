using UnityEngine;

// Gemini 코드
[ExecuteAlways]
public class SnapToTerrain : MonoBehaviour
{
    public bool alignBottom = true;
    public float yOffset = 0.0f;

    void Update()
    {
        if (Application.isPlaying) return;
        Snap();
    }

    void Snap()
    {
        Terrain activeTerrain = Terrain.activeTerrain;
        if (activeTerrain == null) return;

        // 내 자신(this)의 위치만 신경 씀
        float terrainHeight = activeTerrain.SampleHeight(transform.position) + activeTerrain.transform.position.y;
        float targetY = terrainHeight + yOffset;

        if (alignBottom)
        {
            Renderer meshRenderer = GetComponentInChildren<Renderer>();
            if (meshRenderer != null)
            {
                float pivotToBottomDist = transform.position.y - meshRenderer.bounds.min.y;
                targetY += pivotToBottomDist;
            }
        }

        if (Mathf.Abs(transform.position.y - targetY) > 0.001f)
        {
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        }
    }
}
