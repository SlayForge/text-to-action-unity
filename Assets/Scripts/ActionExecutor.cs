using System.Collections.Generic;
using UnityEngine;

public class ActionExecutor : MonoBehaviour
{
    public List<Action> actions;
    public bool executeSequentially; // if true, execute actions one by one in the list
    public bool loopActions; // if true, loop the list of actions when finished
    public bool executeMultiple; // if true, execute multiple actions simultaneously
    public float timeBetweenActions; // time between each action when executing sequentially
    public float maxExecutionTime; // maximum time to execute all actions

    private List<Action> executingActions = new List<Action>(); // list of actions currently being executed
    private float startTime; // time when the actions started executing
    private float currentTime; // current time since the actions started executing

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartExecutingActions();
        }

        if (executingActions.Count > 0)
        {
            currentTime = Time.time - startTime;
            if (currentTime >= maxExecutionTime)
            {
                Debug.Log("Reached maximum execution time.");
                StopExecutingActions();
            }
            else
            {
                for (int i = executingActions.Count - 1; i >= 0; i--)
                {
                    Action action = executingActions[i];
                    if (action.IsFinished)
                    {
                        executingActions.RemoveAt(i);
                        Debug.Log($"Finished action {action.actionType}.");
                    }
                }
            }
        }
    }

    private void StartExecutingActions()
    {
        if (actions.Count == 0)
        {
            Debug.LogWarning("No actions to execute.");
            return;
        }

        executingActions.Clear();

        if (executeSequentially)
        {
            StartCoroutine(ExecuteActionsSequentially());
        }
        else
        {
            foreach (Action action in actions)
            {
                if (executeMultiple)
                {
                    action.ExecuteAction();
                    executingActions.Add(action);
                }
                else
                {
                    StartCoroutine(ExecuteActionWithDelay(action, 0));
                }
            }

            if (!executeMultiple)
            {
                StartCoroutine(WaitForActionsToFinish());
            }
        }

        startTime = Time.time;
        currentTime = 0;
    }

    public IEnumerator<WaitForSeconds> ExecuteActionsSequentially()
    {
        foreach (Action action in actions)
        {
            float delay = executeMultiple ? 0 : timeBetweenActions;

            action.ExecuteAction();
            executingActions.Add(action);

            yield return new WaitForSeconds(delay);

            while (executingActions.Contains(action))
            {
                yield return null;
            }

            if (loopActions && action == actions[actions.Count - 1])
            {
                StartCoroutine(ExecuteActionsSequentially());
            }
        }

        if (!loopActions)
        {
            StopExecutingActions();
        }
    }

    public IEnumerator<WaitForSeconds> ExecuteActionWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);

        action.ExecuteAction();
        executingActions.Add(action);

        if (action.duration > 0)
        {
            yield return new WaitForSeconds(action.duration);
        }
        else
        {
            while (executingActions.Contains(action))
            {
                yield return null;
            }
        }

        executingActions.Remove(action);

        if (loopActions && action == actions[actions.Count - 1])
        {
            StartCoroutine(ExecuteActionWithDelay(actions[0], timeBetweenActions));
        }
    }
    private IEnumerator<WaitForSeconds> WaitForActionsToFinish()
    {
        while (executingActions.Count > 0)
        {
            yield return null;
        }

        StopExecutingActions();
    }

    private void StopExecutingActions()
    {
        StopAllCoroutines();
        executingActions.Clear();
        Debug.Log("Stopped executing actions.");
    }

}