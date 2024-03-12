using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fusion;
using Fusion.Sockets;
public class SpawnCarNetwork : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector3 GetRandomSpawnPoint()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if(spawnPoints.Length == 0 )
        {
            return Vector3.zero;
        } else
        {
            return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;
        }
    }

	public void OnConnectedToServer(NetworkRunner runner)
	{   
        if(runner.Topology == Topologies.Shared)
        {
            Debug.Log("OnConnectedToServer, starting player prefab as local player");
            runner.Spawn(playerPrefab, GetRandomSpawnPoint(), Quaternion.identity, runner.LocalPlayer);
        }else
        {
            Debug.Log("OnConnectedToServer");
        }
		
	}

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            Debug.Log("OnPlayerJoined we are server. Spawing player");
			runner.Spawn(playerPrefab, GetRandomSpawnPoint(), Quaternion.identity,player);
		} else
        {
            Debug.Log("OnPlayerJoined");
        }
	}

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
		throw new NotImplementedException();
	}

	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
	{
		throw new NotImplementedException();
	}

	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
	{
		throw new NotImplementedException();
	}

	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
	{
		throw new NotImplementedException();
	}

	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
		throw new NotImplementedException();
	}

	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
	{
		throw new NotImplementedException();
	}

	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
	{
		throw new NotImplementedException();
	}

	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
	{
		throw new NotImplementedException();
	}

	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
	{
		throw new NotImplementedException();
	}

	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
	{
		throw new NotImplementedException();
	}

	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
	{
		throw new NotImplementedException();
	}

	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
	{
		throw new NotImplementedException();
	}

	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
	{
		throw new NotImplementedException();
	}

	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
	{
		throw new NotImplementedException();
	}

	public void OnSceneLoadDone(NetworkRunner runner)
	{
		throw new NotImplementedException();
	}

	public void OnSceneLoadStart(NetworkRunner runner)
	{
		throw new NotImplementedException();
	}
}
