using UI.Windows.TailChoose;

namespace Game
{
    public class GameSceneManager : SceneManager
    {
        protected override void Awake()
        {
            new ShowWindowSignal(TailChooseWindow.PrefabKey).Fire();
        }
    }
}
