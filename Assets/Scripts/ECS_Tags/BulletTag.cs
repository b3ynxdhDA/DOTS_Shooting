using Unity.Entities;
using UnityEngine;

/// <summary>
/// �e�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct BulletTag : IComponentData
{
    // �e�̐i�ރX�s�[�h
    public float _bulletSpeed;

    // �e�̗^����_���[�W
    public float _hitDamage;
}
