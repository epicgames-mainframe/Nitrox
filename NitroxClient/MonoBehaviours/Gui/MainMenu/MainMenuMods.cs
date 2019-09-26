using NitroxClient.MonoBehaviours.DiscordRP;
using NitroxClient.Unity.Helper;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NitroxModel.Logger;

namespace NitroxClient.MonoBehaviours.Gui.MainMenu
{
    public class MainMenuMods : MonoBehaviour
    {
        private void OnEnable()
        {
            Log.Debug("MainMenuMods.OnEnable()");
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            ;
            Log.Debug("Subscribed to sceneLoaded event");
        }

        private void OnDisable()
        {
            Log.Debug("MainMenuMods.OnDisable()");
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            Log.Debug("Unsubscribed from sceneLoaded event");
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            Log.Debug("Scene loaded: '" + scene.name + "'");
            if (scene.name == "XMenu")
            {
                Log.Debug("Initiating multiplayer menu mod");
                MultiplayerMenuMods();
            }
        }

        private void MultiplayerMenuMods()
        {
            Log.Debug("MultiplayerMenuMods() reached");
            GameObject startButton = GameObjectHelper.RequireGameObject("Menu canvas/Panel/MainMenu/PrimaryOptions/MenuButtons/ButtonPlay");
            GameObject showLoadedMultiplayer = Instantiate(startButton, startButton.transform.parent);
            showLoadedMultiplayer.name = "ButtonMultiplayer";
            Text buttonText = showLoadedMultiplayer.RequireGameObject("Circle/Bar/Text").GetComponent<Text>();
            buttonText.text = "Multiplayer";
            Log.Warn("Overriden Translation Updater! no clue what this might cause");
            Destroy(buttonText.GetComponent<TranslationLiveUpdate>());
            showLoadedMultiplayer.transform.SetSiblingIndex(3);
            Button showLoadedMultiplayerButton = showLoadedMultiplayer.GetComponent<Button>();
            showLoadedMultiplayerButton.onClick.RemoveAllListeners();
            showLoadedMultiplayerButton.onClick.AddListener(ShowMultiplayerMenu);

            MainMenuRightSide rightSide = MainMenuRightSide.main;
            GameObject savedGamesRef = FindObject(rightSide.gameObject, "SavedGames");
            GameObject LoadedMultiplayer = Instantiate(savedGamesRef, rightSide.transform);
            LoadedMultiplayer.name = "Multiplayer";
            LoadedMultiplayer.RequireTransform("Header").GetComponent<Text>().text = "Multiplayer";
            Destroy(LoadedMultiplayer.RequireGameObject("Scroll View/Viewport/SavedGameAreaContent/NewGame"));
            Destroy(LoadedMultiplayer.GetComponent<MainMenuLoadPanel>());

            MainMenuMultiplayerPanel panel = LoadedMultiplayer.AddComponent<MainMenuMultiplayerPanel>();
            panel.SavedGamesRef = savedGamesRef;
            panel.LoadedMultiplayerRef = LoadedMultiplayer;

            rightSide.groups.Add(LoadedMultiplayer);
            //DiscordController.Main.InitDRPMenu();
        }

        private void ShowMultiplayerMenu()
        {
            MainMenuRightSide rightSide = MainMenuRightSide.main;
            rightSide.OpenGroup("Multiplayer");
        }

        private GameObject FindObject(GameObject parent, string name)
        {
            Component[] trs = parent.GetComponentsInChildren(typeof(Transform), true);
            foreach (Component t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }

            return null;
        }
    }
}
