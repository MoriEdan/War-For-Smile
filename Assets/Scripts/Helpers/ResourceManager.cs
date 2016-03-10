using UnityEngine;
using System.Collections;

namespace Helpers
{
    public static class ResourceManager
    {
        public static bool isDoingSetup { get; set; }

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
