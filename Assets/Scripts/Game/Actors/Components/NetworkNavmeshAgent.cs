﻿using System;
using Game.Data;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Tools
{
    public class NetworkNavmeshAgent : NetworkBehaviour
    {

        private NavMeshAgent agent;

        private PositionSyncData _localPositionSyncData;
        private PositionSyncData _cachedPositionSyncData;
        private PositionSyncData _serverPositionSyncData;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            _localPositionSyncData.forward = transform.forward;
        }

        private void LateUpdate()
        {
            if (hasAuthority)
            {
                _localPositionSyncData.velocity = agent.velocity;
                _localPositionSyncData.forward = agent.transform.forward;
                _localPositionSyncData.enabled = agent.enabled;
                if (DoesNeedSync())
                {
                    _cachedPositionSyncData = _localPositionSyncData;
                    SyncUp(_localPositionSyncData, transform.position);
                }
            }
            else
            {
                agent.velocity = Vector3.Lerp(agent.velocity, _localPositionSyncData.velocity, 10 * Time.deltaTime);
            }
        }


        private bool DoesNeedSync()
        {
            return !_cachedPositionSyncData.Equals(_localPositionSyncData);
        }

        [Command]
        private void SyncUp(PositionSyncData positionSyncData, Vector3 position)
        {
            _serverPositionSyncData = positionSyncData;
            SyncDown(positionSyncData, position);
        }

        [ClientRpc]
        private void SyncDown(PositionSyncData positionSyncData, Vector3 position)
        {
            if (hasAuthority) return;
            _localPositionSyncData = positionSyncData;
            agent.transform.position = position;
            agent.transform.forward = positionSyncData.forward;
            agent.enabled = positionSyncData.enabled;
        }
        
    }
}