using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// �I�[�g�G�C���e�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
[InternalBufferCapacity(3)]
public struct BossMoveElement : IBufferElementData
{
    // �^�[�Q�b�g�ւ̊p�x
    public Entity _movePoints;
}
