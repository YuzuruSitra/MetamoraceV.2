using Character;
using Object;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Block
{
    public class BlockC : BlockBase 
    {
        [SerializeField] private float _rayDistance;
        [SerializeField] private float _destroyPower;
        [SerializeField] private float _transitionTime;
        [SerializeField] private float _waitTime;

        private enum Effect
        {
            Stan,
            Bomb,
            Cloud
        }
        
        protected override void SendEffect(GameObject player)
        {
            var selectedEffect = GetRandomEffect();
            selectedEffect = Effect.Bomb;
            switch (selectedEffect)
            {
                case Effect.Stan:
                    DoStan(player);
                    break;
                case Effect.Bomb:
                    DoBomb();
                    break;
                case Effect.Cloud:
                    DoCloud();
                    break;
            }
        }
        
        private Effect GetRandomEffect()
        {
            var effects = (Effect[])System.Enum.GetValues(typeof(Effect));
            var randomIndex = Random.Range(0, effects.Length);
            return effects[randomIndex];
        }

        private void DoStan(GameObject player)
        {
            var status = player.GetComponent<CharacterStatus>();
            status.ReceiveChangeState(CharacterStatus.Condition.Stan);
        }
        
        private void DoBomb()
        {
            Vector3[] directions = {
                Vector3.up,
                Vector3.down,
                Vector3.left,
                Vector3.right
            };

            foreach (var direction in directions)
            {
                if (!Physics.Raycast(transform.position, direction, out var hit, _rayDistance)) continue;
                if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Block")) continue;
                Debug.DrawRay(transform.position, direction * _rayDistance, Color.green, 5.0f);
                var block = hit.collider.GetComponent<BlockBase>();
                Debug.Log("Called");
                 block.DestroyBlock(_destroyPower, gameObject);
            }
        }

        private void DoCloud()
        {
            var cloudHandler = GameObject.FindWithTag("CloudBlockC").GetComponent<BlockCCloudHandler>();
            cloudHandler.LaunchCloud(transform, _transitionTime, _waitTime);
        }
        
    }
}
