using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// �v���C���[�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct PlayerMoveDate : IComponentData
{
    // �����X�s�[�h
    public float _moveSpeed;

    // �ᑬ���[�h�̃X�s�[�h
    public float _slowSpeed;

    // ��������
    public float3 _moveDirection;
}
