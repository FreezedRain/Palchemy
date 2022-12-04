using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    SetState(State.Idle);
                    break;
            }
            _character.Visuals.Bump();
        }

        private void Awake()
        {
            _tasks = new();
            _character = GetComponent<CharacterLogic>();
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
                print($"Going to task {_tasks[_taskIdx]}");
                _taskCoroutine = StartCoroutine(CoDoTask(_tasks[_taskIdx]));
            }
        }

        private IEnumerator CoDoTask(BaseInteractable goal)
        {
            _taskInProgress = true;
            NavMeshPath path = new NavMeshPath();
            // Find path to the goal
            if (!NavMesh.CalculatePath(transform.position, goal.transform.position, NavMesh.AllAreas, path))
            {
                _taskInProgress = false;
                yield break;
            }

            int pathIndex = 0;
            // Wait until we can interact
            while (true)
            {
                // Advance to next path point if close
                if ((path.corners[pathIndex] - transform.position).magnitude < 0.125f)
                {
                    pathIndex = Mathf.Clamp(pathIndex + 1, 0, path.corners.Length - 1);
                }
                
                Vector2 movementDirection = path.corners[pathIndex] - transform.position;

                // If on the last point of the path, move towards the goal
                // if (pathIndex == path.corners.Length - 1)
                // {
                //     movementDirection =_tasks[_taskIdx].transform.position - transform.position;
                // }
                
                // If we should steer, rotate the direction
                if (ShouldSteer(out float amount))
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
                    if (_taskCoroutine != null)
                        StopCoroutine(_taskCoroutine);
                    break;
                case State.Learn:
                    _teacher.Interacted += OnTeacherInteracted;
                    _tasks.Clear();
                    break;
                case State.Work:
                    _teacher.Interacted -= OnTeacherInteracted;
                    _taskIdx = -1;
                    _taskInProgress = false;
                    break;
            }
            _currentState = state;
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.green;
        //     if (path == null)
        //         return;
        //     for (int i = 0; i < path.corners.Length - 1; i++)
        //     {
        //         Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        //     }
        //
        //     Gizmos.color = Color.cyan;
        //     Gizmos.DrawLine(transform.position, closestGolem.transform.position);
        // }

        private bool ShouldSteer(out float amount)
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
                float obstacleAngle = Vector2.SignedAngle(_character.Forward, obstacleDiff.normalized) / 90f;

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
            if (interactable is GolemInteractable golemInteractable)
                return;
            _tasks.Add(interactable);
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

        private CharacterLogic _character;
        private Interactor _teacher;

        private Coroutine _taskCoroutine;

        private State _currentState = State.Idle;
        private List<BaseInteractable> _tasks;
        private bool _taskInProgress;
        private int _taskIdx = -1;
        private InputState _inputState;
    }
}