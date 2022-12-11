using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Potions.Gameplay
{
    public static class CustomNavMesh
    {
        public static List<Vector3> CalculatePath(Vector3 from, Transform goal, LayerMask obstacleMask, float avoidRadius = 0.1f)
        {
            var path = new List<Vector3>();
            NavMeshPath navMeshPath = new();

            bool pathFound = false;
            float minPathLength = float.MaxValue;

            for (int i = 0; i < 8; i++)
            {
                Vector3 offset = Quaternion.AngleAxis(360f / 8 * i, Vector3.forward) * Vector2.right * 1f;
                NavMeshPath newPath = new();
                if (NavMesh.CalculatePath(from, goal.position + offset, NavMesh.AllAreas, newPath))
                {
                    pathFound = true;
                    float length = 0f;

                    for (int j = 0; j < newPath.corners.Length - 1; j++)
                    {
                        length += Vector3.Distance(newPath.corners[j], newPath.corners[j + 1]);
                    }

                    if (length < minPathLength)
                    {
                        minPathLength = length;
                        navMeshPath = newPath;
                    }
                    // Debug.Log($"Found path {i}, total length: {length}, best length: {minPathLength}");
                    // NavMeshHit _navMeshHit;
                    // NavMesh.SamplePosition(goal.position, out _navMeshHit, 2.5f, NavMesh.AllAreas);
                    // if (!NavMesh.CalculatePath(from, _navMeshHit.position, NavMesh.AllAreas, navMeshPath))
                    // {
                    //     return null;
                    // }
                }
            }

            if (!pathFound) return null;

            bool raycastEverHit = false;
            for (int i = 0; i < navMeshPath.corners.Length; i++)
            {
                Vector3 point = navMeshPath.corners[i];

                RaycastHit2D hit = Physics2D.CircleCast(point, avoidRadius, (goal.position - point).normalized, 999f, obstacleMask);
                

                if (hit.collider != null && hit.collider.transform.parent.gameObject != goal.gameObject)
                {
                    path.Add(point);
                }
                else
                {
                    raycastEverHit = true;
                    path.Add(point);
                    path.Add(goal.position);
                    break;
                }
            }
            
            if (!raycastEverHit)
                path.Add(goal.position);
            
            return path;
        }
    }
}