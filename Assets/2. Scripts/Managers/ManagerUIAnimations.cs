using UnityEngine;
using System.Collections;

public class ManagerUIAnimations : MonoBehaviour {
    private ArrayList states = new ArrayList();
    private float lastRealTime = 0.0f;
	
	void Update () {
        if (states.Count > 0)
        {
            Animate();
        }
    }

    public void ResetLastRealTime()
    {
        lastRealTime = 0f;
    }

    //Add animation to list of animations (to play multiple animations)
    public void AddAnimation(GameObject go)
    {
        if (Time.timeScale != 1)
        {
            AnimationState state = go.GetComponent<Animation>()["Menu"];
            states.Add(state);
            lastRealTime = Time.realtimeSinceStartup;
        }
    }

    //calculate own delta time to animate when timescale = 0
    private void Animate()
    {
        for (int i = 0; i < states.Count; i++)
        {
            AnimationState state = (AnimationState)states[i];
            state.weight = 1;
            state.enabled = true;
            state.time += (Time.realtimeSinceStartup - lastRealTime);

            if (state.time >= state.length)
                states.Remove(state);
        }
    }
}
