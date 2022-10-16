using Game.Utility;
using Gummi.MVC;

namespace Game.Controllers
{
    public class StartController : SubController<GameState, StartView>
    {
        public void Play()
        {
            SceneLoader.LoadGame();
        }

        public void Quit() => SceneLoader.Quit();
    }
}