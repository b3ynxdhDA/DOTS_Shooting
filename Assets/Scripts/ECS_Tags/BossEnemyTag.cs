using Unity.Entities;

/// <summary>
/// �{�X�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct BossEnemyTag : IComponentData
{
    // �{�X�̑�P�`�Ԃ̎��̗̑�
    public float _1st_bossHp;

    // �{�X�̑�Q�`�Ԃ̎��̗̑�
    public float _2nd_bossHp;

    // �{�X�̑�R�`�Ԃ̎��̗̑�
    public float _3rd_bossHp;

}
