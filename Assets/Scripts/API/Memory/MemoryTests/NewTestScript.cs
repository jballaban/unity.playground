using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using API.Memory;
using API.Memory.Helpers;
using System.Collections.Generic;
using System;

public class NewTestScript
{
	const string TAG_FRIEND = "friend";
	const string TAG_ENEMY = "enemy";
	const string TAG_FRIEND_HURT = "hurt friend";
	const string AREA_DANGER = "area danger";

	class PersonComponent : MonoBehaviour
	{
		public HashSet<string> tags = new HashSet<string>() { "person" };
		public float health = 100f;
		public MemoryComponent memory;

		void Awake()
		{
			memory = GetComponent<MemoryComponent>();
		}
	}

	class PersonMemory : ObjectMemory
	{
		public float health { get; set; }
		public PersonMemory(GameObject gameobject) : base(gameobject)
		{
			health = gameobject.GetComponent<PersonComponent>().health;
		}
	}

	[Test]
	public void MemoryComponent_Tests()
	{
		var selfcontainer = new GameObject();
		selfcontainer.AddComponent<MemoryComponent>();
		var self = selfcontainer.AddComponent<PersonComponent>();
		selfcontainer.transform.position = new Vector3(100, 0, 50);
		// TODO: mark current area has home
		// bad guy
		var enemycontainer = new GameObject();
		var enemy = enemycontainer.AddComponent<PersonComponent>();
		enemycontainer.transform.position = new Vector3(100, 0, 100);
		// hurt friendly
		var hurtfriendcontainer = new GameObject();
		var hurtfriend = hurtfriendcontainer.AddComponent<PersonComponent>();
		hurtfriend.health = 10f; // hurt
		hurtfriendcontainer.transform.position = new Vector3(50, 0, 100);
		// healthy friendly
		var healthyfriendcontainer = new GameObject();
		var healthyfriend = healthyfriendcontainer.AddComponent<PersonComponent>();
		healthyfriendcontainer.transform.position = new Vector3(50, 0, 100);

		// we see enemy
		self.memory.Remember(new PersonMemory(enemycontainer), enemy.tags, TAG_ENEMY);
		// TODO: MARK AREA AS DANGEROUS
		// TODO: we go home to get a weapon
		// enemy moves
		enemy.transform.position = new Vector3(100, 0, 150);
		// we remember enemy and go towards last known location but enemy is gone
		var enemiesmemory = self.memory.Recall<PersonMemory>(TAG_ENEMY);
		Assert.AreEqual(enemiesmemory.Count, 1);
		var enemymemory = enemiesmemory[0];
		self.transform.position = enemymemory.position;
		// wander until he see enemy in new location
		self.transform.position = new Vector3(100, 0, 150);
		// see enemy
		self.memory.Remember(new PersonMemory(enemycontainer), enemy.tags, TAG_ENEMY);
		// TODO: mark area as dangerous
		// attacks enemy and our health is reduced
		self.health = 60f;
		// run away randomly
		self.transform.position = new Vector3(50, 0, 100);
		// sees friends and realize one is hurt
		self.memory.Remember(new PersonMemory(hurtfriendcontainer), hurtfriend.tags, TAG_FRIEND, TAG_FRIEND_HURT);
		self.memory.Remember(new PersonMemory(healthyfriendcontainer), healthyfriend.tags, TAG_FRIEND);
		// friend goes and kills enemy but we don't know it happened
		UnityEngine.Object.Destroy(enemycontainer);
		// friend got hurt in the battle
		hurtfriend.health -= 1;
		// we look for our enemy again to finish him not knowing our friend did
		enemiesmemory = self.memory.Recall<PersonMemory>(TAG_ENEMY);
		Assert.AreEqual(1, enemiesmemory.Count);
		enemymemory = enemiesmemory[0];
		self.transform.position = enemymemory.position;
		// can't find him so we decide to forget he exists but keep danger marking in this area as he may still be around
		self.memory.Forget(enemymemory);
		enemiesmemory = self.memory.Recall<PersonMemory>(TAG_ENEMY);
		Assert.AreEqual(0, enemiesmemory.Count);
		// remember we have friends
		var friendsmemory = self.memory.Recall<PersonMemory>(TAG_FRIEND);
		Assert.AreEqual(2, friendsmemory.Count);
		// go find our most hurt friends
		friendsmemory = self.memory.Recall<PersonMemory>(TAG_FRIEND_HURT);
		Assert.AreEqual(1, friendsmemory.Count);
		var friendmemory = friendsmemory[0];
		Assert.AreEqual(10f, friendmemory.health);
		self.transform.position = friendmemory.position;
		// see our hurt friend
		friendmemory = new PersonMemory(hurtfriendcontainer);
		self.memory.Remember(friendmemory, hurtfriend.tags, TAG_FRIEND, TAG_FRIEND_HURT);
		// realize he's even more hurt than we remember
		Assert.AreEqual(9f, self.memory.Recall<PersonMemory>(friendmemory.id).health);
		// heal them
		hurtfriend.health = 100f;
		// retag them as healthy
		self.memory.Remember(friendmemory, hurtfriend.tags, TAG_FRIEND);
		// ensure we don't know any hurt people
		Assert.AreEqual(0, self.memory.Recall(TAG_FRIEND_HURT).Count);
		// TODO: learn from our friend that he killed the bad guy so de-ecalate the area danger factor
	}
}