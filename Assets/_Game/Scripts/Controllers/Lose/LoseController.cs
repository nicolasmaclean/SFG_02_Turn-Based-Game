using Gummi.MVC;
using UnityEngine.SceneManagement;

namespace Game.Controllers
{
    public class LoseController : SubController<GameState, LoseView>
    {
        public void GoToStart()
        {
            SceneManager.LoadScene(0);
        }
    }
}