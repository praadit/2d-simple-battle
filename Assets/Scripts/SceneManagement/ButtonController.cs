using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SceneController _scene;
    private void Start()
    {
        _scene = SceneController.instance;
    }

    public void goToSurvival()
    {
        _scene.ToSurvival();
    }

    public void goToArcade()
    {
        _scene.ToArcade();
    }
}
