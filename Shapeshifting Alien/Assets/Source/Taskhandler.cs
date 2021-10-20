using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taskhandler : MonoBehaviour
{
    private List<GameObject> completedTasks;
    // Start is called before the first frame update
    void Start()
    {
        completedTasks = new List<GameObject>();
    }

    public bool TaskCompleted(GameObject task)
    {
        return completedTasks.Contains(task);
    }

    public void CompleteTask(GameObject task)
    {
        completedTasks.Add(task);
    }

    public int NumTasks()
    {
        return completedTasks.Count();
    }
}
