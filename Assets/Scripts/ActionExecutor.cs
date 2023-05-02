using UnityEngine;

public class ActionExecutor : MonoBehaviour
{
    public Action actionScriptableObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            actionScriptableObject.ExecuteAction();
        }
    }
}
