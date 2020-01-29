using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerGlobalShaderDataHelper : MonoBehaviour
{
    public PlayerModel playerModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGlobalShaderProps();
    }

    public void UpdateGlobalShaderProps()
    {
        Shader.SetGlobalVector("_playerPosition", transform.position);
        Shader.SetGlobalFloat("_playerSpeed", playerModel.playerSpeed);
        Shader.SetGlobalFloat("_perfectTimeThreshold", playerModel.perfectTimeThreshold);
        Shader.SetGlobalFloat("_maxAcceptableTimeThreshold", playerModel.maxAcceptableTimeThreshold);
    }
}
