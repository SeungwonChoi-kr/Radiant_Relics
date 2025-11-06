using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CircleFenceGenerator : MonoBehaviour
{
    [Header("울타리 설정")]
    public GameObject fencePiecePrefab; // 프로젝트 창의 Fence_a Prefab
    public int numberOfPieces = 8;
    public float radius = 5f;

    // Inspector 값 변경 시 자동 생성
    private void OnValidate()
    {
        if (fencePiecePrefab != null && numberOfPieces > 0)
            GenerateFence();
    }

    // 울타리 생성
    public void GenerateFence()
    {
        if (fencePiecePrefab == null || numberOfPieces <= 0) return;

        // 기존 자식만 삭제 (부모 자신은 삭제하지 않음)
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // 울타리 조각 생성
        for (int i = 0; i < numberOfPieces; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfPieces;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

#if UNITY_EDITOR
            // PrefabUtility 대신 Instantiate 사용 → 부모 GameObject 안전
            GameObject piece = (GameObject)Instantiate(fencePiecePrefab, transform);
            piece.transform.localPosition = pos;
            piece.transform.LookAt(transform.position);

            // 편집 모드에서도 Undo 가능
            Undo.RegisterCreatedObjectUndo(piece, "Generate Fence Piece");
#else
            GameObject piece = Instantiate(fencePiecePrefab, transform);
            piece.transform.localPosition = pos;
            piece.transform.LookAt(transform.position);
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
