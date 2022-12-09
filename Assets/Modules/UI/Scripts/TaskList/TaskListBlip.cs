using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Potions.Gameplay.BaseInteractable;

public class TaskListBlip : MonoBehaviour
{
    public enum TASK_BLIP_STATE
    {
        NONE,
        IDLE,
        DOING,
        DONE
    }

    public Sprite spriteUp;
    public Sprite spriteDown;

    private TASK_BLIP_STATE state = TASK_BLIP_STATE.NONE;

    public SpriteRenderer detail;
    public SpriteRenderer progress;

    public void SetState(TASK_BLIP_STATE newState, InteractionType type = InteractionType.Any)
    {
        state = newState;
        
        detail.transform.eulerAngles = Vector3.zero;
        
        switch(newState)
        {
            case TASK_BLIP_STATE.NONE:
                progress.enabled = false;
                detail.sprite = null;
                break;
            case TASK_BLIP_STATE.IDLE:
                progress.enabled = false;
                if (type == InteractionType.Pickup) detail.sprite = spriteUp;
                if (type == InteractionType.Drop) detail.sprite = spriteDown;
                detail.color = new Color(1, 1, 1, 0.5f);
                break;
            case TASK_BLIP_STATE.DOING:
                progress.enabled = true;
                if (type == InteractionType.Pickup) detail.sprite = spriteUp;
                if (type == InteractionType.Drop) detail.sprite = spriteDown;
                detail.color = new Color(1, 1, 1, 1f);
                break;
            case TASK_BLIP_STATE.DONE:
                progress.enabled = false;
                if (type == InteractionType.Pickup) detail.sprite = spriteUp;
                if (type == InteractionType.Drop) detail.sprite = spriteDown;
                detail.color = new Color(1, 1, 1, 0.5f);
                break;
        }
    }

    private void Update()
    {
        if (state == TASK_BLIP_STATE.DOING)
        {
            progress.transform.eulerAngles = new Vector3(0, 0, progress.transform.eulerAngles.z - Time.deltaTime * 200);
        }
    }
}
