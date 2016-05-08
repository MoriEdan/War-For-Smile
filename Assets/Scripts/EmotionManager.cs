using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    private float _currentDisplayTextTimer;
    private bool _isDisplayingText = false;
    [SerializeField] private float _displayTextDelay;

    private bool _emotionDetected = false;

    // 1 - Smile | 2 - Sad
    public int EmotionToFind = 1;

    void Start ()
    {
	    
	}
	
	void Update ()
	{
	    if (!_isCheckingEmotion)
	    {
	        _currentRandomEmotionTimer += Time.deltaTime;
	    }
	    if (_isDisplayingText)
	    {
	        _currentDisplayTextTimer += Time.deltaTime;
	        CheckEmotionText();
	    }

        // 1 - gdy emotion meter spadnie ponizej 20% wyswietl komunikat do wywolania emocji
        // gdy gracz zareaguje rozpocznij uzupelnianie amunicji
	    WatchEmotionMeter();

        // 2 - losowo wyswietl komunikat o wywolaniu emocji, gdy gracz nie zareguje przyspiesz melancholie
	    if (!_isCheckingEmotion)
	    {
	        CheckRandomEmotion();
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

    private void CheckRandomEmotion()
    {
        if (_currentRandomEmotionTimer >= _currentRandomEmotionDelay)
        {
            _isCheckingEmotion = true;

            _currentRandomEmotionTimer = 0.0f;
            _currentRandomEmotionDelay = UnityEngine.Random.Range(2, 5);

            EmotionToFind = UnityEngine.Random.Range(1, 3);
            DisplayEmotionText(true);
        }
    }

    private void CheckEmotionText()
    {
        if (_currentDisplayTextTimer >= _displayTextDelay)
        {
            DisplayEmotionText(false);
            _isCheckingEmotion = false;
            _currentDisplayTextTimer = 0.0f;

            if (!_emotionDetected)
            {
                Debug.Log("Nie wykryto emocji");
            }
        }

        // miejsce na sprawdzenie emocji
        // ...
    }

    private void DisplayEmotionText(bool enable)
    {
        _isDisplayingText = enable;
        EmotionTextHeader.gameObject.SetActive(enable);
        if (EmotionToFind == 1)
        {
            EmotionTextSmile.gameObject.SetActive(enable);
        }
        else
        {
            EmotionTextSad.gameObject.SetActive(enable);
        }
    }
}
