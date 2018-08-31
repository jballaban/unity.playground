using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using API.Memory;
using API.Memory.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using API.Memory.Contract;

public class MemoryTests
{
	const string TAG_PERSON = "person";
	const string TAG_FRIEND = "friend";
	const string TAG_ENEMY = "enemy";
	const string TAG_FRIEND_HURT = "hurt friend";
	const string TAG_AREA_DANGER = "area danger";
	const string TAG_WEAPON = "weapon";
	const string TAG_HOME = "home";
	const string TAG_BANDIT = "bandit";
	const string TAG_SOLDIER = "soldier";
	const string TAG_FACT = "fact";
	//	readonly InformationMemory<HashSet<string>> FACT_PEOPLE_TYPES = new InformationMemory<HashSet<string>>(TAG_PERSON, new HashSet<string>());

	class PersonComponent : MonoBehaviour
	{
		public HashSet<string> tags = new HashSet<string>() { TAG_PERSON };
		public string type { get; set; }
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

	class DangerArea : PlaceMemory
	{
		public HashSet<string> tags = new HashSet<string>() { TAG_AREA_DANGER };

		public DangerArea(Vector3 position, float level) : base(position)
		{
			this.level = level;
		}

		public float level { get; set; }
	}

	[Test]
	public void MemoryIDTest()
	{
		var id = new MemoryID<ObjectMemory>();
		var id2 = new MemoryID<PlaceMemory>();
		var id3 = new MemoryID<ObjectMemory>();
		var id4 = new MemoryID<ObjectMemory>(10);
		var id5 = new MemoryID<ObjectMemory>(10);
		var id6 = new MemoryID<ObjectMemory>(11);
		Assert.AreNotEqual(id, id2);
		Assert.AreEqual(id, id3);
		Assert.AreEqual(id4, id5);
		Assert.AreNotEqual(id, id4);
		Assert.AreNotEqual(id5, id6);
		Assert.AreNotEqual(id5, 11);
	}

	class ObjectMemoryWithDetail : ObjectMemory
	{
		public string detail;

		public ObjectMemoryWithDetail(GameObject gameobject) : base(gameobject) { }
	}

	class PlaceMemoryWithDetail : PlaceMemory
	{
		public string detail;
		public PlaceMemoryWithDetail(Vector3 position) : base(position) { }
	}

	[Test]
	public void ObjectMemoryTests()
	{
		var o = new GameObject();
		var memory = new GameObject().AddComponent<MemoryComponent>();
		var o2 = memory.Remember<ObjectMemoryWithDetail>(o);
		o2.detail = "test";
		Assert.AreEqual("test", memory.Recall<ObjectMemoryWithDetail>(o).detail);
		Assert.AreEqual(1, memory.Recall<ObjectMemoryWithDetail>().Count());
	}

	[Test]
	public void PlaceMemoryTests()
	{
		var o = new GameObject();
		o.transform.position = new Vector3(100, 100, 100);
		var memory = new GameObject().AddComponent<MemoryComponent>();
		var o2 = memory.Remember<PlaceMemoryWithDetail>(o.transform.position);
		o2.detail = "hasguy";
		var o3 = memory.Remember<PlaceMemoryWithDetail>(Vector3.zero);
		o3.detail = "";
		Assert.AreEqual(o.transform.position, memory.Recall<PlaceMemory>(o.transform.position).position); // do support MemoryID ancestory!
		Assert.AreEqual("hasguy", memory.Recall<PlaceMemoryWithDetail>(o.transform.position).detail);
		Assert.AreEqual("", memory.Recall<PlaceMemoryWithDetail>(Vector3.zero).detail);
		Assert.AreEqual(2, memory.Recall<PlaceMemoryWithDetail>().Count());
		Assert.AreEqual(0, memory.Recall<PlaceMemory>().Count()); // don't support type ancestory yet
	}

	[Test]
	public void TopicMemoryTests()
	{
		var bully = new GameObject();
		var nice = new GameObject();
		var friend = new GameObject();
		var memory = new GameObject().AddComponent<MemoryComponent>();
		var friends2 = memory.Remember<TopicMemory>("friends");
		var enemies2 = memory.Remember<TopicMemory>("enemies");
		var bully2 = memory.Remember<ObjectMemory>(bully);
		var nice2 = memory.Remember<ObjectMemory>(nice);
		var friend2 = memory.Remember<ObjectMemory>(friend);
		memory.Associate(friends2, nice2);
		memory.Associate(friends2, friend2);
		memory.Associate(enemies2, bully2);
		Assert.AreEqual(2, memory.GetAssociations<ObjectMemory>(memory.Recall<TopicMemory>("friends")).Count());
		Assert.AreEqual(1, memory.GetAssociations<ObjectMemory>(memory.Recall<TopicMemory>("enemies")).Count());
	}

