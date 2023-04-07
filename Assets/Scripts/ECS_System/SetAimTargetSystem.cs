using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/// <summary>
/// �I�[�g�G�C���e�̃^�[�Q�b�g���v���C���[�Ɉ�ԋ߂��G�ɐݒ肷��
/// </summary>
public class SetAimTargetSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<PlayerTag,AimGunPortTag>()
            .ForEach((ref AimGunPortTag aimGunPort, ref Translation playerTranslation) =>
            {
                // ��ԋ߂��ɋ���G
                Entity closestTargetEntity = Entity.Null;
                float closestDistance = float.MaxValue;

                // �v���C���[�̃|�W�V������
                float3 playerPosition = playerTranslation.Value;

                Entities
                    .WithAll<EnemyTag>()
                    .ForEach((Entity targetEntity, ref Translation targetTranslation) =>
                    {
                        // �v���C���[����^�[�Q�b�g���̓G�܂ł̋���
                        float enemyDistance = math.distance(playerPosition, targetTranslation.Value);

                        // �^�[�Q�b�g���Ă���G��NULL�Ȃ�
                        if(closestTargetEntity == Entity.Null)
                        {
                            // ��ԋ߂��̓G���^�[�Q�b�g�ɐݒ肷��
                            closestTargetEntity = targetEntity;
                            closestDistance = enemyDistance;
                        }
                        else
                        {
                            // �^�[�Q�b�g���̓G���߂��ɕʂ̓G��������
                            if(enemyDistance < closestDistance)
                            {
                                closestTargetEntity = targetEntity;
                                closestDistance = enemyDistance;
                            }
                        }
                    });
                // �R���|�[�l���g�ɐݒ肷��
                aimGunPort._targetEntity = closestTargetEntity;
            });
    }
}
