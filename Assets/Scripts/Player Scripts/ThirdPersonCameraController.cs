using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public float rotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;

    public Transform Obstruction;
    float zoomSpeed = 2f;
    void Start(){
        Obstruction = Target;
        //Cursor.visible = false; // Hides the cursor when game starts
        //Cursor.lockState = CursorLockMode.Locked; // locks the cursor in place so that when you move it doesnt move outside the window
    }

    void LateUpdate(){ // Late update on camera control, this is different to the once a frame one ... i think

        CamControl();
        ViewObstructed();
    }

    void CamControl(){ // Camera is based on the mouse with the player locked in the central view.
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        //mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(Target);

        // Locks Camera to facing direction when held.
        if (Input.GetKey(KeyCode.LeftShift))
        {

        } else {// this rotates the camera when shift is not held, this could be flipped but i kinda like this, could even be used as some sort of feature :L
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            Player.rotation = Quaternion.Euler(0, mouseX, 0);
        }

    }

    void ViewObstructed(){ //This deals with any obstructions to the camera, Basically turns off the rendering of the object and lets you see the player. 
                            //Relys on things being tagged properly in unity.
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, 4.5f))
        {
            if(hit.collider.gameObject.tag != "Player")
            {
                Obstruction = hit.transform;
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if(Vector3.Distance(Obstruction.position, transform.position) >= 3f && Vector3.Distance(transform.position, Target.position) >= 1.5f)
                {
                   transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime); 
                }
            }
            else{
                Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if(Vector3.Distance(transform.position, Target.position) < 4.5f){
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }
            }
        }


    }

    
}
