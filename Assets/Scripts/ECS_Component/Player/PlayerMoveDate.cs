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

    // �����X�s�[�h
    public float3 _moveDirection;
}
