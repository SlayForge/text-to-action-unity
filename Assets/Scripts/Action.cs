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

    public void ExecuteAction()
    {
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
