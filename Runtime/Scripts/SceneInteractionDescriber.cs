using System;
using System.Collections.Generic;
using UnityEngine;

 namespace SlayForge
{
    [CreateAssetMenu(fileName = "New SceneInteractionDescriber", menuName = "ScriptableObject/SceneInteractionDescriber")]
    public class SceneInteractionDescriber : ScriptableObject
    {
        public List<Destination> destinations;
        public List<SceneObject> interactables;

        [Serializable]
        public class Destination
        {
            public string name;
            public Vector3 position;
        }

        [Serializable]
        public class SceneObject
        {
            public string name;
            public Vector3 position;
            public bool alreadyInteracted;
            Action OnInteract;
        }
    }
}
