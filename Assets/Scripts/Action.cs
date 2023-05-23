using UnityEngine;

public enum ActionType
{
    None,
    Move,
    Jump,
    Attack,
}

[CreateAssetMenu(fileName = "New Action", menuName = "Actions/Action")]
public class Action : ScriptableObject
{
    public ActionType actionType;
    public float duration; // duration of the action in seconds

    private float startTime; // time when the action started

    public bool IsFinished
    {
        get { return Time.time - startTime >= duration; }
    }

    public void ExecuteAction()
    {
        startTime = Time.time;
        switch (actionType)
        {
            case ActionType.Move:
                Move();
                break;
            case ActionType.Jump:
                Jump();
                break;
            case ActionType.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    private void Move()
    {
        Debug.Log("Moving...");
    }

    private void Jump()
    {
        Debug.Log("Jumping...");
    }

    private void Attack()
    {
        Debug.Log("Attacking...");
    }
}
