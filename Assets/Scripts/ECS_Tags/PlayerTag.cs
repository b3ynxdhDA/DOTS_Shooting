using Unity.Entities;

/// <summary>
/// �e�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct PlayerTag : IComponentData
{
    // �����X�s�[�h
    public float _moveSpeed;
    // �v���C���[�̗̑�
    public float _playerHp;
}
