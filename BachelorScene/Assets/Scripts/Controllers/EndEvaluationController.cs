using UnityEngine;
using UnityEngine.UI;

public class EndEvaluationController : MonoBehaviour
{
    public bool showEvaluation = false;

    public string StartedTimerTime {
        get
        {
            return _startedTimerTime;
        }
        set 
        {
            _startedTimerTime = value;
            StartedTimerTimeText.text = _startedTimerTime;
            StartedTimerTimeText.enabled = true;
            StartedTimerDescriptionText.enabled = true;
        } 
    }
    public string JawGripTime { 
        get
        {
            return _jawGripTime;
        }
        set
        {
            _jawGripTime = value;
            JawGripTimeText.text = _jawGripTime;
            JawGripTimeText.enabled = true;
            JawGripDescriptionText.enabled = true;
        }
    }

    public string RubbedBabyTime
    {
        get
        {
            return _rubbedBabyTime;
        }
        set
        {
            _rubbedBabyTime = value;
            RubbBabyTimeText.text = _rubbedBabyTime;
            RubbBabyTimeText.enabled = true;
            RubbBabyDescriptionText.enabled = true;
        }
    }

    public string HeatLampTime { 
        get
        {
            return _heatLampTime;
        }
        set
        {
            _heatLampTime = value;
            HeatLampTimeText.text = _heatLampTime;
            HeatLampTimeText.enabled = true;
            HeatLampDescriptionText.enabled = true;
        }
    }
    public string StartedVentilatingTime { 
        get
        {
            return _startedVentilatingTime;
        }
        set
        {
            _startedVentilatingTime = value;
            StartedVentilatingTimeText.text = _startedVentilatingTime;
            StartedVentilatingTimeText.enabled = true;
            StartedVentilatingDescriptionText.enabled = true;
        }
    }
    public string AdjustedOxygenPercentage
    {
        get
        {
            return _adjustedOxygenPercentage;
        }
        set
        {
            _adjustedOxygenPercentage = value;
            AdjustedOxygenPercentageText.text = _adjustedOxygenPercentage;
            AdjustedOxygenPercentageText.enabled = true;
            AdjustedOxygenPercentageDescrtionText.enabled = true;
        }
    }
    public string StartedCprTime {
        get
        {
            return _startedCprTime;
        }
        set
        {
            _startedCprTime = value;
            StartedCprTimeText.text = _startedCprTime;
            StartedCprTimeText.enabled = true;
            StartedCprDescriptionText.enabled = true;
        }
    }
    public string BabyRecoveredTime {
        get
        {
            return _babyRecoveredTime;
        }
        set
        {
            _babyRecoveredTime = value;
            BabyRecoveredTimeText.text = _babyRecoveredTime;
            BabyRecoveredTimeText.enabled = true;
            BabyRecoveredDescriptionText.enabled = true;
        }
    }
    public string Points {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            PointsText.text = _points;
            PointsText.enabled = true;
            PointsDescriptionText.enabled = true;
        }
    }
    public string ScenarioDescription
    {
        get
        {
            return _scenarioDescription;
        } 
        set
        {
            _scenarioDescription = value;
            ScenarioDescriptionText.text = _scenarioDescription;
            ScenarioDescriptionText.enabled = true;
        }
    }
    public string VentilationRate
    {
        get
        {
            return _ventilationRate;
        }
        set
        {
            _ventilationRate = value;
            VentilationRateText.text = _ventilationRate;
            VentilationRateText.enabled = true;
            VentilationRateDescriptionText.enabled = true;
        }
    }
    public string CprRate
    {
        get
        {
            return _cprRate;
        }set
        {
            _cprRate = value;
            CprRateText.text = _cprRate;
            CprRateText.enabled = true;
            CprRateDescriptionText.enabled = true;
        }
    }
    public string VentilationDuration
    {
        get
        {
            return _ventilationDuration;
        } 
        set
        {
            _ventilationDuration = value;
            VentilationDurationText.text = _ventilationDuration;
            VentilationDurationText.enabled = true;
            VentilationDurationDescriptionText.enabled = true;
        }
    }
    public string HlrDuration
    {
        get
        {
            return _hlrDuration;
        }
        set
        {
            _hlrDuration = value;
            HlrDurationText.text = _hlrDuration;
            HlrDurationText.enabled = true;
            HlrDurationDescriptionText.enabled = true;
        }
    }

    public void Congratulate()
    {
        CongratulationText.enabled = true;
    }

    public GameObject EndEvaluationDisplay;
    public Text CongratulationText;
    public Text StartedTimerDescriptionText;
    public Text StartedTimerTimeText;
    public Text JawGripDescriptionText;
    public Text JawGripTimeText;
    public Text RubbBabyDescriptionText;
    public Text RubbBabyTimeText;
    public Text HeatLampDescriptionText;
    public Text HeatLampTimeText;
    public Text StartedVentilatingDescriptionText;
    public Text StartedVentilatingTimeText;
    public Text VentilationDurationDescriptionText;
    public Text VentilationDurationText;
    public Text VentilationRateDescriptionText;
    public Text VentilationRateText;
    public Text AdjustedOxygenPercentageDescrtionText;
    public Text AdjustedOxygenPercentageText;
    public Text StartedCprDescriptionText;
    public Text StartedCprTimeText;
    public Text HlrDurationDescriptionText;
    public Text HlrDurationText;
    public Text CprRateDescriptionText;
    public Text CprRateText;
    public Text BabyRecoveredDescriptionText;
    public Text BabyRecoveredTimeText;
    public Text PointsDescriptionText;
    public Text PointsText;
    public Text ScenarioDescriptionText;

    private string _startedTimerTime;
    private string _jawGripTime;
    private string _heatLampTime;
    private string _startedVentilatingTime;
    private string _ventilationRate;
    private string _adjustedOxygenPercentage;
    private string _startedCprTime;
    private string _cprRate;
    private string _babyRecoveredTime;
    private string _points;
    private string _scenarioDescription;
    private string _ventilationDuration;
    private string _hlrDuration;
    private string _rubbedBabyTime;
    private bool evaluationIsShowing = false;

    private void Start()
    {
        Hide();
    }
    // Update is called once per frame
    void Update()
    {
        if(showEvaluation && !evaluationIsShowing)
        {
            Show();
        } else if (!showEvaluation && evaluationIsShowing)
        {
            Hide();
        }

        /*
        if(showEvaluation)
        {
            setPositionInfrontOfPlayer();
        }
        */

        //TESTING
        if (Input.GetKey(KeyCode.O))
        {
            showEvaluation = !showEvaluation;
        }
    }
    private void setPositionInfrontOfPlayer(float distance = 1.25f)
    {
        Camera camera = Camera.current ?? Camera.allCameras?[0];

        if (camera != null)
        {
            transform.position = camera.transform.position + camera.transform.forward * distance;
            transform.rotation = Quaternion.Euler(0f, camera.transform.rotation.eulerAngles.y + 90f, 0f);
        }
    }

    void Show() {
        //setPositionInfrontOfPlayer();
        EndEvaluationDisplay.SetActive(true);
        evaluationIsShowing = true;

        
    }
    void Hide() {
        EndEvaluationDisplay.SetActive(false);
        evaluationIsShowing = false;
    }
}