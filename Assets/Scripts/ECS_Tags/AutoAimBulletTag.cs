using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// �I�[�g�G�C���e�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct AutoAimBulletTag : IComponentData
{
    // �^�[�Q�b�g�ւ̊p�x
    [HideInInspector]public float3 _moveDirection;
}
