using UnityEngine;

namespace Potions.Gameplay
{
    public class TaskListBlip : MonoBehaviour
    {
        public enum TASK_BLIP_STATE
        {
            NONE,
            IDLE,
            DOING,
            DONE
        }

        public Sprite SpriteUp;
        public Sprite SpriteDown;

        public SpriteRenderer Detail;
        public SpriteRenderer Progress;

        public void SetState(TASK_BLIP_STATE newState,
            BaseInteractable.InteractionType type = BaseInteractable.InteractionType.Any)
        {
            _state = newState;

            Detail.transform.eulerAngles = Vector3.zero;

            switch (newState)
            {
                case TASK_BLIP_STATE.NONE:
                    Progress.enabled = false;
                    Detail.sprite = null;
                    break;
                case TASK_BLIP_STATE.IDLE:
                    Progress.enabled = false;
                    if (type == BaseInteractable.InteractionType.Pickup) Detail.sprite = SpriteUp;
                    if (type == BaseInteractable.InteractionType.Drop) Detail.sprite = SpriteDown;
                    Detail.color = new Color(1, 1, 1, 0.5f);
                    break;
                case TASK_BLIP_STATE.DOING:
                    Progress.enabled = true;
                    if (type == BaseInteractable.InteractionType.Pickup) Detail.sprite = SpriteUp;
                    if (type == BaseInteractable.InteractionType.Drop) Detail.sprite = SpriteDown;
                    Detail.color = new Color(1, 1, 1, 1f);
                    break;
                case TASK_BLIP_STATE.DONE:
                    Progress.enabled = false;
                    if (type == BaseInteractable.InteractionType.Pickup) Detail.sprite = SpriteUp;
                    if (type == BaseInteractable.InteractionType.Drop) Detail.sprite = SpriteDown;
                    Detail.color = new Color(1, 1, 1, 0.5f);
                    break;
            }
        }

        private void Update()
        {
            if (_state == TASK_BLIP_STATE.DOING)
            {
                Progress.transform.eulerAngles =
                    new Vector3(0, 0, Progress.transform.eulerAngles.z - Time.deltaTime * 200);
            }
        }

        private TASK_BLIP_STATE _state = TASK_BLIP_STATE.NONE;
    }
}