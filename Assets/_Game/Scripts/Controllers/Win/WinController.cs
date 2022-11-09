using Gummi.MVC;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Game.Controllers
{
    public class WinController : SubController<GameState, WinView>
    {
        public void GoToStart()
        {
            SceneManager.LoadScene(0);
        }
    }
}