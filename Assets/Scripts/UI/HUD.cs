using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI accuracyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAccuracy();
    }

    void UpdateAccuracy()
    {
        var accuracyRemapped = HelperUtilities.Remap(PlayerModel.Instance.accuracy, 0, PlayerModel.maxScore, 0, 100f);
        accuracyText.text = $"Accuracy: {accuracyRemapped:###.##}%";
    }
}
