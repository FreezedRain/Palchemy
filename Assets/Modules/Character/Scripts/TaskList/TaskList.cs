using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{

    public GameObject blip_prefab;

    private float taskWidth = 0.15f;
    private float taskHeight = 0.15f;

    private List<TaskListBlip> blips = new List<TaskListBlip>();

    private void Start()
    {
        Setup(5, 2);

        SetStateExecuting(7, 4);
    }

    public void Setup(int numTasksPerRow, int numRows)
    {
        float width = numTasksPerRow * taskWidth;
        float height = numRows * taskHeight;

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numTasksPerRow; j++)
            {

                GameObject blip_obj = Instantiate(blip_prefab);

                blip_obj.transform.SetParent(transform);
                blip_obj.transform.localPosition = new Vector2(-width / 2 + j * width / numTasksPerRow, height / 2 - i * height / numRows);

                blips.Add(blip_obj.GetComponent<TaskListBlip>());
            }
        }
    }

    public void SetStateLearning(int numTasksLearnt)
    {
        for (int i = 0; i < blips.Count; i++)
        {
            if (i < numTasksLearnt)
            {
                blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.IDLE);
            } else
            {
                blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.NONE);
            }
        }
    }

    public void SetStateExecuting(int numTasksLearnt, int executingIndex)
    {
        for (int i = 0; i < blips.Count; i++)
        {
            if (i < numTasksLearnt)
            {
                if (i < executingIndex - 1)
                {
                    blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.DONE);
                }
                else if (i == executingIndex - 1)
                {
                    blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.DOING);
                }
                else
                {
                    blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.IDLE);
                }
            }
            else
            {
                blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.NONE);
            }
        }
    }
}
