using System.Collections.Generic;
using UnityEngine;

public class TextActionInterpreter : MonoBehaviour
{
    [SerializeField] private TextAsset textFile;
    [SerializeField] private string text;
    [SerializeField] private ActionExecutor actionExecutor;

    private Dictionary<string, ActionType> tagToAction = new Dictionary<string, ActionType>
    {
        { "<Move>", ActionType.Move },
        { "<Jump>", ActionType.Jump },
        { "<Attack>", ActionType.Attack },
    };

    private void Start()
    {
        if (textFile != null)
        {
            text = textFile.text;
        }

        ProcessText(text);
    }

    private void ProcessText(string inputText)
    {
        string[] words = inputText.Split(' ');

        foreach (string word in words)
        {
            if (tagToAction.TryGetValue(word, out ActionType actionType))
            {
                ExecuteActionByTag(actionType);
            }
            else
            {
                // Regular text without tags can be processed here, if needed
            }
        }
    }

    private void ExecuteActionByTag(ActionType actionType)
    {
        Action actionToExecute = actionExecutor.actions.Find(action => action.actionType == actionType);

        if (actionToExecute != null)
        {
            actionExecutor.StartCoroutine(actionExecutor.ExecuteActionWithDelay(actionToExecute, 0));
        }
        else
        {
            Debug.LogWarning($"No action found for tag: {actionType}");
        }
    }
}
