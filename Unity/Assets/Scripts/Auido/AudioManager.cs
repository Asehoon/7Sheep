using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;
    public float fadeDuration = 1.5f;

    public AudioClip explorationClip;
    public AudioClip battleClip;
    public AudioClip successClip;

    private Coroutine fadeCoroutine;

    private bool isMuted = false;
    private float originalVolume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        originalVolume = audioSource.volume;
    }

    private void Start()
    {
        // 처음 실행 시 탐험 음악 자동 재생
        PlayExploration();
    }

    public void PlayExploration() => PlayClipWithFade(explorationClip);
    public void PlayBattle() => PlayClipWithFade(battleClip);
    public void PlaySuccess() => PlayClipWithFade(successClip);

    private void PlayClipWithFade(AudioClip newClip)
    {
        if (audioSource.clip == newClip && audioSource.isPlaying)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeToNewClip(newClip));
    }

    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        float startVolume = isMuted ? 0f : originalVolume;

        // 페이드 아웃
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 0f;

        // 클립 변경 및 재생
        audioSource.clip = newClip;
        audioSource.Play();

        // 페이드 인 (음소거 상태면 0 유지)
        float targetVolume = isMuted ? 0f : originalVolume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = targetVolume;

        fadeCoroutine = null;
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        audioSource.volume = isMuted ? 0f : originalVolume;
    }

    public bool IsMuted() => isMuted;
}
