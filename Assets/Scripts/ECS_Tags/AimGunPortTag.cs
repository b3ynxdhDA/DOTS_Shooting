using Unity.Entities;

/// <summary>
/// �^�[�Q�b�g��_���e�𔭎˂���G���e�B�e�B�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct AimGunPortTag : IComponentData
{
    // �^�[�Q�b�g�̃G���e�B�e�B
    public Entity _targetEntity;
}
