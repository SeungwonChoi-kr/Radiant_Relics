using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CircleFenceGenerator : MonoBehaviour
{
    [Header("울타리 설정")]
    public GameObject fencePiecePrefab;
    public int numberOfPieces = 8;
    public float radius = 5f;

    private void OnValidate()
    {
        if (fencePiecePrefab != null && numberOfPieces > 0)
            GenerateFence();
    }

    public void GenerateFence()
    {
        if (fencePiecePrefab == null || numberOfPieces <= 0) return;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < numberOfPieces; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfPieces;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            Vector3 dir = pos.normalized;
            Quaternion finalRotation = Quaternion.LookRotation(dir) * Quaternion.Euler(-90, 0, 0);

#if UNITY_EDITOR
            GameObject piece = (GameObject)Instantiate(fencePiecePrefab, transform);
            piece.transform.localPosition = pos;
            piece.transform.localRotation = finalRotation;

            Undo.RegisterCreatedObjectUndo(piece, "Generate Fence Piece");
#else
            GameObject piece = Instantiate(fencePiecePrefab, transform);
            piece.transform.localPosition = pos;
            piece.transform.localRotation = finalRotation;
#endif
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CircleFenceGenerator))]
public class CircleFenceGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CircleFenceGenerator script = (CircleFenceGenerator)target;

        if (GUILayout.Button("Generate Fence"))
        {
            script.GenerateFence();
        }
    }
}
#endif
