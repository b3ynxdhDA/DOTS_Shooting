using Unity.Entities;

/// <summary>
/// �e�̔��˒n�_�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // ���˂���e�̃G���e�B�e�B
    public Entity _prefabEntity;
}
