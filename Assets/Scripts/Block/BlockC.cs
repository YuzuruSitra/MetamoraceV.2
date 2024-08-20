using Character;
using UnityEngine;

namespace Block
{
    public class BlockC : BlockBase 
    {
        protected override void SendEffect(GameObject player)
        {
            var status = player.GetComponent<CharacterStatus>();
            status.ReceiveChangeState(CharacterStatus.Condition.Stan);
        }
    }
}
