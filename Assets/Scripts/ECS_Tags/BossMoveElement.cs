using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// �{�X�L�����N�^�[�̈ړ��n�_�̃R���|�[�l���g�o�b�t�@�[
/// </summary>
[GenerateAuthoringComponent]
[InternalBufferCapacity(3)]
public struct BossMoveElement : IBufferElementData
{
    // �{�X�L�����N�^�[�̈ړ�����ʒu
    public float3 _movePoints;
}
