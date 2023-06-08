using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace SlayForge
{

    public abstract class ScriptedActionNPC : MonoBehaviour
    {
        public TextAsset actionFile;
        public SceneInteractionDescriber sceneInteractionDescriber;
        private Dictionary<string, Action<string, Action>> actionHandlers = new Dictionary<string, Action<string, Action>>();
        private IEnumerator currentAction;

        public event Action OnInteractComplete;

        public event Action OnMoveComplete;

        public event Action OnPointComplete;

        public event Action OnSayComplete;

        public abstract void Interact(string interactableName, Action OnDone);

        public void InterpretActions(XmlNode actionsNode)
        {
            foreach (XmlNode childNode in actionsNode.ChildNodes)
            {
                if (childNode.Name == "simultaneous")
                    ExecuteActionsSimultaneously(childNode);
                else if (childNode.Name == "sequential")
                    ExecuteActionsSequentially(childNode, i);
            }
        }

        public abstract void MoveTo(string destination, Action OnDone);

        public abstract void PointAt(string destination, Action OnDone);

        public abstract void Say(string text, Action OnDone);

        protected void CompleteCurrentAction(string type)
        {
            currentAction = null;
            switch (type)
            {
                case "walk":
                    OnMoveComplete?.Invoke();
                    break;

                case "say":
                    OnSayComplete?.Invoke();
                    break;

                case "point":
                    OnPointComplete?.Invoke();
                    break;

                case "interact":
                    OnInteractComplete?.Invoke();
                    break;
            }
        }

        private void Awake()
        {
            actionHandlers["walk"] = (destination, onComplete) => MoveTo(destination, onComplete);
            actionHandlers["say"] = (text, onComplete) => Say(text, onComplete);
            actionHandlers["point"] = (destination, onComplete) => PointAt(destination, onComplete);
            actionHandlers["interact"] = (interactableName, onComplete) => Interact(interactableName, onComplete);
        }

        private void ExecuteAction(XmlNode actionNode, Action OnDone)
        {
            string type = actionNode.Attributes["type"].Value;
            if (actionHandlers.TryGetValue(type, out var handler))
            {
                string parameter = null;
                if (type == "walk" || type == "point")
                {
                    parameter = actionNode.Attributes["destination"]?.Value;
                }
                else if (type == "say")
                {
                    parameter = actionNode.Attributes["text"]?.Value;
                }

                handler(parameter, OnDone);

            }
        }

        int i = 0;
        private void ExecuteActionsSequentially(XmlNode sequentialNode, int childIndex)
        {

            if (sequentialNode == null || sequentialNode.ChildNodes.Count == 0 || i >= sequentialNode.ChildNodes.Count)
            {
                Debug.Log("No sequential actions to execute.");
                i = 0;
                return;
            }

            XmlNode actionNode = sequentialNode.ChildNodes[i];
            ExecuteAction(actionNode, () => { ExecuteActionsSequentially(sequentialNode, i); });
            i++;
        }

        private void ExecuteActionsSimultaneously(XmlNode simultaneousNode)
        {
            foreach (XmlNode actionNode in simultaneousNode.ChildNodes)
            {
                ExecuteAction(actionNode, () => { });
            }
            Debug.Log("All simultaneous actions have finished.");
        }
        private void Start()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(actionFile.text);
            XmlNode actionsNode = doc.DocumentElement.SelectSingleNode("/actions");
            InterpretActions(actionsNode);
        }
    }
}