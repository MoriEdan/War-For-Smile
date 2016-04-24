using System;
using UnityEngine;
using System.Collections;

public class EmotionManager : MonoBehaviour
{
    public Player PlayerReference;

    private float _emotionValueToTrigger = 20.0f;
    public bool IsLoadingEmotions = false;
    public float EmotionRegenerationRate;

	void Start ()
    {
	    
	}
	
	void Update ()
    {
        // 1 - gdy emotion meter spadnie ponizej 20% wyswietl komunikat do wywolania emocji
        // gdy gracz zareaguje rozpocznij uzupelnianie amunicji
	    WatchEmotionMeter();

	    // 2 - losowo wyswietl komunikat o wywolaniu emocji, gdy gracz nie zareguje przyspiesz melancholie
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
}
