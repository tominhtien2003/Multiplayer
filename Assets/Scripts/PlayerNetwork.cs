using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(0,
    NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Owner);
    private NetworkVariable<MyCustomData> test = new NetworkVariable<MyCustomData>(new MyCustomData { _int = 0, _bool = false },
    NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public FixedString128Bytes _message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref _message);
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        randomNumber.OnValueChanged += RandomNumber_OnValueChanged;
        test.OnValueChanged += Test_OnValueChanged;
    }
    private void Test_OnValueChanged(MyCustomData previousValue, MyCustomData newValue)
    {
        Debug.Log(newValue._int + " " + newValue._bool + " " + newValue._message);
    }
    private void RandomNumber_OnValueChanged(int previousValue, int newValue)
    {
        Debug.Log(OwnerClientId + " RandomNumber : " + randomNumber.Value);
    }
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0, 10);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            test.Value = new MyCustomData
            {
                _int = randomNumber.Value,
                _bool = true,
                _message = "Ahihi"
            };
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //TestServerRpc(new ServerRpcParams());
            TestClientRpc(new ClientRpcParams());
        }
        float moveSpeed = 4f;
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRPC " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams){
        Debug.Log("ClientRpc");
    }
}
