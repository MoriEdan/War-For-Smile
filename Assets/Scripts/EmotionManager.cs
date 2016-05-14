using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using UnityEngine.UI;

using Helpers;

public class EmotionManager : MonoBehaviour
{
    public Player PlayerReference;

    private float _emotionValueToTrigger = 20.0f;
    public bool IsLoadingEmotions = false;
    public float EmotionRegenerationRate;

    private float _currentRandomEmotionTimer;
    [SerializeField] private float _currentRandomEmotionDelay;
    private bool _isCheckingEmotion = false;

    public Text EmotionTextHeader;
    public Text EmotionTextSmile;
    public Text EmotionTextSad;
    public Text EmotionTextCorrect;
    public Text EmotionTextWrong;

    private float _currentDisplayTextTimer;
    private bool _isDisplayingText = false;
    [SerializeField] private float _displayTextDelay;

    private float _currentDisplayEmotionDecisionTextTimer;
    private bool _isDisplayingDecisionText = false;
    private float _displayEmotionDecisionTextDelay = 1.0f;

    private bool _wasEmotionDetected = false;

    // 0 - Netural | 1 - Smile | 2 - Sad
    public Emotion EmotionToFind = Emotion.Netural;

    public float ChasingBorderSpeedMultiplier = 1.5f;

    void Start ()
    {
	    
	}
	
	void Update ()
	{
	    if (ResourceManager.isDoingSetup)
	    {
	        return;
	    }

	    if (!_isCheckingEmotion)
	    {
	        _currentRandomEmotionTimer += Time.deltaTime;
	    }
	    if (_isDisplayingText)
	    {
	        _currentDisplayTextTimer += Time.deltaTime;
	        CheckEmotion();
	    }
	    if (_isDisplayingDecisionText)
	    {
	        _currentDisplayEmotionDecisionTextTimer += Time.deltaTime;
	        DisplayEmotionDecisionText();
	    }

        // 1 - gdy emotion meter spadnie ponizej 20% wyswietl komunikat do wywolania emocji
        // gdy gracz zareaguje rozpocznij uzupelnianie amunicji
	    WatchEmotionMeter();

        // 2 - losowo wyswietl komunikat o wywolaniu emocji, gdy gracz nie zareguje przyspiesz melancholie
	    if (!_isCheckingEmotion && !_isDisplayingDecisionText)
	    {
	        PrepareCheckingRandomEmotion();
	    }
	}

    private void WatchEmotionMeter()
    {
        if (PlayerReference.EmotionMetterSlider.value <= _emotionValueToTrigger)
        {
            // wyswietl komunikat do wywolania emocji
            IsLoadingEmotions = true;
            Debug.Log("Zrob emocje jakas");
        }

        if (IsLoadingEmotions)
        {
            // if almost 100% emotions
            if (Math.Abs(PlayerReference.EmotionMetterSlider.value - 100.0f) < 1e-3)
            {
                IsLoadingEmotions = false;
                return;
            }

            PlayerReference.UpdateEmotionMeterSlider(EmotionRegenerationRate * Time.deltaTime);
        }
    }

    private void PrepareCheckingRandomEmotion()
    {
        if (_currentRandomEmotionTimer >= _currentRandomEmotionDelay)
        {
            _isCheckingEmotion = true;

            _currentRandomEmotionTimer = 0.0f;
            _currentRandomEmotionDelay = UnityEngine.Random.Range(2, 5);

            ResourceManager.DetectedEmotion = 0;
            _wasEmotionDetected = false;

            EmotionToFind = (Emotion) UnityEngine.Random.Range(1, 3);
            DisplayEmotionText(true);
        }
    }

    private void CheckEmotion()
    {
        if (_currentDisplayTextTimer >= _displayTextDelay)
        {
            DisplayEmotionText(false);
            _isCheckingEmotion = false;
            _currentDisplayTextTimer = 0.0f;

            // if time-up and no emotions
            if (!_wasEmotionDetected)
            {
                Debug.Log("Nie wykryto emocji");
                ProcessWrongEmotion();
            }

            return;
        }

        // check for found emotion
        switch (ResourceManager.DetectedEmotion)
        {
            case Emotion.Netural:
                //Debug.Log("Neutral");
                break;

            case Emotion.Fun:
                if (EmotionToFind == Emotion.Fun)
                {
                    ProcessCorrectEmotion();
                }
                else if (EmotionToFind == Emotion.Sad)
                {
                    ProcessWrongEmotion();
                }
                break;

            case Emotion.Sad:
                if (EmotionToFind == Emotion.Sad)
                {
                    ProcessCorrectEmotion();
                }
                else if (EmotionToFind == Emotion.Fun)
                {
                    ProcessWrongEmotion();
                }
                break;

            default:
                Debug.Log("Invalid Emotion Detected");
                ProcessWrongEmotion();
                break;
        } 
    }

    private void ProcessCorrectEmotion()
    {
        Debug.Log("Dobra emocja");
        _wasEmotionDetected = true;

        _isDisplayingDecisionText = true;
        DisplayEmotionDecisionText(true);

        // give benefits to player
        PlayerReference.UpdateEmotionMeterSlider(1000.0f);
        PlayerReference.SetCurrentHeatBar(0.0f);

        StopDetectingEmotions();
    }

    private void ProcessWrongEmotion()
    {
        Debug.Log("Zla emocja");
        _wasEmotionDetected = true;

        _isDisplayingDecisionText = true;
        DisplayEmotionDecisionText(true);

        // speed-up chasing border
        ResourceManager.ChasingBorderSpeed *= ChasingBorderSpeedMultiplier;

        StopDetectingEmotions();
    }

    private void StopDetectingEmotions()
    {
        DisplayEmotionText(false);
        _isCheckingEmotion = false;
        _currentDisplayTextTimer = 0.0f;
    }

    private void DisplayEmotionText(bool enable)
    {
        _isDisplayingText = enable;
        EmotionTextHeader.gameObject.SetActive(enable);
        if (EmotionToFind == Emotion.Fun)
        {
            EmotionTextSmile.gameObject.SetActive(enable);
        }
        else if (EmotionToFind == Emotion.Sad)
        {
            EmotionTextSad.gameObject.SetActive(enable);
        }
    }

    private void DisplayEmotionDecisionText(bool isSetup = false)
    {
        if (_currentDisplayEmotionDecisionTextTimer >= _displayEmotionDecisionTextDelay)
        {
            _currentDisplayEmotionDecisionTextTimer = 0.0f;
            _isDisplayingDecisionText = false;
            EmotionTextCorrect.gameObject.SetActive(false);
            EmotionTextWrong.gameObject.SetActive(false);
            return;
        }

        if (isSetup)
        {
            if (EmotionToFind == ResourceManager.DetectedEmotion)
            {
                EmotionTextCorrect.gameObject.SetActive(true);
            }
            else
            {
                EmotionTextWrong.gameObject.SetActive(true);
            }
        }

    }
}
