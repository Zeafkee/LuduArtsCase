using LuduInteraction.Runtime.Core;
using LuduInteraction.Runtime.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LuduInteraction.Runtime.UI
{
    /// <summary>
    /// Manages the visual feedback for interactions (Prompt Text & Progress Bar).
    /// </summary>
    public class InteractionUI : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        [Tooltip("The detector to listen to.")]
        [SerializeField] private InteractionDetector m_Detector;

        [Header("UI Elements")]
        [Tooltip("Container for the prompt (to show/hide).")]
        [SerializeField] private GameObject m_PromptContainer;
        
        [Tooltip("Text element for interaction prompt.")]
        [SerializeField] private TextMeshProUGUI m_PromptText;
        
        [Tooltip("Slider for hold progress.")]
        [SerializeField] private Slider m_ProgressBar;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (m_Detector != null)
            {
                // We need to subscribe to events from InteractionDetector.
                // Note: We haven't defined UnityEvents in InteractionDetector yet.
                // We should add them there first, or use C# events.
                // For now, assuming InteractionDetector has these events based on our earlier plan.
                
                m_Detector.OnFocus += HandleFocus;
                m_Detector.OnLoseFocus += HandleLoseFocus;
                m_Detector.OnHoldProgress += HandleProgress;
            }
        }

        private void OnDisable()
        {
            if (m_Detector != null)
            {
                m_Detector.OnFocus -= HandleFocus;
                m_Detector.OnLoseFocus -= HandleLoseFocus;
                m_Detector.OnHoldProgress -= HandleProgress;
            }
        }

        private void Start()
        {
            HidePrompt();
            if (m_ProgressBar != null) m_ProgressBar.value = 0f;
        }

        #endregion

        #region Private Methods

        private void HandleFocus(IInteractable interactable)
        {
            if (m_PromptContainer != null) m_PromptContainer.SetActive(true);
            
            if (m_PromptText != null) 
                m_PromptText.text = interactable.InteractionPrompt;
            
            // Show progress bar only for Hold interactions
            if (m_ProgressBar != null)
            {
                bool isHold = interactable.InteractionType == InteractionType.Hold;
                m_ProgressBar.gameObject.SetActive(isHold);
                if (isHold) m_ProgressBar.value = 0f;
            }
        }

        private void HandleLoseFocus()
        {
            HidePrompt();
        }

        private void HidePrompt()
        {
            if (m_PromptContainer != null) m_PromptContainer.SetActive(false);
            if (m_ProgressBar != null) m_ProgressBar.gameObject.SetActive(false);
        }

        private void HandleProgress(float progress)
        {
            if (m_ProgressBar != null && m_ProgressBar.gameObject.activeSelf)
            {
                m_ProgressBar.value = progress;
            }
        }

        #endregion
    }
}
