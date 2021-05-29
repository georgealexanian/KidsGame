using UI.Windows.AnimalChoose;

namespace Game
{
    public class MenuSceneManager : SceneManager
    {
        protected override void Awake()
        {
            new ShowWindowSignal(AnimalChooseWindow.PrefabKey).Fire();
        }
    }
}
