using UnityEngine;
using System.Collections;
using Client.MirNetwork;

namespace Assets.script.Mir.scens
{
    public class GameManager : MonoBehaviour
    {



        // Use this for initialization
        void Start()
        {
            log.LogUtil.log("GameManager ","Start...");
            //DontDestroyOnLoad(this);
            // MirNetwork.Connect();
        }

        // Update is called once per frame
        void Update()
        {
            log.LogUtil.log("GameManager ","Update...");
            MirNetwork.Process();
        }
    }
}