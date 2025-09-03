using CMIYC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Slider fillImage;
    [SerializeField] private TextMeshProUGUI timer;

    private LootSack lootSack;
    



    // Start is called before the first frame update
    void Start()
    {
        lootSack = FindObjectOfType<LootSack>();

        if (lootSack == null)
        {
            Debug.LogError("No LootSack Found");

            UpdateBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(lootSack != null)
        {
            UpdateBar();
        }

        timer.text = GameManager.Instance.timerText.text;
    }


    private void UpdateBar()
    {
        float ratio = lootSack.totalEncumbrance / lootSack.maxEncumbrance;
        fillImage.value = Mathf.Clamp01(ratio);
    }

}
