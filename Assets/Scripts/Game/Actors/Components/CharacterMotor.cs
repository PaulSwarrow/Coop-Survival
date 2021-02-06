﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Tools;
using Libs.GameFramework;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Game.Actors.Components
{
    public class CharacterMotor : NetworkBehaviour
    {
        private Dictionary<MovementSpeed, float> speedValues = new Dictionary<MovementSpeed, float>
        {
            [MovementSpeed.idle] = 0,
            [MovementSpeed.walk] = 2,
            [MovementSpeed.jog] = 3,
            [MovementSpeed.run] = 6,
        };

        private MovementSpeed speed = MovementSpeed.walk;


        [Inject] private CharacterAnimator animator;
        [Inject] private NavMeshAgent agent;
        [Inject] private ObstacleDetector obstacleDetector;
        private bool aim;
        private Coroutine action;

        private void Start()
        {
            agent.speed = speedValues[speed];
        }

        private void Update()
        {
            animator.NormalizedVelocity = agent.velocity * ((int) speed / agent.speed);

            if (action == null && obstacleDetector.CheckClimbAbility(out var info))
            {
                var hash = Animator.StringToHash("JumpOver");
                agent.enabled = false;
                action = StartCoroutine(animator.ActionCoroutine(0, hash, true, OnActionComplete));
            }
        }

        private void OnActionComplete()
        {
            agent.enabled = true;
            action = null;
        }

        public void SetAim(bool value)
        {
            aim = value;
            animator.Aim = value;
        }

        public void SetSpeed(MovementSpeed value)
        {
            speed = value;
            agent.speed = speedValues[speed];
        }

        public void Move(Vector3 vector)
        {
            agent.velocity = vector * agent.speed;
        }

        public void Look(Vector3 forward)
        {
            animator.Forward = forward;
            if (aim)
            {
                agent.transform.forward = forward;
            }
            else
            {
                var currentForward = agent.transform.forward;
                var delta = Vector3.SignedAngle(currentForward, forward, Vector3.up);
                var q = Quaternion.Euler(0, 6 * Time.deltaTime * delta, 0);
                agent.transform.forward = q * currentForward;
            }
        }
    }
}