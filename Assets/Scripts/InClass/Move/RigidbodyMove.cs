using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //������ �ٵ� ������ �� ��ũ��Ʈ�� ������ �ȵ�
public class RigidbodyMove : MonoBehaviour
{
    private Rigidbody rigidbody; //RigidBody Cashing Ready
    private float spd = 5.0f;
    
    private float jumpValue = 2.0f;
    private float dashValue = 5.0f;

    private Vector3 directionValue = Vector3.zero;

    public LayerMask layerGround;
    private bool flagOnGrounded;
    private float defaultGroundDistance = 0.2f; //���� �ִ��� �Ǵ� ���ذ�

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        Vector3 posOrigin = transform.position + (Vector3.up * 0.1f);

        Debug.DrawLine(posOrigin, transform.position + (Vector3.up * 0.1f) + (Vector3.down * defaultGroundDistance));

        if(Physics.Raycast(posOrigin, Vector3.down, out hitInfo, defaultGroundDistance, layerGround)) flagOnGrounded = true;
        else flagOnGrounded = false;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();//Cashing
    }

    void Update() //������ ���� Update
    {
        CheckGroundStatus();

        //����� �Է� �� �ޱ�, �¿�, �յ�
        directionValue = Vector3.zero;
        directionValue.x = Input.GetAxis("Horizontal");
        directionValue.z = Input.GetAxis("Vertical");

        //����� �Է���  �ִٸ�, ���� ������ �Է� �� �������� �����϶�
        if(directionValue != Vector3.zero)
        {
            transform.forward = directionValue;
        }

        if(Input.GetButtonDown("Jump") && flagOnGrounded)
        {
            rigidbody.drag = 0f;

            rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpValue * - 2f * Physics.gravity.y), ForceMode.VelocityChange);
            //Mathf.Sqrt(jumpValue - 2f * Physics.gravity.y) ���� ����, gravity = -9.81, ������ó�� ���̰�
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //���� �������� ���� �������
            Debug.Log("�뽬");
            rigidbody.drag = 8f;

            float posDashEnd = Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime;
            //Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime; �ڿ������� ����

            Vector3 dashVelocity = Vector3.Scale(transform.forward, dashValue * new Vector3(posDashEnd, 0, posDashEnd));
            rigidbody.AddForce(dashVelocity, ForceMode.VelocityChange);
        }

    }

    private void FixedUpdate() //������ ���� Update
    {
        rigidbody.MovePosition(rigidbody.position + directionValue * spd * Time.fixedDeltaTime);
    }
}
