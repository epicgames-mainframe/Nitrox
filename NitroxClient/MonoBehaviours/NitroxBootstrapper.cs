using System;
using NitroxClient.MonoBehaviours.Gui.MainMenu;
using NitroxModel.Logger;
using UnityEngine;

namespace NitroxClient.MonoBehaviours
{
    public class NitroxBootstrapper : MonoBehaviour
    {
        public void Awake()
        {
            try
            {
                DontDestroyOnLoad(gameObject);
                gameObject.AddComponent<SceneCleanerPreserve>();
                gameObject.AddComponent<MainMenuMods>();

#if DEBUG
            AttachWarpToCommand();
#endif

                CreateDebugger();
            }
            catch(Exception ex)
            {
                Log.Error(ex.StackTrace + "\n" + ex.Message);
                return;
            }
        }

        private void AttachWarpToCommand()
        {
            GameObject consoleRoot = new GameObject();
            consoleRoot.AddComponent<WarpToCommand>();
        }

        private void CreateDebugger()
        {
            GameObject debugger = new GameObject();
            debugger.name = "Debug manager";
            debugger.AddComponent<NitroxDebugManager>();
            debugger.transform.SetParent(transform);
        }
    }
}
