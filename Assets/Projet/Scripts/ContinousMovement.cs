using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinousMovement : MonoBehaviour
{
    public float speed = 1;
    private Vector2 inputAxis;
    public XRNode inputSourrce;
    public float gravity = -9.881f;
    public float additionalHeigth = 0.2f;
    private CharacterController character;
    private XRRig rig;

    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
      InputDevice device =  InputDevices.GetDeviceAtXRNode(inputSourrce);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }
    private void FixedUpdate()
    {
        CapsulFollowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * speed);
    }
    void CapsulFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeigth;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height*0.5f + character.skinWidth, capsuleCenter.z);
    }

}
