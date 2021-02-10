using System;
using App;
using Game.Data;
using Libs.GameFramework;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Actors.Components
{
    public class ObstacleDetector : MonoBehaviour
    {
        [SerializeField] private float characterHeight = 2;

        [SerializeField] private float stepHeight = .4f;
        [SerializeField] private float maxHeight = 2.4f;
        [SerializeField] private float radius = .5f;
        [SerializeField] private float maxHorizontalAngle = 35;
        private float minHeight => stepHeight;


        public bool CheckClimbAbility(out ClimbPointInfo info)
        {
            info = default;
            if (TryFindEdge(out var edgeHit, out var hitCenter))
            {
                if (CheckCanEnter(edgeHit.point, hitCenter, out var availableHeight))
                {
                    var startPoint = edgeHit.point;
                    startPoint.y = transform.position.y;

                    var forwardVector = -edgeHit.normal;
                    forwardVector.y = 0;
                    forwardVector.Normalize();

                    var deltaAngle = Vector3.Angle(transform.forward, forwardVector);
                    if (deltaAngle > maxHorizontalAngle)
                    {
                        return false;
                    }

                    // var climbHeight = edgeHit.point.y - transform.position.y;


                    var climbType = GetClimbType(edgeHit.point, forwardVector);

                    Debug.DrawRay(edgeHit.point, forwardVector,
                        climbType == ClimbType.Over ? Color.green : Color.blue);
                    info = new ClimbPointInfo
                    {
                        startPoint = startPoint,
                        climbType = climbType,
                        climbHeight = availableHeight
                    };
                    return true;
                }
            }

            // show = false;

            return false;
        }

        private bool TryFindEdge(out RaycastHit edgeHit, out Vector3 hitCenter)
        {
            var currentHeight = minHeight;
            var forward = transform.forward;

            while (currentHeight < maxHeight)
            {
                var point = transform.position;
                currentHeight += radius;
                point.y += currentHeight;
                if (Physics.SphereCast(point, radius, forward, out var hit, 1))
                {
                    if (hit.normal.y > 0.01f) //edge
                    {
                        Debug.DrawLine(point, hit.point, Color.green);
                        edgeHit = hit;
                        hitCenter = point + forward * hit.distance;
                        return true;
                    }
                }
            }

            edgeHit = default;
            hitCenter = default;
            return false;
        }

        private bool CheckCanEnter(Vector3 edgePoint, Vector3 hitCenter, out float climbHeight)
        {
            //check can climb
            if (Physics.CheckCapsule(edgePoint + Vector3.up * .1f, edgePoint + Vector3.up * characterHeight, 1))
            {
                var ray = new Ray(hitCenter, Vector3.down);

                if (Physics.Raycast(ray, out var hit, maxHeight))
                {
                    climbHeight = edgePoint.y - hit.point.y;
                    var d = Vector3.Distance(hit.point, edgePoint);
                    var angle = Mathf.Asin(climbHeight / d) * Mathf.Rad2Deg;
                    Debug.DrawLine(ray.origin, ray.origin + Vector3.down * hit.distance, Color.blue);
                    return angle > 45;
                }
                else
                {
                    Debug.DrawLine(ray.origin, ray.origin + Vector3.down * maxHeight, Color.green);
                    climbHeight = (edgePoint - transform.position).y;
                    return true;
                }

            }

            climbHeight = default;
            return false;
        }

        private ClimbType GetClimbType(Vector3 edgePoint, Vector3 climbVector)
        {
            for (var deep = 0f; deep < .5f; deep += .1f)
            {
                var point = Vector3.up + edgePoint + climbVector * deep;
                Debug.DrawLine(point, point + Vector3.down * (1 + stepHeight));
                if (!Physics.Raycast(point, Vector3.down, 1 + stepHeight))
                {
                    //select animation from climg height;
                    Debug.DrawRay(edgePoint, climbVector, Color.green);
                    return ClimbType.Over;
                }
            }

            return ClimbType.UpTo;
        }
    }
}