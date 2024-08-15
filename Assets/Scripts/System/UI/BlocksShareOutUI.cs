using System.Battle;
using UnityEngine;

namespace System.UI
{
    public class BlocksShareOutUI : MonoBehaviour
    {
        [SerializeField] private BlockGenerator _blockGenerator;
        [SerializeField] private UnityEngine.UI.Text _team1;
        [SerializeField] private UnityEngine.UI.Text _team2;
        
        private void Start()
        {
            _team1.text = "0%";
            _team2.text = "0%";
        }
        
        private void Update()
        {
            _team1.text = _blockGenerator.BlocksShareTeam1 + "%";
            _team2.text = _blockGenerator.BlocksShareTeam2 + "%";
        }
    }
}
