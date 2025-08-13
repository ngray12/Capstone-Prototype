using UnityEngine;
using System;

namespace CMIYC
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Economy")]
        [SerializeField] private int carriedCash = 0;
        [SerializeField] private int bankedCash = 0;
        [SerializeField] private float carriedWeight = 0f;

        [Header("Encumbrance Settings")]
        [Tooltip("At or above this weight, movement is at minimum multiplier.")]
        public float maxCarryWeight = 100f;
        [Range(0.1f, 1f)] public float minSpeedMultiplier = 0.4f;
        [Range(0.1f, 1f)] public float minJumpMultiplier = 0.5f;

        [Header("Alarm")]
        public bool alarmTriggered = false;
        public float alarmLevel = 0f; // 0..1
        public float alarmDecayPerSecond = 0.05f;

        [Header("State")]
        public CMIYC.GameState state = GameState.Heist;

        public event Action<int,int> OnCashChanged;           // carried, banked
        public event Action<float> OnWeightChanged;           // carriedWeight
        public event Action<bool> OnAlarmChanged;             // alarm on/off
        public event Action<GameState> OnStateChanged;        // state

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!alarmTriggered && alarmLevel > 0f)
            {
                alarmLevel = Mathf.Max(0f, alarmLevel - alarmDecayPerSecond * Time.deltaTime);
            }
        }

        public float GetSpeedMultiplier()
        {
            if (maxCarryWeight <= 0f) return 1f;
            float t = Mathf.Clamp01(carriedWeight / maxCarryWeight);
            return Mathf.Lerp(1f, minSpeedMultiplier, t);
        }

        public float GetJumpMultiplier()
        {
            if (maxCarryWeight <= 0f) return 1f;
            float t = Mathf.Clamp01(carriedWeight / maxCarryWeight);
            return Mathf.Lerp(1f, minJumpMultiplier, t);
        }

        public void AddCash(int value, float weight)
        {
            carriedCash += value;
            carriedWeight += Mathf.Max(0f, weight);
            OnCashChanged?.Invoke(carriedCash, bankedCash);
            OnWeightChanged?.Invoke(carriedWeight);
        }

        public void DepositAll()
        {
            bankedCash += carriedCash;
            carriedCash = 0;
            carriedWeight = 0f;
            OnCashChanged?.Invoke(carriedCash, bankedCash);
            OnWeightChanged?.Invoke(carriedWeight);
        }

        public void TriggerAlarm(float intensity = 1f)
        {
            alarmTriggered = true;
            alarmLevel = Mathf.Clamp01(alarmLevel + intensity);
            OnAlarmChanged?.Invoke(alarmTriggered);
            if (state == GameState.Heist) SetState(GameState.Escape);
        }

        public void ResetAlarm()
        {
            alarmTriggered = false;
            alarmLevel = 0f;
            OnAlarmChanged?.Invoke(alarmTriggered);
        }

        public void SetState(GameState newState)
        {
            state = newState;
            OnStateChanged?.Invoke(state);
        }

        public (int carried, int banked, float weight) Snapshot()
        {
            return (carriedCash, bankedCash, carriedWeight);
        }

        public void OnPlayerCaught()
        {
            SetState(GameState.Caught);
            // Optional: lose carried cash when caught
            carriedCash = 0;
            carriedWeight = 0f;
            OnCashChanged?.Invoke(carriedCash, bankedCash);
            OnWeightChanged?.Invoke(carriedWeight);
        }
    }
}
