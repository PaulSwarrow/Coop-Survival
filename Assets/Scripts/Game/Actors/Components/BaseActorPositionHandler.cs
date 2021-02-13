using Game.Data;
using Mirror;
using UnityEngine;

namespace Game.Actors.Components
{
    public abstract class BaseActorPositionHandler : NetworkBehaviour
    {
        private PositionSyncData _localPositionSyncData;
        private PositionSyncData _cachedPositionSyncData;
        private PositionSyncData _serverPositionSyncData;

        private void Awake()
        {
            _localPositionSyncData.forward = transform.forward;
        }

        private void LateUpdate()
        {
            if (hasAuthority)
            {
                WriteData(ref _localPositionSyncData);
                if (DoesNeedSync())
                {
                    _cachedPositionSyncData = _localPositionSyncData;
                    SyncUp(_localPositionSyncData, transform.position);
                }
            }
            else
            {
                SyncLateUpdate(ref _localPositionSyncData);
            }
        }


        protected abstract void WriteData(ref PositionSyncData data);
        protected abstract void ApplyData(ref PositionSyncData data, Vector3 position);

        protected virtual void SyncLateUpdate(ref PositionSyncData data)
        {
            
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
            
            ApplyData(ref _localPositionSyncData, position);
        }
    }
}