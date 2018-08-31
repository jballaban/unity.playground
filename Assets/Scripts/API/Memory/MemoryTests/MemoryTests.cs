using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using API.Memory;
using API.Memory.Helpers;
using System.Collections.Generic;
using System;

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
    readonly InformationMemory<HashSet<string>> FACT_PEOPLE_TYPES = new InformationMemory<HashSet<string>>(TAG_PERSON, new HashSet<string>());

    class PersonComponent : MonoBehaviour
    {
        public HashSet<string> tags = new HashSet<string>() { TAG_PERSON };
        public string type { get; set; }
        public float health = 100f;
        public TaggedMemoryComponent memory;

        void Awake()
        {
            memory = GetComponent<TaggedMemoryComponent>();
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
    public void MemoryComponent_Tests()
    {
        var selfcontainer = new GameObject();
        selfcontainer.AddComponent<TaggedMemoryComponent>();
        var self = selfcontainer.AddComponent<PersonComponent>();
        selfcontainer.transform.position = new Vector3(100, 0, 50);
        var homecontainer = new GameObject();
        homecontainer.transform.position = new Vector3(0, 0, 0);

        // know home
        self.memory.Remember(new PlaceMemory(homecontainer.transform.position), TAG_HOME);
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
        self.memory.Remember(new PersonMemory(enemycontainer), TAG_PERSON, TAG_ENEMY);
        // add enemy type to our memory
        //    var factmemory = self.memory.Recall<InformationMemory<HashSet<string>>>(FACT_PEOPLE_TYPES.id, FACT_PEOPLE_TYPES);
        //   factmemory.data.Add(enemy.type);
        // self.memory.Remember(FACT_PEOPLE_TYPES, null, TAG_FACT);
        //  Assert.AreEqual(self.memory.Recall<InformationMemory<HashSet<string>>>(FACT_PEOPLE_TYPES.id).data, factmemory.data);
        // Assert.IsTrue(self.memory.Recall<InformationMemory<HashSet<string>>>(FACT_PEOPLE_TYPES.id).data.Contains(TAG_BANDIT));
        // mark area as dangerous
        var area = new DangerArea(enemycontainer.transform.position, 1f);
        self.memory.Remember(area, TAG_AREA_DANGER);
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
        self.memory.Remember(new PersonMemory(enemycontainer), TAG_PERSON, TAG_ENEMY);
        // Mark area as dangerous
        area = new DangerArea(enemycontainer.transform.position, 1f);
        self.memory.Remember(area, TAG_AREA_DANGER);
        // attacks enemy and our health is reduced
        self.health = 60f;
        // run away randomly
        self.transform.position = new Vector3(50, 0, 100);
        // sees friends and realize one is hurt
        self.memory.Remember(new PersonMemory(hurtfriendcontainer), TAG_PERSON, TAG_FRIEND, TAG_FRIEND_HURT);
        self.memory.Remember(new PersonMemory(healthyfriendcontainer), TAG_PERSON, TAG_FRIEND);
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
        self.memory.Remember(friendmemory, TAG_PERSON, TAG_FRIEND, TAG_FRIEND_HURT);
        // realize he's even more hurt than we remember
        //  Assert.AreEqual(9f, self.memory.Recall<PersonMemory>(friendmemory.id).health);
        // heal them
        hurtfriend.health = 100f;
        // retag them as healthy
        self.memory.Remember(friendmemory, TAG_PERSON, TAG_FRIEND);
        // ensure we don't know any hurt people
        Assert.AreEqual(0, self.memory.Recall<PersonMemory>(TAG_FRIEND_HURT).Count);
        // go home
        self.transform.position = self.memory.Recall<PlaceMemory>(TAG_HOME)[0].position;
        Assert.AreEqual(homecontainer.transform.position, self.transform.position);
        // find nearest dangerzone to scout within 10 radius
        //    var areas = self.memory.RecallNearby<DangerArea>(TAG_AREA_DANGER, self.transform.position, 10f);
        //  Assert.AreEqual(0, areas.Count);
        // nothing dangerous thta close to our home
        // look further
        //   areas = self.memory.RecallNearby<DangerArea>(TAG_AREA_DANGER, self.transform.position, 1000f);
        //    Assert.AreEqual(2, areas.Count);
        // go to first area and clear it
        //  self.transform.transform.position = areas[0].position;
        // self.memory.Forget(areas[0]);
        // areas = self.memory.RecallNearby<DangerArea>(TAG_AREA_DANGER, self.transform.position, 1000f);
        //   Assert.AreEqual(1, areas.Count);
    }
}