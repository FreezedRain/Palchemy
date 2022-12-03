using System;
using System.Collections;
using System.Collections.Generic;
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
            if (_currentState == State.Learn)
            {
                SetState(State.Work);
            }
            else
            {
                _teacher = interactor;
                SetState(State.Learn);
            }
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
            MoveTowards(_teacher.transform.position, 1.25f);
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
                StartCoroutine(CoDoTask(_tasks[_taskIdx]));
            }
        }

        private void OnTeacherInteracted(BaseInteractable interactable)
        {
            if (interactable is GolemInteractable)
            {
                return;
            }
            _tasks.Add(interactable);
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
            // Move towards the goal until we can interact
            while (_character.Interactor.Interactable != goal)
            {
                if (pathIndex < path.corners.Length && !MoveTowards(path.corners[pathIndex], 0.1f))
                {
                    pathIndex++;
                    print($"Path index {pathIndex}, total {path.corners}");
                }

                yield return null;
            }
            
            // Stop
            _inputState.Direction = Vector2.zero;
            
            // Interact with the goal (after a small delay)
            yield return new WaitForSeconds(0.2f);
            Interacted?.Invoke();

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
                    _teacher.Interacted += OnTeacherInteracted;
                    _tasks.Clear();
                    break;
                case State.Work:
                    _teacher.Interacted -= OnTeacherInteracted;
                    _taskIdx = -1;
                    break;
            }
            _currentState = state;
        }
        
        private List<BaseInteractable> _tasks;

        private CharacterLogic _character;

        private Interactor _teacher;

        private bool _taskInProgress;
        private int _taskIdx = -1;
        private InputState _inputState;
        
        private enum State
        {
            Idle,
            Learn,
            Work
        }

        private State _currentState = State.Idle;
    }
}