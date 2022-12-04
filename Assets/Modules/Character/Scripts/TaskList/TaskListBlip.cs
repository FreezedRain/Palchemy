using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListBlip : MonoBehaviour
{
    public enum TASK_BLIP_STATE
    {
        NONE,
        IDLE,
        DOING,
        DONE
    }

    public Sprite spriteIdle;
    public Sprite spriteDoing;
    public Sprite spriteDone;

    private TASK_BLIP_STATE state = TASK_BLIP_STATE.NONE;

    public SpriteRenderer detail;

    public void SetState(TASK_BLIP_STATE newState)
    {
        state = newState;
        
        detail.transform.eulerAngles = Vector3.zero;
        
        switch(newState)
        {
            case TASK_BLIP_STATE.NONE:
                detail.sprite = null;
                break;
            case TASK_BLIP_STATE.IDLE:
                detail.sprite = spriteIdle;
                break;
            case TASK_BLIP_STATE.DOING:
                detail.sprite = spriteDoing;
                break;
            case TASK_BLIP_STATE.DONE:
                detail.sprite = spriteDone;
                break;
        }
    }

    private void Update()
    {
        if (state == TASK_BLIP_STATE.DOING)
        {
            detail.transform.eulerAngles = new Vector3(0, 0, detail.transform.eulerAngles.z - Time.deltaTime * 200);
        }
    }
}
