using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningCube : MonoBehaviour {
    public float speed = 2f;
    public Transform center;
    private float distance;
    private float height;
    private float moveSpeed;
    private float randomTime;

	// Use this for initialization
	void Start () {
        randomTime = Random.Range(0f, 2000f);
        distance = Random.Range(.3f, 1f);
        height = Random.Range(-0.5f, 0.5f);
        moveSpeed = Random.Range(-.5f, .5f);
        Renderer ren = GetComponentInChildren<Renderer>();
        ren.material = new Material(ren.material);
        ren.material.color = new Color(Random.Range(0.3f, 1f), Random.Range(0.3f, 1f), Random.Range(0.3f, 1f));
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(Time.deltaTime * 3f * speed, Time.deltaTime * 2f * speed, Time.deltaTime * 5.6f * speed));
        float r = Time.time * moveSpeed + randomTime;
        Vector3 add = new Vector3(Mathf.Sin(r) * distance, height, Mathf.Cos(r) * distance);
        transform.position = center.position + add;
	}
}
