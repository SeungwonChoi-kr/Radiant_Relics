using UnityEngine;



public class DayNightCycle : MonoBehaviour
{
    // 인스펙터 설정 변수

    // 태양 (Directional Light)을 할당할 변수
    public Light sunLight;
    // 하루가 진행되는 시간 (초). 기본값 10분 = 600초
    public float dayDuration = 600f;
    // 시간의 배율 (1.0f가 기본 속도)
    public float timeScale = 1.0f;
    // 낮 시간대에 사용할 Skybox Material (예: Procedural Skybox)
    public Material daySkyboxMaterial;
    // 밤 시간대에 사용할 Skybox Material (예: Real Stars Skybox Material)
    public Material nightSkyboxMaterial;

    // 내부 상태 변수
    // 현재 시간 (0.0f = 자정, 0.25f = 일출, 0.5f = 정오, 0.75f = 일몰)
    private float currentTimeOfDay = 0.25f;

    // 낮 시간대의 태양의 최대 강도
    private const float DayIntensity = 1.2f;
    // 밤 시간대의 태양의 최소 강도 (달빛/배경광 효과)
    private const float NightIntensity = 0.2f;
    // 낮 시간대의 색상 (Ambient Light 및 Sun Light)
    private static readonly Color DayColor = Color.white;
    // 밤 시간대의 색상 (약간 푸른색, Ambient Light 및 Sun Light)
    private static readonly Color NightColor = new Color(0.1f, 0.1f, 0.3f, 1f);
    private bool wasNight = false; // 이전 프레임의 밤 상태를 저장

    void Update()
    {
        // 1. 시간 업데이트
        UpdateDayTime();
        // 2. 태양 회전
        RotateSun();
        // 3. 환경 조명, 강도 및 Skybox 조절
        UpdateLighting();
    }

    /// <summary>
    /// 게임 내 시간을 진행시킵니다.
    /// </summary>
    private void UpdateDayTime()
    {
        // 시간이 흐르는 비율을 계산하여 currentTimeOfDay에 추가합니다.
        currentTimeOfDay += (Time.deltaTime / dayDuration) * timeScale;
        // 하루가 지나면 (1.0f를 넘으면) 0으로 초기화합니다.
        if (currentTimeOfDay >= 1.0f){
            currentTimeOfDay = 0.0f;
        }
    }

    /// <summary>
    /// Directional Light를 회전시켜 태양이 뜨고 지는 효과를 만듭니다.
    /// </summary>
    private void RotateSun()
    {
        // 현재 시간에 360도를 곱하여 태양의 현재 각도를 계산합니다.
        // -90도(수평선 아래)에서 시작하여 수직으로 이동하도록 조정합니다.
        float sunAngle = currentTimeOfDay * 360f;

        // X축 회전은 고도를, Y축 회전은 방위를 결정합니다. (Y축 170f는 임의의 값)
        sunLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);
    }

    /// <summary>
    /// 현재 시간에 따라 태양의 강도, 색상, 주변광, Skybox를 조절합니다.
    /// </summary>
    private void UpdateLighting()
    {
        // Lerp(선형 보간)에 사용할 비율 변수
        float t = 0f;

        // **1. 태양의 밝기 및 색상 조절**
        if (currentTimeOfDay < 0.25f) // 자정 (0.0f) -> 일출 (0.25f)
        {
            t = Mathf.InverseLerp(0.0f, 0.25f, currentTimeOfDay);
            // 밤 상태에서 낮 상태로 전환
            sunLight.color = Color.Lerp(NightColor, DayColor, t);
            sunLight.intensity = Mathf.Lerp(NightIntensity, DayIntensity, t);
        }
        else if (currentTimeOfDay < 0.5f) // 일출 (0.25f) -> 정오 (0.5f)
        {
            // 낮 상태 유지
            sunLight.color = DayColor;
            sunLight.intensity = DayIntensity;
        }
        else if (currentTimeOfDay < 0.75f) // 정오 (0.5f) -> 일몰 (0.75f)
        {
            t = Mathf.InverseLerp(0.5f, 0.75f, currentTimeOfDay);
            // 낮 상태에서 밤 상태로 전환
            sunLight.color = Color.Lerp(DayColor, NightColor, t);
            sunLight.intensity = Mathf.Lerp(DayIntensity, NightIntensity, t);
        }
        else // 일몰 (0.75f) -> 자정 (1.0f)
        {
            // 밤 상태 유지
            sunLight.color = NightColor;
            sunLight.intensity = NightIntensity;
        }

        // **2. 주변광 (Ambient Light) 조절**
        // 태양의 현재 강도를 사용하여 주변광의 색상과 밝기를 동적으로 설정합니다.
        RenderSettings.ambientLight = Color.Lerp(NightColor, DayColor, sunLight.intensity / DayIntensity);


        // **3. Skybox 전환 로직**
        bool isCurrentlyDay = (currentTimeOfDay >= 0.22f && currentTimeOfDay < 0.8f);

        // 밤에서 낮으로 전환될 때 ResourceManager에 알림
        if (wasNight && isCurrentlyDay)
        {
            if (ResourceManager.Instance != null)
            {
                ResourceManager.Instance.RespawnAllPointsForNewDay();
            }
        }

        //밤낮 정보를 GameManager에 전달
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isNight = !isCurrentlyDay;
        }

        // 일출/일몰 시간 근처에서 Skybox를 전환합니다.
        if (isCurrentlyDay)
        {
            // 낮 Skybox 적용
            RenderSettings.skybox = daySkyboxMaterial;
    
        }
        else
        {
            // 밤 Skybox 적용
            RenderSettings.skybox = nightSkyboxMaterial;
            
        }
        wasNight = !isCurrentlyDay;
    }
}
