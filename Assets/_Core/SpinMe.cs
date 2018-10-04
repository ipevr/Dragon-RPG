using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMe : MonoBehaviour {

	[SerializeField] float xRotationsPerMinute = 1f;
	[SerializeField] float yRotationsPerMinute = 1f;
	[SerializeField] float zRotationsPerMinute = 1f;
	
	void Update () {
        // xDegreesPerFrame = Time.deltaTime, 60, 360, xRotationsPerMinute
        // degrees frame^-1 = seconds frame^-1, seconds minute^-1, degrees rotation^-1, rotation minute^-1
        // degrees frame^-1 = seconds frame^-1 / seconds minute^-1, degrees rotation^-1, rotation minute^-1
        // degrees frame^-1 = frame^-1 minute, degrees rotation^-1 * rotation minute^-1
        // degrees frame^-1 = frame^-1 minute, degrees minute^-1
        // degrees frame^-1 = frame^-1 minute * degrees minute^-1
        // degrees frame^-1 = frame^-1 degrees
        // degrees frame^-1 = seconds frame^-1 / seconds minute^-1 * degrees rotation^-1 * rotation minute^-1
        // xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute
        // xDegreesPerFrame = Time.deltaTime * 60 * xRotationsPerMinute
        // degrees frame^-1 = degrees rotation^-1 * seconds frame^-1 / seconds minute^-1 * rotation minute^-1
        // xDegreesPerFrame = 360 * Time.deltaTime / 60 * xRotationsPerMinute

        float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute;
        transform.RotateAround (transform.position, transform.right, xDegreesPerFrame);

		float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute;
        transform.RotateAround (transform.position, transform.up, yDegreesPerFrame);

        float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute;
        transform.RotateAround (transform.position, transform.forward, zDegreesPerFrame);
	}
}
