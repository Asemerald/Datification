using UnityEngine;

public class CustomisationPhaseUIManager : MonoBehaviour
{
    public static CustomisationPhaseUIManager Instance;
    
    [Header("Animators")]
    [SerializeField] private Animator textWindowAnimator;
    [SerializeField] private Animator transitionPanelAnimator;
    
    [Space,Header("GameObjects")]
    [SerializeField] private GameObject placeholderCustomisationUI;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject transitionPanel;
    
    private void Awake()
    {
        if(Instance != this && Instance!= null) Destroy(this);
        Instance = this;
    }
    void Start()
    {
        nextButton.SetActive(true);
        confirmButton.SetActive(false);
        transitionPanel.SetActive(false);
        
        placeholderCustomisationUI.SetActive(false);
    }
    
    
    //Manages CustomisationPhase UI apparition 
    public void CustomisationPhaseActivation(bool isOn)
    {
        placeholderCustomisationUI.SetActive(isOn);
    }
    
    //Manages theme text apparition
    public void ThemeActivation(bool isOn)
    {
        TextWindowParameterReset();
        textWindowAnimator.SetBool("ThemeOn",isOn);
    }
    
    //Manages waiting text apparition
    public void WaitingActivation(bool isOn, bool isLastStep)
    {
        TextWindowParameterReset();
        textWindowAnimator.SetBool("WaitingOn",isOn);
        
        if(!isLastStep) return;
        confirmButton.SetActive(true);
        nextButton.SetActive(false);
    }
    
    //Manages result text apparition
    public void ResultActivation(bool isOn)
    {
        TextWindowParameterReset();
        textWindowAnimator.SetBool("ResultOn",isOn);
    }

    //Manage transitionPanel apparition
    public void TransitionPanelActivation(bool isOn)
    {
        if (isOn)
        {
            transitionPanel.SetActive(isOn);
            transitionPanelAnimator.SetBool("isOn",isOn);
        }
        else
        {
            transitionPanelAnimator.SetBool("isOn",isOn);
        }
    }

    
    //Resets all animator parameters
    private void TextWindowParameterReset()
    {
        textWindowAnimator.SetBool("WaitingOn",false);
        textWindowAnimator.SetBool("ResultOn",false);
        textWindowAnimator.SetBool("ThemeOn",false);
    }
}
