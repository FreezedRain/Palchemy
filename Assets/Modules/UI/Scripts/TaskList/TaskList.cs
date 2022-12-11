using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{

    public GameObject blip_prefab;

    private float taskWidth = 0.12f;
    private float taskHeight = 0.12f;

    private List<TaskListBlip> blips = new List<TaskListBlip>();

    // private void Start()
    // {
    //     Setup(5, 2);
    //
    //     SetStateExecuting(7, 4);
    // }

    public void Show()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(1f, 0f, 1f);
        LeanTween.scaleY(gameObject, 1f, 0.125f).setEaseOutCubic();
        print("Showing");
    }
    
    public void Hide()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = new Vector3(1f, 1f, 1f);
        LeanTween.scaleY(gameObject, 0f, 0.125f).setEaseInCubic();
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
                blip_obj.transform.localPosition = new Vector2(-width / 2 + j * width / (numTasksPerRow - 1), height / 2 - i * height / (float)numRows);
                blip_obj.transform.localScale = new Vector3(1f, 1f, 1f);

                blips.Add(blip_obj.GetComponent<TaskListBlip>());
            }
        }
    }

    public void SetStateLearning(List<Potions.Gameplay.GolemInputProvider.Task> tasks)
    {
        for (int i = 0; i < blips.Count; i++)
        {
            if (tasks != null && i < tasks.Count)
            {
                blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.IDLE, tasks[i].Type);
            } else
            {
                blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.NONE);
            }
        }
    }

    public void SetStateExecuting(List<Potions.Gameplay.GolemInputProvider.Task> tasks, int executingIndex)
    {
        for (int i = 0; i < blips.Count; i++)
        {
            if (i < tasks.Count)
            {
                if (i < executingIndex)
                {
                    blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.DONE, tasks[i].Type);
                }
                else if (i == executingIndex)
                {
                    blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.DOING, tasks[i].Type);
                }
                else
                {
                    blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.IDLE, tasks[i].Type);
                }
            }
            else
            {
                blips[i].SetState(TaskListBlip.TASK_BLIP_STATE.NONE);
            }
        }
    }
}
