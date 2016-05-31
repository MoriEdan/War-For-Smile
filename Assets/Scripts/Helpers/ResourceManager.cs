using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

namespace Helpers
{
    public static class ResourceManager
    {
        // global vars
        public static bool IsDoingSetup { get; set; }
        public static Emotion DetectedEmotion { get; set; }
        public static float ChasingBorderSpeed = 1.0f;
        public static bool DisplayWebcamView { get; set; }

        private static GameObjectList _gameObjectList;
        public static void SetGameObjectList(GameObjectList objectList)
        {
            _gameObjectList = objectList;
        }

        public static GameObject GetGameObject(string objectName)
        {
            return _gameObjectList.GetGameObject(objectName);
        }
    }
}
