﻿using UnityEngine;
using System.Collections;
using Cosmos.Input;
using System;

namespace Cosmos
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class MultiplayerYBotController : ControllerBase<MultiplayerYBotController>
    {
        [SerializeField]
        [Range(0, 1)]
        float forwardDampTime = 0.1f;
        [SerializeField]
        [Range(0, 1)]
        float turnDampTime = 0.05f;
        Animator animator;
        int verticalHash = Animator.StringToHash("Vertical");
        int inputHash = Animator.StringToHash("Input");
        int attack00Hash = Animator.StringToHash("Attack_00");
        int attack01Hash = Animator.StringToHash("Attack_01");
        float moveMagnitude = 0;
        float turnSmoothVelocity;
        public float turnSmoothTime = 0.3f;
        public Transform CameraTarget { get; private set; }

        [SerializeField] float walkSpeed = 1.5f;
        [SerializeField] float runSpeed = 5;
        [SerializeField] float rotSpeed = 5;
        float currentSpeed;

        IInputManager inputManager;

        MultiplayerYBotCamera cameraCache;
        public MultiplayerYBotCamera Camera
        {
            get
            {
                if (cameraCache == null)
                {
                    cameraCache = CosmosEntry.ControllerManager.GetController<MultiplayerYBotCamera>(typeof(MultiplayerYBotCamera).Name);
                }
                return cameraCache;
            }
        }

        protected override void RefreshHandler()
        {
            MoveAndRot();
        }
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponentInChildren<Animator>();
            CameraTarget = transform.Find("CameraTarget").transform;
            CosmosEntry.InputManager.SetInputDevice(new StandardInputDevice());
            inputManager = CosmosEntry.InputManager;
        }
        void MoveAndRot()
        {
            var v = inputManager.GetAxis(InputAxisType._Vertical);
            var h = inputManager.GetAxis(InputAxisType._Horizontal);
            Vector2 input = new Vector2(h, v);
            Vector2 inputDir = input.normalized;

            moveMagnitude = input.normalized.magnitude;
            if (input != Vector2.zero)
            {
                animator.SetBool(inputHash, true);
                //输入方向与相机forword夹角；
                float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
                transform.position += transform.forward * currentSpeed * moveMagnitude * Time.deltaTime;
            }
            else
                animator.SetBool(inputHash, false);
            if (inputManager.GetButton(InputButtonType._LeftShift))
            {
                currentSpeed = runSpeed;
                moveMagnitude = input.normalized.magnitude*2;
            }
            else
                currentSpeed = walkSpeed;
            animator.SetFloat(verticalHash, moveMagnitude, forwardDampTime, Time.deltaTime);
        }
    }
}