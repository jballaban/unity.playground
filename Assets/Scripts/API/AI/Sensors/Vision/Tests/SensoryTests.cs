using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using API.AI.Sensory;
using API.AI.Sensory.Helpers;
using System.Collections.Generic;

public class SensoryTests
{

	GameObject PersonPrefab()
	{
		var person = new GameObject();
		person.AddComponent<Rigidbody>();
		return person;
	}

	Dictionary<string, GameObject> Scenario1()
	{
		var scene = new Dictionary<string, GameObject>();
		scene["self"] = GameObject.Instantiate(PersonPrefab(), new Vector3(10, 10, 10), Quaternion.identity);
		scene["self"].AddComponent<VisionSensorComponent>().range = 5f;
		scene["self"].AddComponent<SensoryComponent>();
		scene["enemy"] = GameObject.Instantiate(PersonPrefab(), new Vector3(20, 10, 10), Quaternion.identity);
		return scene;
	}

	[Test]
	public void VisionTests()
	{
		var scenario = Scenario1();
		var vision = scenario["self"].GetComponent<SensoryComponent>().visionSensor as VisionSensorComponent;
		Assert.AreEqual(0, vision.known.Count);
		// move enemy close enough to see
		scenario["enemy"].transform.position = scenario["self"].transform.position;
		Assert.AreEqual(1, vision.known.Count);
	}
}
