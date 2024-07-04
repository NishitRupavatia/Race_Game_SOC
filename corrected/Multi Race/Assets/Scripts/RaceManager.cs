using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RaceManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text countdownText;
    public GameObject finishLine;
    public Image fadeImage;
    public GameObject resultsPanel;
    public TMP_Text resultsText;

    public AudioClip countDownClip3;
    public AudioClip countDownClip2;
    public AudioClip countDownClip1;
    public AudioClip countDownClipGo;

    private AudioSource audioSource;
    private float startTime;
    private bool raceStarted = false;
    private bool raceFinished = false;
    private bool countdownActive = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        resultsPanel.SetActive(false);  // Hide results panel at the start
        StartCoroutine(StartRace());
    }

    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            float t = Time.time - startTime;
            string minutes = Mathf.Floor(t / 60).ToString("00");
            string seconds = (t % 60).ToString("00.00");
            timerText.text = minutes + ":" + seconds;
        }
    }

    IEnumerator StartRace()
    {
        yield return new WaitForSeconds(1f); // Optional delay before starting countdown

        FadeOut(); // Fade out effect before countdown starts

        countdownActive = true;
        StartCoroutine(CountdownRoutine(3));
        yield return new WaitForSeconds(4f); // Wait for countdown to finish
        countdownActive = false;

        countdownText.gameObject.SetActive(false);
        FadeIn(); // Fade in effect after countdown finishes

        startTime = Time.time;
        raceStarted = true;
    }

    IEnumerator CountdownRoutine(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString();
            PlayCountdownSound(i);
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        PlayCountdownSound(0); // Play "GO!" sound
        yield return new WaitForSeconds(1f);
    }

    void PlayCountdownSound(int count)
    {
        if (!countdownActive) return; // Ensure sound only plays during countdown

        switch (count)
        {
            case 3:
                audioSource.PlayOneShot(countDownClip3);
                break;
            case 2:
                audioSource.PlayOneShot(countDownClip2);
                break;
            case 1:
                audioSource.PlayOneShot(countDownClip1);
                break;
            case 0:
                audioSource.PlayOneShot(countDownClipGo);
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == finishLine)
        {
            raceFinished = true;
            timerText.color = Color.green;
            DisplayResults();
        }
    }

    void DisplayResults()
    {
        float raceTime = Time.time - startTime;
        string minutes = Mathf.Floor(raceTime / 60).ToString("00");
        string seconds = (raceTime % 60).ToString("00.00");
        resultsText.text = "Your Time: " + minutes + ":" + seconds;

        audioSource.Stop();  // Stop all audio
        resultsPanel.SetActive(true);  // Show results panel
        Time.timeScale = 0f;  // Stop the gameplay

        FadeOut(); // Fade out effect when showing results
    }

    void FadeOut()
    {
        if (fadeImage != null)
        {
            fadeImage.CrossFadeAlpha(1f, 0f, false); // Ensure image is fully visible initially
            fadeImage.CrossFadeAlpha(0f, 2f, false); // Fade out effect over 2 seconds
        }
    }

    void FadeIn()
    {
        if (fadeImage != null)
        {
            fadeImage.CrossFadeAlpha(1f, 2f, false); // Fade in effect over 2 seconds
        }
    }
}

