using System;
using System.Collections.Generic;
using LuduInteraction.Runtime.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LuduInteraction.Runtime.Player
{
    /// <summary>
    /// Detects interactable objects using Raycast and handles interaction input.
    /// </summary>
    public class InteractionDetector : MonoBehaviour
    {
        #region Fields

        [Header("Detection Settings")]
        [Tooltip("The camera to raycast from.")]
        [SerializeField] private Camera m_Camera;

        [Tooltip("How far the player can reach.")]
        [SerializeField] private float m_InteractionRange = 3.0f;

        [Tooltip("Layers to include in interaction raycast.")]
        [SerializeField] private LayerMask m_InteractionLayer;

        [Header("Input Settings")]
        [Tooltip("Reference to the Interact Action (from Input Action Asset).")]
        [SerializeField] private InputActionReference m_InteractAction;

        [Header("Debug")]
        [SerializeField] private bool m_ShowDebugRay = true;

        // State
        private IInteractable m_CurrentInteractable;
        private bool m_IsHolding;
        private float m_HoldTimer;
        
        // Cache to avoid GetComponent calls
        private Dictionary<Collider, IInteractable> m_InteractableCache = new Dictionary<Collider, IInteractable>();

        #endregion

        #region Events

        public event Action<IInteractable> OnFocus;
        public event Action OnLoseFocus;
        public event Action<float> OnHoldProgress;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_Camera == null)
            {
                m_Camera = Camera.main;
            }
        }

        private void OnEnable()
        {
            if (m_InteractAction != null)
            {
                m_InteractAction.action.Enable();
                m_InteractAction.action.started += OnInteractStarted;
                m_InteractAction.action.canceled += OnInteractCanceled;
            }
        }

        private void OnDisable()
        {
            if (m_InteractAction != null)
            {
                m_InteractAction.action.started -= OnInteractStarted;
                m_InteractAction.action.canceled -= OnInteractCanceled;
                m_InteractAction.action.Disable();
            }
        }

        private void Update()
        {
            DetectInteractable();
            HandleHoldInteraction();
        }

        private void OnDrawGizmos()
        {
            if (m_ShowDebugRay && m_Camera != null)
            {
                Gizmos.color = m_CurrentInteractable != null ? Color.green : Color.red;
                Gizmos.DrawRay(m_Camera.transform.position, m_Camera.transform.forward * m_InteractionRange);
            }
        }

        #endregion

        #region Input Callbacks

        private void OnInteractStarted(InputAction.CallbackContext context)
        {
            if (m_CurrentInteractable == null) return;

            m_CurrentInteractable.OnInteractStart(gameObject);

            // Re-trigger focus event to refresh UI prompt text immediately (useful for Toggles)
            OnFocus?.Invoke(m_CurrentInteractable);

            if (m_CurrentInteractable.InteractionType == InteractionType.Hold)
            {
                m_IsHolding = true;
                m_HoldTimer = 0f;
            }
        }

        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            if (m_IsHolding && m_CurrentInteractable != null)
            {
                m_CurrentInteractable.OnInteractCancel(gameObject);
            }
            
            m_IsHolding = false;
            m_HoldTimer = 0f;
            OnHoldProgress?.Invoke(0f);
        }

        #endregion

        #region Private Methods

        private void HandleHoldInteraction()
        {
            if (!m_IsHolding || m_CurrentInteractable == null) return;

            // Check if we lost focus while holding (e.g. looked away)
            if (!m_CurrentInteractable.CanInteract) // Simplified check, could also check Raycast result
            {
                OnInteractCanceled(default);
                return;
            }

            // Get duration from HoldInteractable cast
            float duration = 2.0f; // Default fallback
            if (m_CurrentInteractable is HoldInteractable holdInt)
            {
                duration = holdInt.HoldDuration;
            }

            m_HoldTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(m_HoldTimer / duration);

            m_CurrentInteractable.OnInteractHold(gameObject, progress);
            OnHoldProgress?.Invoke(progress);

            if (m_HoldTimer >= duration)
            {
                m_CurrentInteractable.OnInteractComplete(gameObject);
                
                // Refresh UI after completion
                if (m_CurrentInteractable != null && m_CurrentInteractable.CanInteract)
                {
                    OnFocus?.Invoke(m_CurrentInteractable);
                }
                
                m_IsHolding = false;
                m_HoldTimer = 0f;
                OnHoldProgress?.Invoke(0f);
            }
        }

        /// <summary>
        /// Casts a ray to find interactable objects.
        /// </summary>
        private void DetectInteractable()
        {
            Ray ray = new Ray(m_Camera.transform.position, m_Camera.transform.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, m_InteractionRange, m_InteractionLayer))
            {
                Collider hitCollider = hit.collider;

                // 1. Check Cache
                if (!m_InteractableCache.TryGetValue(hitCollider, out IInteractable interactable))
                {
                    // 2. Not in cache, try GetComponent
                    interactable = hitCollider.GetComponent<IInteractable>();
                    
                    // Add to cache
                    m_InteractableCache.Add(hitCollider, interactable);
                }

                // 3. Handle Found Interactable
                if (interactable != null && interactable.CanInteract)
                {
                    if (m_CurrentInteractable != interactable)
                    {
                        // Remove highlight from previous
                        if (m_CurrentInteractable is BaseInteractable prevBase) prevBase.SetHighlight(false);

                        m_CurrentInteractable = interactable;
                        Debug.Log($"Focused: {hitCollider.gameObject.name}");
                        
                        // Add highlight to new
                        if (m_CurrentInteractable is BaseInteractable newBase) newBase.SetHighlight(true);

                        OnFocus?.Invoke(m_CurrentInteractable);
                    }
                }
                else
                {
                    ClearInteractable();
                }
            }
            else
            {
                ClearInteractable();
            }
        }

        private void ClearInteractable()
        {
            if (m_CurrentInteractable != null)
            {
                // If holding, cancel
                if (m_IsHolding)
                {
                    OnInteractCanceled(default);
                }

                // Remove highlight
                if (m_CurrentInteractable is BaseInteractable baseInt) baseInt.SetHighlight(false);

                Debug.Log("Lost Focus");
                m_CurrentInteractable = null;
                OnLoseFocus?.Invoke();
            }
        }

        #endregion
    }
}
