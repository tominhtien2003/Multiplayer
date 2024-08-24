using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] Button serverBtn, hostBtn, clientBtn;
    private void Awake() {
        serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        }) ;
        hostBtn.onClick.AddListener(() =>{
            NetworkManager.Singleton.StartHost();
        }) ;
        clientBtn.onClick.AddListener(() =>{
            NetworkManager.Singleton.StartClient();
        }) ;
    }
}
