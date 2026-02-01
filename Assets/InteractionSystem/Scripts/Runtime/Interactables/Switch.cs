    using LuduInteraction.Runtime.Core;
    using UnityEngine;
    using UnityEngine.Events;

    namespace LuduInteraction.Runtime.Interactables
    {
        /// <summary>
        /// A switch or lever that toggles state and triggers events.
        /// </summary>
        public class Switch : ToggleInteractable
        {
            #region Fields

            [Header("Animation Settings")]
            [Tooltip("Reference to the Animator component.")]
            [SerializeField] private Animator m_Animator;

            [Tooltip("Name of the Trigger parameter in the Animator.")]
            [SerializeField] private string m_AnimParamName = "On";

            [Header("Logic Settings")]
            [Tooltip("A GameObject to enable/disable when the switch is toggled.")]
            [SerializeField] private GameObject m_TargetObject;

            [Header("Events")]
            [Tooltip("Invoked when the switch is toggled ON.")]
            [SerializeField] private UnityEvent m_OnSwitchOn;
            
            [Tooltip("Invoked when the switch is toggled OFF.")]
            [SerializeField] private UnityEvent m_OnSwitchOff;

            [Header("Prompts")]
            [SerializeField] private string m_OnPrompt = "Turn Off";
            [SerializeField] private string m_OffPrompt = "Turn On";

            #endregion

            #region Properties

            /// <inheritdoc />
            public override string InteractionPrompt => m_IsOn ? m_OnPrompt : m_OffPrompt;

            #endregion

            #region Overrides

            /// <inheritdoc />
            public override void OnInteractComplete(GameObject interactor)
            {
                // Toggle state logic
                HandleAnimation();
                
                // Play Sound
                if (m_AudioSource != null && m_InteractionSound != null)
                {
                    m_AudioSource.PlayOneShot(m_InteractionSound);
                }

                TriggerEvents();
            }

            #endregion

            #region ISaveable Implementation

        public override void LoadState(bool state)
        {
            base.LoadState(state);
            
            // Snap visual state
            if (m_Animator != null)
            {
                m_Animator.SetBool(m_AnimParamName, m_IsOn);
            }
        }

        public override bool SaveState()
        {
            return m_IsOn;
        }

        #endregion

        #region Private Methods

            private void HandleAnimation()
            {
                if (m_Animator != null)
                {
                    // Use SetBool for On/Off states based on the Toggle state
                    m_Animator.SetBool(m_AnimParamName, m_IsOn);
                }
            }

            private void TriggerEvents()
            {
                // Toggle target object if assigned
                if (m_TargetObject != null)
                {
                    m_TargetObject.SetActive(m_IsOn);
                }

                if (m_IsOn)
                {
                    Debug.Log($"Switch {gameObject.name} turned ON");
                    m_OnSwitchOn?.Invoke();
                }
                else
                {
                    Debug.Log($"Switch {gameObject.name} turned OFF");
                    m_OnSwitchOff?.Invoke();
                }
            }

            #endregion
        }
    }
