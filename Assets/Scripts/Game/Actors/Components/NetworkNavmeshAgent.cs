using System;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Tools
{
    public class NetworkNavmeshAgent : NetworkBehaviour
    {
        private struct Data
        {
            public Vector3 velocity;
            public Vector3 position;
            public Vector3 forward;
        }

        private NavMeshAgent agent;
        private Vector3 requiredVelocity;

        private Data localData;
        private Data serverData;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            localData.forward = transform.forward;
        }

        private void LateUpdate()
        {
            if (hasAuthority)
            {
                if (DoesNeedSync())
                {
                    localData.velocity = agent.velocity;
                    localData.position = agent.transform.position;
                    localData.forward = agent.transform.forward;
                    SyncUp(localData);
                }
            }
            else
            {
                agent.velocity = Vector3.Lerp(agent.velocity, requiredVelocity, 10 * Time.deltaTime);
            }
        }


        private bool DoesNeedSync()
        {
            return localData.velocity != agent.velocity || localData.forward != agent.transform.forward;
        }

        [Command]
        private void SyncUp(Data data)
        {
            serverData = data;
            SyncDown(data);
        }

        [ClientRpc]
        private void SyncDown(Data data)
        {
            if (hasAuthority) return;
            localData = data;
            agent.transform.position = data.position;
            agent.transform.forward = data.forward;
            requiredVelocity = data.velocity;
        }
    }
}