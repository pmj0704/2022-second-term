using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterFollowCamera))]
public class CameraWithEditor : Editor
{
    private CharacterFollowCamera characterFollowCamera;

    public override void OnInspectorGUI()
    {
        characterFollowCamera = (CharacterFollowCamera)target;
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        if (!characterFollowCamera || !characterFollowCamera.camTarget) return;

        Transform camTarget = characterFollowCamera.camTarget;
        Vector3 posTarget = camTarget.position;
        posTarget.y += characterFollowCamera.targetHeight;

        Color _color = Color.red;
        _color.a = 0.3f;
        Handles.color = _color;
        Handles.DrawSolidDisc(posTarget, Vector3.up, characterFollowCamera.camDistance);

        Handles.color = Color.green;
        Handles.DrawWireDisc(posTarget, Vector3.up, characterFollowCamera.camDistance);

        Handles.color = Color.blue;
        characterFollowCamera.camDistance = Handles.ScaleSlider(
                                            characterFollowCamera.camDistance,  //������ ����
                                            posTarget,                          //���� ��ġ
                                            -camTarget.forward,                 //����, ī�޶� ���� �ݴ� ����
                                            Quaternion.identity,                //ȸ�� ��
                                            characterFollowCamera.camDistance,  //�ִ� ��
                                            0.1f                                //�ּ� ��
                                          );
        characterFollowCamera.camDistance = Mathf.Clamp(characterFollowCamera.camDistance, 2f, float.MaxValue);

        Handles.color = Color.yellow;
        characterFollowCamera.camHeight = Handles.ScaleSlider(
                                            characterFollowCamera.camHeight,    //������ ����
                                            posTarget,                          //���� ��ġ
                                            camTarget.up,                 //����, ī�޶� ���� �ݴ� ����
                                            Quaternion.identity,                //ȸ�� ��
                                            characterFollowCamera.camHeight,    //�ִ� ��
                                            0.1f                                //�ּ� ��
                                          );
        characterFollowCamera.camHeight = Mathf.Clamp(characterFollowCamera.camHeight, 2f, float.MaxValue);

        GUIStyle guiStyleLabel = new GUIStyle();
        guiStyleLabel.fontSize = 15;
        guiStyleLabel.normal.textColor = Color.white;

        guiStyleLabel.alignment = TextAnchor.UpperCenter;
        Handles.Label(posTarget + (-camTarget.forward * characterFollowCamera.camDistance), "Distance", guiStyleLabel);

        guiStyleLabel.alignment = TextAnchor.MiddleRight;
        Handles.Label(posTarget + (Vector3.up * characterFollowCamera.camHeight), "Height", guiStyleLabel);

        characterFollowCamera.TopDownFollowCamera();
    }
}
