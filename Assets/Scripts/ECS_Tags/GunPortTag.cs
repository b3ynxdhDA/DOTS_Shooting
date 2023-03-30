using Unity.Entities;

/// <summary>
/// �e�̔��˒n�_�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // ���˂���e�̃G���e�B�e�B
    public Entity _straightBulletEntity;

    // ���˂���e�̃G���e�B�e�B
    public Entity _aimBulletEntity;

    // ����Œe�𔭎˂��邩
    public int _gunPortAmount;

    public enum BulletKind
    {
        Straight,
        Diffusion,
        Aim
    }

    public BulletKind bulletKind;
}
