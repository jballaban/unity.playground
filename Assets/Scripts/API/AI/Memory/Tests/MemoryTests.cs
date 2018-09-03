using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using API.AI.Memory;
using API.AI.Memory.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using API.AI.Memory.Contract;

public class MemoryTests
{
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
        Assert.AreEqual(1, memory.RecallAll<ObjectMemoryWithDetail>().Count());
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
        Assert.AreEqual(2, memory.RecallAll<PlaceMemoryWithDetail>().Count());
        Assert.AreEqual(0, memory.RecallAll<PlaceMemory>().Count()); // don't support type ancestory yet
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
        memory.Disassociate(o_m, p_m);
        Assert.AreEqual(1, memory.GetAssociations<ObjectMemory>(memory.Recall<PlaceMemory>(p)).Count());
        var t_m = memory.Remember<TopicMemory>("last seen");
        memory.Associate(o_m, p_m, t_m);
        Assert.AreEqual(1, memory.GetAssociations<ObjectMemory>(memory.Recall<TopicMemory>("last seen")).Count());
    }

    [Test]
    public void MemoryLocationTests()
    {
        var memory = new GameObject().AddComponent<MemoryComponent>();
        var o = new GameObject();
        var p = new Vector3(10, 10, 10);
        var p2 = new Vector3(20, 10, 10);
        var o_m = memory.Remember<ObjectMemory>(o);
        var p_m = memory.Remember<PlaceMemory>(p);
        memory.Remember<PlaceMemory>(p2);
        memory.Associate(o_m, p_m);
        var p3 = new Vector3(11, 10, 10);
        Assert.AreEqual(1, memory.RecallNear<PlaceMemory>(p3, 2f).Count());
        Assert.AreEqual(2, memory.RecallNear<PlaceMemory>(p3, 20f).Count());
        Assert.AreEqual(0, memory.RecallNear<PlaceMemory>(Vector3.zero, 5f).Count());
    }

    [Test]
    public void ComplexMemoryTests()
    {
        var memory = new GameObject().AddComponent<MemoryComponent>();
        var o = new GameObject();
        var p = new Vector3(10, 10, 10);
        var o_m = memory.Remember<ObjectMemory>(o);
        var p_m = memory.Remember<PlaceMemory>(p);
        var o2 = new GameObject();
        var o2_m = memory.Remember<ObjectMemory>(o2);
        memory.Associate(o2_m, p_m);
        memory.Associate(o_m, p_m);
        memory.Associate(o2_m, memory.Remember<TopicMemory>("friend"));
        // get all friends near a location
        var topic = memory.Recall<TopicMemory>("friend");
        var friends =
            memory.RecallNear<PlaceMemory>(new Vector3(5, 5, 5), 10f)
            .Select(x => memory.GetAssociations<ObjectMemory>(x).Where(y => memory.RecallAll<TopicMemory>().Contains(topic)));
        Assert.AreEqual(1, friends.Count());
    }

    [Test]
    public void MemoryComponentTests()
    {
        var memory = new GameObject().AddComponent<MemoryComponent>();
        Assert.IsNull(memory.Recall<PlaceMemory>(Vector3.zero));
    }

}