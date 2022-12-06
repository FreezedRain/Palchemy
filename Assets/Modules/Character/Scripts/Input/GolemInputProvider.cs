using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace Potions.Gameplay
{
    public class GolemInputProvider : MonoBehaviour, IInputProvider
    {
        public event Action Interacted;

        public InputState GetState() => _inputState;

        public void Interact(Interactor interactor)
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
            _seeker = GetComponent<Seeker>();
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
            Path path = null;
            int pathIndex = 0;
            // var pathCoroutine = StartCoroutine(CoUpdatePath(goal.transform, 0.5f, p =>
            // {
            //     path = p;
            //     pathIndex = 0;
            // }));
            _seeker.StartPath(transform.position, goal.transform.position, p => path = p);

            // Wait until we can interact
            while (true)
            {
                if (path != null)
                {
                    // Advance to next path point if close
                    if ((path.vectorPath[pathIndex] - transform.position).magnitude < 0.25f)
                    {
                        pathIndex = Mathf.Clamp(pathIndex + 1, 0, path.vectorPath.Count - 1);
                    }
                
                    Vector2 movementDirection = path.vectorPath[pathIndex] - transform.position;

                    // // If on the last point of the path, move towards the goal
                    // if (pathIndex == path.vectorPath.Count - 1 && _currentTask)
                    // {
                    //     movementDirection =_currentTask.transform.position - transform.position;
                    // }
                
                    // If we should steer, rotate the direction
                    if (ShouldSteer(movementDirection.normalized, out float amount))
                    {
                        movementDirection = Quaternion.AngleAxis(-amount * _steerAmount, Vector3.forward) *
                                            movementDirection;
                    }

                    MoveTowards(transform.position + (Vector3) movementDirection, 0.05f);
                }
                else
                {
                    _inputState.Direction = Vector2.zero;
                }

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
            // StopCoroutine(pathCoroutine);
        }

        private IEnumerator CoUpdatePath(Transform goal, float delay, OnPathDelegate callback)
        {
            while (true)
            {
                if (!goal)
                {
                    callback?.Invoke(null);
                    yield return null;
                }
                _seeker.StartPath(transform.position, goal.transform.position, callback);
                yield return new WaitForSeconds(delay);
            }
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
                    if (_taskCoroutine != null)
                        StopCoroutine(_taskCoroutine);
                    break;
                case State.Learn:
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
            for (int i = 0; i < _debugPath.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(_debugPath.corners[i], _debugPath.corners[i + 1]);
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
                if (golem == _character)
                    continue;
                
                Vector2 obstacleDiff = golem.transform.position - transform.position;
                float obstacleAngle = Vector2.SignedAngle(moveDirection, obstacleDiff.normalized) / 90f;
                float golemAngle = Vector2.SignedAngle(moveDirection, golem.Forward) / 90f;

                // If close enough and facing towards each other
                if (obstacleDiff.magnitude < 2.5f && Mathf.Abs(obstacleAngle) < 1f)
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

        private CharacterLogic _character;
        private Seeker _seeker;
        private Interactor _teacher;

        private Coroutine _taskCoroutine;
        private BaseInteractable _currentTask => _taskIdx < _tasks.Count ? _tasks[_taskIdx] : null;

        private State _currentState = State.Idle;
        private List<BaseInteractable> _tasks;
        private bool _taskInProgress;

        private int _taskIdx = -1;
        private InputState _inputState;

        // Debug
        private NavMeshPath _debugPath;
    }
}