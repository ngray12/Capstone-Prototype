using UnityEngine;
using UnityEngine.UI;

namespace CMIYC
{
    public class CashHUD : MonoBehaviour
    {
        public Text carriedText;
        public Text bankedText;
        public Image alarmFill; // radial or bar

        private void Start()
        {
            Refresh();
            var gm = GameManager.Instance;
            gm.OnCashChanged += OnCashChanged;
            gm.OnAlarmChanged += OnAlarmChanged;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnCashChanged -= OnCashChanged;
            GameManager.Instance.OnAlarmChanged -= OnAlarmChanged;
        }

        void OnCashChanged(int carried, int banked) => Refresh();
        void OnAlarmChanged(bool active) => Refresh();

        void Refresh()
        {
            var gm = GameManager.Instance;
            var snap = gm.Snapshot();
            if (carriedText) carriedText.text = $"Carried: ${snap.carried} ({snap.weight:0}kg)";
            if (bankedText) bankedText.text = $"Banked: ${snap.banked}";
            if (alarmFill) alarmFill.fillAmount = Mathf.Clamp01(gm.alarmLevel);
        }
    }
}
