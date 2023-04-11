using Unity.Entities;
using UnityEngine;

/// <summary>
/// �e�̔��˒n�_�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // ���˂���e�̃G���e�B�e�B
    public Entity _straightBulletEntity;

    // �A�˂̊Ԋu
    public float _shootCoolTime;

    // �A�˂̃N�[���^�C��(���˒n�_���ƂɘA�˂̃N�[���^�C����ς��邽��Tag���ێ�����)
    [HideInInspector] public float _shootInterval;

    // ���˂���e�̑��x
    [HideInInspector] public float _bulletSpeed;
}
