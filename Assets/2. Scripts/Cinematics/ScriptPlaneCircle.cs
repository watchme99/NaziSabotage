using UnityEngine;

public class ScriptPlaneCircle : MonoBehaviour {
    public Transform dropPoint;
    public float speed;
	
	void FixedUpdate () {
        transform.RotateAround(dropPoint.position, Vector3.up, speed * Time.deltaTime);
    }
}
