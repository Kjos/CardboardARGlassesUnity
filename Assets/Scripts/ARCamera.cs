using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VR;

public class ARCamera : MonoBehaviour {
	public float rotationY = 10f;
    public bool neckmodel = true;
	public float xFovMultiplier = 1f;
    public float extrapolation = 1f;
    public string onClickChangeToScene = "";
    public bool debugRotate = false;
    public bool switchEyes = true;

    private Camera camera;
	private Matrix4x4 flipMatrix = Matrix4x4.Scale (new Vector3 (-1, 1, 1));
    private Quaternion prevCameraQuat = Quaternion.identity;
    private Quaternion prevDiff = Quaternion.identity;

	// Use this for initialization
	void Start () {
		if (!neckmodel) UnityEngine.XR.InputTracking.disablePositionalTracking = true;

		camera = GetComponent<Camera> ();

		// Adjust projection matrix with angle
		//flipMatrix = Matrix4x4.Rotate(Quaternion.AngleAxis (-rotationY, Vector3.right)) * flipMatrix;

		camera.transform.parent.localScale = new Vector3 (-1, 1, -1);
	}

	// Update is called once per frame
	private Vector3 move = new Vector3(1, 1, 0);
    private bool touched = false;
	void Update () {
		if (debugRotate && Application.isEditor) {
			camera.transform.Rotate (move, Time.deltaTime);
		}
        if (Input.touchCount == 0)
        {
            if (touched) SceneManager.LoadScene(onClickChangeToScene);
        } else
        {
            touched = true;
        }
	}

	void OnPreCull ()
    {
        Quaternion currentCameraQuat = camera.transform.localRotation;
        Quaternion diff = Quaternion.Inverse(prevCameraQuat) * currentCameraQuat;
        Quaternion factorDiff = Quaternion.LerpUnclamped(prevDiff, diff, extrapolation);
        prevCameraQuat = currentCameraQuat;
        prevDiff = diff;

        if (Application.isEditor)
        {
            camera.transform.parent.rotation *= factorDiff;

            camera.ResetWorldToCameraMatrix();
			camera.ResetProjectionMatrix ();
			camera.projectionMatrix = XFovMatrix (camera.projectionMatrix);
			camera.projectionMatrix = camera.projectionMatrix * flipMatrix;
		} else {
			camera.transform.parent.rotation = Quaternion.AngleAxis (-rotationY, camera.transform.right);
            camera.transform.parent.rotation *= factorDiff;
            camera.ResetWorldToCameraMatrix();

			camera.ResetStereoProjectionMatrices ();
			Matrix4x4 left = camera.GetStereoProjectionMatrix (Camera.StereoscopicEye.Left);
			Matrix4x4 right = camera.GetStereoProjectionMatrix (Camera.StereoscopicEye.Right);
			left = XFovMatrix (left);
			right = XFovMatrix (right);
			left *= flipMatrix;
			right *= flipMatrix;
			camera.SetStereoProjectionMatrix (Camera.StereoscopicEye.Left, switchEyes ? right : left);
			camera.SetStereoProjectionMatrix (Camera.StereoscopicEye.Right, switchEyes ? left : right);
		}
	}

	void OnPreRender () {
		GL.invertCulling = true;
	}

	void OnPostRender () {
		GL.invertCulling = false;
	}


	//   -------------------------------------------------------  Set Matrix Functions
	Matrix4x4 XFovMatrix(Matrix4x4 input)
	{
		input[ 0, 0 ] *= xFovMultiplier;
		return input;
	}
}
