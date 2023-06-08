using System;
using System.Collections;
using UnityEngine;

namespace SlayForge
{
    public class SimpleBotController : ScriptedActionNPC
    {
        public override void Interact(string interactableName, Action OnDone)
        {
            Debug.Log($"Interacting with {interactableName}");
            CompleteCurrentAction("interact");
        }

        public override void MoveTo(string destination, Action OnDone)
        {
            Vector3 destinationPosition = base.sceneInteractionDescriber.destinations.Find(_ => _.name == destination).position;

            StartCoroutine(MoveToPosition(destinationPosition, 2, OnDone));
        }

        private IEnumerator MoveToPosition(Vector3 target, float duration, Action OnDone)
        {
            Vector3 startPosition = this.gameObject.transform.position;

            float timeToFinish = Time.time + duration;

            while (Time.time < timeToFinish)
            {
                float t = 1f - (timeToFinish - Time.time) / duration;

                this.gameObject.transform.position = Vector3.Lerp(startPosition, target, t);

                yield return null;
            }

            this.gameObject.transform.position = target;

            OnDone.Invoke();
        }

        public override void PointAt(string destination, Action OnDone)
        {
            Debug.Log($"Pointing at {destination}");
            CompleteCurrentAction("point");
        }

        public override void Say(string text, Action OnDone)
        {
            Debug.Log($"Saying {text}");
            CompleteCurrentAction("say");
        }
    }
}