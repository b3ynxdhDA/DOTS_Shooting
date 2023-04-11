using Unity.Entities;
using UnityEngine;

namespace shooting
{
    public class BossPhaseSytem : SystemBase
    {
        // �ϐ��錾------------------------------------------------------------------
        // ���s�^�C�~���O���Ǘ����Ă���V�X�e���O���[�v
        private BeginSimulationEntityCommandBufferSystem _entityCommandBufferSystem;

        // �Q�[���}�l�[�W���[�̎Q��
        private GameManager _gameManager = GameManager.instance;

        // �{�X�}�l�[�W���[�̎Q��
        private BossManager _bossManager;

        // �萔�錾--------------------------------------------------------



        protected override void OnCreate()
        {
            // EntityCommandBuffer�̎擾
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

            _bossManager = _gameManager.BossManager;

            // �{�X�L�����N�^�[��HP��������
            SetKomaDate(_bossManager.BossKomaData0);


        }

        protected override void OnUpdate()
        {
            // �Q�[���̃X�e�[�g���Q�[�����ȊO�Ȃ珈�����Ȃ�
            if (_gameManager.gameState != GameManager.GameState.GameNow)
            {
                return;
            }

            Entities
                .WithName("Boss_Phase")
                .WithAll<EnemyTag, BossEnemyTag>()
                .WithoutBurst()
                .ForEach((ref EnemyTag enemyTag) =>
                {

                // ���݂̃{�X�̍U���i�K�ɍ��킹������������
                switch (_bossManager.BossPhaseCount)
                    {
                    // ��1�i�K
                    case 0:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                            {
                            // @���̋���Z�b�g����
                            SetKomaDate(_bossManager.BossKomaData1);

                            // �{�X�̍U���i�K���グ��
                            _bossManager.UpdateBossCount();
                            }
                            break;
                    // ��2�i�K
                    case 1:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                            {
                            // @���̋���Z�b�g����
                            SetKomaDate(_bossManager.BossKomaData2);

                            // �{�X�̍U���i�K���グ��
                            _bossManager.UpdateBossCount();
                            }
                            break;
                    // ��3�i�K
                    case 2:
                        // �{�X��HP��0��菬�����Ȃ�����
                        if (enemyTag._enemyHp < 0)
                            {

                            }
                            break;
                    }

                }).Run();

            // �w�肵��Job�������ECB�ɓo�^�������߂����s
            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }

        /// <summary>
        /// ��̃X�e�[�^�X��ݒ肷��
        /// </summary>
        /// <param name="komaData"></param>
        private void SetKomaDate(KomaData komaData)
        {
            Entities
               .WithName("Set_Boss_KomaDate")
                   .WithAll<EnemyTag, BossEnemyTag>()
                   .WithoutBurst()
                   .ForEach((Entity entity, ref EnemyTag enemyTag, ref GunPortTag gunPortTag) =>
                   {
                       enemyTag._enemyHp = komaData.hp;
                       gunPortTag._shootCoolTime = komaData.shootCoolTime;
                       gunPortTag._bulletSpeed = komaData.bulletSpeed;

                       SetShootKInd(entity, komaData);

                   }).Run();
        }

        private void SetShootKInd(Entity entity, KomaData komaData)
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            switch (komaData.shootKind)
            {
                case KomaData.ShootKind.StraightGunPortTag:
                    entityManager.AddComponentData(entity, new StraightGunPortTag
                    {
                        _lines = komaData.shootLine
                    });
                    break;
                case KomaData.ShootKind.WideGunPortTag:
                    entityManager.AddComponentData(entity, new WideGunPortTag
                    {
                        _lines = komaData.shootLine
                    });
                    break;
                case KomaData.ShootKind.AimGunPortTag:
                    entityManager.AddComponentData(entity, new AimGunPortTag { });
                    break;
            }

        }

    }
}