using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Scenarios
{
    class Scenario1VentilateTheBaby : Scenario
    {
        private Scenario1ObjectCollection objectCollection;

        //StopWatch
        private bool startedStopWatch = false;
        private int points; 
        private float startedStopWatchTime;

        //Ventilating
        private bool startedVentilating = false;
        private float startedVentilatingTime;

        //HeatLamp
        private float heatLampTurnedOnTime;
        private bool heatLampOn = false;

        //PulseOxymeter
        private bool pulseOxymeterAttached;
        private float pulseOxymeterAttachedTime;

        //Kjevegrep
        private bool performedJawGrip = false;
        private float performedJawGripTime;

        //HLR
        private bool startedCPR = false;
        private float startedCPRTime;
        private bool cprFinished = false;
        private float cprFinishedTime;

        //OxygenPercentage
        private bool oxygenTurnedToHundred = false;
        private float oxygenTurnedToHundredTime;

        //BabyLivesState
        private bool enteredBabyLiveState = false;
        private float enteredBabyLivesStateTime;
        private bool increasedHeartRateAfterCompressing = false;

        //Rubbing
        private bool performedRubbing = false;
        private float performedRubbingTime;

        private void playSuccessSoundIfEnabled()
        {
            if (objectCollection.UseSuccessSound)
            {
                objectCollection.SceneAmbientAudioManager.GetComponent<AudioManager>().Play("Success");
            }
        }

        public void Start_OnEnter()
        {
            Debug.Log("Start_OnEnter");
            

            objectCollection = GameObject.Find("Scenario1ObjectCollection").GetComponent<Scenario1ObjectCollection>();
            checkIfAnyObjectsOrComponentsAreMissing();

            //Setup baby parameters for this scenario
            objectCollection.BabyController.IsBreathing = false;               //Baby should not breathe
            objectCollection.BabyController.RespirationRatePM = 0f;            //Baby should not breathe
            objectCollection.BabyController.BPMController.SetBpmRange(45, 55); //Baby heartbeat BPM 45-55
            objectCollection.BabyController.SetTintPercentage(1f);             //Baby is blue
            objectCollection.HeatLamp.LightState = false;                                           //Heatlamp is off
        }

        //Stupid method for logging, so that errors make more sense. 
        private void checkIfAnyObjectsOrComponentsAreMissing()
        {
            if (objectCollection == null)
            {
                Debug.LogError("ObjectCollection is null. Errors may occur after this.");
            }
        }

        public string Start_OnStay()
        {
            Debug.Log("Start_OnStay");
            Debug.Log("State: Stopwatch");

            objectCollection.EndEvaluationController.ScenarioDescription = this.Description;
            return "stopwatch";
        }

        public string Stopwatch_OnStay()
        {
            
            checkHeatLamp();
            checkPulsOximeter();
            checkRubbing();

            //Check if stopwatch is started, record when it happens and stop checking.
            if (!startedStopWatch && objectCollection.StopWatch.isStopWatchStarted())
            {
                startedStopWatch = !startedStopWatch;
                startedStopWatchTime = Time.time;
                points++;
                playSuccessSoundIfEnabled();
                objectCollection.EndEvaluationController.StartedTimerTime = $"{startedStopWatchTime:F0} sekunder";
                Debug.Log("State: Jawgrip");
                return "jawGrip";
            }
            return null;
        }

        public string JawGrip_OnStay()
        {
            
            checkHeatLamp();
            checkPulsOximeter();
            checkRubbing();

            //TODO: Sjekk at bruker har tatt kjevegrep, og lagre når dette skjer. 
            if (!performedJawGrip && objectCollection.JawGrip.HoldingGrip)
            {
                performedJawGrip = true;
                performedJawGripTime = Time.time;
                points++;
                playSuccessSoundIfEnabled();
                objectCollection.EndEvaluationController.JawGripTime = $"{performedJawGripTime:F0} sekunder";
                Debug.Log("State: Ventilate");
                return "ventilate";
            }
            return null;

            
        }

        public string Ventilate_OnStay()
        {

            //TESTING ONLY 
            if (Input.GetKey(KeyCode.N))
            {
                Debug.Log("State: CPR");
                objectCollection.EndEvaluationController.VentilationDuration = objectCollection.Ventilator.VentilationDuration.ToString("F0") + ", optimal varighet er 60-120 sekunder";
                objectCollection.EndEvaluationController.VentilationRate = objectCollection.Ventilator.VentilationRate.ToString("F0") + ", optimal rate 30-60";
                return "cpr";
            }
            
            checkHeatLamp();
            checkPulsOximeter();
            checkRubbing();

            //Check if user started ventilating the baby, record when it happens.
            if (!startedVentilating && objectCollection.Ventilator.StartedVentilating)
            {
                startedVentilating = true;
                startedVentilatingTime = Time.time;
                points++;
                playSuccessSoundIfEnabled();
                objectCollection.EndEvaluationController.StartedVentilatingTime = $"{startedVentilatingTime:F0} sekunder, optimalt skal denne tiden være rundt 60 sekunder";
                Debug.Log("Started ventilating");
                objectCollection.BabyController.DecreaseTintPercentage(0.025f, 2f);
            }
            if (startedVentilating && objectCollection.Ventilator.SuccessfulVentilation)
            {
                points++;
                objectCollection.BabyController.DecreaseTintPercentage(0.025f, 2f);
                playSuccessSoundIfEnabled();
                objectCollection.EndEvaluationController.VentilationDuration = objectCollection.Ventilator.VentilationDuration.ToString("F0") + ", optimal varighet er 60-120 sekunder";
                objectCollection.EndEvaluationController.VentilationRate = objectCollection.Ventilator.VentilationRate.ToString("F0") + ", optimal rate 30-60";
                Debug.Log("State: CPR");
                return "cpr";            
            }
            
            //Registrer korrekt ventilering i minimum, 30 sekunder. Skal så ha mellom 30 og 60 ventileringer til.

            return null;
        }

        public string Cpr_OnStay()
        {
            //TODO: HLR skal startes etter ventilering, da skal det tilføres 100% oksygen, HLR skal gjøres i minimum 30 sekunder

            checkHeatLamp();
            checkPulsOximeter();
            checkRubbing();

            if (!startedCPR && objectCollection.CprController.StartedCPR)
            {
                startedCPR = true;
                startedCPRTime = Time.time;
                points++;
                playSuccessSoundIfEnabled();
                objectCollection.EndEvaluationController.StartedCprTime = $"{startedCPRTime:F0} sekunder";
                objectCollection.BabyController.DecreaseTintPercentage(0.05f, 2f);
            }

            if (!oxygenTurnedToHundred && (objectCollection.OxygenTurnKnob.Percent >= 0.90))
            {
                oxygenTurnedToHundred = true;
                oxygenTurnedToHundredTime = Time.time;
                points++;
                playSuccessSoundIfEnabled();
            }

            if (objectCollection.CprController.CompressionDuration > 10 && !increasedHeartRateAfterCompressing)
            {
                increasedHeartRateAfterCompressing = true;
                objectCollection.BabyController.BPMController.SetBpmRange(55, 80);
            }

            //Check all qualifiers to proceed to next state
            if (startedCPR && objectCollection.CprController.FinishCompression)
            {
                cprFinished = true;
                cprFinishedTime = Time.time;
                points++;
                playSuccessSoundIfEnabled();
                objectCollection.EndEvaluationController.HlrDuration = objectCollection.CprController.CompressionDuration.ToString("F3");
                objectCollection.EndEvaluationController.CprRate = objectCollection.CprController.CompressionRate.ToString("F3") + ", optimal rate: 90";
                Debug.Log("State: BabyLives");
                return "babyLives";

            }

            return null;
        }

        public string BabyLives_OnStay()
        {
            
            //Baby skal puste selv, får normal eller bedre hjerterytme
            //Scenario goals have been acomplished, baby should start crying and moving

            checkHeatLamp();
            checkPulsOximeter();
            checkRubbing();

            if (!enteredBabyLiveState)
            {
                enteredBabyLiveState = true;
                enteredBabyLivesStateTime = Time.time; 
                objectCollection.BabyController.IsBreathing = true;
                objectCollection.BabyController.RespirationRatePM = 60f;
                objectCollection.BabyController.BPMController.SetBpmRange(120, 160);
                objectCollection.BabyController.SetTintPercentage(0f, 3f);     
                objectCollection.BabyContraction.BabyHasTonus = true;
                objectCollection.BabyContraction.BabyIsCrying = true;
            }

            displayEndEvaluation();

            if ((Time.time - enteredBabyLivesStateTime) > 15f)
            {
                return "endEvaluation";
            }

            return null;
            
        }

        private void displayEndEvaluation()
        {
            objectCollection.EndEvaluationController.StartedTimerTime = startedStopWatch ? $"{startedStopWatchTime:F0} sekunder" : "Du startet ikke stoppeklokken!";
            objectCollection.EndEvaluationController.JawGripTime = performedJawGrip ? $"{performedJawGripTime:F0} sekunder" : "Du tok ikke kjevegrep, og sjekket ikke om luftveiene var blokkert!";
            objectCollection.EndEvaluationController.RubbedBabyTime= performedRubbing ? $"{performedRubbingTime:F0} sekunder" : "Du stimulerte ikke baby!";
            objectCollection.EndEvaluationController.StartedVentilatingTime = startedVentilating ? $"{startedVentilatingTime:F0} sekunder, optimalt skal denne tiden være rundt 60 sekunder" : "Du forsøkte ikke å ventilere!";
            objectCollection.EndEvaluationController.AdjustedOxygenPercentage = oxygenTurnedToHundred ? $"{oxygenTurnedToHundredTime} sekunder" : "Du økte ikke oksygenkonsentrasjonen i masken før HLR";
            objectCollection.EndEvaluationController.StartedCprTime = startedCPR ? $"{startedCPRTime:F0} sekunder" : "Du forsøkte ikke HLR!";
            objectCollection.EndEvaluationController.HeatLampTime = heatLampOn ? $"{heatLampTurnedOnTime:F0} sekunder" : "Varmelampen var ikke slått på, er du sikker på at barnet ikke var nedkjølt?";
            objectCollection.EndEvaluationController.BabyRecoveredTime = $"{enteredBabyLivesStateTime:F0} sekunder"; // maybe ugly?
            objectCollection.EndEvaluationController.Points = $"{points} av 8";
            objectCollection.EndEvaluationController.ScenarioDescription = this.Description;
            objectCollection.EndEvaluationController.VentilationRate = objectCollection.Ventilator.VentilationRate.ToString("F0") + ", optimal rate 30-60";
            objectCollection.EndEvaluationController.CprRate = objectCollection.CprController.CompressionRate.ToString("F0") + ", optimal rate: 90";
            objectCollection.EndEvaluationController.VentilationDuration = objectCollection.Ventilator.VentilationDuration.ToString("F0") + ", optimal varighet er 60-120 sekunder";
            objectCollection.EndEvaluationController.HlrDuration = objectCollection.CprController.CompressionDuration.ToString("F0");
            objectCollection.EndEvaluationController.Congratulate();
            objectCollection.EndEvaluationController.showEvaluation = true;
        }

        private string EndEvaluation_OnStay()
        {
            return "end";
        }
        public void End_OnEnter()
        {
            Debug.Log("End_OnEnter");
            Debug.Log(Duration);
            Debug.Log(string.Join(", ", StateMachine.StateChanges.Select(r => $"{r.From} -> {r.To} @ {r.Time}")));
        }

        public string End_OnStay()
        {
            objectCollection.EndEvaluationController.showEvaluation = true;

            return null;
        }

        public void End_OnLeave()
        {

        }

        public void EmptyMethod()
        {
            //Method that does nothing. It might be our best code yet.
        }

        private void checkHeatLamp()
        {
            //Sjekk at de har slått på varmelampe.
            if (!heatLampOn && objectCollection.HeatLamp.LightState)
            {
                heatLampTurnedOnTime = Time.time;
                heatLampOn = true;
                points++;
                objectCollection.EndEvaluationController.HeatLampTime = $"{heatLampTurnedOnTime:F0} sekunder";

                playSuccessSoundIfEnabled();
                objectCollection.BabyController.DecreaseTintPercentage(0.05f, 2f);
            }
        }

        private void checkPulsOximeter()
        {
            //Registrer at pulsoksymeter ble satt på, dette er ikke nødvendig for å gjøre ferdig scenarioet, eller å gjøre det korrekt. 
            if (!pulseOxymeterAttached && objectCollection.PulseOxymeter.IsOximeterAttached)
            {
                pulseOxymeterAttached = true;
                pulseOxymeterAttachedTime = Time.time;

                playSuccessSoundIfEnabled();
            }
        }

        private void checkRubbing()
        {
            //bool test = objectCollection.RubbingControll.SuccessfulRubbing;
            if (!performedRubbing && objectCollection.RubbingControll.SuccessfulRubbing)
            {
                performedRubbing = true;
                performedRubbingTime = Time.time;
                objectCollection.EndEvaluationController.RubbedBabyTime = $"{performedRubbingTime:F0} sekunder";
                playSuccessSoundIfEnabled();
            }
        }

        public Scenario1VentilateTheBaby()
        {
            Name = "Scenario 1";
            Description = "Du har nettopp tatt imot et barn født til termin. Barnet er forløst med vakuum på grunn av protrahert forløp stadium II. Ved fødsel har ikke barnet egen respirasjon, barnet er bleikt og nedsatt tonus";

            List<State<string>> states = new List<State<string>>
            {
                new State<string>("start", Start_OnEnter, Start_OnStay, EmptyMethod),
                new State<string>("stopwatch", EmptyMethod, Stopwatch_OnStay, EmptyMethod),
                new State<string>("jawGrip", EmptyMethod, JawGrip_OnStay, EmptyMethod),
                new State<string>("ventilate", EmptyMethod, Ventilate_OnStay, EmptyMethod),
                new State<string>("cpr", EmptyMethod, Cpr_OnStay, EmptyMethod),
                new State<string>("babyLives", EmptyMethod, BabyLives_OnStay, EmptyMethod),
                new State<string>("endEvaluation", EmptyMethod, EndEvaluation_OnStay, EmptyMethod),
                new State<string>("end", End_OnEnter, End_OnStay, End_OnLeave)
            };

            StateMachine = new StateMachine<string>(states, "start");
        }
    }
}