	[Test]
	public void AssociationTests()
	{
		var memory = new GameObject().AddComponent<MemoryComponent>();
		var o = new GameObject();
		var p = new Vector3(10, 10, 10);
		var o2 = new GameObject();
		var o_m = memory.Remember<ObjectMemory>(o);
		var p_m = memory.Remember<PlaceMemory>(p);
		var o2_m = memory.Remember<ObjectMemory>(o2);
		memory.Associate(o_m, p_m);
		memory.Associate(o2_m, p_m);
		Assert.AreEqual(1, memory.GetAssociations<PlaceMemory>(o_m).Count());
		Assert.AreEqual(memory.Recall<PlaceMemory>(p), memory.GetAssociations<PlaceMemory>(o_m).First());
		Assert.AreEqual(2, memory.GetAssociations<ObjectMemory>(memory.Recall<PlaceMemory>(p)).Count());
	}

	[Test]
	public void MemoryComponentTests()
	{
		var memory = new GameObject().AddComponent<MemoryComponent>();
		Assert.IsNull(memory.Recall<PlaceMemory>(Vector3.zero));
	}



	/* 
		[Test]
		public void MemoryComponent_Tests()
		{
			var selfcontainer = new GameObject();
			selfcontainer.AddComponent<MemoryComponent>();
			var self = selfcontainer.AddComponent<PersonComponent>();
			selfcontainer.transform.position = new Vector3(100, 0, 50);
			var homecontainer = new GameObject();
			homecontainer.transform.position = new Vector3(0, 0, 0);

			// know home
			self.memory.Remember(new PlaceMemory(homecontainer.transform.position), null, TAG_HOME);
			// put weapon in home
			var weaponcontainer = new GameObject();
			weaponcontainer.transform.position = homecontainer.transform.position;
			// know weapon is there
			self.memory.Remember(new ObjectMemory(weaponcontainer), null, TAG_WEAPON);
			// bad guy
			var enemycontainer = new GameObject();
			var enemy = enemycontainer.AddComponent<PersonComponent>();
			enemycontainer.transform.position = new Vector3(100, 0, 100);
			enemy.type = TAG_BANDIT; // make him a bandito
			// hurt friendly
			var hurtfriendcontainer = new GameObject();
			var hurtfriend = hurtfriendcontainer.AddComponent<PersonComponent>();
			hurtfriend.type = TAG_SOLDIER;
			hurtfriend.health = 10f; // hurt
			hurtfriendcontainer.transform.position = new Vector3(50, 0, 100);
			// healthy friendly
			var healthyfriendcontainer = new GameObject();
			var healthyfriend = healthyfriendcontainer.AddComponent<PersonComponent>();
			healthyfriend.type = TAG_SOLDIER;
			healthyfriendcontainer.transform.position = new Vector3(50, 0, 100);

			// we see enemy
			self.memory.Remember(new PersonMemory(enemycontainer), enemy.tags, TAG_ENEMY);
			// add enemy type to our memory
			var factmemory = self.memory.Recall<InformationMemory<HashSet<string>>>(FACT_PEOPLE_TYPES.id, FACT_PEOPLE_TYPES);
			factmemory.data.Add(enemy.type);
			self.memory.Remember(FACT_PEOPLE_TYPES, null, TAG_FACT);
			Assert.AreEqual(self.memory.Recall<InformationMemory<HashSet<string>>>(FACT_PEOPLE_TYPES.id).data, factmemory.data);
			Assert.IsTrue(self.memory.Recall<InformationMemory<HashSet<string>>>(FACT_PEOPLE_TYPES.id).data.Contains(TAG_BANDIT));
			// mark area as dangerous
			var area = new DangerArea(enemycontainer.transform.position, 1f);
			self.memory.Remember(area, area.tags);
			// go get a weapon
			var weaponsmemory = self.memory.Recall<ObjectMemory>(TAG_WEAPON);
			Assert.AreEqual(1, weaponsmemory.Count);
			self.transform.position = weaponsmemory[0].position;
			// enemy moves
			enemy.transform.position = new Vector3(100, 0, 150);
			//  go back to enemy last know location
			var enemiesmemory = self.memory.Recall<PersonMemory>(TAG_ENEMY);
			Assert.AreEqual(enemiesmemory.Count, 1);
			var enemymemory = enemiesmemory[0];
			self.transform.position = enemymemory.position;
			// don't see enemy at location so forget him
			self.memory.Forget(enemymemory);
			// wander until he see enemy in new location
			self.transform.position = new Vector3(100, 0, 150);
			// see enemy
			self.memory.Remember(new PersonMemory(enemycontainer), enemy.tags, TAG_ENEMY);
			// Mark area as dangerous
			area = new DangerArea(enemycontainer.transform.position, 1f);
			self.memory.Remember(area, area.tags);
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
			// go home
			self.transform.position = self.memory.Recall<PlaceMemory>(TAG_HOME)[0].position;
			Assert.AreEqual(homecontainer.transform.position, self.transform.position);
		} */
}