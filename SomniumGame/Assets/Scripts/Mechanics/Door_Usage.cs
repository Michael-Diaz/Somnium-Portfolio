using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Usage : MonoBehaviour
{
    private float openState;
    private float targetYAngle;

    private Transform doorHinge;
    private MeshCollider doorCollider;

    private float timer;
    private bool moveDoorAnim;
    private Quaternion snapshotAngle;

    // Start is called before the first frame update
    void Start()
    {
        openState = 0.0f;
        targetYAngle = 0.0f;

        doorHinge = gameObject.transform.parent.GetComponent<Transform>();
        doorCollider = gameObject.GetComponent<MeshCollider>();

        timer = 0.0f;
        moveDoorAnim = false;
    }

    void Update()
    {
        if (moveDoorAnim)
        {
            timer += Time.deltaTime;
            float animPercent = timer / 0.5f;

            if (doorHinge.eulerAngles.y != targetYAngle)
            {
                doorHinge.rotation = Quaternion.Lerp(snapshotAngle, Quaternion.Euler(0.0f, targetYAngle, 0.0f), animPercent);
                doorCollider.enabled = false;
            }
            else
            {
                doorCollider.enabled = true;

                moveDoorAnim = false;
                timer = 0.0f;
            }
        }
    }

    public void alterDoor(float sideState)
    {
        if (openState != 0.0f)
            openState = 0.0f;
        else
            openState = sideState;

        snapshotAngle = doorHinge.rotation;

        targetYAngle = (openState >= 0.0f) ? (openState * 90.0f) : ((openState * 90.0f) + 360.0f);
        moveDoorAnim = true;

        timer = 0.0f;
    }
}
