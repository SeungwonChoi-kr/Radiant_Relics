using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndingSceneController : MonoBehaviour
{
    [Tooltip("엔딩 타임라인이 부착된 PlayableDirector 컴포넌트를 연결하세요.")]
    public PlayableDirector endingDirector;

    // 메인 메뉴 씬 이름을 'Start'로 수정했습니다. (대소문자 일치)
    [Tooltip("타임라인 종료 후 돌아갈 메인 메뉴 씬 이름")]
    [SerializeField]
    private string mainMenuSceneName = "Start"; // << 'Start'로 정확히 수정됨

    void Start()
    {
        // 1. PlayableDirector 컴포넌트 자동 가져오기 
        if (endingDirector == null)
        {
            endingDirector = GetComponent<PlayableDirector>();
            if (endingDirector == null)
            {
                Debug.LogError("PlayableDirector 컴포넌트가 이 오브젝트에 없거나 연결되지 않았습니다.");
                return;
            }
        }

        // 2. 타임라인이 끝났을 때 OnTimelineStopped를 호출하도록 구독
        // 이 로직 덕분에 타임라인이 끝나면 자동으로 OnTimelineStopped 함수가 실행됩니다.
        endingDirector.stopped += OnTimelineStopped;

        // 씬 시작 시 커서 설정
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("엔딩 타임라인 종료 이벤트를 구독했습니다.");
    }

    // 3. 타임라인 종료 시 호출되는 함수
    private void OnTimelineStopped(PlayableDirector director)
    {
        // 콘솔에 전환 정보를 출력합니다.
        Debug.Log($"타임라인 재생 완료. 메인 메뉴 씬 ({mainMenuSceneName})으로 전환합니다.");

        // 이벤트 구독 해제 (Cleanup)
        director.stopped -= OnTimelineStopped;

        // 시간 스케일을 원래대로 복구
        Time.timeScale = 1.0f;

        // 4. 'Start' 씬 로드
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
