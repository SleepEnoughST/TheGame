using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    void Update()
    {
        Time.timeScale += (1f / slowdownLength) * (Time.unscaledDeltaTime / 2);
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1f);
        //if (Time.fixedDeltaTime <= 0.02f)
        //{
        //    Time.fixedDeltaTime += Time.deltaTime;
        //    Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.02f);
        //}
        print(Time.timeScale);
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
