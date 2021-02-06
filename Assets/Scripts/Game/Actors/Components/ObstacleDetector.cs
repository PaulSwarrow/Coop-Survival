using Game.Data;
using Libs.GameFramework;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Actors.Components
{
    public class ObstacleDetector : MonoBehaviour
    {
        [Inject] private NavMeshAgent agent;

        private float stepHeight = .4f;
        private float minHeight => stepHeight;
        private float maxHeight = 2.4f;

        public bool CheckClimbAbility(out ClimbPointInfo info)
        {
            if (TryFindEdge(out var edgeHit))
            {
                if (CheckCanEntry(edgeHit.point))
                {
                    var startPoint = edgeHit.point;
                    startPoint.y = agent.transform.position.y;

                    var forwardVector = -edgeHit.normal;
                    forwardVector.y = 0;
                    forwardVector.Normalize();

                    var climbHeight = edgeHit.point.y - agent.transform.position.y;


                    var climbType = GetClimbType(edgeHit.point, forwardVector);

                    Debug.DrawRay(edgeHit.point, forwardVector, climbType == ClimbType.Over ? Color.green : Color.blue);
                    info = new ClimbPointInfo
                    {
                        startPoint = startPoint,
                        climbType = climbType,
                        climbHeight = climbHeight
                    };
                    return true;
                }
            }

            info = default;
            return false;
        }

        private bool TryFindEdge(out RaycastHit edgeHit)
        {
            var currentHeight = minHeight;

            var r = 1;
            while (currentHeight < maxHeight)
            {
                var point = transform.position;
                currentHeight += r;
                point.y = currentHeight;
                if (Physics.SphereCast(point, r, transform.forward, out var hit, 1))
                {
                    if (hit.normal.y > 0) //edge
                    {
                        edgeHit = hit;
                        return true;
                    }
                }
            }

            edgeHit = default;
            return false;
        }

        private bool CheckCanEntry(Vector3 edgePoint)
        {
            //check can climb
            return Physics.CheckCapsule(edgePoint + Vector3.up * .1f, edgePoint + Vector3.up * agent.height, 1);
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

        /*public void TryClimbObstacle(RaycastHit hit, Transform character)
        {
            var stepHeight = .4f;
            var startPoint = hit.point;
            startPoint.y = character.transform.position.y;

            var forward = -hit.normal;
            forward.y = 0;
            forward.Normalize();

            var climbingHeight = hit.point.y - startPoint.y;

            var edge = startPoint + Vector3.up * climbingHeight;

            //check can climb
            if (Physics.CheckCapsule(edge + Vector3.up * .1f, edge + Vector3.up * agent.height, 1))
            {
                //check floor size
                for (var deep = 0f; deep < .5f; deep += .1f)
                {
                    var point = Vector3.up + edge + forward * deep;
                    Debug.DrawLine(point, point + Vector3.down * (1 + stepHeight));
                    if (!Physics.Raycast(point, Vector3.down, 1 + stepHeight))
                    {
                        //select animation from climg height;
                        Debug.DrawRay(edge, forward, Color.green);
                        return;
                    }
                }


                //select animation from climg height;
                Debug.DrawRay(edge, forward, Color.blue);
            }
        }*/
    }
}