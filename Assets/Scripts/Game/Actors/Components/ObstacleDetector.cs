﻿using System;
using Game.Data;
using Libs.GameFramework;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Actors.Components
{
    public class ObstacleDetector : MonoBehaviour
    {
        private bool show;
        private Vector3 gizmoPoint;

        [SerializeField] private float characterHeight = 2;

        [SerializeField] private float stepHeight = .4f;
        [SerializeField] private float maxHeight = 2.4f;
        [SerializeField] private float radius = .5f;
        [SerializeField] private float maxHorizontalAngle = 35;
        private float minHeight => stepHeight;


        public bool CheckClimbAbility(out ClimbPointInfo info)
        {
            info = default;
            show = true;
            if (TryFindEdge(out var edgeHit, out var hitCenter))
            {
                if (CheckCanEnter(edgeHit.point))
                {
                    var availableHeight = GetHeightAvailable(hitCenter);
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

                    Debug.DrawRay(edgeHit.point, forwardVector, climbType == ClimbType.Over ? Color.green : Color.blue);
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

        private float GetHeightAvailable(Vector3 hitCenter)
        {
            var ray = new Ray(hitCenter, Vector3.down);
            return Physics.Raycast(ray, out var hit, maxHeight) ? hit.distance : maxHeight;
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
                if (Physics.SphereCast(point, radius, forward, out var hit, radius))
                {
                    gizmoPoint = point + forward * hit.distance;
                    if (hit.normal.y > 0.01f) //edge
                    {
                        edgeHit = hit;
                        hitCenter = point + forward * hit.distance;
                        return true;
                    }
                }
                
                gizmoPoint = point + forward * radius;
            }

            edgeHit = default;
            hitCenter = default;
            return false;
        }

        private bool CheckCanEnter(Vector3 edgePoint)
        {
            //check can climb
            return Physics.CheckCapsule(edgePoint + Vector3.up * .1f, edgePoint + Vector3.up * characterHeight, 1);
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

        private void OnDrawGizmos()
        {
            if (show) Gizmos.DrawWireSphere(gizmoPoint, radius);
        }
    }
}