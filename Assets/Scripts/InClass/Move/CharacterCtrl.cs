using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] //������ �ٵ� ������ �� ��ũ��Ʈ�� ������ �ȵ�
public class CharacterCtrl : MonoBehaviour
{
    private CharacterController characterController; //CharacterController Cashing Ready
    private float spd = 5.0f;

    private float jumpValue = 2.0f;
    private float dashValue = 5.0f;

    private Vector3 directionValue = Vector3.zero;

    private float gravity = -9.81f;
    public Vector3 drags;
    private Vector3 calcVelocity = Vector3.zero;

    public LayerMask layerGround;
    private bool flagOnGrounded;
    private float defaultGroundDistance = 0.2f; //���� �ִ��� �Ǵ� ���ذ�

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        Vector3 posOrigin = transform.position + (Vector3.up * 0.1f);

        Debug.DrawLine(posOrigin, transform.position + (Vector3.up * 0.1f) + (Vector3.down * defaultGroundDistance));

        if (Physics.Raycast(posOrigin, Vector3.down, out hitInfo, defaultGroundDistance, layerGround)) flagOnGrounded = true;
        else flagOnGrounded = false;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();//Cashing
    }

    void Update() //������ ���� Update
    {
        //CheckGroundStatus();
        flagOnGrounded = characterController.isGrounded;

        if (flagOnGrounded && calcVelocity.y < 0)
        {
            calcVelocity.y = 0.0f;
        }

        //����� �Է� �� �ޱ�, �¿�, �յ�
        //directionValue = Vector3.zero;
        //directionValue.x = Input.GetAxis("Horizontal");
        //directionValue.z = Input.GetAxis("Vertical");
        directionValue = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //�ٸ� �� ���� �׳� 1��

        characterController.Move(directionValue * Time.deltaTime * spd);

        //����� �Է���  �ִٸ�, ���� ������ �Է� �� �������� �����϶�
        if (directionValue != Vector3.zero)
        {
            transform.forward = directionValue;
        }

        if (Input.GetButtonDown("Jump") && flagOnGrounded)
        {
            calcVelocity.y += Mathf.Sqrt(jumpValue - 2f * gravity);
            //Mathf.Sqrt(jumpValue - 2f * Physics.gravity.y) ���� ����, gravity = -9.81, ������ó�� ���̰�
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //���� �������� ���� �������

            float posDashEndX = Mathf.Log(1f / (Time.deltaTime * drags.x + 1)) / -Time.deltaTime;
            float posDashEndZ = Mathf.Log(1f / (Time.deltaTime * drags.z + 1)) / -Time.deltaTime;
            //Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime; �ڿ������� ����

            Vector3 dashVelocity = Vector3.Scale(transform.forward, dashValue * new Vector3(posDashEndX, 0, posDashEndZ));

            calcVelocity += dashVelocity;
        }

        calcVelocity.y += gravity * Time.deltaTime;

        calcVelocity.x /= 1 + drags.x * Time.deltaTime;
        calcVelocity.y /= 1 + drags.y * Time.deltaTime;
        calcVelocity.z /= 1 + drags.z * Time.deltaTime;

        characterController.Move(calcVelocity * Time.deltaTime * spd);
    }
}
