using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Potions.Gameplay
{
    public class GolemInputProvider : MonoBehaviour, IInputProvider
    {
        public event Action Interacted;
        public event Action AltInteracted;

        public ItemHolder ItemHolder => _character.ItemHolder;

        public InputState GetState() => _inputState;

        public void Interact(Interactor interactor)
        {
            var holder = interactor.Character.ItemHolder;
            string heldItemId = holder.ItemId;
            holder.SetItem(_character.ItemHolder.ItemId);
            _character.ItemHolder.SetItem(heldItemId);
            _character.Visuals.Bump();
        }
        
        public void AltInteract(Interactor interactor)
        {
            switch (_currentState)
            {
                case State.Idle:
                    _teacher = interactor;
                    SetState(State.Learn);
                    break;
                case State.Learn:
                    SetState(State.Work);
                    break;
                case State.Work:
                    _teacher = interactor;
                    SetState(State.Learn);
                    break;
            }
            _character.Visuals.Bump();
        }

        private void Awake()
        {
            _tasks = new();
            _character = GetComponent<CharacterLogic>();
        }

        private void Start()
        {
            _taskList.Setup(_maxTaskCount, 1);
            _taskList.SetStateLearning(0);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case State.Idle:
                    _inputState.Direction = Vector2.zero;
                    break;
                case State.Learn:
                    DoLearn();
                    break;
                case State.Work:
                    DoWork();
                    break;
            }
        }

        private void DoLearn()
        {
            MoveTowards(_teacher.transform.position, 1.5f);
            _character.LookTowards((_teacher.transform.position - transform.position).normalized);
        }

        private void DoWork()
        {
            if (_tasks.Count == 0)
            {
                SetState(State.Idle);
                return;
            }
            
            if (!_taskInProgress)
            {
                _taskIdx = (_taskIdx + 1) % _tasks.Count;
                _taskCoroutine = StartCoroutine(CoDoTask(_currentTask));
                _taskList.SetStateExecuting(_tasks.Count, _taskIdx);
            }
        }

        private IEnumerator CoDoTask(BaseInteractable goal)
        {
            _taskInProgress = true;
            _debugPath = null;
            
            // Find path to the goal
            var path = CustomNavMesh.CalculatePath(transform.position, goal.transform, _pathMask);
            
            if (path == null)
            {
                _taskInProgress = false;
                Debug.LogWarning($"Pathfinding failed for {transform.name}!");
                yield break;
            }

            _debugPath = path;

            int pathIndex = 0;
            // Wait until we can interact
            while (true)
            {
                // Advance to next path point if close
                if ((path[pathIndex] - transform.position).magnitude < 0.25f)
                {
                    pathIndex = Mathf.Clamp(pathIndex + 1, 0, path.Count - 1);
                }
                
                Vector2 movementDirection = path[pathIndex] - transform.position;
                
                // If we should steer, rotate the direction
                if (ShouldSteer(movementDirection.normalized, out float amount))
                {
                    movementDirection = Quaternion.AngleAxis(-amount * _steerAmount, Vector3.forward) *
                                        movementDirection;
                }

                MoveTowards(transform.position + (Vector3) movementDirection, 0.0f);

                // If we can interact
                if (_character.Interactor.ClosestInteractable == goal)
                {
                    // Stop and wait a bit
                    _inputState.Direction = Vector2.zero;
                    _character.LookTowards((_currentTask.transform.position - transform.position).normalized);

                    if (_character.Interactor.CanInteract)
                    {
                        yield return new WaitForSeconds(0.2f);
                        // If we can still interact, do it and exit the loop
                        if (_character.Interactor.ClosestInteractable == goal && _character.Interactor.CanInteract)
                        {
                            Interacted?.Invoke();
                            break;
                        }
                    }
                    else if (_currentTask.CanSkip(_character.Interactor))
                    {
                        yield return new WaitForSeconds(0.4f);
                        _taskInProgress = false;
                        yield break;
                    }
                }
                // Otherwise, wait
                yield return null;
            }

            // Wait a bit after interacting
            yield return new WaitForSeconds(0.2f);
            _taskInProgress = false;
        }

        private bool MoveTowards(Vector3 point, float stoppingDistance)
        {
            Vector3 diff = point - transform.position;
            if (diff.magnitude > stoppingDistance)
            {
                _inputState.Direction = diff.normalized;
                return true;
            }
            _inputState.Direction = Vector2.zero;
            return false;
        }

        private void SetState(State state)
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Learn:
                    if (_taskCoroutine != null)
                        StopCoroutine(_taskCoroutine);
                    // _character.ItemHolder.SetItem(null);
                    _teacher.Interacted += OnTeacherInteracted;
                    _tasks.Clear();
                    _taskList.SetStateLearning(0);
                    break;
                case State.Work:
                    _teacher.Interacted -= OnTeacherInteracted;
                    _taskIdx = -1;
                    _taskInProgress = false;
                    _taskList.SetStateExecuting(_tasks.Count, 0);
                    break;
            }
            _currentState = state;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (_debugPath == null)
                return;
            for (int i = 0; i < _debugPath.Count - 1; i++)
            {
                Gizmos.DrawWireSphere(_debugPath[i], 0.2f);
                Gizmos.DrawLine(_debugPath[i], _debugPath[i + 1]);
            }

            // Gizmos.color = Color.yellow;
            // Gizmos.DrawLine(transform.position, transform.position + (Vector3) _character.Forward);
            //
            // Gizmos.color = Color.cyan;
            // Gizmos.DrawLine(transform.position, closestGolem.transform.position);
        }

        private bool ShouldSteer(Vector2 moveDirection, out float amount)
        {
            // Move towards the goal
            float totalAngle = 0f;
            int totalObstacles = 0;

            // Find average avoidance angle
            foreach (var golem in CharacterLogic.AllCharacters)
            {
                // Skip ourselves
                if (golem == _character || !golem.IsMoving)
                    continue;
                
                Vector2 obstacleDiff = golem.transform.position - transform.position;
                float obstacleAngle = Vector2.SignedAngle(moveDirection, obstacleDiff.normalized) / 90f;
                float golemAngle = Vector2.SignedAngle(moveDirection, golem.Forward) / 90f;

                // If close enough and facing towards each other
                if (obstacleDiff.magnitude < 2f && Mathf.Abs(obstacleAngle) < 1f)
                {
                    totalObstacles++;
                    totalAngle += obstacleAngle;
                }
            }

            // If no obstacles, don't steer
            if (totalObstacles == 0)
            {
                amount = 0f;
                return false;
            }
            
            // Compute steering amount
            float averageAngle = totalAngle / totalObstacles;
            amount = (1f - Mathf.Abs(averageAngle)) * Mathf.Sign(averageAngle);
            return true;
        }

        private void OnTeacherInteracted(BaseInteractable interactable)
        {
            if (interactable is GolemInteractable golemInteractable || _tasks.Count >= _maxTaskCount)
                return;
            _tasks.Add(interactable);
            _taskList.SetStateLearning(_tasks.Count);
            _character.Visuals.Bump();
        }
        
        private enum State
        {
            Idle,
            Learn,
            Work
        }

        [SerializeField]
        private float _steerAmount;
        [SerializeField]
        private int _maxTaskCount;
        [SerializeField]
        private TaskList _taskList;
        [SerializeField]
        private LayerMask _pathMask;

        private CharacterLogic _character;
        private Interactor _teacher;

        private Coroutine _taskCoroutine;
        private BaseInteractable _currentTask => _taskIdx < _tasks.Count ? _tasks[_taskIdx] : null;

        private State _currentState = State.Idle;
        private List<BaseInteractable> _tasks;
        private bool _taskInProgress;
        private int _taskIdx = -1;
        private InputState _inputState;

        // Debug
        private List<Vector3> _debugPath;
    }
}