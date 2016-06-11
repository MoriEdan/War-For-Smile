using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Affdex;
using Assets.Scripts.Helpers;
using Helpers;

public class PlayerEmotions : ImageResultsListener
{
    public Player Player;

    public float CurrentSurprise;
    public float CurrentSmile;
    public float CurrentMouthOpen;

    public bool Working = false;

    private void Start ()
    {

    }
	
    private void Update ()
    {
	    
	}

    public override void onImageResults(Dictionary<int, Face> faces)
    {
        if (ResourceManager.IsDoingSetup || !Working)
        {
            return;
        }

        foreach (var pair in faces)
        {
            var face = pair.Value;

            face.Emotions.TryGetValue(Emotions.Surprise, out CurrentSurprise);
            face.Expressions.TryGetValue(Expressions.Smile, out CurrentSmile);
            face.Expressions.TryGetValue(Expressions.MouthOpen, out CurrentMouthOpen);

            if (CurrentSmile >= 80)
            {
                ResourceManager.DetectedEmotion = Emotion.Fun;
            }
            else if (CurrentMouthOpen >= 70 || CurrentSurprise >= 70)
            {
                ResourceManager.DetectedEmotion = Emotion.Sad;
            }
            else
            {
                ResourceManager.DetectedEmotion = Emotion.Netural;       
            }
        }
    }

    public override void onFaceFound(float timestamp, int faceId)
    {
        if (!Working)
        {
            return;
        }

        //Debug.Log("Face found");
        AnalyticLogger.AddData(AnalyticEventType.FaceFound);
        Player.PauseScreen.SetActive(false);
        ResourceManager.IsDoingSetup = false;

    }

    public override void onFaceLost(float timestamp, int faceId)
    {
        if(!Working)
        {
            return;
        }

        //Debug.Log("Face lost");
        AnalyticLogger.AddData(AnalyticEventType.FaceLost);
        Player.PauseScreen.SetActive(true);
        ResourceManager.IsDoingSetup = true;
    }
}
