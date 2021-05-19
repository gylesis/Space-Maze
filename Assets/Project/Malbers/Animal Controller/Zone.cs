﻿using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations.Scriptables;
using UnityEngine.Events;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MalbersAnimations.Controller
{
    /// <summary>When an animal Enter a Zone this will activate a new State or a new Mode </summary>
    [AddComponentMenu("Malbers/Animal Controller/Zone")]
    public class Zone : MonoBehaviour
    {
        /// <summary>Use the Trigger for Heads only</summary>
        public bool debug;

        /// <summary>Set the Action Zone to Automatic</summary>
        public bool automatic;

        [FormerlySerializedAs("HeadOnly")]
        /// <summary>Use the Trigger for Heads only</summary>
        public bool BoneOnly;

      
        [FormerlySerializedAs("HeadName")]
        public string BoneName = "Head";


        public ZoneType zoneType = ZoneType.Mode;
        public StateAction stateAction = StateAction.Activate;
        public StanceAction stanceAction = StanceAction.Enter;

        public LayerReference Layer = new LayerReference(1048576); //Animal Layer
        public IntReference stateStatus = new IntReference(-1);

        [SerializeField] private List<Tag> tags;

        public ModeID modeID;
        public StateID stateID;

        public StanceID stanceID;
        public MAction ActionID;
        /// <summary> Mode Index Value</summary>
        [SerializeField] private IntReference modeIndex = new IntReference(0);

        /// <summary>Mode Ability Index</summary> 
        public int ModeIndex => modeID.ID == 4 ? ActionID.ID : modeIndex.Value;

        /// <summary>ID of the Zone regarding the Type of Zone(State,Stance,Mode) </summary> 
        public int ZoneID;

        [Tooltip("Value of the Ability Status")]
        public AbilityStatus m_abilityStatus = AbilityStatus.PlayOneTime;
        [Tooltip("Time of Ability Activation")]
        public float AbilityTime =  3f;


        [Tooltip("Amount of Force that will be applied to the Animal")]
        public FloatReference Force = new FloatReference(10);
        [Tooltip("Aceleration to applied the Force when the Animal enters the zone")]
        [FormerlySerializedAs("EnterDrag")]
        public FloatReference EnterAceleration = new FloatReference(2);
        [Tooltip("Exit Drag to decrease the Force when the Animal exits the zone")]
        public FloatReference ExitDrag = new FloatReference(4);

        [Tooltip("Limit the Current Force the animal may have")]
        [FormerlySerializedAs("Bounce")]
        public FloatReference LimitForce = new FloatReference(8);

        [Tooltip("Change if the Animal is Grounded when entering the Force Zone")]
        public BoolReference ForceGrounded = new  BoolReference();


        /// <summary>Current Animal the Zone is using </summary>
        public List<MAnimal> CurrentAnimals { get; internal set; }

        /// <summary> Store the Animals Mode Enter Actions </summary>
      //  public Dictionary<MAnimal, UnityAction> ModeListeners { get; internal set; }


        /// <summary>List of all collliders entering the Zone</summary>
        internal List<Collider> m_Colliders = new List<Collider>();

        public float ActionDelay = 0;
        [Tooltip("Value Assigned to the Mode Float Value when using the Mode Zone")]
         [Min(0)] public float ModeFloat = 0;
        public bool RemoveAnimalOnActive = false;


        public AnimalEvent OnEnter = new AnimalEvent();
        public AnimalEvent OnExit = new AnimalEvent();
        public AnimalEvent OnZoneActivation = new AnimalEvent();


        public AnimalEvent OnZoneFailed = new AnimalEvent();

        internal Collider ZoneCollider;
        //   protected Stats AnimalStats;

        /// <summary>Keep a Track of all the Zones on the Scene </summary>
        public static List<Zone> Zones;

      
        private int GetID
        {
            get
            {
                switch (zoneType)
                {
                    case ZoneType.Mode:
                        return modeID;
                    case ZoneType.State:
                        return stateID;
                    case ZoneType.Stance:
                        return stanceID;
                    case ZoneType.Force:
                        return 100;
                    default:
                        return 0;
                }
            }
        } 

        /// <summary>Is the zone a Mode Zone</summary>
        public bool IsMode => zoneType == ZoneType.Mode;

        /// <summary>Is the zone a Mode Zone</summary>
        public bool IsState => zoneType == ZoneType.State;

        /// <summary>Is the zone a Mode Zone</summary>
        public bool IsStance => zoneType == ZoneType.Stance;

        public List<Tag> Tags { get => tags; set => tags = value; }

        void OnTriggerEnter(Collider other)
        {
            if (IgnoreCollider(other)) return;                             //If the collider does not fill the requirements skip

            if (Tags != null && Tags.Count > 0)                             //Check if we are using Tags and if the entering animal does not have that tag the this zone is not for that animal
            {
                bool hasTag = false;
                foreach (var t in tags)
                {
                    if (t != null && other.transform.HasMalbersTagInParent(t))
                    {
                        hasTag = true;
                        break;
                    }
                }

                if (!hasTag)
                {
                    Debug.LogWarning($"The Zone:<B>[{name}]</B> cannot be activated by <B>[{other.transform.root.name}]</B>. The Zone is using Tags and <B>[{other.transform.root.name}]</B> does not have any.");
                    return;
                }
            }

            MAnimal animal = other.GetComponentInParent<MAnimal>();             //Get the animal on the entering collider
            if (!animal || animal.Sleep || !animal.enabled) return;       //If there's no animal, or is Sleep or disabled do nothing

            if (!m_Colliders.Contains(other))   m_Colliders.Add(other);            //if the entering collider is not already on the list add it
               

            if (CurrentAnimals.Contains(animal)) return;                        //if the animal is already on the list do nothing
            else
            {

               // animal.IsOnZone = true; //Let know the animal is on a zone
                animal.Zone = this; //Let know the animal is on a zone

                CurrentAnimals.Add(animal);                                     //Set a new Animal
                OnEnter.Invoke(animal);

                if (automatic)
                {
                    ActivateZone(animal);
                }
                else
                {
                    PrepareZone(animal);
                }
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (IgnoreCollider(other)) return;                             //If the collider does not fill the requirements skip

            MAnimal animal = other.GetComponentInParent<MAnimal>();

            if (!animal) return;                                            //If there's no animal script found skip all

            if (m_Colliders.Contains(other))                       //Remove the collider from the list that is exiting the zone.
                m_Colliders.Remove(other);

            if (CurrentAnimals.Contains(animal))    //Means that the Entering animal still exist on the zone
            {
                if (!m_Colliders.Exists(x => x.transform.root == animal.transform))  //Check if the Collider was removed
                {
                    OnExit.Invoke(animal);                                      //Invoke On Exit when all animal's colliders has exited the Zone
                    ResetStoredAnimal(animal);
                    CurrentAnimals.Remove(animal);

                    //animal.IsOnZone = true;   //Let know the animal  Not On the Zone anymore
                    animal.Zone = null;     //Let know the animal  Not On the Zone anymore
                }
            }
        }

        private bool IgnoreCollider(Collider other) =>
            !isActiveAndEnabled ||                                          //Check if the Zone is Active
            other.isTrigger ||                                              //No Triggers
            other.transform.root == transform.root ||                       //Triggers in the same hierarchy
            !MTools.CollidersLayer(other, Layer.Value) ||                   //Just accept animal layer only
            BoneOnly && !other.name.ToLower().Contains(BoneName.ToLower()); //If is Head Only and no head was found Skip


        /// <summary>Activate the Zone depending the Zone Type</summary>
        /// <param name="forced"></param>
        public virtual void ActivateZone(MAnimal animal)
        {
            Debug.Log("ActivateZone");
            var isZoneActive = false;
            
            //animal.IsOnZone = true; //Let know the animal is on a zone
            animal.Zone = this; //Let know the animal is on a zone


            switch (zoneType)
            {
                case ZoneType.Mode:
                    isZoneActive = ActivateModeZone(animal);
                    break;
                case ZoneType.State:
                    isZoneActive = ActivateStateZone(animal); //State Zones does not require to be delay or prepared to be activated Check if it can be activated
                    break;
                case ZoneType.Stance:
                    isZoneActive = ActivateStanceZone(animal); //State Zones does not require to be delay or prepared to be activated
                    break;
                case ZoneType.Force:
                    isZoneActive = SetForceZone(animal, true); //State Zones does not require to be delay or prepared to be activated
                    break;
            }
            if (isZoneActive)
            {
                if (debug) Debug.Log($"<b>{name}</b> [Zone Activate] -> <b>[{animal.name}]</b>");

                OnZoneActive(animal);
            }
        }

        public virtual void ActivateZone()
        {
            foreach (var animal in CurrentAnimals) ActivateZone(animal);
        }

        protected virtual void PrepareZone(MAnimal animal)
        {
            switch (zoneType)
            {
                case ZoneType.Mode:
                    var PreMode = animal.Mode_Get(ZoneID);

                    if (PreMode == null || !PreMode.HasAbilityIndex(ModeIndex)) //If the Animal does not have that mode or that Ability Index exti
                    {
                        OnZoneFailed.Invoke(animal);

                        Debug.LogWarning($"<B>[{name}]</B> cannot be activated by <B>[{animal.name}]</B>." +
                            $" It does not have The <B>[Mode {modeID.name}]</B> with <B>[Ability {ModeIndex}]</B>");
                        return;
                    }

                    PreMode.SetAbilityIndex(ModeIndex);

                    //ModeListeners.Add(animal, () => OnZoneActive(animal));  //Store the PreMode to listen when a Mode is executed
                    //PreMode.OnEnterMode.AddListener(ModeListeners[animal]);

                    break;
                case ZoneType.State:
                    var PreState = animal.State_Get(ZoneID);
                    if (!PreState) OnZoneFailed.Invoke(animal);
                    break;
                case ZoneType.Stance:
                    break;
                case ZoneType.Force:
                    break;
            }
        }


        /// <summary>Enables the Zone using the State</summary>
        private bool ActivateStateZone(MAnimal animal)
        {
            var Succesful = false;
            switch (stateAction)
            {
                case StateAction.Activate:
                    if (animal.ActiveStateID != ZoneID)
                    {
                        animal.State_Activate(ZoneID);
                        Succesful = true;
                    }
                    break;
                case StateAction.AllowExit:
                    if (animal.ActiveStateID == ZoneID)
                    {
                        animal.ActiveState.AllowExit();
                        Succesful = true;
                    }
                    break;
                case StateAction.ForceActivate:
                    animal.State_Force(ZoneID);
                    Succesful = true;
                    break;
                case StateAction.Enable:
                    animal.State_Enable(ZoneID);
                    Succesful = true;
                    break;
                case StateAction.Disable:
                    animal.State_Disable(ZoneID);
                    Succesful = true;
                    break;
                case StateAction.ChangeEnterStatus:
                    if (animal.ActiveStateID == ZoneID)
                    {
                        animal.State_SetStatus(stateStatus);
                        Succesful = true;
                    }
                    break;
                case StateAction.ChangeExitStatus:
                    if (animal.ActiveStateID == stateID)
                    {
                        animal.State_SetExitStatus(stateStatus);
                        Succesful = true;
                    }
                    break;
                default:
                    break;
            }
            return Succesful;
        }

        /// <summary>Enables the Zone using the Modes</summary>
        private bool ActivateModeZone(MAnimal animal)
        {
            if (!animal.IsPlayingMode)
            {
                animal.Mode_SetPower(ModeFloat); //Set the correct height for the Animal Animation 
                return animal.Mode_TryActivate(ZoneID, ModeIndex);
            }
            
            return false;
        }

        /// <summary>Enables the Zone using the Stance</summary>
        private bool ActivateStanceZone(MAnimal animal)
        {
            switch (stanceAction)
            {
                case StanceAction.Enter:
                    animal.Stance_Set(ZoneID);
                    break;
                case StanceAction.Exit:
                    animal.Stance_Reset();
                    break;
                case StanceAction.Toggle:
                    animal.Stance_Toggle(ZoneID);
                    break;
                case StanceAction.Stay:
                    animal.Stance_Set(ZoneID);
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>Enables the Zone using External Forces</summary>
        private bool SetForceZone(MAnimal animal ,bool ON)
        {
            if (ON) //ENTERING THE FORCE ZONE!!!
            {
                var StartExtForce = animal.CurrentExternalForce + animal.GravityStoredVelocity; //Calculate the Starting force


                if (StartExtForce.magnitude > LimitForce)
                    StartExtForce = StartExtForce.normalized * LimitForce; //Add the Bounce

                animal.CurrentExternalForce = StartExtForce;
                animal.ExternalForce = transform.up * Force;
                animal.ExternalForceAcel = EnterAceleration;

                if (animal.ActiveState.ID == StateEnum.Fall) //If we enter to a zone from the Fall state.. Reset the Fall Current Distance
                {
                    var fall = animal.ActiveState as Fall;
                    fall.FallCurrentDistance = 0;
                }

                animal.Grounded = ForceGrounded.Value;
            }
            else
            {
                if (animal.ActiveState.ID == StateEnum.Fall) animal.UseGravity = true;  //If we are on the Fall State -- Reactivate the Gravity

                if (ExitDrag > 0) //
                {
                    animal.ExternalForceAcel = ExitDrag;
                    animal.ExternalForce = Vector3.zero;
                }
            }
            return ON;
        }


        internal void OnZoneActive(MAnimal animal)
        {
            OnZoneActivation.Invoke(animal);
            
            if (RemoveAnimalOnActive)
            {
                ResetStoredAnimal(animal);
                CurrentAnimals.Remove(animal);
            }
        }


        public void TargetArrived(GameObject go)
        {
            var animal = go.FindComponent<MAnimal>();
            ActivateZone(animal);
        }    

        public virtual void ResetStoredAnimal(MAnimal animal)
        {
            //animal.IsOnZone = false; //Tell the Animal is no longer on a Zone
            animal.Zone = null; //Tell the Animal is no longer on a Zone

            switch (zoneType)
            {
                case ZoneType.Mode:

                    var mode = animal.Mode_Get(ZoneID);

                    if (mode != null) //Means we found the current Active mode
                    {
                        if (mode.AbilityIndex == ModeIndex) mode.ResetAbilityIndex(); //Only reset when it has the same Index... works for zones near eachother 

                        //if (ModeListeners.TryGetValue(animal, out UnityAction action))
                        //{
                        //    mode.OnEnterMode.RemoveListener(action);
                        //    ModeListeners.Remove(animal);  //Remove the Premode
                        //}
                    }

                    break;
                case ZoneType.State:

                    break;
                case ZoneType.Stance:
                    if (stanceAction == StanceAction.Stay && animal.Stance == stanceID.ID) animal.Stance_Reset();
                    break;
                case ZoneType.Force:
                    SetForceZone(animal, false);
                    break;
                default:
                    break;
            } 
        }

        void OnEnable()
        {
            if (Zones == null)
                Zones = new List<Zone>();


            if (ZoneCollider == null)
                ZoneCollider = GetComponent<Collider>();                  //Get the reference for the collider


            ZoneCollider.isTrigger = true;                                //Force Trigger
            Zones.Add(this);                                              //Save the the Action Zones on the global Action Zone list

            if (ZoneID == 0) ZoneID = GetID;

            CurrentAnimals = new List<MAnimal>();                          //Get the reference for the collider
            //ModeListeners = new Dictionary<MAnimal, UnityAction>();
        }

        void OnDisable()
        {
            Zones.Remove(this);                                              //Remove the the Action Zones on the global Action Zone list

            foreach (var animals in CurrentAnimals)
            {
                ResetStoredAnimal(animals);
            }

            CurrentAnimals = new List<MAnimal>();                          //Get the reference for the collider
           // ModeListeners = new Dictionary<MAnimal, UnityAction>();
        } 


        [HideInInspector] public bool EditorShowEvents = false;
        [HideInInspector] public bool ShowStatModifiers = false;
    }

    public enum StateAction
    {
        /// <summary>Tries to Activate the State of the Zone</summary>
        Activate,
        /// <summary>If the Animal is already on the state of the zone it will allow to exit and activate states below the Active one</summary>
        AllowExit,
        /// <summary>Force the State of the Zone to be enable even if it cannot be activate at the moment</summary>
        ForceActivate,
        /// <summary>Enable a  Disabled State </summary>
        Enable,
        /// <summary>Disable State </summary>
        Disable,
        /// <summary>Set a State Status on a State</summary>
        ChangeEnterStatus,
        /// <summary>Set a State Status on a State</summary>
        ChangeExitStatus,

    }
    public enum StanceAction
    {
        /// <summary>Enters a Stance</summary>
        Enter,
        /// <summary>Exits a Stance</summary>
        Exit,
        /// <summary>Toggle a Stance</summary>
        Toggle,
        /// <summary>While the Animal is inside the collider the Animal will stay on the Stance</summary>
        Stay,
    }
    public enum ZoneType
    {
        Mode,
        State,
        Stance,
        Force
    }


    #if UNITY_EDITOR
    [CustomEditor(typeof(Zone))/*, CanEditMultipleObjects*/]
    public class ZoneEditor : Editor
    {
        private Zone m;

        SerializedProperty
            HeadOnly, stateAction, HeadName, zoneType, stateID, modeID, modeIndex, ActionID, auto, debug,  m_abilityStatus, AbilityTime,
            OnZoneActivation, OnExit, OnEnter, ForceGrounded, OnZoneFailed,
            stanceAction, layer, stanceID, RemoveAnimalOnActive, m_tag, ModeFloat, Force, EnterAceleration, ExitAceleration, stateStatus, Bounce;

        //MonoScript script;
        private void OnEnable()
        {
            m = ((Zone)target);
            //script = MonoScript.FromMonoBehaviour((MonoBehaviour)target);

            HeadOnly = serializedObject.FindProperty("BoneOnly");
            HeadName = serializedObject.FindProperty("BoneName");

            RemoveAnimalOnActive = serializedObject.FindProperty("RemoveAnimalOnActive");
            layer = serializedObject.FindProperty("Layer");

            stateStatus = serializedObject.FindProperty("stateStatus");
            OnZoneFailed = serializedObject.FindProperty("OnZoneFailed");


            Force = serializedObject.FindProperty("Force");
            EnterAceleration = serializedObject.FindProperty("EnterAceleration");
            ExitAceleration = serializedObject.FindProperty("ExitDrag");
            Bounce = serializedObject.FindProperty("LimitForce");
            ForceGrounded = serializedObject.FindProperty("ForceGrounded");

            m_abilityStatus = serializedObject.FindProperty("m_abilityStatus");
            AbilityTime = serializedObject.FindProperty("AbilityTime");


            m_tag = serializedObject.FindProperty("tags");
            ModeFloat = serializedObject.FindProperty("ModeFloat");
            zoneType = serializedObject.FindProperty("zoneType");
            stateID = serializedObject.FindProperty("stateID");
            stateAction = serializedObject.FindProperty("stateAction");
            stanceAction = serializedObject.FindProperty("stanceAction");
            modeID = serializedObject.FindProperty("modeID");
            stanceID = serializedObject.FindProperty("stanceID");
            modeIndex = serializedObject.FindProperty("modeIndex");
            ActionID = serializedObject.FindProperty("ActionID");
            auto = serializedObject.FindProperty("automatic");
            debug = serializedObject.FindProperty("debug");


            OnEnter = serializedObject.FindProperty("OnEnter");
            OnExit = serializedObject.FindProperty("OnExit");
            OnZoneActivation = serializedObject.FindProperty("OnZoneActivation"); 
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MalbersEditor.DrawDescription("Area to modify States, Stances or Modes on an Animal");

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(auto, new GUIContent("Automatic", "As soon as the animal enters the zone it will execute the logic. If False then Call the Method Zone.Activate()"));

                MalbersEditor.DrawDebugIcon(debug);

                //var currentGUIColor = GUI.color;
                //GUI.color = debug.boolValue ? Color.red : currentGUIColor;
                //debug.boolValue = GUILayout.Toggle(debug.boolValue, "Debug", EditorStyles.miniButton, GUILayout.Width(50));
                //GUI.color = currentGUIColor;



                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(layer, new GUIContent("Animal Layer", "Layer to detect the Animal"));

                EditorGUILayout.PropertyField(zoneType, new GUIContent("Zone Type", "Choose between a Mode or a State for the Zone"));

                ZoneType zone = (ZoneType)zoneType.intValue;


                switch (zone)
                {
                    case ZoneType.Mode:
                        EditorGUILayout.PropertyField(modeID, new GUIContent("Mode ID", "Which Mode to Set when entering the Zone"));

                        serializedObject.ApplyModifiedProperties();


                        if (m.modeID != null && m.modeID == 4)
                        {
                            EditorGUILayout.PropertyField(ActionID, new GUIContent("Action Index", "Which Action to Set when entering the Zone"));

                            if (ActionID.objectReferenceValue == null)
                            {
                                EditorGUILayout.HelpBox("Please Select an Action ID", MessageType.Error);
                            }
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(modeIndex, new GUIContent("Ability Index", "Which Ability to Set when entering the Zone"));
                            if (ActionID.objectReferenceValue == null)
                            {
                                EditorGUILayout.HelpBox("Please Select an Ability ID", MessageType.Error);
                            }
                        }

                        EditorGUILayout.PropertyField(m_abilityStatus, new GUIContent("Status","Ability Status"));

                        if (m_abilityStatus.intValue == (int)AbilityStatus.ActiveByTime )
                        {
                            EditorGUILayout.PropertyField(AbilityTime);
                        }
                        EditorGUILayout.PropertyField(ModeFloat, new GUIContent("Mode Power"));

                        break;
                    case ZoneType.State:
                        EditorGUILayout.PropertyField(stateID, new GUIContent("State ID", "Which State will Activate when entering the Zone"));
                        EditorGUILayout.PropertyField(stateAction, new GUIContent("Action", "Set what action for State the animal will apply when entering the zone"));

                        int stateaction = stateAction.intValue;
                        if (stateaction == (int)StateAction.ChangeEnterStatus || stateaction == (int)StateAction.ChangeExitStatus)
                        {
                            EditorGUILayout.PropertyField(stateStatus, new GUIContent("Status", "Set the State Status"));
                        }

                        if (stateID.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("Please Select an State ID", MessageType.Error);
                        }
                        break;
                    case ZoneType.Stance:
                        EditorGUILayout.PropertyField(stanceID, new GUIContent("Stance ID", "Which Stance will Activate when entering the Zone"));
                        EditorGUILayout.PropertyField(stanceAction, new GUIContent("Action", "Set what action for stance the animal will apply when entering the zone"));
                        if (stanceID.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("Please Select an Stance ID", MessageType.Error);
                        }
                        break;
                    case ZoneType.Force:
                        EditorGUILayout.PropertyField(Force);
                        EditorGUILayout.PropertyField(EnterAceleration);
                        EditorGUILayout.PropertyField(ExitAceleration);
                        EditorGUILayout.PropertyField(Bounce);
                        EditorGUILayout.PropertyField(ForceGrounded, new GUIContent("Grounded? "));
                        break;
                    default:
                        break;
                }



                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.PropertyField(RemoveAnimalOnActive, 
                        new GUIContent("Reset on Active", "Remove the stored Animal on the Zone when the Zones gets Active, Reseting it to its default state"));
                  
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(m_tag, 
                        new GUIContent("Tags", "Set this parameter if you want the zone to Interact only with gameObject with that tag"));
                    EditorGUI.indentLevel--;

                    EditorGUILayout.PropertyField(HeadOnly, 
                        new GUIContent("Bone Only", "Activate when a bone enter the Zone.\nThat Bone needs a collider!!"));
                  
                    if (HeadOnly.boolValue)
                        EditorGUILayout.PropertyField(HeadName, 
                            new GUIContent("Bone Name", "Name for the Bone you need to check if it has enter the zone"));
                }
                EditorGUILayout.EndVertical();

                 
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.indentLevel++;
                m.EditorShowEvents = EditorGUILayout.Foldout(m.EditorShowEvents, "Events");
                EditorGUI.indentLevel--;

                if (m.EditorShowEvents)
                {
                    EditorGUILayout.PropertyField(OnEnter, new GUIContent("On Animal Enter Zone"));
                    EditorGUILayout.PropertyField(OnExit, new GUIContent("On Animal Exit Zone"));
                    EditorGUILayout.PropertyField(OnZoneActivation, new GUIContent("On Zone Active"));
                    EditorGUILayout.PropertyField(OnZoneFailed, new GUIContent("On Zone Failed"));
                }
                EditorGUILayout.EndVertical();



                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Zone Inspector");
                    EditorUtility.SetDirty(target);
                }


                if (Application.isPlaying && debug.boolValue)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

                    EditorGUILayout.ObjectField("Zone Collider", m.ZoneCollider, typeof(Collider), false);

                    EditorGUILayout.LabelField("Current Animals (" + m.CurrentAnimals.Count + ")", EditorStyles.boldLabel);
                    foreach (var item in m.CurrentAnimals)
                    {
                        EditorGUILayout.ObjectField(item.name, item, typeof(MAnimal), false);
                    }

                    EditorGUILayout.LabelField("Current Colliders (" + m.m_Colliders.Count + ")", EditorStyles.boldLabel);
                    foreach (var item in m.m_Colliders)
                    {
                        EditorGUILayout.ObjectField(item.name, item, typeof(Collider), false);
                    }

                    EditorGUILayout.EndVertical();
                    Repaint();
                    EditorGUI.EndDisabledGroup();
                }

            }
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
