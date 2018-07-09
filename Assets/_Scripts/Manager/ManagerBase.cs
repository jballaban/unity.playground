using UnityEngine;

public class ManagerBase : MonoBehaviour
{
	public DepotManager Depots;
	public TreeManager Trees;
	public LogManager Logs;

	public void Start()
	{
		Depots = FindObjectOfType<DepotManager>();
		Trees = FindObjectOfType<TreeManager>();
		Logs = FindObjectOfType<LogManager>();
	}
}