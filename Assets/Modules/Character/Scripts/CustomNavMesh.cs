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
            if (!NavMesh.CalculatePath(from, goal.position, NavMesh.AllAreas, navMeshPath))
            {
                NavMeshHit _navMeshHit;
                NavMesh.SamplePosition(goal.position, out _navMeshHit, 2.5f, NavMesh.AllAreas);
                if (!NavMesh.CalculatePath(from, _navMeshHit.position, NavMesh.AllAreas, navMeshPath))
                {
                    return null;
                }
            }

            
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
                    path.Add(point);
                    path.Add(goal.position);
                    break;
                }
            }
            
            return path;
        }
    }
}