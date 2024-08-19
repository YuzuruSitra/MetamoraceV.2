using UnityEngine.SceneManagement;

namespace System.Battle
{
    public class BattleExitHandler
    {
        private const string SceneName = "Master_Wait";
        
        public void ReturnRoom()
        {
            SceneManager.LoadScene(SceneName);
        }
        
    }
}
