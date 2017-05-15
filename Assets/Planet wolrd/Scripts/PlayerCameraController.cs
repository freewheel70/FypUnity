using UnityEngine;
using System.Collections;

public class PlayerCameraController : MonoBehaviour{

    private bool startMoveCamera = false;  

    private float perspectiveZoomSpeed = 0.5f;  // The rate of change of the field of view in perspective mode.
    private float orthoZoomSpeed = 0.3f;  // The rate of change of the orthographic size in orthographic mode.

    private GameObject player;

    private Vector3 offset;
    private Quaternion rotation;
    Camera myCamera;

    private Mass myMass;

    Transform labelHolder;
   // TextMesh debugLabel;

    // Use this for initialization
    void Start(){
        player = this.transform.parent.gameObject;
        offset = transform.position - player.transform.position;
        rotation = transform.rotation;
        myCamera = this.GetComponent<Camera>();
        myMass = player.GetComponent<Mass>();

        labelHolder = player.transform.Find("LabelHolder");
        //debugLabel = labelHolder.Find("DebugLabel").GetComponent<TextMesh>();
    }

    void LateUpdate(){        
        transform.rotation = rotation;
        updateCamera();
    }

    public void updateCamera(){        

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        transform.position = player.transform.position + offset;
        //debugLabel.text = "CMR " + currentMassRate();
        return;
#else
        if (Input.touchCount == 2){
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
           
            // ... change the orthographic size based on the change in distance between the touches.

            float newOrthographicSize = myCamera.orthographicSize + deltaMagnitudeDiff * orthoZoomSpeed;
            float massRate = currentMassRate();

            newOrthographicSize = Mathf.Max(newOrthographicSize, 0.8f * massRate);
            newOrthographicSize = Mathf.Min(newOrthographicSize, 25.0f * massRate);
            myCamera.orthographicSize = newOrthographicSize;

           // debugLabel.text="OGS "+newOrthographicSize;
        }
#endif
    }


    private float currentMassRate(){
        return 1.0f * myMass.currentMass / myMass.initMass;
    }

}
